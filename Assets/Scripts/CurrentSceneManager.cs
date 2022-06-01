using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// gère les items dans la scène
public class CurrentSceneManager : MonoBehaviour
{
  public bool isPlayerPresentByDefault = false;
  public int coinsPickedUpInThisSceneCount;

  // item et heals présents dans la scène
  public int maxItem;                  // nb max d'items présents en même temps dans la scène
  public int maxHeal;				   // nb max de heals présents en même temps dans la scène

  private int currentItem = 2;             // nb courant d'items présents dans la scène
  private int currentHeal = 2;             // nb courant de heals présents dans la scène

  public GameObject [] newItem;        // tableau contenant les différents items pouvant apparaître sur la scène
                                       // TODO à fusionner avec itemNames ?
  public GameObject [] newHeal;        // tableau contenant les différents heals pouvant apparaître sur la scène

  public GameObject [] itemSpawnPosition; // emplacements dans la scène où faire apparaître les items
  public GameObject [] healSpawnPosition; // emplacements dans la scène où faire apparaître les heals

  public int itemSpawnTime;            // temps d'attente une fois un item collecté, avant l'apparition du prochain
  public int healSpawnTime;            // temps d'attente une fois un heal collecté, avant l'apparition du prochain


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

    // nombre maximum d'items et de heals dans la scène
    currentItem = maxItem;
	currentHeal = maxHeal;
  }


  /* Lance le mécanisme d'apparition d'items
  * StartCoroutine permet de paralléliser le processus
  * on peut donc détruire le gameObject appelant (car Coroutine started depuis class statique)
  */
  public void CollectedItem() {
	if (currentItem == 1) {
      // plus d'items dans la scène :
      // on en génère un nouveau après avoir attendu
	  currentItem--;
      StartCoroutine(SpawnItemAfterTime());
    }
  }

  /* Lance le mécanisme d'apparition de heals
  * StartCoroutine permet de paralléliser le processus
  * on peut donc détruire le gameObject appelant (car Coroutine started depuis class statique)
  */
  public void CollectedHeal() {
	if (currentHeal == 1) {
      // plus d'items dans la scène :
      // on en génère un nouveau après avoir attendu
	  currentHeal--;
      StartCoroutine(SpawnHealAfterTime());
    }
  }

  /* génère un nouvel item aléatoire
  * l'item est généré à un emplacement aléatoire parmi ceux définis dans la scène
  */
  private void InstantiateItem() {
  	currentItem++;
    GameObject nb = (GameObject) Instantiate(newItem[Random.Range(0, newItem.Length)],
      itemSpawnPosition[Random.Range(0, itemSpawnPosition.Length)].transform.position, new Quaternion(0, 0, 0, 1));
    BoxCollider2D m_collider = nb.GetComponent<BoxCollider2D>();

    // le collider était désactivé lors de mes test (Brett)
    // réactivation manuelle pour que l'item soit collectable
    m_collider.enabled = true;
  }

  /* génère un nouveau heal aléatoire
  * le heal est généré à un emplacement aléatoire parmi ceux définis dans la scène
  */
  private void InstantiateHeal() {
  	currentHeal++;
    GameObject nb = (GameObject) Instantiate(newHeal[Random.Range(0, newHeal.Length)],
      healSpawnPosition[Random.Range(0, healSpawnPosition.Length)].transform.position, new Quaternion(0, 0, 0, 1));
	CircleCollider2D m_collider = nb.GetComponent<CircleCollider2D>();

    // le collider était désactivé lors de mes test (Brett)
    // réactivation manuelle pour que l'item soit collectable
    m_collider.enabled = true;
  }

  /* Attend 'itemSpawnTime' secondes avant de générer l'item suivant
  */
  private IEnumerator SpawnItemAfterTime()
  {
    yield return new WaitForSeconds(CurrentSceneManager.instance.itemSpawnTime);
    InstantiateItem();
  }

  /* Attend 'healSpawnTime' secondes avant de générer le heal suivant
  */
  private IEnumerator SpawnHealAfterTime()
  {
	yield return new WaitForSeconds(CurrentSceneManager.instance.healSpawnTime);
    InstantiateHeal();
  }
}
