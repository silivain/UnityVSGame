using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject[] SelectCorners;          // selection displays
    private int selectIndex = 0;                // index of currently selected display

    public GameObject PauseMenuWindow;          // PauseMenu window
    public GameObject countdown;                // countdown when starting scene
    public GameObject GOAudio;                  // AudioSource
    public GameObject SettingsMenu;             // SettingsMenu window
    private SettingsMenu SettingsMenuScript;    // SettingsMenu.cs
    private bool controlBlocked = false;        // true if this script has been called less than 'delay' sec
    private float delay = 0.25f;                // delay before recalling this script -> avoid spamming
    public PlayerControls controls;             // inputs
    public GameObject Player1;                  // access to Player1 scripts
    public GameObject Player2;                  // access to Player2 scripts


    /* instantiate inputs
    * sets active inputs to InGame scenario
    */
    private void Awake() {
        controls = new PlayerControls();    // link with script handling inputs
        controlInGame();
    }


    /* disable PauseMenu window when starting
    * initialize SettingsMenuScript
    */
    private void Start() {
        // disable PauseMenu window
        PauseMenuWindow.SetActive(false);

        // selection display visible and set to default selection
        SelectCorners[selectIndex].SetActive(false);
        selectIndex = 0;
        SelectCorners[selectIndex].SetActive(true);

        // link to SettingsMenu.cs
        if (!SettingsMenu.TryGetComponent<SettingsMenu>(out SettingsMenuScript)) {
            Debug.Log("failed to pull SettingsMenu.cs in PauseMenu.cs");
        }
    }


    /* Enable or disable PauseMenu window
    * countdown has to be finished
    */
    private void PauseGame() {
        if (!countdown.activeSelf && !controlBlocked) {
            controlBlocked = true;
            StartCoroutine(Delay());

            PauseMenuWindow.SetActive(true);
            GOAudio.GetComponent<AudioSource>().Pause();
            Time.timeScale = 0f;

            controlInMenu();    // sets active inputs to InGame scenario
        }
    }


    /* Move selection display upward
    * update index
    */
    private void selectUp() {
        if (selectIndex != 0 && gameObject.activeSelf && !controlBlocked) {
            SelectCorners[selectIndex--].SetActive(false);
            SelectCorners[selectIndex].SetActive(true);
        }
    }


    /* Move selection display downward
    * update index
    */
    private void selectDown() {
        if (selectIndex != 2 && gameObject.activeSelf && !controlBlocked) {
            SelectCorners[selectIndex++].SetActive(false);
            SelectCorners[selectIndex].SetActive(true);
        }
    }


    /* block controls during 'delay' secs
    * avoid spamming
    */
    IEnumerator Delay() {
        yield return new WaitForSecondsRealtime(delay);     // /!\ WaitForSeconds doesnt work if time suspended /!\
        controlBlocked = false;
    }


    /* sets active inputs to InGame scenario
    */
    private void controlInGame() {
        controls.UI.Disable();      // disable 'UI' InputActionMap
        controls.Player1.Enable();  // listen to 'Player1' InputActionMap
        controls.Player2.Enable();  // listen to 'Player2' InputActionMap
        controls.Player1.Start.performed += ctx => PauseGame();
        controls.Player2.Start.performed += ctx => PauseGame();

    }


    /* sets active inputs to PauseMenu scenario
    */
    public void controlInMenu() {
        controls.Player1.Disable(); // disable 'Player1' InputActionMap
        controls.Player2.Disable(); // disable 'Player2' InputActionMap
        controls.UI.Enable();       // listen to 'UI' InputActionMap
        controls.UI.GoUp.performed += ctx => selectUp();
        controls.UI.GoDown.performed += ctx => selectDown();
        controls.UI.Start.performed += ctx => selectSetting();
    }


    /* Disable in game inputs :
       - PlayerMovement
       - PauseMenu
    */
    private void DisableInGameInputs() {
        Player1.GetComponent<PlayerMovement>().controls.Player1.Disable();
        Player1.GetComponent<PlayerMovement>().controls.Player2.Disable();
        Player1.GetComponent<PlayerHealth>().controls.Player1.Disable();
        Player1.GetComponent<PlayerWeapon>().controls.Player1.Disable();
        Player2.GetComponent<PlayerMovement>().controls.Player1.Disable();
        Player2.GetComponent<PlayerMovement>().controls.Player2.Disable();
        Player2.GetComponent<PlayerHealth>().controls.Player2.Disable();
        Player2.GetComponent<PlayerWeapon>().controls.Player2.Disable();
        controls.Player1.Disable();
        controls.Player2.Disable();
    }


    /* Interacts with currently selected setting
    */
    public void selectSetting() {
        if (gameObject.activeSelf && !controlBlocked) {
            switch(selectIndex) {
                case 0: // continue
                    Time.timeScale = 1f;                        // time to normal speed
                    PauseMenuWindow.SetActive(false);           // disable PauseMenu window
                    controlInGame();                            // set active inputs to InGame scenario
                    GOAudio.GetComponent<AudioSource>().Play(); // turn ingame soundtrack back on
                    break;

                case 1: // SettingsMenu
                    PauseMenuWindow.SetActive(false);           // disable PauseMenu window
                    controls.UI.Disable();                      // disable PauseMenu inputs

                    // set the volume slider value in settings to the current volume value
                    float currentVolume;
                    SettingsMenuScript.audioMixer.GetFloat("Master", out currentVolume);
                    SettingsMenuScript.synchroVolume(currentVolume);

                    // call SettingsMenu so it goes back to current script when finished
                    SettingsMenuScript.Caller("PauseMenu");     
                    SettingsMenu.SetActive(true);               // enable SettingsMenu window

                    // reactivate SettingsMenu inputs if necessary
                    if (!SettingsMenuScript.controls.UI.enabled) {
                        SettingsMenuScript.Awake();
                    }
                    break;

                case 2: // MainMenu
                    Time.timeScale = 1f;                    // time to normal speed
                    PauseMenuWindow.SetActive(false);       // disable PauseMenu window
                    selectIndex = 0;                        // default display selection
                    controls.UI.Disable();                  // disable PauseMenu inputs
                    SceneManager.LoadScene("MainMenu");     // load MainMenu
                    break;
            }
        }
    }
}
