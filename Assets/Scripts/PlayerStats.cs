using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// stats des joueurs, de leurs items et de leurs armes
// TODO classe mère de PlayerStats, ItemStats et WeaponStats (Stats ?)
// -> éviter les multiples définitions de nbItem, weapon, equip et stats
public class PlayerStats : MonoBehaviour
{
	// nombre de stats (commun aux items, armes et équipements)
    private static int nbStats = 3;

    // nombre total d'armes dans le jeu
    private static int nbEquip = 3;

    // tableau contenant les différentes stats du player
    // TODO décrire la norme : ce que contient chaque indice du tab
    public float[] stats;

    // tableau contenant les différentes stats de l'item du player
    // l'index est le même que pour les stats propres au player
    // les stats totales du players sont égales à player stats + item stats
    private float[] itemStats;

    // arme du player (peut n'en avoir aucune)
    // les buffs de l'arme sont décrits dans la classe correspondante (TODO)
    private GameObject weapon;

    // stats de l'arme du player
    private float[] weaponStats;

    // liste des équipements montés
    private string[] equipList;

    // conteur du nombre d'équipements montés
    private int equipCount;

    // stats cumulées de tous les équipements montés
    private float[] equipStats;


    void Awake() {
      // TODO init tous les tabs de stats (à faire à chaque début de scène ?? -> opti)
      itemStats = new float[nbStats];
      weaponStats = new float[nbStats];
      equipStats = new float[nbStats];
      equipList = new string[nbEquip];
      equipCount = 0;
    }


    /* Équipe l'item sur le player, ce qui modifie ses stats totales
    * si le player était déjà équipé d'un item, le remplace (pas forcé, à discuter)
    * les stats de l'item sont stockées dans la classe 'ItemStats'
    */
    public void setItem(GameObject i) {
      itemStats = ItemStats.instance.getStats(i.name);
      // TODO (lancer une anim) + changer l'apparence du player en fonction de l'item
    }


    /* Équipe l'équipement sur l'arme du player, ce qui modifie ses stats totales
    * si le player était déjà équipé du même équipement, ne modifie rien
    * les stats de l'équipement sont stockées dans la classe 'WeaponStats'
    * pour l'instant, les équipements apportent les même bonus,
    * quelle que soit l'arme sur laquelle on l'équipe (peut changer pour avoir des armes plus spécialisées)
    */
    public void equip(GameObject e) {
      foreach(string s in equipList) {
        if (s.Equals(e.name)) {
          // l'équipement est déjà monté
          return;
        }
      }
      equipList[equipCount ++] = e.name;

      float[] tab = WeaponStats.instance.getEquipStats(e.name);
      for(int i = 0; i < nbStats; ++i) {
        equipStats[i] += tab[i];
      }
    }


    /* Fonction déclenchée par la récupération d'un item dans la scène
    * le paramètre 'other' correspondau collider de l'item collecté
    void OnTriggerEnter2D(Collider2D other) {
        // appel au CurrentSceneManager pour tenir le compte du nombre de PowerUp dans la scène
        CurrentSceneManager.instance.CollectedItem();

        // appel au buffer pour équiper le buff/l'objet sur le player
        if (other.transform.CompareTag("Item")) {
          setItem(other.gameObject);
        }else if (other.transform.CompareTag("Equipement")) {
          equip(other.gameObject);
        }

        Destroy(other.gameObject);
    }
    */
}
