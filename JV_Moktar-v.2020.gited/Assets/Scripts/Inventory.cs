using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public int coinsCount;
    public Text coinsCountText;

    public static Inventory instance;

    private void Awake() {  //mécanisme de singleton : garantit qu'il n'y ait qu'une seule instance de Inventory
                            //permet aussi d'appeler ce script de puis n'importe quel autre script sans utiliser de ref
      if(instance != null) {
        Debug.LogWarning("Il y a plus d'une instance d'Inventory dans la scène.");
        return;
      }

      instance = this;
    }

    public void AddCoins(int count)
    {
      coinsCount += count;
      coinsCountText.text = coinsCount.ToString();
    }

    public void RemoveCoins(int count)
    {
      coinsCount -= count;
      coinsCountText.text = coinsCount.ToString();
    }
}
