//video 4

﻿using UnityEngine;

public class WeakSpot : MonoBehaviour
{
    public GameObject objectToDestroy;

    private void OnTriggerEnter2D(Collider2D collision) {
      if(collision.CompareTag("Player")) { //le tag se trouve en haut à droite, sous le nom du gameobject dans l'interface unity (l'objet doit être sélectionné)
        Destroy(objectToDestroy);
      }
    }
}
