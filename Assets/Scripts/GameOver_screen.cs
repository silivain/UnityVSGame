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
public class GameOver_screen : MonoBehaviour
{   
    public GameObject[] SelectMenu;     // array with menus to enable/disable
    public GameObject[] SelectCorners;  // tableau des images de selection de menu
    private int selectIndex = 0;        // indice désignant le menu actuellement sélectionné
    public PlayerControls controls;     // contrôles du joueur

    public Text textFinPartie;          // texte de fin de partie
    public AudioSource currentMusic;    // musique de combat
    public AudioSource gameOverMusic;   // musique du menu GameOver
    private float delay = 1f;           // durée pdnt laquelle les ctrls sont bloqués à l'apparition du GO screen

    public GameObject[] UIToDisable;    // éléments à désactiver lors de la fin de partie (barres vie, muns etc)
    public GameObject Player1;          // access to Player1 scripts
    public GameObject Player2;          // access to Player2 scripts
    public GameObject PauseMenu;        // access to PauseMenu


    // récupère la gestion des inputs
    private void Awake() {
        controls = new PlayerControls();                        // on recup le script qui gère les inputs
        controls.UI.Enable();                                   // on utilise l'InputActionMap 'UI'
        controls.UI.GoUp.performed += ctx => selectUp();
        controls.UI.GoDown.performed += ctx => selectDown();
        controls.UI.Start.performed += ctx => selectScene();
    }


    /* sélectionne le bouton par défaut au démarrage
    */
    private void Start() {
        SelectCorners[selectIndex].SetActive(false);
        selectIndex = 0;
        SelectCorners[selectIndex].SetActive(true);
    }


    /* affiche le menu de GameOver
    * fct appelée dans 'PlayerHealth'
    */
    public void Setup(string _tag){
        Time.timeScale = 0f;        // stops time
        DisableInGameInputs();      // disable all ingame inputs

        // modifie l'affichage selon le joueur qui a gagné
        if (_tag == "Player 1") {
            textFinPartie.text = "Le joueur 2 gagne !";
        }else {
            textFinPartie.text = "Le joueur 1 gagne !";
        }

        // désactive les UI ingame envahissants
        foreach(GameObject go in UIToDisable) {
            go.SetActive(false);
        }

        gameObject.SetActive(true); // active l'écran de GameOver
        currentMusic.Stop();        // arrête la musique de combat
        gameOverMusic.Play();       // lance la musique de GameOver
        StartCoroutine(Delay());    // bloque controles pour éviter missclick
    }


    /* Disable in game inputs :
       - PlayerMovement
       - PlayerHealth
       - PlayerWeapon
       - game_paused
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
        PauseMenu.GetComponent<game_paused>().controls.Player1.Disable();
        PauseMenu.GetComponent<game_paused>().controls.Player2.Disable();
    }


    /* Déplace la sélection vers le haut
    * met à jour l'index
    */
    private void selectUp() {
        if (gameObject.activeSelf) {
            SelectCorners[selectIndex].SetActive(false);
            selectIndex = Mathf.Max(selectIndex - 1, 0);
            SelectCorners[selectIndex].SetActive(true);
        }
    }


    /* Déplace la sélection vers le bas
    * met à jour l'index
    */
    private void selectDown() {
        if (gameObject.activeSelf) {
            SelectCorners[selectIndex].SetActive(false);
            selectIndex = Mathf.Min(selectIndex + 1, 2);
            SelectCorners[selectIndex].SetActive(true);
        }
    }


    /* charge la scène sélectionnée par le joueur
    */
    public void selectScene() {
        if (gameObject.activeSelf) {
            switch(selectIndex) {
                case 0:
                    controls.UI.Disable();                  // disable inputs
                    Time.timeScale = 1f;                    // reset time
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);  // reload current level
                    break;
                case 1:
                    SelectMenu[1].GetComponent<SettingsMenu_InGame>().Caller("GameOver_screen");

                    // set the volume slider value in settings to the current volume value
                    float currentVolume;
                    SelectMenu[1].GetComponent<SettingsMenu_InGame>().audioMixer.GetFloat("Master", out currentVolume);
                    SelectMenu[1].GetComponent<SettingsMenu_InGame>().synchroVolume(currentVolume);
                    SelectMenu[1].SetActive(true);          // active settings menu
                    SelectMenu[0].SetActive(false);         // désactive GO menu
                    controls.UI.Disable();                  // disable inputs

                    // reactivate SettingsMenu inputs if necessary
                    if (!SelectMenu[1].GetComponent<SettingsMenu_InGame>().controls.UI.enabled) {
                        SelectMenu[1].GetComponent<SettingsMenu_InGame>().Awake();
                    }
                    break;
                case 2:
                    controls.UI.Disable();                  // disable inputs
                    Time.timeScale = 1f;                    // reset time
                    SceneManager.LoadScene("MainMenu");     // reload current level
                    break;
            }
        }
    }


    /* bloque les contrôles pdnt 'delay' secs
    * évite que les joueurs aillent instantanément au menu à cause d'un missclick
    */
    IEnumerator Delay() {
        yield return new WaitForSecondsRealtime(delay);     // /!\ WaitForSeconds marche pas si le temps est suspendu /!\
        //blockedControls = false;
    }
}
