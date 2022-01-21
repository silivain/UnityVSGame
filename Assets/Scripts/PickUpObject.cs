using UnityEngine;

// ramassage d'objets (seulement les pièces so far)
// TODO : script pas encore utilisé
public class PickUpObject : MonoBehaviour
{
	/* ramasse la pièce et incrémente la variable dans l'inventaire
	*/
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
          Inventory.instance.AddCoins(1);
          CurrentSceneManager.instance.coinsPickedUpInThisSceneCount ++;
          Destroy(gameObject);
        }
    }
}
