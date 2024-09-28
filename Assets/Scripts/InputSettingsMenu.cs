using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public class InputSettingsMenu : MonoBehaviour //video 17
{
	private int resolutionIndex;				// index of current resolution in resolutions[]
    public PlayerControls controls;    			// InputSystem
    public GameObject[] SelectCorners;  		// selection displays
    private int selectIndex = 0;        		// index of currently selected display
    private int cornersNb = 1;					// number of settings
    public GameObject SettingsMenu;				// settings menu gameobject
    private SettingsMenu SettingsMenuScript;	// settings menu script


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
		/*
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
		*/

		// selection display visible and set to default selection
        SelectCorners[selectIndex].SetActive(false);
        selectIndex = 0;
        SelectCorners[selectIndex].SetActive(true);

        // link to SettingsMenu.cs
        if (!SettingsMenu.TryGetComponent<SettingsMenu>(out SettingsMenuScript)) {
            Debug.Log("failed to pull SettingsMenu.cs in InputSettingsMenu.cs");
        }
	}


	/*  on resolution setting : apply previous resolution setting
	* on volume setting : decreases volume by 5
	*/
	private void selectLeft() {}


	/*  on resolution setting : apply next resolution setting
	* on volume setting : increases volume by 5
	*/
	private void selectRight() {}


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
                case 0:	// back to settings menu
                	SettingsMenu.SetActive(true);			// enable settings menu
                    this.gameObject.SetActive(false);		// disable input settings menu
					controls.UI.Disable();					// disable inputs

					// selection display visible and set to default selection
        			SelectCorners[selectIndex].SetActive(false);
			        selectIndex = 0;
        			SelectCorners[selectIndex].SetActive(true);

					// enable inputs of settings menu if necessary
					if (!SettingsMenuScript.controls.UI.enabled) {
						SettingsMenuScript.controls.UI.Enable();
					}
                    break;
            }
        }
    }
}
