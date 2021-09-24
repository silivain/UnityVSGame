using UnityEngine;

public class PickUpInstrument : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
          //affecter l'instrument au joueur
          Destroy(gameObject);
        }
    }
}
