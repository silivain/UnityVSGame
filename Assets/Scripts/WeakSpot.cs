using UnityEngine;

// point faible des mobs
// zone qui les tue si le joueur entre en contact
// TODO script pas encore utilisé
public class WeakSpot : MonoBehaviour	//video 4
{
    public GameObject objectToDestroy;	// mob dont le collider est l'enfant


	/* détruit le mob
	*/
    private void OnTriggerEnter2D(Collider2D collision) {
      if(collision.CompareTag("Player")) {
        Destroy(objectToDestroy);
      }
    }
}
