using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject[] SelectMenu;                 // tableau contenant les go des différents menus
    public GameObject[] SelectCorners;              // tableau des images de selection de menu
    private int selectIndex = 0;                    // indice désignant le menu actuellement sélectionné
    public PlayerControls controls;                 // contrôles du joueur


    // récupère la gestion des inputs
    private void Awake() {
        controls = new PlayerControls();controls = new PlayerControls();    // on recup le script qui gère les inputs
        controls.UI.Enable();                                               // on utilise l'InputActionMap 'UI'
        controls.UI.GoLeft.performed += ctx => selectLeft();
        controls.UI.GoRight.performed += ctx => selectRight();
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


    /* Déplace la sélection vers la gauche
    * met à jour l'index
    */
    private void selectLeft() {
        if (selectIndex != 0 && gameObject.activeSelf) {
            Debug.Log("in MainMenu/selectLeft");
            SelectCorners[selectIndex].SetActive(false);
            selectIndex = 0;
            SelectCorners[selectIndex].SetActive(true);
        }
    }


    /* Déplace la sélection vers la droite
    * met à jour l'index
    */
    private void selectRight() {
        if (selectIndex != 1 && gameObject.activeSelf) {
            Debug.Log("in MainMenu/selectRight");
            SelectCorners[selectIndex].SetActive(false);
            selectIndex = 1;
            SelectCorners[selectIndex].SetActive(true);
        }
    }


    /* Déplace la sélection vers le bas
    * met à jour l'index
    */
    private void selectDown() {
        if (selectIndex != 2 && gameObject.activeSelf) {
            Debug.Log("in MainMenu/selectDown");
            SelectCorners[selectIndex].SetActive(false);
            selectIndex = 2;
            SelectCorners[selectIndex].SetActive(true);
        }
    }


    /* Active la scène rattachée au bouton actuellement sélectionné
    */
    private void selectScene() {
        if (gameObject.activeSelf) {
            switch(selectIndex) {
                case 0:
                    SelectMenu[1].SetActive(true);  // active select scene
                    SelectMenu[0].SetActive(false); // désactive main menu
                    break;
                case 1:
                    SelectMenu[1].SetActive(true);  // active select scene TODO: settings menu
                    SelectMenu[0].SetActive(false); // désactive main menu
                    break;
                case 2:
                    Application.Quit();             // quitte le jeu
                    break;
            }
        }
    }
}
