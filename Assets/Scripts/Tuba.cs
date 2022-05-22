using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// gère le déplacement des projectiles du souba ingame
// applique des dégats le cas échéant ou détruit le projectile après un temps donné (et donc une distance donnée)
// les projectiles sont instancés dans 'PlayerWeapon.cs'
public class Tuba : MonoBehaviour
{
	public float tubaSpeed;           	// sousa bullet speed
	public int damageOnCollision = 15;  	// sousa bullet damage
	//public GameObject bulletEffect;  	// TODO bullet visual effect
	private Rigidbody2D rb;             // sousa bullet rigidbody
	private float splashRange = 4f;


  // Start is called before the first frame update
	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update()
	{
		// keeps the bullet movin
		rb.velocity = transform.right * tubaSpeed;
	}

	/* When a bullet hit a collider, check the tag of the hit object
	* if the object is a player, deals damage
	*/
	void OnTriggerEnter2D(Collider2D other) {
		//Instantiate(bulletEffect, transform.position, transform.position); TODO visual effect
		var hitColliders = Physics2D.OverlapCircleAll(transform.position, splashRange);
		foreach (var hitCollider in hitColliders)
		{
			if (other.transform.tag != transform.tag && other.transform.tag.Substring(0, 6) == "Player")
			{
				PlayerHealth playerHealth = other.transform.GetComponent<PlayerHealth>();
				playerHealth.TakeDamage(damageOnCollision);
				// TODO appel à la fonction de recul en passant les arguments nécessaires
				// le collider 'other', le rigidbody du go bullet (pour pouvoir recup sa velocity)
				PlayerMovement.instance.Recoil(other, rb);
			}
		}
		if (other.transform.tag != transform.tag && other.transform.tag != "Weapon") {
			Debug.Log("destroying after collision\nother.transform.tag = " + other.transform.tag + "\ntransform.tag = " + transform.tag);
			Destroy(gameObject);
		}

	}
}
