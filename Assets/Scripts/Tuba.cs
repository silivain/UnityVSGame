using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// gère le déplacement des projectiles du souba ingame
// applique des dégats le cas échéant ou détruit le projectile après un temps donné (et donc une distance donnée)
// les projectiles sont instancés dans 'PlayerWeapon.cs'
public class Tuba : MonoBehaviour
{
	public float tubaSpeed;           	// tuba bullet speed
	//public GameObject bulletEffect;  	// TODO bullet visual effect
	private Rigidbody2D rb;             // tuba bullet rigidbody



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

	// Gestion de la collision avec un obstacle
	void OnTriggerEnter2D(Collider2D other) {
		//Instantiate(bulletEffect, transform.position, transform.position); TODO visual effect
		/*
		* - Détruit le projectile lorsque collision
		* - check que les proj ne s'annulent pas entre eux
		* - check qu'on a pas trigger un item ramassable
		* - check que la cible est bien un player avant d'appliquer les dégats
		*/
		if (other.GetType() != typeof(BoxCollider2D) && other.transform.tag.Substring(0, 4) != "Proj"
		&& other.transform.tag != "Weapon"
		&& other.transform.tag[other.transform.tag.Length - 1] != transform.tag[transform.tag.Length - 1]
		&& other.transform.tag != "SpeedBonus"
		&& other.transform.tag != "DamageBonus"
		&& other.transform.tag != "AmmunitionBonus"
		&& other.transform.tag != "Heal"
		&& other.transform.tag != "HealDelayed")
		{
			GameObject  player = GameObject.FindGameObjectWithTag("Player 1");
			PlayerWeapon playerWeapon = player.transform.GetComponent<PlayerWeapon>();
			playerWeapon.Explosion(gameObject);
		}
	}
}
