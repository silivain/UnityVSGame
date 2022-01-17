using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawning : MonoBehaviour
{

    public static ItemSpawning instance;

    private void Awake()
    {
      if (instance != null)
      {
        Debug.LogWarning("Il y a plus d'une instance de ItemSpawning dans la scène.");
        return;
      }
      instance = this;
    }

    public IEnumerator SpawnAfterTime()
    {
      // attend 'PowerUpSpawnTime' secondes avant de générer le PowerUp suivant
      yield return new WaitForSeconds(CurrentSceneManager.instance.PowerUpSpawnTime);

      // génère un nouveau PowerUp aléatoire
      // généré à un emplacement aléatoire parmi ceux définis dans la scène
      CurrentSceneManager.instance.InstantiatePowerUp();
    }

    void OnTriggerEnter2D(Collider2D other) {
        // appel au CurrentSceneManager pour tenir le compte du nombre de PowerUp dans la scène
        CurrentSceneManager.instance.CollectedPowerUp();
        Destroy(gameObject);
    }
}
