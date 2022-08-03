using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private string[] levelToLoad = {"MainMenu"};    // tableau contenant les noms des scènes à charger
    public GameObject[] SelectCorners;              // tableau des images de selection de menu
    private int selectIndex = 0;                    // indice désignant le menu actuellement sélectionné
    public PlayerControls controls;                 // contrôles du joueur


    // récupère la gestion des inputs
    private void Awake() {
        controls = new PlayerControls();controls = new PlayerControls();						    // on recup le script qui gère les inputs
        controls.UI.Enable();
    }


    // déplace la sélection entre les différents menus
    void FixedUpdate() {
        /*
        if(SceneManager.GetActiveScene().name != "MainMenu") {
            controls.Gameplay.Start.performed += ctx => Launch();
        }
        */

        controls.UI.GoLeft.performed += ctx => selectLeft();
        controls.UI.GoRight.performed += ctx => selectRight();
    }


    /* Déplace la sélection vers la gauche
    * met à jour l'index
    */
    private void selectLeft() {
        SelectCorners[selectIndex].SetActive(false);
        selectIndex = (selectIndex == 0) ? (SelectCorners.Length - 1) : (selectIndex - 1);
        SelectCorners[selectIndex].SetActive(true);
    }


    /* Déplace la sélection vers la droite
    * met à jour l'index
    */
    private void selectRight() {
        SelectCorners[selectIndex].SetActive(false);
        selectIndex = (selectIndex + 1) % SelectCorners.Length;
        SelectCorners[selectIndex].SetActive(true);
    }


    /* TODO : Charge la scène actuellement sélectionné
    * pour l'instant, charge "MainMenu"
    */
    private void Launch() {
        SceneManager.LoadScene("MainMenu");
    }
}
