//video 2

﻿using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float moveSpeed;
    public float climbSpeed;
    public float jumpForce;

    private bool isJumping;
    private bool isGrounded;
    [HideInInspector]
    public bool isClimbing;
    private bool m_FacingRight = true;  //Pour determiner la direction dans laquelle se trouve le perso (et ses enfants)

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask collisionLayers;

    public Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public CapsuleCollider2D playerCollider;

    private Vector3 velocity = Vector3.zero;
    private float horizontalMovement;
    private float verticalMovement;

    public string horizontalAxis;
    public KeyCode jump;
    public KeyCode fire;

    public GameObject bullet;
    public Transform throwPoint;
    private Vector3 throwPointPosition;
    private Vector3 playerPosition;
    private Vector3 direction;

    public static PlayerMovement instance;

    /*
    mécanisme de singleton : garantit qu'il n'y ait qu'une seule instance de PlayerMovement
    permet aussi d'appeler ce script de puis n'importe quel autre script sans utiliser de ref
    */
    private void Awake() {
      /*
      if(instance != null) {
        Debug.LogWarning("Il y a plus d'une instance de PlayerMovement dans la scène.");
        return;
      }
      instance = this;
      */
      throwPointPosition = throwPoint.transform.position;
      playerPosition = transform.position;
      direction = new Vector3(1, 1, 1);
    }


    void Update() {
      throwPointPosition = throwPoint.position;
      playerPosition = transform.position;

      if (Input.GetKeyDown(jump) && isGrounded && !isClimbing) {
        isJumping = true;
      }

      if (Input.GetAxis(horizontalAxis) > 0 && throwPointPosition.x < playerPosition.x) { // on va vers la droite mais le throwpoint est à gauche
        playerPosition = transform.position;  // on update la position du perso
        direction = new Vector3(1, 1, 1);     // direction dans laquelle le perso regarde
        throwPointPosition.x = playerPosition.x + Mathf.Abs(playerPosition.x - throwPointPosition.x); // on recalcule le throwpoint
        throwPoint.position = throwPointPosition; // et on le met à jour
      }
      if (Input.GetAxis(horizontalAxis) < 0 && throwPointPosition.x > playerPosition.x) { // on va vers la gauche mais le throwpoint est à droite
        playerPosition = transform.position;  // on update la position du perso
        direction = new Vector3(-1, 1, 1);   // direction dans laquelle le perso regarde
        throwPointPosition.x = playerPosition.x - Mathf.Abs(playerPosition.x - throwPointPosition.x); // on recalcule le throwpoint
        throwPoint.position = throwPointPosition; // et on le met à jour
      }

      if (Input.GetKeyDown(fire)) {
        GameObject bulletClone = (GameObject) Instantiate(bullet, throwPoint.position, throwPoint.rotation);
        bulletClone.transform.localScale = direction;
        //anim.SetTrigger("fire anim");
      }

      horizontalMovement = Input.GetAxis(horizontalAxis) * moveSpeed * Time.fixedDeltaTime;
      verticalMovement = Input.GetAxis("Vertical") * climbSpeed * Time.fixedDeltaTime;

      //Si le personnage se deplace vers la droite, mais n'est pas dirigé vers la droite
      if (horizontalMovement > 0 && !m_FacingRight)
			{
				Flip(); // on le retourne
			}
			// idem dans le sens contraire (va à gauche, dirigé à droite)
			else if (horizontalMovement < 0 && m_FacingRight)
			{
				Flip();
			}


      animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
      animator.SetBool("isClimbing", isClimbing);
    }

    void FixedUpdate()
    {
      isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, collisionLayers);
      MovePlayer(horizontalMovement, verticalMovement);
    }

    void MovePlayer(float _horizontalMovement, float _verticalMovement) {
      if(!isClimbing)
      {
        Vector3 targetVelocity = new Vector2(_horizontalMovement, rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, .05f);

        if(isJumping)
        {
          rb.AddForce(new Vector2(0f, jumpForce));
          isJumping = false;
        }
      }
      else  //video 12
      {
        Vector3 targetVelocity = new Vector2(0, _verticalMovement);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, .05f);
      }
    }

    private void Flip()
	{
		// Change la direction suposé du personnage
		m_FacingRight = !m_FacingRight;

		transform.Rotate(0f, 180f, 0f);
	}

    private void OnDrawGizmos() { //Gizmos = indicateurs visuels de Unity
      Gizmos.color = Color.red;
      Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
