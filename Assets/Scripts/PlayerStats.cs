using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // tableau contenant les différentes stats du player
    // TODO décrire la norme : ce que contient chaque indice du tab
    public float[] stats;

    // tableau contenant les différentes stats de l'item du player
    // l'index est le même que pour les stats propres au player
    // les stats totales du players sont égales à player stats + item stats
    public float[] itemStats;

    // arme du player (peut n'en avoir aucune)
    // les buffs de l'arme sont décrits dans la classe correspondante (TODO)
    public GameObject weapon;



    /* Équipe l'item sur le player, ce qui modifie ses stats totales
    * si le player était déjà équipé d'un item, le remplace (pas forcé, à discuter)
    * les stats de l'item sont stockées dans la classe 'ItemStats'
    */
    public void setItem(GameObject i) {
      itemStats = ItemStats.instance.getStats(i.name);
      // TODO (lancer une anim) + changer l'apparence du player en fonction de l'item
    }

    /* Équipe l'arme sur le player, ce qui modifie ses stats totales
    * si le player était déjà équipé d'une arme, la remplace
    * les stats de l'arme sont stockées dans la classe 'WeaponStats'
    */
    public void setWeapon(GameObject w)
    {
      weapon = w;
      // TODO implémenter le mécanisme de modif de stats en fct de l'arme équipée
      // TODO (lancer une anim) + changer l'apparence du player en fonction de l'item
    }

    /* Fonction déclenchée par la récupération d'un item dans la scène
    * le paramètre 'other' correspondau collider de l'item collecté
    */
    void OnTriggerEnter2D(Collider2D other) {
        // appel au CurrentSceneManager pour tenir le compte du nombre de PowerUp dans la scène
        CurrentSceneManager.instance.CollectedItem();

        // appel au buffer pour équiper le buff/l'objet sur le player
        if (other.transform.CompareTag("Item")) {
          setItem(other.gameObject);
        }else if (other.transform.CompareTag("Weapon")) {
          setWeapon(other.gameObject);
        }

        Destroy(other.gameObject);
    }
}
