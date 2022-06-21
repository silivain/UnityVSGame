using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTrombone : MonoBehaviour
{
    public int Damage = 10;                         // Dégats par défaut
    public float FallBackForce = 50;                // Force repoussant le joueur par défaut
    public BoxCollider2D tromboneHitbox;            // collider servant de hitbox
    public Transform CoulisseRangePoint;            // Range de la coulisse
    public LayerMask playerLayers;                  // Layer des joueurs

    /*private bool Extended = false;                  // Vrai si la coulisse est tendue*/

    void Start() {
        HittingLoop();
    }

    IEnumerator HittingLoop() {
        while(true) {
            yield return new WaitForSeconds(5f);

            StartCoroutine(CoulisseAnim());

            Collider2D[] coulisseHitbox = Physics2D.OverlapAreaAll(transform.position, CoulisseRangePoint.position, playerLayers);

            foreach(Collider2D enemy in coulisseHitbox) {
                Debug.Log("we hit " + enemy.name);
                PlayerHealth playerHealth = enemy.transform.GetComponent<PlayerHealth>();
                playerHealth.TakeDamage(Damage);
                // TODO appel à la fonction de recul en passant les arguments nécessaires
                // le collider 'other', le rigidbody du go bullet (pour pouvoir recup sa velocity)
                PlayerMovement.instance.RecoilCac(enemy, transform);
            }
    	}
    }

    IEnumerator CoulisseAnim()
    {
        //TODO
        yield return new WaitForSeconds(0.2f);
    }

    // Gestion de la collision avec un GO
    void OnTriggerEnter2D(Collider2D other) {

        // Si le GO n'est pas un joueur, ne rien faire
        if(other.transform.tag.Substring(0, 4) != "Play") {
            return;
        }

        // Application des dégats au joueur
        PlayerHealth playerHealth = other.transform.GetComponent<PlayerHealth>();
        playerHealth.TakeDamage(Damage);

        // FallbackForce
        Vector3 FoePos = other.transform.position;
        Vector3 FallBackDirection = FoePos - transform.position;
        other.attachedRigidbody.AddForce(FallBackForce * FallBackDirection.normalized, ForceMode2D.Impulse);
    }
}
