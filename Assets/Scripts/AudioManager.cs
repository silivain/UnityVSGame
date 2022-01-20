using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // TODO this script is not used yet
    // should start audio when a game is started
    // TODO set different playlists for menus, ingame, type of lvls etc

    public AudioClip[] playlist;    // audio list
    public AudioSource audioSource; // audio source
    private int musicIndex = 0;     // current index in the audio list

    void Start()
    {
      audioSource.clip = playlist[0];  // loads first song in audio list 'playlist'
      audioSource.Play();              // plays it using 'audioSource'
    }

    void Update()
    {
      // check if a song is currently playing
      // if it aint the case, plays next song in 'playlist'
      if(!audioSource.isPlaying)
      {
        PlayNextSong();
      }
    }

    /* Plays next song in 'playlist' using 'audioSource'
    * loop on audio list, last song -> first song
    */
    void PlayNextSong()
    {
      audioSource.clip = playlist[(++ musicIndex) % playlist.Length];
      audioSource.Play();
    }
}
