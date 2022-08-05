 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// gère le compte à rebours avant le combat
public class CountDownTimer : MonoBehaviour
{

    public int countDownTime;       // compte à rebours
    public Text countDownDisplay;   // affichage du compte à rebours
    public GameObject p1;           // joueur 1
    public GameObject p2;           // joueur 2
    public bool start = true;       // vrai tant que le compte à rebours n'est pas fini


    /* affiche "ready ?" à l'écran
    * bloque les joueurs le temps du compte à rebours
    * lance le compte à rebours via 'CountDownToStart'
    */
    private void Start() {
        countDownDisplay.text = "Reeeeeeady ?";

        p1.GetComponent<PlayerMovement>().enabled = false;
        p2.GetComponent<PlayerMovement>().enabled = false;
        p1.GetComponent<PlayerWeapon>().enabled = false;
        p2.GetComponent<PlayerWeapon>().enabled = false;
        p1.GetComponent<PlayerHealth>().enabled = false;
        p2.GetComponent<PlayerHealth>().enabled = false;

        StartCoroutine(CountDownToStart());
    }


    /* lance le compte à rebours
    * temps d'attente dépendant de la scène
    * une fois le compte à rebours fini :
    *   - débloque les joueurs
    *   - désactive le compte à rebours
    */
    IEnumerator CountDownToStart() {
        yield return new  WaitForSeconds(1.0f);

        while(countDownTime <= 4) {
            countDownDisplay.text = countDownTime.ToString();
            if(SceneManager.GetActiveScene().name == "SimpleSceneLalaland") {
                yield return new  WaitForSeconds(0.968f);
            }else if(SceneManager.GetActiveScene().name == "SimpleSceneOmen") {
                yield return new  WaitForSeconds(1.413f);
            }else if(SceneManager.GetActiveScene().name == "SimpleSceneSwing") {
                yield return new  WaitForSeconds(0.3f);
            }
            countDownTime++;
        }

        start = false;
        countDownDisplay.text = "Pouet!";
        yield return new WaitForSeconds(0.90f);

        p1.GetComponent<PlayerMovement>().enabled = true;
        p2.GetComponent<PlayerMovement>().enabled = true;
        p1.GetComponent<PlayerWeapon>().enabled = true;
        p2.GetComponent<PlayerWeapon>().enabled = true;
        p1.GetComponent<PlayerHealth>().enabled = true;
        p2.GetComponent<PlayerHealth>().enabled = true;

        countDownDisplay.gameObject.SetActive(false);
    }
}
