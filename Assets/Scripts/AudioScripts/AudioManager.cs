using UnityEngine;

public class AudioManager : SingletonBase<AudioManager>
{
    public AudioSource sfxSource;

    [Header("Audio Clip")]
    public AudioClip itemPickupSound;
    public AudioClip otherSound;


    public void PlayItemPickupSound()
    {
        sfxSource.PlayOneShot(itemPickupSound);
    }

    public void PlayOtherSound()
    {
        sfxSource.PlayOneShot(otherSound);
    }


}
