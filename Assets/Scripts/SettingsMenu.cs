using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

// menu des paramètres
public class SettingsMenu : MonoBehaviour //video 17
{
	public AudioMixer audioMixer;		// mixer audio du jeu
	public Dropdown resolutionDropdown;	// menu déroulant des résolutions
	Resolution[] resolutions;			// liste des résolutions


	/* au démarrage :
	* récupère les résolutions de l'écran avec un taux de rafraichissement à 60Hz
	* crée le menu déroulant correspondant à ces résolutions
	* applique la résolution par défaut de l'écran
	* passe en plein écran
	*/
	public void Start() {
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
	}


	/* règle le volume à 'volume'
	*/
	public void SetVolume(float volume) {
		audioMixer.SetFloat("volume", volume);
	}


	/* active ou désactive le plein écran
	*/
	public void SetFullScreen(bool isFullScreen) {
		Screen.fullScreen = isFullScreen;
	}


	/* règle la résolution sur resolutions['resolutionIndex']
	*/
	public void SetResolution(int resolutionIndex) {
		Resolution resolution = resolutions[resolutionIndex];
		Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
	}
}
