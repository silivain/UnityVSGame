using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// gère les items dans la scène
public class CurrentSceneManager : MonoBehaviour
{
  public bool isPlayerPresentByDefault = false;
  public int coinsPickedUpInThisSceneCount;

  // item présents dans la scène
  public int maxItem;                  // nb max d'items présents en même temps dans la scène

  private int currentItem;             // nb courant d'items présents dans la scène

  public GameObject [] newItem;        // tableau contenant les différents items pouvant apparaître sur la scène
                                       // TODO à fusionner avec itemNames ?

  public Vector3 [] itemSpawnPosition; // emplacements dans la scène où faire apparaître les items

  public int itemSpawnTime;            // temps d'attente une fois un item collecté, avant l'apparition du prochain


  public static CurrentSceneManager instance; // instance de la classe

  // évite les doublons -> classe "statique"
  private void Awake()
  {
    if (instance != null)
    {
      Debug.LogWarning("Il y a plus d'une instance de CurrentSceneManager dans la scène.");
      return;
    }
    instance = this;

    // nombre maximum d'items dans la scène (pas encore utilisé)
    currentItem = maxItem;
  }

  /* Lance le mécanisme d'apparition d'items
  * StartCoroutine permet de paralléliser le processus
  * on peut donc détruire le gameObject appelant (car Coroutine started depuis class statique)
  */
  public void CollectedItem() {
    if (-- currentItem == 0) {
      // plus d'items dans la scène :
      // on en génère un nouveau après avoir attendu
      StartCoroutine(SpawnAfterTime());
    }
  }

  /* génère un nouvel item aléatoire
  * l'item est généré à un emplacement aléatoire parmi ceux définis dans la scène
  */
  private GameObject InstantiateItem() {
    GameObject nb = (GameObject) Instantiate(newItem[Random.Range(0, newItem.Length)],
      itemSpawnPosition[Random.Range(0, itemSpawnPosition.Length)], new Quaternion(0, 0, 0, 1));
    BoxCollider2D m_collider = nb.GetComponent<BoxCollider2D>();

    // le collider était désactivé lors de mes test (Brett)
    // réactivation manuelle pour que l'item soit collectable
    m_collider.enabled = true;
    ++ currentItem;
    return nb;
  }

  /* Attend 'itemSpawnTime' secondes avant de générer l'item suivant
  */
  private IEnumerator SpawnAfterTime()
  {
    yield return new WaitForSeconds(CurrentSceneManager.instance.itemSpawnTime);

    InstantiateItem();
  }
}
