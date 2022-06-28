using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* Inflige des dégats et repousse le joueur entrant dans cette zone
* faire des stats variables selon les traps ? TODO
*/
public class TrapDamage : MonoBehaviour
{

    public int Damage = 10;                        // Dégats par défaut
    public float FallBackForce = 50;               // Force repoussant le joueur par défaut

    IEnumerator trapReistanceDelay(PlayerHealth playerHealth) {
        yield return new WaitForSeconds(2f);
        playerHealth.trapResistance = false;
    }

    // Gestion de la collision avec un GO
    void OnTriggerEnter2D(Collider2D other) {

        /* Si le GO n'est pas un joueur
        * ou si le joueur est invulnérable
        * ne rien faire
        */
        if(other.transform.tag.Substring(0, 4) != "Play") {
            return;
        }

        // Application des dégats au joueur
        PlayerHealth playerHealth = other.transform.GetComponent<PlayerHealth>();

        if (!playerHealth.trapResistance) {

            /* Applique les dégats
            * rend le joueur invulnérable pdnt 2 sec
            */
            playerHealth.TakeDamage(Damage);
            playerHealth.trapResistance = true;
            StartCoroutine(trapReistanceDelay(playerHealth));

            /* FallackForce
            * Vecteur calculé selon la diff entre position du joueur et position du piège
            */
            Vector3 FallBackDirection = other.transform.position - transform.position;
            other.attachedRigidbody.AddForce(FallBackForce * FallBackDirection.normalized, ForceMode2D.Impulse);
        }
    }


    /* Si un joueur reste au contact du piège après l'invulnérabilité
    * rappeler les dégats pour ce ptit malin
    */
    void OnTriggerStay2D(Collider2D other) {
        OnTriggerEnter2D(other);
    }
}
