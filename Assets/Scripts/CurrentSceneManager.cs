using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// gère les collectables dans la scène dans la scène
public class CurrentSceneManager : MonoBehaviour
{
  public bool isPlayerPresentByDefault = false;
  public int coinsPickedUpInThisSceneCount;

  // armes, heals et bonus présents dans la scène
  public int maxWeapon;                  // nb max d'armes présentes en même temps dans la scène
  public int maxHeal;				     // nb max de heals présents en même temps dans la scène
  public int maxBonus;				     // nb max de bonus présents en même temps dans la scène

  private int currentWeapon;            // nb courant d'armes présentes dans la scène
  private int currentHeal;              // nb courant de heals présents dans la scène
  private int currentBonus;             // nb courant de bonus présents dans la scène

  public GameObject [] newWeapon;       // tableau contenant les différentes armes pouvant apparaître sur la scène
  public GameObject [] newHeal;         // tableau contenant les différents heals pouvant apparaître sur la scène
  public GameObject [] newBonus;        // tableau contenant les différents bonus pouvant apparaître sur la scène

  public GameObject [] weaponSpawnPosition; // emplacements dans la scène où faire apparaître les armes
  public GameObject [] healSpawnPosition;   // emplacements dans la scène où faire apparaître les heals
  public GameObject [] bonusSpawnPosition;  // emplacements dans la scène où faire apparaître les bonus

  private bool [] occupiedWeaponSpawn;      // emplacements d'armes déjà occupés par un collectable
  private bool [] occupiedHealSpawn;        // emplacements de heal déjà occupés par un collectable
  private bool [] occupiedBonusSpawn;       // emplacements de bonus déjà occupés par un collectable

  public int weaponSpawnTime;           // cooldown avant respawn de la prochaine arme
  public int healSpawnTime;             // cooldown avant respawn du prochain heal
  public int bonusSpawnTime;            // cooldown avant respawn du prochain bonus

  private bool spawningWeapon = false;          // vrai si une arme est en train d'être générée
  private bool spawningHeal = false;            // vrai si un heal est en train d'être généré
  private bool spawningBonus = false;           // vrai si un bonus est en train d'être généré


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

    // on fait spawn une clari sur le spawnpos1
    GameObject nb = (GameObject) Instantiate(newWeapon[0],
      weaponSpawnPosition[0].transform.position, new Quaternion(0, 0, 0, 1));
    CircleCollider2D m_collider = nb.GetComponent<CircleCollider2D>();

    // init des tableaux d'occupation des spawns à false
    occupiedWeaponSpawn = new bool[weaponSpawnPosition.Length];
    for(int i = 0; i < occupiedWeaponSpawn.Length; ++i) {
        occupiedWeaponSpawn[i] = false;
    }
    occupiedWeaponSpawn[0] = true;  // on vient de spawn la clari juste au-dessus
    occupiedHealSpawn = new bool[healSpawnPosition.Length];
    for(int i = 0; i < occupiedHealSpawn.Length; ++i) {
        occupiedHealSpawn[i] = false;
    }
    occupiedBonusSpawn = new bool[bonusSpawnPosition.Length];
    for(int i = 0; i < occupiedBonusSpawn.Length; ++i) {
        occupiedBonusSpawn[i] = false;
    }

    // nombre actuel de collectables dans la scène
    currentWeapon = GameObject.FindGameObjectsWithTag("Weapon").Length;
	currentHeal = GameObject.FindGameObjectsWithTag("Heal").Length;
    currentBonus = GameObject.FindGameObjectsWithTag("AmmunitionBonus").Length
        + GameObject.FindGameObjectsWithTag("DamageBonus").Length
        + GameObject.FindGameObjectsWithTag("SpeedBonus").Length;
  }


  // Gère le spawn des collectables
  private void FixedUpdate() {
      if (currentWeapon < maxWeapon && !spawningWeapon) {
          spawningWeapon = true;
          StartCoroutine(SpawnWeaponAfterTime());
      }
      if (currentHeal < maxHeal && !spawningHeal) {
          spawningHeal = true;
          StartCoroutine(SpawnHealAfterTime());
      }
      if (currentBonus < maxBonus && !spawningBonus) {
          spawningBonus = true;
          StartCoroutine(SpawnBonusAfterTime());
      }
  }


  /* maj du nombre d'armes dans la scène
  * maj des emplacements dispos
  */
  public void CollectedWeapon(Vector3 pos) {
    for(int j = 0; j < weaponSpawnPosition.Length; ++j) {
        if (weaponSpawnPosition[j].transform.position == pos) {
            occupiedWeaponSpawn[j] = false;
            break;
        }
    }
    currentWeapon--;
  }


  /* maj du nombre de heal dans la scène
  * maj des emplacements dispos
  */
  public void CollectedHeal(Vector3 pos) {
      for(int j = 0; j < healSpawnPosition.Length; ++j) {
          if (healSpawnPosition[j].transform.position == pos) {
              occupiedHealSpawn[j] = false;
              break;
          }
      }
      currentHeal--;
  }


  /* maj du nombre de bonus dans la scène
  * maj des emplacements dispos
  */
  public void CollectedBonus(Vector3 pos) {
      for(int j = 0; j < bonusSpawnPosition.Length; ++j) {
          if (bonusSpawnPosition[j].transform.position == pos) {
              occupiedBonusSpawn[j] = false;
              break;
          }
      }
      currentBonus--;
  }


  /* génère une nouvelle arme aléatoire
  * l'arme est générée à un emplacement aléatoire parmi ceux définis dans la scène
  */
  private void InstantiateWeapon() {
  	currentWeapon++;

    // on s'assure de générer une arme à un emplacement libre
    int pos = Random.Range(0, weaponSpawnPosition.Length);
    while(occupiedWeaponSpawn[pos] && !isFullyOccupied(occupiedWeaponSpawn)) {
        pos = Random.Range(0, weaponSpawnPosition.Length);
    }
    occupiedWeaponSpawn[pos] = true;

    GameObject nb = (GameObject) Instantiate(newWeapon[Random.Range(0, newWeapon.Length)],
      weaponSpawnPosition[pos].transform.position, new Quaternion(0, 0, 0, 1));
    CircleCollider2D m_collider = nb.GetComponent<CircleCollider2D>();

    // le collider était désactivé lors de mes test (Brett)
    // réactivation manuelle pour que l'arme soit collectable
    m_collider.enabled = true;
    spawningWeapon = false;
  }


  /* génère un nouveau heal aléatoire
  * le heal est généré à un emplacement aléatoire parmi ceux définis dans la scène
  */
  private void InstantiateHeal() {
  	currentHeal++;

    // on s'assure de générer un heal à un emplacement libre
    int pos = Random.Range(0, healSpawnPosition.Length);
    while(occupiedHealSpawn[pos] && !isFullyOccupied(occupiedHealSpawn)) {
        pos = Random.Range(0, healSpawnPosition.Length);
    }
    occupiedHealSpawn[pos] = true;

    GameObject nb = (GameObject) Instantiate(newHeal[Random.Range(0, newHeal.Length)],
      healSpawnPosition[pos].transform.position, new Quaternion(0, 0, 0, 1));
	CircleCollider2D m_collider = nb.GetComponent<CircleCollider2D>();

    // le collider était désactivé lors de mes test (Brett)
    // réactivation manuelle pour que l'item soit collectable
    m_collider.enabled = true;
    spawningHeal = false;
  }


  /* génère un nouveau bonus aléatoire
  * le bonus est généré à un emplacement aléatoire parmi ceux définis dans la scène
  */
  private void InstantiateBonus() {
  	currentBonus++;

    // on s'assure de générer un bonus à un emplacement libre
    int pos = Random.Range(0, bonusSpawnPosition.Length);
    while(occupiedBonusSpawn[pos] && !isFullyOccupied(occupiedBonusSpawn)) {
        pos = Random.Range(0, bonusSpawnPosition.Length);
    }
    occupiedBonusSpawn[pos] = true;

    GameObject nb = (GameObject) Instantiate(newBonus[Random.Range(0, newBonus.Length)],
      bonusSpawnPosition[pos].transform.position, new Quaternion(0, 0, 0, 1));
	CircleCollider2D m_collider = nb.GetComponent<CircleCollider2D>();

    // le collider était désactivé lors de mes test (Brett)
    // réactivation manuelle pour que le bonus soit collectable
    m_collider.enabled = true;
    spawningBonus = false;
  }


  /* retourne vrai si toutes les valeurs de 'tab' sont vraies
  */
  private bool isFullyOccupied(bool[] tab) {
      foreach(bool b in tab) {
          if (!b) {
              return false;
          }
      }
      return true;
  }


  /* Attend 'weaponSpawnTime' secondes avant de générer l'arme suivante
  */
  private IEnumerator SpawnWeaponAfterTime()
  {
    yield return new WaitForSeconds(CurrentSceneManager.instance.weaponSpawnTime);
    InstantiateWeapon();
  }


  /* Attend 'healSpawnTime' secondes avant de générer le heal suivant
  */
  private IEnumerator SpawnHealAfterTime()
  {
	yield return new WaitForSeconds(CurrentSceneManager.instance.healSpawnTime);
    InstantiateHeal();
  }


  /* Attend 'bonusSpawnTime' secondes avant de générer le bonus suivant
  */
  private IEnumerator SpawnBonusAfterTime()
  {
	yield return new WaitForSeconds(CurrentSceneManager.instance.bonusSpawnTime);
    InstantiateBonus();
  }
}
