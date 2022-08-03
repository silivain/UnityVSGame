using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


/* classe gérant la détection et attribution des inputs
*/
public class InputTools : MonoBehaviour
{
    public static InputTools instance;     // instance statique de la classe


    // évite les doublons -> classe "statique"
    private void Awake()
    {
      if (instance != null)
      {
        Debug.LogWarning("Il y a plus d'une instance de InputTools dans la scène.");
        return;
      }
      instance = this;
    }


    /* renvoie le tableau des devices propres au joueur
    */
    public static InputDevice[] inputSelect(string _tag) {
        InputDevice[] gamepadTab = new InputDevice[2];			        // tableau qui stocke les manettes connectées
        List<InputDevice> playerDevices = new List<InputDevice>();      // tableau contenant toutes les inputs du joueur
		int i = 0;
		int deviceNumber = _tag == "Player 1" ? 0 : 1;                  // indice de la manette utilisée selon le joueur

		/* on récupère toutes les inputs qui ne sont pas des manettes
        * on les ajoute dans 'playerDevices'
        * on récupère aussi les 2 premières manettes connectées
		* on vérifie le type de l'input puis on l'ajoute au tab 'gamepadTab'
		*/
		foreach(InputDevice input in InputSystem.devices) {
            if (input.description.deviceClass != "Gamepad") {
                playerDevices.Add(input);
            }
			if (input.description.deviceClass == "Gamepad" && i < gamepadTab.Length) {
				gamepadTab[i++] = input;
			}
		}

        if (gamepadTab[deviceNumber] != null) {
            playerDevices.Add(gamepadTab[deviceNumber]);
        }
        return playerDevices.ToArray();
    }
}
