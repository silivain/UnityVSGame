using UnityEngine;
using UnityEngine.UI;

// mécanisme de barre de vie
public class HealthBar : MonoBehaviour
{
    public Slider slider;       // slider (item coulissant)
    public Gradient gradient;   // gradient, à check
    public Image fill;          // image de fond


    /* remplit la barre de vie de manière à ce que vie max = 'health'
    * colorise la barre de manière appropriée
    * (mécanisme de couleurs en fonction du % de vie)
    */
    public void SetMaxHealth(int health) {
      slider.maxValue = health;
      slider.value = health;
      fill.color = gradient.Evaluate(1f);
    }

    /* fixe la barre de vie à 'health'
    * colorise la barre de manière appropriée
    */
    public void SetHealth(int health) {
      slider.value = health;
      fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
