using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip[] playlist;
    public AudioSource audioSource;
    private int musicIndex = 0;

    void Start()
    {
      audioSource.clip = playlist[0];
      audioSource.Play();
    }

    void Update()
    {
      if(!audioSource.isPlaying)
      {
        PlayNextSong();
      }
    }

    void PlayNextSong()
    {
      audioSource.clip = playlist[(++ musicIndex) % playlist.Length];
      audioSource.Play();
    }
}
