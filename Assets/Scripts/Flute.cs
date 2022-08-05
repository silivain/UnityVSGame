using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flute : MonoBehaviour
{
    public float fluteSpeed;          // bullet speed
	public int damageOnCollision = 5;  // bullet damage
	//public GameObject bulletEffect;  // TODO bullet visual effect
	private Rigidbody2D rb;            // bullet rigidbody
    private Transform transformParent;


    /* récupère le rb de la flute
    * récupère la transform du joueur qui a tiré le proj
    * appelle la fct responsable de la traj du proj
    */
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        transformParent = GameObject.FindWithTag(transform.tag.Substring(4, transform.tag.Length - 4)).transform;
        reverseDelay();
    }


    // garde la vitesse de la flute constante
    void Update() {
        rb.velocity = transform.right * fluteSpeed;
    }


    /* modif la traj du proj pour lui donner une traj sinusoidale
    */
    IEnumerator reverse() {
        float modif = 0.1f;
        if (transform.rotation.y >= transformParent.rotation.y) {
            modif = -0.1f;
        }
        for(int i = 0; i < 60; ++i) {
            rb.SetRotation(rb.rotation + modif);
            yield return null;
        }
    }


    /* appelle 2 fois la fct qui modif la traj du proj
    *       après un temps d'attente aléatoire
    */
    IEnumerator reverseDelay() {
        float alea = Random.Range(0f, 0.15f);
        yield return new WaitForSeconds(0.25f + alea);
        reverse();

        alea = Random.Range(0f, 0.15f);
        yield return new WaitForSeconds(0.25f + alea);
        reverse();
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
