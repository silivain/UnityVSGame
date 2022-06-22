using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* Inflige des dégats et repousse le joueur entrant dans cette zone
* faire des stats variables selon les traps ? TODO
*/
public class TrapDamage : MonoBehaviour
{

    public int Damage = 15;                        // Dégats par défaut
    public float FallBackForce = 50;               // Force repoussant le joueur par défaut

    // Gestion de la collision avec un GO
    void OnTriggerEnter2D(Collider2D other) {

        // Si le GO n'est pas un joueur, ne rien faire
        if(other.transform.tag.Substring(0, 4) != "Play") {
            return;
        }

        // Application des dégats au joueur
        PlayerHealth playerHealth = other.transform.GetComponent<PlayerHealth>();
        playerHealth.TakeDamage(Damage);

        /* FallackForce
        * Vecteur calculé selon la diff entre position du joueur et position du piège
        */
        Vector3 FallBackDirection = other.transform.position - transform.position;
        other.attachedRigidbody.AddForce(FallBackForce * FallBackDirection.normalized, ForceMode2D.Impulse);
    }
}
