using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// gère le déplacement des projectiles du souba ingame
// applique des dégats le cas échéant ou détruit le projectile après un temps donné (et donc une distance donnée)
// les projectiles sont instancés dans 'PlayerWeapon.cs'
public class Sousa : MonoBehaviour
{
	public float sousaSpeed;           	// sousa bullet speed
	public int damageOnCollision = 5;  	// sousa bullet damage
	//public GameObject bulletEffect;  	// TODO bullet visual effect
	private Rigidbody2D rb;            	// sousa bullet rigidbody


  // Start is called before the first frame update
	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update()
	{
		// keeps the bullet movin
		rb.velocity = transform.right * sousaSpeed;
	}

	// Gestion de la collision avec un obstacle
	void OnTriggerEnter2D(Collider2D other) {
		//Instantiate(bulletEffect, transform.position, transform.position); TODO visual effect

		/* Inflige les dégats du projectile à un joueur lorsque collision
		* - check pour pas se prendre son propre proj
		* - check que la cible est bien un player avant d'appliquer les dégats
		*/
		if(other.transform.tag[other.transform.tag.Length - 1] != transform.tag[transform.tag.Length - 1]
		&& other.transform.tag.Substring(0, 4) == "Play"
		&& other is CapsuleCollider2D) {
			PlayerHealth playerHealth = other.transform.GetComponent<PlayerHealth>();
			PlayerWeapon PlayerWeapon = other.transform.GetComponent<PlayerWeapon>();

			playerHealth.TakeDamage(damageOnCollision + (int) PlayerWeapon.currentDamageBonus);
			// TODO appel à la fonction de recul en passant les arguments nécessaires
			// le collider 'other', le rigidbody du go bullet (pour pouvoir recup sa velocity)
			PlayerMovement.instance.Recoil(other, rb);
		}

		/* Détruit le projectile lorsque collision avec un obstacle
		* - check que les proj ne s'annulent pas entre eux
		* - check qu'on a pas trigger un item ramassable
		*/
		if(other.transform.tag.Substring(0, 4) != "Proj"
		&& other.transform.tag != "Weapon"
		&& other.transform.tag != "SpeedBonus"
		&& other.transform.tag != "DamageBonus"
		&& other.transform.tag != "AmmunitionBonus"
		&& other.transform.tag != "Heal"
		&& other.transform.tag != "HealDelayed") {
			Destroy(gameObject);
		}
	}
}
