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

    public GameObject AmmunitionBonusP1;
    public GameObject AmmunitionBonusP2;
    public GameObject DamageBonusP1;
    public GameObject DamageBonusP2;
    public GameObject SpeedBonusP1;
    public GameObject SpeedBonusP2;


    /* Applique le bonus de vitesse pendant 'speedDuration' secondes
    * active l'affichage du bonus de vitesse pour le joueur concerné
    */
    IEnumerator speedFunction(PlayerMovement pMovement) {
        string numPlayer = pMovement.transform.name[pMovement.transform.name.Length - 1].ToString();

        if (numPlayer == "1") {
            SpeedBonusP1.SetActive(true);
        }else{
            SpeedBonusP2.SetActive(true);
        }

        pMovement.currentSpeedBonus = speedBonus;
        yield return new WaitForSeconds(0.5f);
        collectingSpeedBonus = false;
        yield return new WaitForSeconds(speedDuration - 0.5f);
        pMovement.currentSpeedBonus = 0;

        if (numPlayer == "1") {
            SpeedBonusP1.SetActive(false);
        }else{
            SpeedBonusP2.SetActive(false);
        }
    }


    /* Applique le bonus de dégats pendant 'damageDuration' secondes
    * active l'affichage du bonus de dégats pour le joueur concerné
    */
    IEnumerator damageFunction(PlayerWeapon pWeapon) {
        string numPlayer = pWeapon.transform.name[pWeapon.transform.name.Length - 1].ToString();

        if (numPlayer == "1") {
            DamageBonusP1.SetActive(true);
        }else{
            DamageBonusP2.SetActive(true);
        }

        pWeapon.currentDamageBonus = damageBonus;
        Debug.Log("currentDamageBonus = " + pWeapon.currentDamageBonus);
        yield return new WaitForSeconds(0.5f);
        collectingDamageBonus = false;
        yield return new WaitForSeconds(damageDuration - 0.5f);
        Debug.Log("avant fin du damageBonus, pWeapon.currentDamageBonus = " + pWeapon.currentDamageBonus);
        pWeapon.currentDamageBonus = 0;

        if (numPlayer == "1") {
            DamageBonusP1.SetActive(false);
        }else{
            DamageBonusP2.SetActive(false);
        }
    }


    /* Réaprovisionne le joueur en munitions
    * le nombre exact de munitions bonus pour chaque arme
    * est donné dans 'PlayerWeapon.bonusAmmunition'
    */
    IEnumerator ammunitionFunction(PlayerWeapon pWeapon) {
        string numPlayer = pWeapon.transform.name[pWeapon.transform.name.Length - 1].ToString();

        if (numPlayer == "1") {
            AmmunitionBonusP1.SetActive(true);
        }else{
            AmmunitionBonusP2.SetActive(true);
        }

        pWeapon.ammunition = Mathf.Min(pWeapon.ammunition + pWeapon.bonusAmmunition[pWeapon.weaponID],
            pWeapon.maxAmmunition[pWeapon.weaponID]);
        pWeapon.AmmoDisplay();
        yield return new WaitForSeconds(0.5f);
        collectingAmmunition = false;
        yield return new WaitForSeconds(1.5f);

        if (numPlayer == "1") {
            AmmunitionBonusP1.SetActive(false);
        }else{
            AmmunitionBonusP2.SetActive(false);
        }
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
