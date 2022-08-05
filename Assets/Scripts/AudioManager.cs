using UnityEngine;
using UnityEngine.Audio;

// gère l'audio de la scène
public class AudioManager : MonoBehaviour
{
	public AudioClip[] playlist;    			// contient les musique de la scène actuelle
	public AudioSource audioSource;				// AudioManager
	private int musicIndex = 0;					// index actuel dans le tab 'playlist'
	public GameObject currentSceneManager;		// CurrentSceneManager
	private CountDownTimer countDownTimer;		// script CountDownTimer
	public bool playlistIsStarted = false;		// vrai si une musique est en lecture
	public bool isOn = false;					// ??
	//private bool playlistAsBegun = false;		// ??
	public AudioMixerGroup soundEffectMixer;	// AudioMixerGroup gérant les effets sonores

	public static AudioManager instance;		// instance de la classe


	/* évite les doublons -> classe "statique"
	* récupère le script 'CountDownTimer' de la scène
	*/
    private void Awake() {
      if (instance != null) {
        Debug.LogWarning("Il y a plus d'une instance de AudioManager dans la scène.");
        return;
      }
      instance = this;

	  countDownTimer = currentSceneManager.GetComponent<CountDownTimer>();
    }


	/* Lance la première musique de la playlist 'playlist'
	*/
	void Start() {
		audioSource.clip = playlist[0];
	}


	/* Lance la musique du compte à rebours (première musique de 'playlist')
	* une fois le compte à rebours terminé, lance la musique de la scène
	*/
	void Update() {
		if(!audioSource.isPlaying && countDownTimer.start) {
			audioSource.Play();
		}
		if(!playlistIsStarted && !countDownTimer.start) {
			isOn = true;						// TODO : ???
			audioSource.clip = playlist[1];
			audioSource.Play();
			playlistIsStarted = true;			// TODO : pq un bool alors qu'on a 'musicIndex' ?
		}
	}


	/* lance la prochaine musique de 'playlist'
	* boucle sur 'playlist'
	*/
	void PlayNextSong() {
		audioSource.clip = playlist[(++ musicIndex) % playlist.Length];
		audioSource.Play();
	}


	/* tuto unity 2D fr ep #22
	* finir l'ep (moitié environ)
	*/
	public AudioSource PlayClipAt(AudioClip clip, Vector3 pos) {

		GameObject tempGO = new GameObject("TempAudio");
		tempGO.transform.position = pos;

		AudioSource audioSource = tempGO.AddComponent<AudioSource>();
		audioSource.clip = clip;
		audioSource.outputAudioMixerGroup = soundEffectMixer;

		audioSource.Play();
		Destroy(tempGO, clip.length);
		return audioSource;
	}
}
