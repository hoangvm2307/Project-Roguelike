using UnityEngine;
public struct Beat
{
    public float time;
    public int index;
    public float accuracy;
    public BeatState state;

    public Beat(float time, int index)
    {
        this.time = time;
        this.index = index;
        this.accuracy = 0;
        this.state = BeatState.Upcoming;
    }
}
public enum BeatState
{
    Upcoming,
    Current,
    Missed,
    Hit
}