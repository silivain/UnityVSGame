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

    private bool collectingSpeedBonus  = false;  // vrai si a recup un bonus de speed il y a moins de 0.5 sec
    private bool collectingDamageBonus = false;  // vrai si a recup un bonus de dégats il y a moins de 0.5 sec
    private bool collectingAmmunition  = false;  // vrai si a recup une munn il y a moins de 0.5 sec


    /* Applique le bonus de vitesse pendant 'speedDuration' secondes
    */
    IEnumerator speedFunction(PlayerMovement pMovement) {
        pMovement.currentSpeedBonus = speedBonus;
        yield return new WaitForSeconds(0.5f);
        collectingSpeedBonus = false;
        yield return new WaitForSeconds(speedDuration - 0.5f);
        pMovement.currentSpeedBonus = 0;
    }


    /* Applique le bonus de dégats pendant 'damageDuration' secondes
    */
    IEnumerator damageFunction(PlayerWeapon pWeapon) {
        pWeapon.currentDamageBonus = damageBonus;
        Debug.Log("currentDamageBonus = " + pWeapon.currentDamageBonus);
        yield return new WaitForSeconds(0.5f);
        collectingDamageBonus = false;
        yield return new WaitForSeconds(damageDuration - 0.5f);
        Debug.Log("avant fin du damageBonus, pWeapon.currentDamageBonus = " + pWeapon.currentDamageBonus);
        pWeapon.currentDamageBonus = 0;
    }


    /* Réaprovisionne le joueur en munitions
    * le nombre exact de munitions bonus pour chaque arme
    * est donné dans 'PlayerWeapon.bonusAmmunition'
    */
    IEnumerator ammunitionFunction(PlayerWeapon pWeapon) {
        pWeapon.ammunition += pWeapon.bonusAmmunition[pWeapon.weaponID];
        pWeapon.AmmoDisplay();
        yield return new WaitForSeconds(0.5f);
        collectingAmmunition = false;
    }


	// Gestion de la collision avec un collectable
	void OnTriggerEnter2D(Collider2D other) {

        if (other.transform.tag == "SpeedBonus" && !collectingSpeedBonus) {
            collectingSpeedBonus = true;
            CurrentSceneManager.instance.CollectedBonus(other.transform.position);
            StartCoroutine(speedFunction(gameObject.GetComponent<PlayerMovement>()));
            Destroy(other.gameObject);

        }else if (other.transform.tag == "DamageBonus" && !collectingDamageBonus) {
            collectingDamageBonus = true;
            CurrentSceneManager.instance.CollectedBonus(other.transform.position);
            StartCoroutine(damageFunction(gameObject.GetComponent<PlayerWeapon>()));
            Destroy(other.gameObject);

        }else if (other.transform.tag == "HealDelayed") {
            //TODO
        }else if (other.transform.tag == "AmmunitionBonus" && !collectingAmmunition) {
            collectingAmmunition = true;
            CurrentSceneManager.instance.CollectedBonus(other.transform.position);
            StartCoroutine(ammunitionFunction(gameObject.GetComponent<PlayerWeapon>()));
            Destroy(other.gameObject);
        }
	}
}
