using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// gère le ramassage et les bonus des collectables
public class Collectables : MonoBehaviour
{
    public float speedBonus = 5;           	// valeur du bonus de vitesse
    public float speedDuration = 10;        // durée du bonus de vitesse

    public float damageBonus = 3;           // bonus de dégats
    public float damageDuration = 10;       // durée du bonus de dégats


    /* Applique le bonus de vitesse pendant 'speedDuration' secondes
    */
    IEnumerator speedFunction(PlayerMovement pMovement) {
        pMovement.currentSpeedBonus = speedBonus;
        yield return new WaitForSeconds(speedDuration);
        pMovement.currentSpeedBonus = 0;
    }


    /* Applique le bonus de dégats pendant 'damageDuration' secondes
    */
    IEnumerator damageFunction(PlayerWeapon pWeapon) {
        pWeapon.currentDamageBonus = damageBonus;
        Debug.Log("currentDamageBonus = " + pWeapon.currentDamageBonus);
        yield return new WaitForSeconds(damageDuration);
        pWeapon.currentDamageBonus = 0;
    }


    /* Réaprovisionne le joueur en munitions
    * le nombre exact de munitions bonus pour chaque arme
    * est donné dans 'PlayerWeapon.bonusAmmunition'
    */
    IEnumerator ammunitionFunction(PlayerWeapon pWeapon) {
        pWeapon.ammunition += pWeapon.bonusAmmunition[pWeapon.weaponID];
        pWeapon.AmmoDisplay();
        yield return new WaitForSeconds(0f);    // dégueu, pas moyen de faire autrement ?
    }


	// Gestion de la collision avec un collectable
	void OnTriggerEnter2D(Collider2D other) {

        if (other.transform.tag == "SpeedBonus") {
            CurrentSceneManager.instance.CollectedBonus(other.transform.position);
            StartCoroutine(speedFunction(gameObject.GetComponent<PlayerMovement>()));
            Destroy(other.gameObject);

        }else if (other.transform.tag == "DamageBonus") {
            CurrentSceneManager.instance.CollectedBonus(other.transform.position);
            StartCoroutine(damageFunction(gameObject.GetComponent<PlayerWeapon>()));
            Destroy(other.gameObject);

        }else if (other.transform.tag == "HealDelayed") {
            //TODO
        }else if (other.transform.tag == "AmmunitionBonus") {
            CurrentSceneManager.instance.CollectedBonus(other.transform.position);
            StartCoroutine(ammunitionFunction(gameObject.GetComponent<PlayerWeapon>()));
            Destroy(other.gameObject);
        }
	}
}
