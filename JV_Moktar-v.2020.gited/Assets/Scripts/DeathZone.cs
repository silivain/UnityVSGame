﻿using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Player")) {
          PlayerLives.instance.Fall(collision.transform);
        }
    }
}
