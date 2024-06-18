using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class RhythmBar : MonoBehaviour
{
   
    public RectTransform leftBeatMarkerPrefab;   
    public RectTransform rightBeatMarkerPrefab;   
    public int numberOfBeats = 4;   
    public float beatDistance = 100f;   
    public float centerX = 0f;   

    private List<RectTransform> leftBeatMarkers;
    private List<RectTransform> rightBeatMarkers;

    void Start()
    {
        leftBeatMarkers = new List<RectTransform>();
        rightBeatMarkers = new List<RectTransform>();

       
        for (int i = 0; i < numberOfBeats; i++)
        {
            RectTransform beatMarker = Instantiate(leftBeatMarkerPrefab, transform);
            leftBeatMarkers.Add(beatMarker);
        }
 
        for (int i = 0; i < numberOfBeats; i++)
        {
            RectTransform beatMarker = Instantiate(rightBeatMarkerPrefab, transform);
            rightBeatMarkers.Add(beatMarker);
        }
    }

    void Update()
    {
        float songPositionInBeats = RhythmManager.Instance.songPositionInBeats;
        float beatPosition = songPositionInBeats % numberOfBeats;

        float fullCycleLength = numberOfBeats * beatDistance;

        for (int i = 0; i < leftBeatMarkers.Count; i++)
        {
            // Calculate the normalized position from 0 to 1
            float normalizedPosition = Mathf.Repeat((beatPosition + i) / numberOfBeats, 1.0f);

            // Move left markers from left to center
            float positionLeft = Mathf.Lerp(-fullCycleLength / 2, centerX, normalizedPosition);
            leftBeatMarkers[i].anchoredPosition = new Vector2(positionLeft, 0);

            // Move right markers from right to center
            float positionRight = Mathf.Lerp(fullCycleLength / 2, centerX, normalizedPosition);
            rightBeatMarkers[i].anchoredPosition = new Vector2(positionRight, 0);
        }
    }
}
