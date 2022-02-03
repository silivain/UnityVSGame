using UnityEngine;

// mécanismes de mouvements des joueurs
public class PlayerMovement : MonoBehaviour {	//video 2

    public float moveSpeed;		// vitesse de déplacement latéral
    public float climbSpeed;	// vitesse sur échelles
    public float jumpForce;		// puissance de saut
	public float dashForce;		// puissance de dash

    private bool isJumping;		// vrai si le perso est en l'air
	private bool isDashing;		// vrai si le perso est en train de dash
    private bool isGrounded;	// vrai si le perso touche le sol ou un échelle

	[HideInInspector]			// cache les variables suivantes dans l'inspecteur d'unity
    public bool isClimbing;						// vrai si le joueur grimpe

    public Transform groundCheck;				// détecte la présence du sol (tilemap 0)
    public float groundCheckRadius;				// marge de détection
    public LayerMask collisionLayers;			// layers pris en compte par le groundCheck

    public Rigidbody2D rb;						// rigidbody du joueur
    public Animator animator;					// animator
    public SpriteRenderer spriteRenderer;		// spriteRenderer
    public CapsuleCollider2D playerCollider;	// collider

    private Vector3 velocity = Vector3.zero;	// vitesse courante du joueur (3D)
    private float horizontalMovement;			// vitesse horizontale
    private float verticalMovement;				// vitesse verticale

    public string horizontalAxis;				// axe horizontal (utile pour les controles)
    public KeyCode jump;						// touche de saut
	public KeyCode dash;						// touche de dash
    public KeyCode fire;						// touche de tir

    public GameObject bullet;					// gameobject tiré lorsque 'fire' pressed
    public Transform throwPoint;				// point depuis lequel les projectiles sont instanciés
    private Vector3 throwPointPosition;			// position du point ci-dessus
    private Vector3 playerPosition;				// position du joueur
	private float powSign;						// 0 ou 1, sert à calculer la direction du joueur (-1^powSign)
	private float sign;							// -1 ou 1, direction du joueur sur l'axe x

    public static PlayerMovement instance;		// instance de la classe

	/* init de variables
	*/
    private void Awake() {
		throwPointPosition = throwPoint.transform.position;
		playerPosition = transform.position;
		powSign = 0f;
		sign = Mathf.Pow(-1f, powSign);
		instance = this;
    }


	/* détecte les différents inputs et appelle les fonctions appropriées
	*/
    void Update() {
      throwPointPosition = throwPoint.position;
      playerPosition = transform.position;

	  // saut
      if (Input.GetKeyDown(jump) && isGrounded && !isClimbing) {
        isJumping = true;
      }

	  // dash
	  if (Input.GetKeyDown(dash) && !isClimbing) {
		  isDashing = true;
      }

	  // tir
      if (Input.GetKeyDown(fire)) {
        GameObject bulletClone = (GameObject) Instantiate(bullet, throwPoint.position, throwPoint.rotation);
        bulletClone.transform.localScale = new Vector3(sign, 1, 1);
		// TODO indiqué au projectile son parent pour pas se le manger lors d'un dash par ex
        //Debug.Log("player pos : " + transform.position + "\ndebug depuis PlayerMovement, l84"); debug
        //anim.SetTrigger("fire anim"); animation
      }

	  // maj des vitesses horizontales et verticales
      horizontalMovement = Input.GetAxis(horizontalAxis) * moveSpeed * Time.fixedDeltaTime;
      verticalMovement = Input.GetAxis("Vertical") * climbSpeed * Time.fixedDeltaTime;

      // changement de direction du joueur
	  if ((horizontalMovement > 0 && sign < 0) || (horizontalMovement < 0 && sign > 0)) {
		Flip();
	  }

	  // huh ? lié à l'animation, mais sert à quoi ?
	  animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
      animator.SetBool("isClimbing", isClimbing);
    }


	/* appelé à un taux de frames fixé, et pas à chaque frame
	* maj du controle par rapport au sol et déplace le joueur
	*/
    void FixedUpdate() {
      isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, collisionLayers);
      MovePlayer(horizontalMovement, verticalMovement);
    }


	/* déplace le joueur en fonction de sa situation et des forces qui lui sont appliquées
	*/
    void MovePlayer(float _horizontalMovement, float _verticalMovement) {
      if(!isClimbing) {
        Vector3 targetVelocity = new Vector2(_horizontalMovement, rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, .05f);

        if(isJumping) {
          rb.AddForce(new Vector2(0f, jumpForce));
          isJumping = false;
        }

		if(isDashing) {
			rb.AddForce(new Vector2(sign * dashForce, 0f));
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
		sign = Mathf.Pow(-1f, powSign);
		throwPointPosition.x += sign * 2 * Mathf.Abs(playerPosition.x - throwPointPosition.x);
        //throwPoint.position = throwPointPosition;	refresh, semble pas nécessaire
	}


	/* Applique une force de recul au joueur après avoir été touché par un projectile
	*/
	public void Recoil(Collider2D targetHit, Rigidbody2D bulletRB) {
		// Vector3 recoilForce = new Vector3(bulletRB.velocity.x, bulletRB.velocity.y, 0);
		// TODO possible vector3 et composante z pour le projectile si traj non horizontale
		// TODO remplacer la constante 0.5f par une valeur recup dans les stats de l'arme
		targetHit.attachedRigidbody.AddForce(0.5f * bulletRB.velocity, ForceMode2D.Impulse);
	}

	/* affiche le groundCheck à l'écran (debug)
	*/
    private void OnDrawGizmos() { //Gizmos = indicateurs visuels de Unity
      Gizmos.color = Color.red;
      Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
