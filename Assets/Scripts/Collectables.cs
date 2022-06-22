using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// gère le ramassage et les bonus des collectables
public class Collectables : MonoBehaviour
{
    public float speedBonus = 5;           	// valeur du bonus de vitesse
    public float speedDuration = 10;        // durée du bonus de vitesse

    public float currentDamageBonus;        // bonus de dégats actuel
    public float damageBonus = 3;           // bonus de dégats
    public float damageDuration = 10;       // durée du bonus de dégats

    public GameObject player;               // le joueur qui récupère le bonus


	// Gestion de la collision avec un collectable
	void OnTriggerEnter2D(Collider2D other) {

        /* Bonus de vitesse
        * augmente la vitesse du joueur de 'speedBonus' pendant 'speedDuration' secondes
        */
		if (other.transform.tag == "SpeedBonus") {
            StartCoroutine(speedFunction(other.transform.GetComponent<PlayerMovement>()));

        }else if (other.transform.tag == "DamageBonus") {
            StartCoroutine(damageFunction());

        }else if (other.transform.tag == "HealDelayed") {
            //TODO
        }
	}

    /* Applique le bonus de vitesse pendant 'speedDuration' secondes
    */
    IEnumerator speedFunction(PlayerMovement pMovement) {
        pMovement.currentSpeedBonus = speedBonus;
        yield return new WaitForSeconds(speedDuration);
        pMovement.currentSpeedBonus = 0;
    }

    /* Applique le bonus de dégats pendant 'damageDuration' secondes
    */
    IEnumerator damageFunction() {
        //TODO on veut pas modif 'currentDamageBonus', mais la var dans 'PlayerWeapon'
        currentDamageBonus = damageBonus;
        yield return new WaitForSeconds(damageDuration);
        currentDamageBonus = 0;
    }
}
