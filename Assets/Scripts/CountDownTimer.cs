 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/* Handles countdown timer before combat
* TODO : modify the time so its look like "1, 2, 1234" at tempo with soundtrack
*/
public class CountDownTimer : MonoBehaviour
{
    public int countDownTime;       // nb of iterations during countdown
    public Text countDownDisplay;   // countdown display
    public GameObject p1;           // Player1 GameObject
    public GameObject p2;           // Player2 GameObject
    public bool start = true;       // true as long as countdown aint finished
    private bool devMode = true;   // skip countdown if true


    /* display "ready ?" on screen
    * disable players movements during countdown
    * initiate countdown calling 'CountDownToStart'
    */
    private void Start() {
        countDownDisplay.text = "Reeeeeeady ?";

        // disable players movements
        p1.GetComponent<PlayerMovement>().enabled = false;
        p2.GetComponent<PlayerMovement>().enabled = false;
        p1.GetComponent<PlayerWeapon>().enabled = false;
        p2.GetComponent<PlayerWeapon>().enabled = false;
        p1.GetComponent<PlayerHealth>().enabled = false;
        p2.GetComponent<PlayerHealth>().enabled = false;

        // initiate countdown
        StartCoroutine(CountDownToStart());
    }


    /* handle countdown depending on the current scene
    * when countdown finished :
    *   - enable players movements
    *   - disable countdown display
    */
    IEnumerator CountDownToStart() {
        if (!devMode) { // skip countdown if true
            yield return new  WaitForSeconds(1.0f);

            while(countDownTime <= 4) { // display countdown
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
            countDownDisplay.text = "Pouet!";
            yield return new WaitForSeconds(0.90f);
        }
        yield return new WaitForSeconds(0f);    // match function return type when in devMode
        start = false;                          // not sure why its needed

        // enable players movements
        p1.GetComponent<PlayerMovement>().enabled = true;
        p2.GetComponent<PlayerMovement>().enabled = true;
        p1.GetComponent<PlayerWeapon>().enabled = true;
        p2.GetComponent<PlayerWeapon>().enabled = true;
        p1.GetComponent<PlayerHealth>().enabled = true;
        p2.GetComponent<PlayerHealth>().enabled = true;

        // disable countdown display
        countDownDisplay.gameObject.SetActive(false);
    }
}
