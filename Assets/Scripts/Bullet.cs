using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* gère les 'bullet' instanciées dans la scène
* applique les dégats ou détruit le proj
* 'bullet' est instanciée dans 'PlayerWeapon'
*/
public class Bullet : MonoBehaviour
{
	public float bulletSpeed;          // vitesse du proj
	public int damageOnCollision = 5;  // dégats du proj
	//public GameObject bulletEffect;  // TODO bullet visual effect
	private Rigidbody2D rb;            // Rigidbody2D du proj


	// récupère le Rigidbody2D du proj
	void Start() {
		rb = GetComponent<Rigidbody2D>();
	}


	// garde la vitesse du proj constante
	void Update() {
		rb.velocity = transform.right * bulletSpeed;
	}


	// Gestion de la collision avec un obstacle
	void OnTriggerEnter2D(Collider2D other) {
		//Instantiate(bulletEffect, transform.position, transform.position); TODO visual effect

		/* Inflige les dégats du projectile à un joueur lorsque collision
		* - check pour pas se prendre son propre proj
		* - check que la cible est bien un player avant d'appliquer les dégats
		* - check que le collider est un 'CapsuleCollider2D' pour appliquer
		*   une seule fois les dégats
		*/
		if(other.transform.tag[other.transform.tag.Length - 1] != transform.tag[transform.tag.Length - 1]
		&& other.transform.tag.Substring(0, 4) == "Play"
		&& other is CapsuleCollider2D) {
			PlayerHealth playerHealth = other.transform.GetComponent<PlayerHealth>();
			PlayerWeapon PlayerWeapon = other.transform.GetComponent<PlayerWeapon>();
			PlayerWeapon tireur = GameObject.FindWithTag(transform.tag.Substring(4, transform.tag.Length - 4)).GetComponent<PlayerWeapon>();

			playerHealth.TakeDamage(damageOnCollision + (int) tireur.currentDamageBonus);
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
