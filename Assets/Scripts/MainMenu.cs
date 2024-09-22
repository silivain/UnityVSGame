using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject SelectSceneMenu;              // SelectSceneMenu window
    public GameObject SettingsMenu;                 // SettingsMenu window
    private SelectSceneMenu SelectSceneMenuScript;  // SelectSceneMenu.cs
    private SettingsMenu SettingsMenuScript;        // SettingsMenu.cs

    public GameObject[] SelectCorners;              // selection displays
    private int selectIndex = 0;                    // index of currently selected display
    public PlayerControls controls;                 // inputs


    /* instantiate inputs
    */
    public void Awake() {
        controls = new PlayerControls();    // link with script handling inputs
        controls.UI.Enable();               // listen to 'UI' InputActionMap
        controls.UI.GoLeft.performed += ctx => selectLeft();
        controls.UI.GoRight.performed += ctx => selectRight();
        controls.UI.GoDown.performed += ctx => selectDown();
        controls.UI.GoUp.performed += ctx => selectUp();
        controls.UI.Start.performed += ctx => selectSetting();
    }


    /* initialize selectIndex and SelectCorners
    * initialize SettingsMenuScript
    */
    public void Start() {
        // selection display visible and set to default selection
        SelectCorners[selectIndex].SetActive(false);
        selectIndex = 0;
        SelectCorners[selectIndex].SetActive(true);

        // link to SelectSceneMenu.cs
        if (!SelectSceneMenu.TryGetComponent<SelectSceneMenu>(out SelectSceneMenuScript)) {
            Debug.Log("failed to pull SelectSceneMenu.cs in MainMenu.cs");
        }

        // link to SettingsMenu.cs
        if (!SettingsMenu.TryGetComponent<SettingsMenu>(out SettingsMenuScript)) {
            Debug.Log("failed to pull SettingsMenu.cs in MainMenu.cs");
        }
    }


    /* Move selection display to the left
    * update index
    */
    private void selectLeft() {
        if (selectIndex != 0 && gameObject.activeSelf) {
            SelectCorners[selectIndex].SetActive(false);
            selectIndex = 0;
            SelectCorners[selectIndex].SetActive(true);
        }
    }


    /* Move selection display to the right
    * update index
    */
    private void selectRight() {
        if (selectIndex != 1 && gameObject.activeSelf) {
            SelectCorners[selectIndex].SetActive(false);
            selectIndex = 1;
            SelectCorners[selectIndex].SetActive(true);
        }
    }


    /* Move selection display downward
    * update index
    */
    private void selectDown() {
        if (selectIndex != 2 && gameObject.activeSelf) {
            SelectCorners[selectIndex].SetActive(false);
            selectIndex = 2;
            SelectCorners[selectIndex].SetActive(true);
        }
    }


    /* Move selection display upward
    * update index
    */
    private void selectUp() {
        if (selectIndex == 2 && gameObject.activeSelf) {
            SelectCorners[selectIndex].SetActive(false);
            selectIndex = 0;
            SelectCorners[selectIndex].SetActive(true);
        }
    }


    /* Interacts with currently selected setting
    */
    private void selectSetting() {
        if (gameObject.activeSelf) {
            switch(selectIndex) {
                case 0: // Start
                    SelectSceneMenu.SetActive(true);    // enable SelectScene window
                    this.gameObject.SetActive(false);   // disable MainMenu window
                    controls.UI.Disable();              // disable MainMenu inputs

                    // reactivate SelectSceneMenu inputs if necessary
                    if (!SelectSceneMenuScript.controls.UI.enabled) {
                        SelectSceneMenuScript.Awake();
                    }
                    break;

                case 1: // Settings
                    // set the volume slider value in settings to the current volume value
                    float currentVolume;
                    SettingsMenuScript.audioMixer.GetFloat("Master", out currentVolume);
                    SettingsMenuScript.synchroVolume(currentVolume);

                    // call SettingsMenu so it goes back to current script when finished
                    SettingsMenuScript.Caller("MainMenu");
                    SettingsMenu.SetActive(true);       // enable SettingsMenu window
                    this.gameObject.SetActive(false);   // disable MainMenu window
                    controls.UI.Disable();              // disable MainMenu inputs

                    // reactivate SettingsMenu inputs if necessary
                    if (!SettingsMenuScript.controls.UI.enabled) {
                        SettingsMenuScript.Awake();
                    }
                    break;

                case 2: // Quit
                    Application.Quit(); // leave game
                    break;
            }
        }
    }
}
