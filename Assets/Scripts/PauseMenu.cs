﻿using UnityEngine;
using UnityEngine.SceneManagement;

// menu pause et ses bouttons
// TODO script pas encore utilisé
public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;	// vrai si le jeu est en pause
    public GameObject pauseMenuUI;				// fenêtre du menu pause
    public GameObject settingsWindow;			// fenêtre des paramètres
    public PlayerControls controls;


    private void Awake() {
        controls = new PlayerControls();						    // on recup le script qui gère les inputs
        controls.Player1.Enable();
        controls.Player2.Enable();
    }


	/* active ou désactive la pause
	*/
    void Update() {
      if(controls.Player1.Start.triggered || controls.Player2.Start.triggered) {
        if(gameIsPaused) {
          Resume();
        }
        else {
          Paused();
        }
      }
    }

	/* déclenche la pause
	* freeze le temps et le joueur, affiche le menu pause
	*/
    public void Paused()
    {
      PlayerMovement.instance.enabled = false;
      pauseMenuUI.SetActive(true);
      Time.timeScale = 0;
      gameIsPaused = true;
    }

	/* désactive la pause
	*/
    public void Resume()
    {
      PlayerMovement.instance.enabled = true;
      pauseMenuUI.SetActive(false);
      Time.timeScale = 1;
      gameIsPaused = false;
    }

	/* affiche le menu des paramètres
	*/
    public void SettingsButton()
    {
      settingsWindow.SetActive(true);
    }

	/* ferme le menu des paramètres
	*/
    public void CloseSettingsWindow()
    {
      settingsWindow.SetActive(false);
    }

	/* charge le main menu
	* supprime tous les gameObjects de la scène
	*/
    public void LoadMainMenu()
    {
      DontDestroyOnLoadScene.instance.RemoveFromDontDestroyOnLoad();
      Resume();
      SceneManager.LoadScene("MainMenu");
    }
}
