using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// manage instantiated bullet in game
// deals damage when needed or destroy bullet
// bullets are instantiated in 'PlayerMovement.cs'
public class clarinet : MonoBehaviour
{
	public int damageOnCollision = 5;  // bullet damage
	public float clarinetForce;
									   //public GameObject bulletEffect;  // TODO bullet visual effect
	private Rigidbody2D rb;            // bullet rigidbody


	// Start is called before the first frame update
	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		rb.AddForce(new Vector2(transform.right.x * 10f * clarinetForce * transform.localScale.x, 3f * clarinetForce));
	}

	// Update is called once per frame
	void Update()
	{
		// keeps the bullet movin
		rb.SetRotation(transform.right.x * 3f * rb.velocity.y);
	}

	/* When a bullet hit a collider, check the tag of the hit object
	* if the object is a player, deals damage
	*/
	void OnTriggerEnter2D(Collider2D other) {
		//Instantiate(bulletEffect, transform.position, transform.position); TODO visual effect
		if(other.transform.tag != transform.tag && other.transform.tag.Substring(0, 6) == "Player") {
			PlayerHealth playerHealth = other.transform.GetComponent<PlayerHealth>();
			playerHealth.TakeDamage(damageOnCollision);
			// TODO appel à la fonction de recul en passant les arguments nécessaires
			// le collider 'other', le rigidbody du go bullet (pour pouvoir recup sa velocity)
			PlayerMovement.instance.Recoil(other, rb);
		}
		if (/*other.transform.tag != transform.tag && */other.transform.tag != "Weapon") {
			Destroy(gameObject);
		}
	}
}
