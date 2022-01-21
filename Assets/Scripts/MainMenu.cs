using UnityEngine;
using UnityEngine.SceneManagement;

// main menu et ses bouttons
public class MainMenu : MonoBehaviour //video 17
{
    public string levelToLoad;			// scène à charger lors de l'utilisation du boutton start
	public GameObject settingsWindow;	// fenêtre des paramètres

	/* boutton start : charge la scène 'levelToLoad'
	*/
    public void StartGame()
    {
      SceneManager.LoadScene(levelToLoad);
    }

	/* boutton settings : ouvre les paramètres
	*/
    public void SettingsButton()
    {
      settingsWindow.SetActive(true);
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
