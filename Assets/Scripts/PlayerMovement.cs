using UnityEngine;
using System.Collections;
using System;
using UnityEngine.InputSystem;

// mécanismes de mouvements des joueurs
public class PlayerMovement : MonoBehaviour {	//video 2

    public float moveSpeed;                     // vitesse de déplacement latéral
    public float currentSpeedBonus = 0;         // bonus de vitesse actuel
    public float climbSpeed;                    // vitesse sur échelles
    public float jumpForce;                     // puissance de saut
    public float dashForce;                     // puissance de dash
    private float fallSpeed = -18f;             // vitesse à laquelle le jouer tombe
    private float fallIncreaseSpeed = -1.5f;    // accélération de la vitesse de chute jusqu'à vitesse max

    private bool isJumping;		                // vrai si le perso est en l'air
    private bool isDashing;		                // vrai si le perso est en train de dash
    private bool isDashReady=true;              //cooldown du Dash
    private bool isGrounded;	                // vrai si le perso touche le sol ou un échelle

	[HideInInspector]			                // cache les variables suivantes dans l'inspecteur d'unity
    public bool isClimbing;					    // vrai si le joueur grimpe

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
    public float dashCooldownTime = 2f;         //nb de second entre 2 dashs

    public Transform throwPoint;				// point depuis lequel les projectiles sont instanciés
    private Vector3 throwPointPosition;			// position du point ci-dessus
    private Vector3 playerPosition;			  	// position du joueur
	private float powSign;				        // 0 ou 1, sert à calculer la direction du joueur (-1^powSign)
	public float xDirection;					// -1 ou 1, direction du joueur sur l'axe x

    public Transform playerShield;              // shield du joueur

    public static PlayerMovement instance;		// instance de la classe
    private PlayerControls controls;            // script gérant les inputs globales
    //private InputActionMap controlsPlayer;    // action map correspondant au joueur


    /* init de variables
    */
    private void Awake() {
        controls = new PlayerControls();    // on recup le script qui gère les inputs

        controlPlayer();                    // récupère les inputs du joueur
                                            // attente passive -> pas besoin d'être dans update

		throwPointPosition = throwPoint.transform.position;		// point de tir du joueur
  		playerPosition = transform.position;					// position de départ du joueur
  		powSign = 0f;											// puissance servant à calculer le sens du joueur
  		xDirection = Mathf.Pow(-1f, powSign);					// sens du joueur (facing left or right)
  		instance = this;										// instance de la classe TODO : remove ?
    }


    /* détecte les différents inputs et appelle les fonctions appropriées
	*/
    void Update()
    {
        throwPointPosition = throwPoint.position;
        playerPosition = transform.position;

        /* maj de la vitesse horizontale
        * lit la valeur du vecteur responsable du mouvement du joueur
        * la val du vec2 est gérée par l'input sys en fct des inputs g. et d.
        */
        Vector2 inputVector;

        if(transform.tag == "Player 1") {
            inputVector = controls.Player1.Move.ReadValue<Vector2>();
        }else {
            inputVector = controls.Player2.Move.ReadValue<Vector2>();
        }

        horizontalMovement = inputVector.x * (moveSpeed + currentSpeedBonus) * Time.fixedDeltaTime;

        // changement de direction du joueur
        if ((horizontalMovement > 0 && xDirection < 0) || (horizontalMovement < 0 && xDirection > 0))
        {
            Flip();
        }
    }


    /* appelé à un taux de frames fixé, et pas à chaque frame
	* maj du controle par rapport au sol et déplace le joueur
	*/
    void FixedUpdate() {
      isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, collisionLayers);
      MovePlayer(horizontalMovement);
    }


    /* active la bonne InputActionMap selon le joueur
    * récupère les inputs 'Jump' et 'Dash'
    */
    void controlPlayer() {
        if (transform.tag == "Player 1") {
            controls.Player1.Enable();
            controls.Player1.Jump.performed += ctx => Jump();
            controls.Player1.Dash.performed += ctx => Dash();
        }else if (transform.tag == "Player 2") {
            controls.Player2.Enable();
            controls.Player2.Jump.performed += ctx => Jump();
            controls.Player2.Dash.performed += ctx => Dash();
        }
    }


    /* fait dasher le joueur
    */
    private void Dash() {
        // dash
        if (!isClimbing && isDashReady)
        {
            isDashing = true;
        }
    }


    /* fait sauter le joueur
    * vérifie qu'il est bien au sol avant
    */
    private void Jump() {
        if (isGrounded && !isClimbing && rb.velocity.y < 0.1) {
            isJumping = true;
        }
    }


	/* déplace le joueur en fonction de sa situation et des forces qui lui sont appliquées
	*/
    void MovePlayer(float _horizontalMovement) {
        float shieldMod = 1f;
        if (playerShield.gameObject.activeSelf && isGrounded) {
            shieldMod = .1f;
        }

        Vector3 targetVelocity = new Vector2(_horizontalMovement * shieldMod, rb.velocity.y);

        if (rb.velocity.y < -0.1f && rb.velocity.y > fallSpeed) {
            rb.velocity = Vector3.SmoothDamp(new Vector2(rb.velocity.x, rb.velocity.y + fallIncreaseSpeed), targetVelocity, ref velocity, .05f);
        }else {
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, .05f);
        }

        if (isJumping) {
            rb.AddForce(new Vector2(0f, jumpForce));
            isJumping = false;
        }

        if(isDashing && isDashReady) {
            isDashReady = false;
            StartCoroutine(cooldownDash());
            rb.AddForce(new Vector2(xDirection * dashForce, 0f));
            isDashing = false;
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


    /* Rend le dash à nouveau disponible
    * après 'dashCooldownTime' secondes
    */
    IEnumerator cooldownDash() {
        if (!isDashReady) {
            yield return new WaitForSeconds(dashCooldownTime);
            isDashReady = true;
        }
    }
}
