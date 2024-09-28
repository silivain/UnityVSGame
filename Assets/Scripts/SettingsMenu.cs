using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public class SettingsMenu : MonoBehaviour //video 17
{
	public GameObject[] SelectMenu;     	// gameobjects of all Menus able to call SettingsMenu
	public Slider volumeSlider;				// volume slider
	public AudioMixer audioMixer;			// main AudioMixer
	public Dropdown resolutionDropdown;		// resolutions dropdown
	private Resolution[] resolutions;		// resolution list
	private int resolutionIndex;			// index of current resolution in resolutions[]
    public PlayerControls controls;    		// InputSystem
    public GameObject[] SelectCorners;  	// selection displays
    private int selectIndex = 0;        	// index of currently selected display
    private int cornersNb = 5;				// number of settings
    private bool _fullScreen;				// full screen
    public GameObject checkmark;			// checkmarck, active if full screen
    private int called_by = 0;				// index of script to return to (according to SelectMenu array)
    public GameObject InputSettingsMenu;	// inputs settings menu gameobject
    private MainMenu MainMenuScript;		// MainMenu.cs
    private PauseMenu PauseMenuScript;		// PauseMenu.cs
    private GameOver GameOverScript;		// GameOver.cs
    private InputSettingsMenu InputSettingsMenuScript; // InputSettingsMenu.cs


    /* instantiate inputs
    */
    public void Awake() {
    	controls = new PlayerControls();	// link with script handling inputs
        controls.UI.Enable();				// listen to 'UI' InputActionMap
        controls.UI.GoUp.performed += ctx => selectUp();
        controls.UI.GoDown.performed += ctx => selectDown();
        controls.UI.GoLeft.performed += ctx => selectLeft();
        controls.UI.GoRight.performed += ctx => selectRight();
        controls.UI.Start.performed += ctx => selectSetting();
    }


	/* collect available resolutions
	* create corresponding dropdown
	* set current resolution to monitor resolution
	* enable full screen
	*/
	private void Start() {
		// collect available resolutions
		resolutions = Screen.resolutions.Where(resolution => resolution.refreshRate == 60).ToArray();
		resolutionDropdown.ClearOptions();

		List<string> options = new List<string>();
		int currentResolutionIndex = 0;

		for (int i = 0; i < resolutions.Length; i ++) {
			string option = resolutions[i].width + "x" + resolutions[i].height;
			options.Add(option);

			// look for monitor resolution
			if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height) {
				currentResolutionIndex = i;
			}
		}

		resolutionDropdown.AddOptions(options);
		resolutionIndex = currentResolutionIndex;
		SetResolution(resolutionIndex);

		Screen.fullScreen = true;
		_fullScreen = true;

		// selection display visible and set to default selection
        SelectCorners[selectIndex].SetActive(false);
        selectIndex = 0;
        SelectCorners[selectIndex].SetActive(true);

        // link to InputSettingsMenu.cs
        if (!InputSettingsMenu.TryGetComponent<InputSettingsMenu>(out InputSettingsMenuScript)) {
            Debug.Log("failed to pull InputSettingsMenu.cs in SettingsMenu.cs");
        }
	}


	/*	Register the script activating settings menu
		so we can go back to the right menu afterwards
	*/
	public void Caller(string caller) {
		if (caller == "PauseMenu") {
			called_by = 0;	// index of PauseMenu in SelectMenu[]
			PauseMenuScript = SelectMenu[1].GetComponent<PauseMenu>();
		}else if (caller == "GameOver") {
			called_by = 2;	// index of GameOver in SelectMenu[]
			GameOverScript = SelectMenu[2].GetComponent<GameOver>();
		}else if (caller == "MainMenu") {
			called_by = 3;	// index of MainMenu in SelectMenu[]
			MainMenuScript = SelectMenu[3].GetComponent<MainMenu>();
		}
	}


	/* set volume to 'volume'
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


	/* enable/disable full screen
	*/
	public void SetFullScreen(bool isFullScreen) {
		Screen.fullScreen = isFullScreen;
		_fullScreen = !_fullScreen;
	}


	/* set resolution to resolutions['resIndex']
	*/
	public void SetResolution(int resIndex) {
		Resolution resolution = resolutions[resIndex];
		bool backToFullScreen = false;

		/* TODO : this was an attempt to avoid a buggy window when
		* setting resolution to monitor resolution
		* similar bug happens when switching to non full screen and not in monitor resolution
		* not working so far, see Build0.13
		*/
		if (resolution.width == Screen.width && resolution.height == Screen.height
			&& _fullScreen) {
			SetFullScreen(!_fullScreen);
			backToFullScreen = true;
		}
		resolutionDropdown.value = resIndex;
		resolutionDropdown.RefreshShownValue();
		Screen.SetResolution(resolution.width, resolution.height, _fullScreen);

		// TODO : read above
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


	/* Move selection display upward
    * update index
    */
    private void selectUp() {
        int tempIndex = selectIndex;
        selectIndex = Mathf.Max(selectIndex - 1, 0);
        SelectCorners[tempIndex].SetActive(false);
        SelectCorners[selectIndex].SetActive(true);
    }


    /* Move selection display downward
    * update index
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
                case 0:	// resolution
                    break;

                case 1:	// full screen
                	checkmark.SetActive(!_fullScreen);	// enable or disable checkmarck
                    SetFullScreen(!_fullScreen);		// enable or disable full screen
                    break;

                case 2:	// volume
                	break;

                case 3: // Input settings
                	InputSettingsMenu.SetActive(true);		// enable input settings menu
                    this.gameObject.SetActive(false);		// disable settings menu
					controls.UI.Disable();					// disable inputs

        			// enable inputs of settings menu script if necessary
        			if (!InputSettingsMenuScript.controls.UI.enabled) {
						InputSettingsMenuScript.controls.UI.Enable();
					}
        			break;

                case 4:	// back to previous menu
                	SelectMenu[called_by].SetActive(true);	// enable calling menu
                    this.gameObject.SetActive(false);		// disable settings menu
					controls.UI.Disable();					// disable inputs

					// selection display visible and set to default selection
        			SelectCorners[selectIndex].SetActive(false);
			        selectIndex = 0;
        			SelectCorners[selectIndex].SetActive(true);

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
