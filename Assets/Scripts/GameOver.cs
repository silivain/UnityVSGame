using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/* Écran de GameOver
* affiché quand l'un des deux joueurs atteint 0 pv
* situé dans le canvas de chaque scène
* TODO : permet de retourner au 'MainMenu', d'aller dans les 'Settings' ou de recommencer la même scène
*/
public class GameOver : MonoBehaviour
{   
    public GameObject[] SelectCorners;          // selection displays
    private int selectIndex = 0;                // index of currently selected display
    public PlayerControls controls;             // inputs

    public Text GameOverText;                   // game over text
    public AudioSource currentMusic;            // after combat soundtrack
    public AudioSource gameOverMusic;           // GameOver Menu soundtrack
    private float delay = 1f;                   // delay before enabling inputs on GameOver window -> avoid spamming

    public GameObject[] UIToDisable;            // UI elements to disable when displaying GameOver window
    public GameObject Player1;                  // access to Player1 scripts
    public GameObject Player2;                  // access to Player2 scripts
    public GameObject PauseMenu;                // access to PauseMenu
    public GameObject SettingsMenu;             // SettingsMenu window
    private SettingsMenu SettingsMenuScript;    // SettingsMenu.cs


    // instantiate inputs
    private void Awake() {
        controls = new PlayerControls();    // link with script handling inputs
        controls.UI.Enable();               // listen to 'UI' InputActionMap
        controls.UI.GoUp.performed += ctx => selectUp();
        controls.UI.GoDown.performed += ctx => selectDown();
        controls.UI.Start.performed += ctx => selectSetting();
    }


    /* initialize selectIndex and SelectCorners
    * initialize SettingsMenuScript
    */
    private void Start() {
        SelectCorners[selectIndex].SetActive(false);
        selectIndex = 0;
        SelectCorners[selectIndex].SetActive(true);

        // link to SettingsMenu.cs
        if (!SettingsMenu.TryGetComponent<SettingsMenu>(out SettingsMenuScript)) {
            Debug.Log("failed to pull SettingsMenu.cs in GameOver.cs");
        }
    }


    /* enable GameOver window
    * method called by PlayerHealth.cs
    */
    public void Setup(string _tag){
        Time.timeScale = 0f;        // stop time
        DisableInGameInputs();      // disable ingame inputs

        // disable unnecessary UI elements
        foreach(GameObject go in UIToDisable) {
            go.SetActive(false);
        }

        // disable all active collectables
        List<GameObject> ItemsToDisable = new List<GameObject>();
        ItemsToDisable.AddRange(GameObject.FindGameObjectsWithTag("Weapon"));
        ItemsToDisable.AddRange(GameObject.FindGameObjectsWithTag("Heal"));
        ItemsToDisable.AddRange(GameObject.FindGameObjectsWithTag("AmmunitionBonus"));
        ItemsToDisable.AddRange(GameObject.FindGameObjectsWithTag("DamageBonus"));
        ItemsToDisable.AddRange(GameObject.FindGameObjectsWithTag("SpeedBonus"));

        foreach(GameObject go in ItemsToDisable) {
            go.SetActive(false);
        }
        

        // modify display according to winning player
        if (_tag == "Player 1") {
            GameOverText.text = "Le joueur 2 gagne !";
        }else {
            GameOverText.text = "Le joueur 1 gagne !";
        }

        gameObject.SetActive(true); // enable GameOver window
        currentMusic.Stop();        // stop ingame soundtrack
        gameOverMusic.Play();       // start game over soundtrack
        StartCoroutine(Delay());    // block inputs
    }


    /* Disable in game inputs :
       - PlayerMovement
       - PlayerHealth
       - PlayerWeapon
       - PauseMenu
    */
    private void DisableInGameInputs() {
        Player1.GetComponent<PlayerMovement>().controls.Player1.Disable();
        Player1.GetComponent<PlayerMovement>().controls.Player2.Disable();
        Player2.GetComponent<PlayerMovement>().controls.Player1.Disable();
        Player2.GetComponent<PlayerMovement>().controls.Player2.Disable();
        Player1.GetComponent<PlayerHealth>().controls.Player1.Disable();
        Player2.GetComponent<PlayerHealth>().controls.Player2.Disable();
        Player1.GetComponent<PlayerWeapon>().controls.Player1.Disable();
        Player2.GetComponent<PlayerWeapon>().controls.Player2.Disable();
        PauseMenu.GetComponent<PauseMenu>().controls.Player1.Disable();
        PauseMenu.GetComponent<PauseMenu>().controls.Player2.Disable();
    }


    /* Move selection display upward
    * update index
    */
    private void selectUp() {
        if (gameObject.activeSelf) {
            SelectCorners[selectIndex].SetActive(false);
            selectIndex = Mathf.Max(selectIndex - 1, 0);
            SelectCorners[selectIndex].SetActive(true);
        }
    }


    /* Move selection display downward
    * update index
    */
    private void selectDown() {
        if (gameObject.activeSelf) {
            SelectCorners[selectIndex].SetActive(false);
            selectIndex = Mathf.Min(selectIndex + 1, 2);
            SelectCorners[selectIndex].SetActive(true);
        }
    }


    /* Interacts with currently selected setting
    */
    public void selectSetting() {
        if (gameObject.activeSelf) {
            switch(selectIndex) {
                case 0: // Restart
                    controls.UI.Disable();  // disable GameOver inputs
                    Time.timeScale = 1f;    // reset time

                    // reload current level
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                    break;

                case 1: // Settings
                    SettingsMenuScript.Caller("GameOver");

                    // set the volume slider value in settings to the current volume value
                    float currentVolume;
                    SettingsMenuScript.audioMixer.GetFloat("Master", out currentVolume);
                    SettingsMenuScript.synchroVolume(currentVolume);

                    SettingsMenu.SetActive(true);       // enable SettingsMenu window
                    this.gameObject.SetActive(false);   // disable GameOver window
                    controls.UI.Disable();              // disable GameOver inputs

                    // reactivate SettingsMenu inputs if necessary
                    if (!SettingsMenuScript.controls.UI.enabled) {
                        SettingsMenuScript.Awake();
                    }
                    break;

                case 2: // MainMenu
                    controls.UI.Disable();                  // disable GameOver inputs
                    Time.timeScale = 1f;                    // reset time
                    SceneManager.LoadScene("MainMenu");     // load MainMenu
                    break;
            }
        }
    }


    /* block inputs during 'delay' secs
    * avoid spamming
    */
    IEnumerator Delay() {
        yield return new WaitForSecondsRealtime(delay);     // /!\ WaitForSeconds doesnt work if time suspended /!\
        //blockedControls = false;
    }
}
