using UnityEngine;
using UnityEngine.Audio;

// TODO this script is not used yet
// should start audio when a game is started
// TODO set different playlists for menus, ingame, type of lvls etc
public class AudioManager : MonoBehaviour
{
	public AudioClip[] playlist;    // audio list
	
	public AudioSource audioSource; // audio source
	private int musicIndex = 0;     // current index in the audio list
	public GameObject countdown; //pour check la fin du countdown
	public GameObject currentSceneManager; 
	public bool playlistIsStarted=false;
	public bool isOn=false;
	
	

	public AudioMixerGroup soundEffectMixer;

	public static AudioManager instance;

	// évite les doublons -> classe "statique"
    private void Awake()
    {
      if (instance != null)
      {
        Debug.LogWarning("Il y a plus d'une instance de CurrentSceneManager dans la scène.");
        return;
      }
      instance = this;
    }

	void Start() {
		audioSource.clip = playlist[0];  // loads first song in audio list 'playlist'
	}

	void Update() {

		if(!audioSource.isPlaying && currentSceneManager.GetComponent<CountDownTimer>().start==true)
		{
			audioSource.Play();
			
		}
		
		if(!playlistIsStarted && currentSceneManager.GetComponent<CountDownTimer>().go==true)
		{
			isOn= true;

			audioSource.clip = playlist[1]; 
			Debug.Log("go theme");             
			playlistIsStarted=true;
		}
		if(!audioSource.isPlaying && playlistIsStarted == true) {
			// check if a song is currently playing
			// if it aint the case, plays next song in 'playlist'
			PlayNextSong();
		}
	}

	/* Plays next song in 'playlist' using 'audioSource'
	* loop on audio list, last song -> first song
	*/
	void PlayNextSong() {
		audioSource.clip = playlist[(++ musicIndex) % playlist.Length];
		audioSource.Play();
	}

	// tuto unity 2D fr ep #22
	// finir l'ep (moitié environ)
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
