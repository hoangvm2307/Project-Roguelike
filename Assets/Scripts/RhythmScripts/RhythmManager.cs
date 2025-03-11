using UnityEngine;
using TMPro; // Đảm bảo đã import TMPro

public class RhythmManager : SingletonBase<RhythmManager>
{
    public AudioSource musicSource;
    public float bpm = 134;
    public float beatTolerance = 0.1f;

    [Header("Visual Feedback")]
    public GameObject feedbackTextPrefab; // Prefab cho text hiển thị kết quả
    public Transform feedbackSpawnPoint; // Vị trí hiển thị feedback
    public Color perfectColor = new Color(1f, 0.8f, 0f); // Màu vàng cho perfect
    public Color goodColor = new Color(0f, 0.8f, 0.2f); // Màu xanh lá cho good
    public Color missedColor = new Color(0.8f, 0f, 0f); // Màu đỏ cho missed

    private float secondsPerBeat;
    private float songPosition;
    public float songPositionInBeats;
    private float dspSongTime;

    public event System.Action OnPerfectBeat;
    public event System.Action OnGoodBeat;
    public event System.Action OnMissedBeat;

    public float beatAccuracy { get; private set; }

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
        beatAccuracy = 1f - (beatError / beatTolerance);

        if (beatError < beatTolerance * 0.3f)
        {
            OnPerfectBeat?.Invoke();
            ShowFeedbackText("PERFECT!", perfectColor);
            return true;
        }
        else if (beatError < beatTolerance)
        {
            OnGoodBeat?.Invoke();
            ShowFeedbackText("GOOD", goodColor);
            return true;
        }
        else
        {
            OnMissedBeat?.Invoke();
            ShowFeedbackText("MISSED", missedColor);
            return false;
        }
    }

    // Hiển thị text feedback
    private void ShowFeedbackText(string text, Color color)
    {
        if (feedbackTextPrefab == null || feedbackSpawnPoint == null) return;

        GameObject feedbackObj = Instantiate(feedbackTextPrefab, feedbackSpawnPoint.position, Quaternion.identity);
        feedbackObj.transform.SetParent(feedbackSpawnPoint);

        TextMeshProUGUI textComponent = feedbackObj.GetComponent<TextMeshProUGUI>();
        if (textComponent != null)
        {
            textComponent.text = text;
            textComponent.color = color;
            textComponent.fontSize = 2;
        }

        // Đảm bảo feedback sẽ tự biến mất
        Destroy(feedbackObj, 0.3f);
    }
}