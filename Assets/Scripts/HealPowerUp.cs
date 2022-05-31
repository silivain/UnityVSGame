using UnityEngine;

// mécanisme d'activation du heal lorsque le joueur ramasse un item approprié
// TODO script pas encore utilisé
public class HealPowerUp : MonoBehaviour  //video 14
{
    public int healthPoints;  // valeur du heal


    /* appelle la méthode de heal avec la valeur 'healthPoints'
    * lorsque le joueur entre en contact avec un item approprié
    * détruit l'item après l'appel
    */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player 1") || collision.CompareTag("Player 2"))
        {
			PlayerHealth playerHealth = collision.transform.GetComponent<PlayerHealth>();
        	playerHealth.HealPlayer(healthPoints);
        	Destroy(gameObject);
        }
    }
}
