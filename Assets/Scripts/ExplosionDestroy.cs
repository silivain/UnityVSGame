using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDestroy : MonoBehaviour
{
    // TODO : à mettre ds un autre script mdr
    void FixedUpdate()
    {
        Destroy(gameObject, .35f);
    }
}
