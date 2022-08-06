using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// supprime le visuel de l'explosion une fois l'animation terminée
public class ExplosionDestroy : MonoBehaviour
{
    void Start() {
        Destroy(gameObject, .35f);
    }
}
