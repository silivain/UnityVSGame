using UnityEngine;
using System.Collections;
using System;
using UnityEngine.InputSystem;

// mécanismes de mouvements des joueurs
public class PlayerMovement : MonoBehaviour {	//video 2

    public float moveSpeed;		// vitesse de déplacement latéral
    public float climbSpeed;	// vitesse sur échelles
    public float jumpForce;		// puissance de saut
    public float dashForce;		// puissance de dash
    private float fallSpeed = -18f;
    private float fallIncreaseSpeed = -1.5f;

    private bool isJumping;		   // vrai si le perso est en l'air
    private bool isDashing;		   // vrai si le perso est en train de dash
    private bool isDashReady=true; //cooldown du Dash
    private bool isGrounded;	   // vrai si le perso touche le sol ou un échelle

	[HideInInspector]			               // cache les variables suivantes dans l'inspecteur d'unity
    public bool isClimbing;						 // vrai si le joueur grimpe

    public Transform groundCheck;				// détecte la présence du sol (tilemap 0)
    public float groundCheckRadius;			    // marge de détection
    public LayerMask collisionLayers;			// layers pris en compte par le groundCheck

    public Rigidbody2D rb;						// rigidbody du joueur
    public Animator animator;					// animator
    public SpriteRenderer spriteRenderer;		// spriteRenderer
    public CapsuleCollider2D playerCollider;	// collider

    private Vector3 velocity = Vector3.zero;	// vitesse courante du joueur (3D)
    private float horizontalMovement;			// vitesse horizontale
    private float verticalMovement;				// vitesse verticale

    public string horizontalAxis;				// axe horizontal (utile pour les controles)
    public int horizontalWay;
    public float dashCooldownTime = 2f; //nb de second entre 2 dashs

    public Transform throwPoint;				// point depuis lequel les projectiles sont instanciés
    private Vector3 throwPointPosition;			// position du point ci-dessus
    private Vector3 playerPosition;			  	// position du joueur
	  private float powSign;				    // 0 ou 1, sert à calculer la direction du joueur (-1^powSign)
	  public float xDirection;					// -1 ou 1, direction du joueur sur l'axe x

    private Transform playerShield;   // shield du joueur

    public static PlayerMovement instance;		// instance de la classe
    public PlayerControls controls;
    public int deviceNumber;            //Numero de device du gamepad

    /* init de variables
    */

    private void Awake() {
        controls = new PlayerControls();
        controls.devices = new[] { InputSystem.devices[deviceNumber] };
        throwPointPosition = throwPoint.transform.position;
  		playerPosition = transform.position;
  		powSign = 0f;
  		xDirection = Mathf.Pow(-1f, powSign);
  		instance = this;
      playerShield = transform.Find("Shield");
    }

    /* détecte les différents inputs et appelle les fonctions appropriées
	*/
    void Update()
    {
        throwPointPosition = throwPoint.position;
        playerPosition = transform.position;
        controls.Gameplay.Jump.performed += ctx => Jump();
        controls.Gameplay.Dash.performed += ctx => Dash();
        controls.Gameplay.GoLeft.performed += ctx => GoLeft();
        controls.Gameplay.GoRight.performed += ctx => GoRight();


        // maj des vitesses horizontales et verticales
        horizontalMovement = horizontalWay * moveSpeed * Time.fixedDeltaTime;
        //verticalMovement = Input.GetAxis("Vertical") * climbSpeed * Time.fixedDeltaTime;

        // changement de direction du joueur
        if ((horizontalMovement > 0 && xDirection < 0) || (horizontalMovement < 0 && xDirection > 0))
        {
            Flip();
        }

        // huh ? lié à l'animation, mais sert à quoi ?
        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        animator.SetBool("isClimbing", isClimbing);
    }

    private void GoRight()
    {
        horizontalWay = 1;
    }

    private void GoLeft()
    {
        horizontalWay = -1;
    }

    private void Dash()
    {
        // dash
        if (!isClimbing && isDashReady)
        {
            isDashing = true;
        }
    }

    private void Jump()
    {
        // saut
        if (isGrounded && !isClimbing && rb.velocity.y < 0.1)
        {
            isJumping = true;
        }
    }


    /* appelé à un taux de frames fixé, et pas à chaque frame
	* maj du controle par rapport au sol et déplace le joueur
	*/
    void FixedUpdate() {
      isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, collisionLayers);
      //MovePlayer(horizontalMovement, verticalMovement);
    }


	/* déplace le joueur en fonction de sa situation et des forces qui lui sont appliquées
	*/
    void MovePlayer(float _horizontalMovement, float _verticalMovement) {
      if(!isClimbing) {
        float shieldMod = 1f;
        if(playerShield.gameObject.activeSelf && isGrounded) {
          shieldMod = .1f;
        }
        Vector3 targetVelocity = new Vector2(_horizontalMovement * shieldMod, rb.velocity.y);

        if (rb.velocity.y < -0.1f && rb.velocity.y > fallSpeed)
        {
            rb.velocity = Vector3.SmoothDamp(new Vector2(rb.velocity.x, rb.velocity.y + fallIncreaseSpeed), targetVelocity, ref velocity, .05f);
        }
        else
        {
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, .05f);
        }

        if (isJumping) {

          rb.AddForce(new Vector2(0f, jumpForce));
          isJumping = false;
        }

		    if(isDashing && isDashReady) {
          isDashReady=false;
          StartCoroutine(cooldownDash());
          rb.AddForce(new Vector2(xDirection * dashForce, 0f));
          isDashing = false;
        }
      }
      else {	//video 12
        Vector3 targetVelocity = new Vector2(0, _verticalMovement);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, .05f);
      }
    }

	/* retourne la transform du personnage ainsi que son throwPoint
	*/
    private void Flip() {
		transform.Rotate(0f, 180f, 0f);

		powSign = (++powSign) % 2;
		xDirection = Mathf.Pow(-1f, powSign);
	}


	/* Applique une force de recul au joueur après avoir été touché par un projectile
	*/
	public void Recoil(Collider2D targetHit, Rigidbody2D bulletRB) {
        // Vector3 recoilForce = new Vector3(bulletRB.velocity.x, bulletRB.velocity.y, 0);
        // TODO possible vector3 et composante z pour le projectile si traj non horizontale
        // TODO remplacer la constante 0.5f par une valeur recup dans les stats de l'arme
        Transform playerHitShield = targetHit.transform.Find("Shield");
        bool isShieldActive = playerHitShield.gameObject.activeSelf;

        if(isShieldActive) {
            targetHit.attachedRigidbody.AddForce(0.1f * bulletRB.velocity, ForceMode2D.Impulse);
        }else {
            targetHit.attachedRigidbody.AddForce(0.5f * bulletRB.velocity, ForceMode2D.Impulse);
        }

	}

    public void RecoilCac(Collider2D targetHit, Transform shooter)
    {
        // Vector3 recoilForce = new Vector3(bulletRB.velocity.x, bulletRB.velocity.y, 0);
        // TODO possible vector3 et composante z pour le projectile si traj non horizontale
        // TODO remplacer la constante 0.5f par une valeur recup dans les stats de l'arme
        Transform target = targetHit.transform;
        Vector3 force = new Vector3(50f, 0f, 0f);
        if (shooter.position.x > target.position.x)
        {
            force *= -1;
        }

            targetHit.attachedRigidbody.AddForce(force, ForceMode2D.Impulse);
	}

/* affiche le groundCheck à l'écran (debug)
*/
  private void OnDrawGizmos() { //Gizmos = indicateurs visuels de Unity
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
  }

  IEnumerator cooldownDash()
    {
        if (!isDashReady)
        {
            yield return new WaitForSeconds(dashCooldownTime);
            isDashReady = true;
        }

    }
    private void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        controls.Gameplay.Disable();
    }
}
