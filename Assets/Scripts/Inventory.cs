using UnityEngine;
using UnityEngine.UI;

// inventaire (seulement pour les pièces de base)
// TODO script pas encore utilisé
public class Inventory : MonoBehaviour
{
    public int coinsCount;            // nb de pièces courant
    public Text coinsCountText;       // numéro affiché

    public static Inventory instance; // instance de la classe

    // évite les doublons -> classe "statique"
    private void Awake() {
      if(instance != null) {
        Debug.LogWarning("Il y a plus d'une instance d'Inventory dans la scène.");
        return;
      }
      instance = this;
    }


    /* ajoute 'count' pièces à l'inventaire
    * met à jour l'affichage
    */
    public void AddCoins(int count)
    {
      coinsCount += count;
      coinsCountText.text = coinsCount.ToString();
    }


    /* retire 'count' pièces de l'inventaire
    * met à jour l'affichage
    */
    public void RemoveCoins(int count)
    {
      coinsCount -= count;
      coinsCountText.text = coinsCount.ToString();
    }
}
