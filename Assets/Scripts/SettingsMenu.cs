using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

// menu des paramètres
public class SettingsMenu : MonoBehaviour //video 17
{
	public GameObject[] SelectMenu;     	// tableau contenant les go des différents menus
	public Slider volumeSlider;				// volume slider
	public AudioMixer audioMixer;			// mixer audio du jeu
	public Dropdown resolutionDropdown;		// menu déroulant des résolutions
	private Resolution[] resolutions;		// liste des résolutions
	private int resolutionIndex;			// index of current resolution in resolutions[]
    public PlayerControls controls;    		// InputSystem
    public GameObject[] SelectCorners;  	// tableau des images de selection de menu
    private int selectIndex = 0;        	// indice désignant le menu actuellement sélectionné
    private int cornersNb = 4;				// TODO : nombre de settings
    private bool _fullScreen;				// plein écran
    public GameObject checkmark;			// checkmarck active si full screen
    private int called_by = 0;				// index of script to return to (according to SelectMenu array)
    private MainMenu MainMenuScript;		// MainMenu.cs
    private game_paused PauseMenuScript;	// game_paused.cs
    private GameOver_screen GameOverScript;	// GameOver_screen.cs


    /* recup les inputs via l'InputActionMap 'UI'
    */
    public void Awake() {
    	controls = new PlayerControls();    // on recup les inputs
        controls.UI.Enable();
        controls.UI.GoUp.performed += ctx => selectUp();
        controls.UI.GoDown.performed += ctx => selectDown();
        controls.UI.GoLeft.performed += ctx => selectLeft();
        controls.UI.GoRight.performed += ctx => selectRight();
        controls.UI.Start.performed += ctx => selectSetting();

        SelectCorners[selectIndex].SetActive(false);
        selectIndex = 0;
        SelectCorners[selectIndex].SetActive(true);
    }


	/* au démarrage :
	* récupère les résolutions de l'écran avec un taux de rafraichissement à 60Hz
	* crée le menu déroulant correspondant à ces résolutions
	* applique la résolution par défaut de l'écran
	* passe en plein écran
	*/
	private void Start() {
		resolutions = Screen.resolutions.Where(resolution => resolution.refreshRate == 60).ToArray();
		resolutionDropdown.ClearOptions();

		List<string> options = new List<string>();
		int currentResolutionIndex = 0;

		for (int i = 0; i < resolutions.Length; i ++) {
			string option = resolutions[i].width + "x" + resolutions[i].height;
			options.Add(option);

			if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height) {
				currentResolutionIndex = i;
			}
		}

		resolutionDropdown.AddOptions(options);
		resolutionIndex = currentResolutionIndex;
		SetResolution(resolutionIndex);

		Screen.fullScreen = true;
		_fullScreen = true;
	}


	/*	Register the script activating settings menu
		so we can go back to the right menu afterwards
	*/
	public void Caller(string caller) {
		if (caller == "game_paused") {
			called_by = 0;	// index of game_paused in SelectMenu[]
			PauseMenuScript = SelectMenu[1].GetComponent<game_paused>();
		}else if (caller == "GameOver_screen") {
			called_by = 2;	// index of GameOver_screen in SelectMenu[]
			GameOverScript = SelectMenu[2].GetComponent<GameOver_screen>();
		}else if (caller == "MainMenu") {
			called_by = 3;	// index of MainMenu in SelectMenu[]
			MainMenuScript = SelectMenu[3].GetComponent<MainMenu>();
		}
	}


	/* règle le volume à 'volume'
	*/
	public void SetVolume(float volume) {
		audioMixer.SetFloat("Master", volume);
		volumeSlider.value = volume;
	}


	/* synchronise current audioMixer value,
	   slideVolume and volume variable
	*/
	public void synchroVolume(float vol) {
		volumeSlider.value = vol;
	}


	/* active ou désactive le plein écran
	*/
	public void SetFullScreen(bool isFullScreen) {
		Screen.fullScreen = isFullScreen;
		_fullScreen = !_fullScreen;
	}


	/* règle la résolution sur resolutions['resolutionIndex']
	*/
	public void SetResolution(int resIndex) {
		Resolution resolution = resolutions[resIndex];
		bool backToFullScreen = false;
		if (resolution.width == Screen.width && resolution.height == Screen.height
			&& _fullScreen) {
			SetFullScreen(!_fullScreen);
			backToFullScreen = true;
		}
		resolutionDropdown.value = resIndex;
		resolutionDropdown.RefreshShownValue();
		Screen.SetResolution(resolution.width, resolution.height, _fullScreen);
		if (backToFullScreen) {
			SetFullScreen(!_fullScreen);
		}
	}


	/*  on resolution setting : apply previous resolution setting
	* on volume setting : decreases volume by 5
	*/
	private void selectLeft() {
		if (selectIndex == 0) {	//check if selectIndex is set on resolution setting
			resolutionIndex = (resolutionIndex - 1 + resolutions.Length) % resolutions.Length;
			SetResolution(resolutionIndex);
		}else if (selectIndex == 2) {	//check if selectIndex is set on volume setting
			float currentVolume;
			audioMixer.GetFloat("Master", out currentVolume);
			float newVolume = Mathf.Max(currentVolume - 5f, -80f);
			SetVolume(newVolume);
		}
	}


	/*  on resolution setting : apply next resolution setting
	* on volume setting : increases volume by 5
	*/
	private void selectRight() {
		if (selectIndex == 0) {	//check if selectIndex is set on resolution setting
			resolutionIndex = (resolutionIndex + 1) % resolutions.Length;
			SetResolution(resolutionIndex);
		}else if (selectIndex == 2) {	//check if selectIndex is set on volume setting
			float currentVolume;
			audioMixer.GetFloat("Master", out currentVolume);
			float newVolume = Mathf.Min(currentVolume + 5f, 20f);
			SetVolume(newVolume);
		}
	}


	/* Déplace la sélection vers le haut
    * met à jour l'index
    */
    private void selectUp() {
        int tempIndex = selectIndex;
        selectIndex = Mathf.Max(selectIndex - 1, 0);
        SelectCorners[tempIndex].SetActive(false);
        SelectCorners[selectIndex].SetActive(true);
    }


    /* Déplace la sélection vers le bas
    * met à jour l'index
    */
    private void selectDown() {
        int tempIndex = selectIndex;
        selectIndex = Mathf.Min(selectIndex + 1, cornersNb - 1);
        SelectCorners[tempIndex].SetActive(false);
        SelectCorners[selectIndex].SetActive(true);
    }


    /* Interacts with currently selected setting
    */
    private void selectSetting() {
        if (gameObject.activeSelf) {
            switch(selectIndex) {
                case 0:		// resolution
                    break;
                case 1:		// full screen
                	checkmark.SetActive(!_fullScreen);	// enable or disable checkmarck
                    SetFullScreen(!_fullScreen);		// enable or disable full screen
                    break;
                case 2:		// volume
                	break;
                case 3:		// back to previous menu
                	SelectMenu[called_by].SetActive(true);	// enable calling menu
                    this.gameObject.SetActive(false);		// disable settings menu
					controls.UI.Disable();					// disable inputs

					// enable inputs of calling script if necessary
					switch(called_by) {
						case 0:
							if (!PauseMenuScript.controls.UI.enabled) {
								PauseMenuScript.controls.UI.Enable();
							}
							break;
						case 2:
							if (!GameOverScript.controls.UI.enabled) {
								GameOverScript.controls.UI.Enable();
							}
							break;
						case 3:
							if (!MainMenuScript.controls.UI.enabled) {
								MainMenuScript.controls.UI.Enable();
							}
							break;
					}
                    break;
            }
        }
    }
}
