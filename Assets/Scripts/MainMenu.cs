using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

// main menu et ses bouttons
public class MainMenu : MonoBehaviour //video 17
{
  public int levelToLoad;			// scène à charger lors de l'utilisation du boutton start
	public GameObject settingsWindow;	// fenêtre des paramètres
  private GameObject P1; //P1 qui joue
  private GameObject P2; //P2


	/* boutton start : charge la scène 'levelToLoad'
	*/
  public void StartGame()
  {
    //SceneManager.LoadScene(levelToLoad);
  }

	/* boutton settings : ouvre les paramètres
	*/
    public void SettingsButton()
    {
      settingsWindow.SetActive(true);
    }

    void Update()
    {
      
    }

	/* ferme les paramètres
	*/
    public void CloseSettingsWindow()
    {
      settingsWindow.SetActive(false);
    }



	/* boutton quit : quit l'appli
	*/
    public void QuitGame()
    {
      Application.Quit();
    }
}
