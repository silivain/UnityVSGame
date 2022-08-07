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
    public GameObject[] SelectCorners;              // tableau des images de selection de menu
    private int selectIndex = 0;                    // indice désignant le menu actuellement sélectionné
    public PlayerControls controls;                 // contrôles du joueur

    public Text textFinPartie;                      // texte de fin de partie
    public AudioSource currentMusic;                // musique de combat
    public AudioSource gameOverMusic;               // musique du menu GameOver
    private float delay = 1f;                       // durée pdnt laquelle les ctrls sont bloqués à l'apparition du GO screen
    private bool blockedControls = true;            // vrai si les controles sont bloqués

    public GameObject[] UIToDisable;                // éléments à désactiver lors de la fin de partie (barres vie, muns etc)


    // récupère la gestion des inputs
    private void Awake() {
        controls = new PlayerControls();controls = new PlayerControls();    // on recup le script qui gère les inputs
        controls.UI.Enable();                                               // on utilise l'InputActionMap 'UI'
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
        Time.timeScale = 0f;            // stoppe le temps

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

        gameObject.SetActive(true);     // active l'écran de GameOver
        currentMusic.Stop();            // arrête la musique de combat
        gameOverMusic.Play();           // lance la musique de GameOver
        StartCoroutine(Delay());        // bloque controles pour éviter missclick
    }


    /* Déplace la sélection vers le haut
    * met à jour l'index
    */
    private void selectUp() {
        if (selectIndex != 0 && gameObject.activeSelf && !blockedControls) {
            SelectCorners[selectIndex].SetActive(false);
            selectIndex = 0;
            SelectCorners[selectIndex].SetActive(true);
        }
    }


    /* Déplace la sélection vers le bas
    * met à jour l'index
    */
    private void selectDown() {
        if (selectIndex != 1 && gameObject.activeSelf && !blockedControls) {
            SelectCorners[selectIndex].SetActive(false);
            selectIndex = 1;
            SelectCorners[selectIndex].SetActive(true);
        }
    }


    /* charge la scène sélectionnée par le joueur
    */
    public void selectScene() {
        if (gameObject.activeSelf && !blockedControls) {
            Time.timeScale = 1f;                    // temps en vitesse normale.
            gameObject.SetActive(false);            // on désactive l'écran de GameOver
            blockedControls = true;                 // rebloque les controles pour la prochaine iteration

            // réactive les UI désactivés à l'apparition du GOScreen
            foreach(GameObject go in UIToDisable) {
                go.SetActive(true);
            }

            SceneManager.LoadScene("MainMenu");     // TODO : charger la scène en fonction de la selection
            // TODO : musique main menu ?
        }
    }


    /* bloque les contrôles pdnt 'delay' secs
    * évite que les joueurs aillent instantanément au menu à cause d'un missclick
    */
    IEnumerator Delay() {
        yield return new WaitForSecondsRealtime(delay);     // /!\ WaitForSeconds marche pas si le temps est suspendu /!\
        blockedControls = false;
    }
}
