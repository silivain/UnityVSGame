using UnityEngine;

public class CurrentSceneManager : MonoBehaviour
{
  public bool isPlayerPresentByDefault = false;
  public int coinsPickedUpInThisSceneCount;

  public int maxPowerUp;
  private int currentPowerUp;
  public GameObject [] newPowerUp;
  public Vector3 [] powerUpSpawnPosition;
  public int PowerUpSpawnTime;

  public static CurrentSceneManager instance;

  private void Awake()
  {
    if (instance != null)
    {
      Debug.LogWarning("Il y a plus d'une instance de CurrentSceneManager dans la scène.");
      return;
    }
    instance = this;

    // nombre maximum de PowerUp dans la scène
    // pas encore utilisé
    currentPowerUp = maxPowerUp;
  }

  public void CollectedPowerUp() {
    if (-- currentPowerUp == 0) {
      // plus de PowerUp dans la scène :
      // on en génère un nouveau après avoir attendu
      StartCoroutine(ItemSpawning.instance.SpawnAfterTime());
    }
  }

  // génère un nouveau PowerUp aléatoire
  // généré à un emplacement aléatoire parmi ceux définis dans la scène
  public GameObject InstantiatePowerUp() {
    GameObject nb = (GameObject) Instantiate(newPowerUp[Random.Range(0, newPowerUp.Length)], powerUpSpawnPosition[Random.Range(0, powerUpSpawnPosition.Length)], new Quaternion(0, 0, 0, 1));
    BoxCollider2D m_collider = nb.GetComponent<BoxCollider2D>();

    // le collider était désactivé lors de mes test (Brett)
    // réactivation manuelle pour que l'item soit collectable
    m_collider.enabled = true;
    ++ currentPowerUp;
    return nb;
  }
}
