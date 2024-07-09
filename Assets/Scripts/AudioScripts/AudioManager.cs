using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource sfxSource; 
    private static AudioManager instance;
    [Header("Audio Clip")]
    public AudioClip itemPickupSound;
    public AudioClip otherSound;
    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AudioManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("AudioManager");
                    instance = obj.AddComponent<AudioManager>();
                }
            }
            return instance;
        }
    }

    public void PlayItemPickupSound()
    {
        sfxSource.PlayOneShot(itemPickupSound);
    }

    public void PlayOtherSound()
    {
        sfxSource.PlayOneShot(otherSound);
    }

 
}
