using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// répertorie les stats des différents items
public class ItemStats : MonoBehaviour
{
  // nombre total d'item dans le jeu
  private static int nbItem = 5;

  // nombre de stats (commun aux items, armes et équipements)
  private static int nbStats = 3;

  /* tableau contenant le nom de tous les items
    l'index du nom de l'item correspond à l'index des stats de cet item dans 'itemStats' */
  private string[] itemNames = new string[nbItem];

  // tableau contenant les stats de tous les items
  private float[][] itemStats = new float[nbItem][];

  public static ItemStats instance; // instance de la classe

  // évite les doublons -> classe "statique"
  private void Awake()
  {
    if (instance != null)
    {
      Debug.LogWarning("Il y a plus d'une instance de ItemStats dans la scène.");
      return;
    }
    instance = this;

    // TODO set les stats de tous les items (à faire à chaque début de scène ?? -> opti)
    for(int i = 0; i < nbItem; ++i) {
      int ip1 = i + 1;
      float[] tab = {0, 0, 0};
      itemNames[i] = ("item" + ip1.ToString());
      itemStats[i] = new float[nbStats];
      itemStats[i] = tab;
    }
  }


  /* Renvoie les stats correspondant à l'item 's'
  * Si aucun item ne correspond à la string passée en argument :
  * debug et return null <- TODO perror au lieu de debug
  */
  public float[] getStats(string s) {
    for(int i = 0; i < nbItem; ++i) {
      if (s.Equals(itemNames[i])) {
        return itemStats[i];
      }
    }
    Debug.Log("item " + s + " doesnt exist");
    return null;
  }
}
