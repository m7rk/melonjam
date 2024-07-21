using FmodStudioEventEmitter.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatManager : MonoBehaviour
{
    [SerializeField] public float BPM = 90f;

    private float bars = 0;
    private int loops;
    private float lastCurr = 0;


    [SerializeField] public float SPEEDUP = 1f;

    // ask FMOD where the beat is.
    int GetEventPos_FromEventEmitter(FMODUnity.StudioEventEmitter _eventEmitter)
    {
        FmodStudioEventEmitter.RESULT res;
        int _eventPos;
        EventInstance[] _events;
        EventInstance eventInstance;
        res = FMODUnity.RuntimeManager.GetEventDescription(_eventEmitter.EventReference).getInstanceList(out _events); // .Events;
        eventInstance = _events[0]; //return the first instance of the event
        res = eventInstance.getTimelinePosition(out _eventPos);
        return _eventPos;
    }

    public float getBarLen()
    {
        return (4 * (60f / BPM));
    }

    public void Update()
    {
        int curr = GetEventPos_FromEventEmitter(GetComponent<FMODUnity.StudioEventEmitter>());
        if (curr < lastCurr)
        {
            loops += 1;
        }
        bars = loops + ((curr/1000f) / getBarLen());
        lastCurr = curr;


    }

    // define phrase as 2 bars
    public float getPhrase()
    {
        // cheating, don't tell
        return SPEEDUP * (bars / 2);
    }
}
