using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trombone : MonoBehaviour
{
    public int damageOnCollision = 8;  	// trombone damage


    /* immunitée après un coup de trombone
    * évite de se manger les dégats de plusieurs colliders
    * (l'animation du coup de trb active 4 colliders différents)
    IEnumerator immunTromboneCD(PlayerWeapon playerWeapon) {
        Debug.Log("beginning immunTromboneCD");
        yield return new WaitForSeconds(0.4f);  // why the fck is it returning and not continuing code
        playerWeapon.immunTromboneHit = false;
        Debug.Log("immunTromboneHit after immunTromboneCD = " + playerWeapon.immunTromboneHit);
    }
    */


    IEnumerator immunTromboneCD(PlayerWeapon playerWeapon) {
        Debug.Log("step 1 ok");
        yield return new WaitForSeconds(1f);
        Debug.Log("tryin step 2");
        yield return new WaitForSeconds(0.4f);
        Debug.Log("step 2 ok");
    }


    // Gestion de la collision avec un obstacle
	void OnTriggerEnter2D(Collider2D other) {
		//Instantiate(bulletEffect, transform.position, transform.position); TODO visual effect

        string otherTag = other.transform.tag;
        string shooterTag = transform.parent.parent.tag;

		/* Inflige les dégats du coup de trb à un joueur lorsque collision
		* - check pour pas se prendre son propre coup
		* - check que la cible est bien un player avant d'appliquer les dégats
		* - check que le collider est un 'CapsuleCollider2D' pour appliquer
		*   une seule fois les dégats
		*/
		if(otherTag[otherTag.Length - 1] != shooterTag[shooterTag.Length - 1]
		&& otherTag.Substring(0, 4) == "Play"
		&& other is CapsuleCollider2D) {
			PlayerHealth playerHealth = other.transform.GetComponent<PlayerHealth>();
			PlayerWeapon playerWeapon = other.transform.GetComponent<PlayerWeapon>();
			PlayerMovement playerMovement = other.transform.GetComponent<PlayerMovement>();
			PlayerWeapon tireur = transform.parent.parent.GetComponent<PlayerWeapon>();

            Debug.Log("immunTromboneHit = " + playerWeapon.immunTromboneHit);

            if (!playerWeapon.immunTromboneHit) {
                playerWeapon.immunTromboneHit = true;
        		playerHealth.TakeDamage(damageOnCollision + (int) tireur.currentDamageBonus);
        		// TODO appel à la fonction de recul en passant les arguments nécessaires
        		// le collider 'other', le rigidbody du go bullet (pour pouvoir recup sa velocity)
        		playerMovement.RecoilCac(other, transform.parent.parent);
                StartCoroutine(immunTromboneCD(playerWeapon));
            }
		}
    }
}
