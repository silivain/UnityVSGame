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

    public static PlayerMovement instance;

    private void Awake() {  //mécanisme de singleton : garantit qu'il n'y ait qu'une seule instance de PlayerMovement
                            //permet aussi d'appeler ce script de puis n'importe quel autre script sans utiliser de ref
      if(instance != null) {
        Debug.LogWarning("Il y a plus d'une instance de PlayerMovement dans la scène.");
        return;
      }

      instance = this;
    }

    void Update() {
      if (Input.GetKeyDown(KeyCode.V) && isGrounded && !isClimbing) {
        isJumping = true;
      }

      horizontalMovement = Input.GetAxis("Horizontal") * moveSpeed * Time.fixedDeltaTime;
      verticalMovement = Input.GetAxis("Vertical") * climbSpeed * Time.fixedDeltaTime;

      Flip(rb.velocity.x);

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

    void Flip(float _velocity) {
      if(_velocity > 0.1f) {
        spriteRenderer.flipX = false;
      }else if(_velocity < -0.1f) {
        spriteRenderer.flipX = true;
      }
    }

    private void OnDrawGizmos() { //Gizmos = indicateurs visuels de Unity
      Gizmos.color = Color.red;
      Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
