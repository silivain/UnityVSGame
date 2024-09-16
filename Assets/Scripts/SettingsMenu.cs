using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

// menu des paramètres
public class SettingsMenu : MonoBehaviour //video 17
{
	public GameObject[] SelectMenu;                 // tableau contenant les go des différents menus
	public Slider volumeSlider;					// volume slider
	public AudioMixer audioMixer;					// mixer audio du jeu
	public Dropdown resolutionDropdown;				// menu déroulant des résolutions
	Resolution[] resolutions;						// liste des résolutions
    public PlayerControls controls;     			// InputSystem
    public GameObject[] SelectCorners;  			// tableau des images de selection de menu
    private int selectIndex = 0;        			// indice désignant le menu actuellement sélectionné
    private int cornersNb = 4;						// TODO : nombre de settings
    private bool _fullScreen;						// plein écran
    public GameObject checkmark;					// checkmarck active si full screen


    /* recup les inputs via l'InputActionMap 'UI'
    */
    public void Awake() {
    	controls = new PlayerControls();    // on recup les inputs
        controls.UI.Enable();
        controls.UI.GoUp.performed += ctx => selectUp();
        controls.UI.GoDown.performed += ctx => selectDown();
        controls.UI.GoLeft.performed += ctx => volumeDown();
        controls.UI.GoRight.performed += ctx => volumeUp();
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
		resolutionDropdown.value = currentResolutionIndex;
		resolutionDropdown.RefreshShownValue();

		Screen.fullScreen = true;
		_fullScreen = true;
	}


	/* règle le volume à 'volume'
	*/
	public void SetVolume(float volume) {
		audioMixer.SetFloat("Master", volume);
		volumeSlider.value = volume;
	}


	/* active ou désactive le plein écran
	*/
	public void SetFullScreen(bool isFullScreen) {
		Screen.fullScreen = isFullScreen;
		_fullScreen = !_fullScreen;
	}


	/* règle la résolution sur resolutions['resolutionIndex']
	*/
	public void SetResolution(int resolutionIndex) {
		Resolution resolution = resolutions[resolutionIndex];
		Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
	}


	/* decreases volume by 5
	*/
	private void volumeDown() {
		if (selectIndex == 2) {	//check if selectIndex is set on volume setting
			float currentVolume;
			audioMixer.GetFloat("Master", out currentVolume);
			float newVolume = Mathf.Max(currentVolume - 5f, -80f);
			SetVolume(newVolume);
			Debug.Log("current volume : " + newVolume);
		}
	}


	/* increases volume by 5
	*/
	private void volumeUp() {
		if (selectIndex == 2) {	//check if selectIndex is set on volume setting
			float currentVolume;
			audioMixer.GetFloat("Master", out currentVolume);
			float newVolume = Mathf.Min(currentVolume + 5f, 20f);
			SetVolume(newVolume);
			Debug.Log("current volume : " + newVolume);
		}
	}


	/* synchronise current audioMixer value,
	   slideVolume and volume variable
	*/
	public void synchroVolume(float vol) {
		volumeSlider.value = vol;
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


    /* Active la scène rattachée au bouton actuellement sélectionné
    */
    private void selectSetting() {
        if (gameObject.activeSelf) {
            switch(selectIndex) {
                case 0:
                    // TODO : affiche le menu déroulant
                    break;
                case 1:
                	checkmark.SetActive(!_fullScreen);	// active ou désactive la checkmarck
                    SetFullScreen(!_fullScreen);		// active ou désactive le plein écran
                    break;
                case 2:
                	break;
                case 3:
                	SelectMenu[1].SetActive(true);					// active main menu
                    SelectMenu[0].SetActive(false);					// désactive settings menu
					controls.UI.Disable();							// disable inputs
                    // reactivate MainMenu inputs if necessary
					if (!SelectMenu[1].GetComponent<MainMenu>().controls.UI.enabled) {
						SelectMenu[1].GetComponent<MainMenu>().Awake();
					}
                    break;
            }
        }
    }
}
