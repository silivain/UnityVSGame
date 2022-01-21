using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// manage instantiated bullet in game
// deals damage when needed or destroy bullet
// bullets are instantiated in 'PlayerMovement.cs'
public class Bullet : MonoBehaviour
{
	public float bulletSpeed;          // bullet speed
	public int damageOnCollision = 5;  // bullet damage
	//public GameObject bulletEffect;  // TODO bullet visual effect
	private Rigidbody2D rb;            // bullet rigidbody


   	// Start is called before the first frame update
	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update()
	{
		// keeps the bullet movin
		rb.velocity = new Vector2(bulletSpeed * transform.localScale.x, 0f);
	}

	/* When a bullet hit a collider, check the tag of the hit object
	* if the object is a player, deals damage
	*/
	void OnTriggerEnter2D(Collider2D other) {
		//Instantiate(bulletEffect, transform.position, transform.position); TODO visual effect
		if(other.transform.CompareTag("Player 1") || other.transform.CompareTag("Player 2")) {
			PlayerHealth playerHealth = other.transform.GetComponent<PlayerHealth>();
			playerHealth.TakeDamage(damageOnCollision);
		}
		Destroy(gameObject);
	}
}
