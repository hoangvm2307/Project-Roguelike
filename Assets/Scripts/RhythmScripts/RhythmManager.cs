using UnityEngine;

public class RhythmManager : MonoBehaviour
{
    public static RhythmManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    public AudioSource musicSource;
    public float bpm = 134;
    public float beatTolerance = 0.1f;

    private float secondsPerBeat;
    private float songPosition;
    public float songPositionInBeats;
    private float dspSongTime;

    void Start()
    {
        secondsPerBeat = 60f / bpm;
        dspSongTime = (float)AudioSettings.dspTime;
        musicSource.Play();
    }

    void Update()
    {
        songPosition = (float)(AudioSettings.dspTime - dspSongTime);
        songPositionInBeats = songPosition / secondsPerBeat;
    }

    public bool IsPlayerHit()
    {
        float beatError = Mathf.Abs(songPositionInBeats - Mathf.Round(songPositionInBeats));

        if (beatError < beatTolerance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
