using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer 
{
    public float timerLength {  get; private set; }
    public float startTime {  get; private set; }

    public Timer (float timerLength)
    {
        this.timerLength = timerLength;
        this.startTime = Mathf.NegativeInfinity;
    }

    public void ResetTimer()
    {
        startTime = Time.time;
    }

    public bool IsDone()
    {
        return (Time.time > startTime + timerLength);

    }

    public float TimePercentage()
    {
        return Mathf.InverseLerp(startTime, startTime + timerLength, Time.time);
    }

    public void SetLenght(float lenght)
    {
        timerLength = lenght;
    }

    public float GetLength()
    {
        return timerLength;
    }


}
