using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatManager : MonoBehaviour
{
    [SerializeField] public float BPM = 90f;
    public static float BPMmeter;
    public static int BeatNumber = 1;
    public static float LastBeatTime;
    private Timer beatTimer;

    private void Start()
    {
        beatTimer = new(60 / BPM);
        beatTimer.ResetTimer();
    }

    void Update()
    {
        BPMmeter = BPM;
        if (beatTimer.IsDone())
        {
            BeatNumber++;
            beatTimer.ResetTimer();
            LastBeatTime = Time.time;   
        }
    }
}
