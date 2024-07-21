using FMOD;
using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatManager : MonoBehaviour
{
    // use a getter if you need this!
    private float BPM;

    private float bars = 0;
    private float lastCurr = 0;
    private bool followingTrack = true;
    public float offset;


    [SerializeField] public float SPEEDUP = 1f;

    public void Start()
    {
        // start right music and BPM
        switch(APPSTATE.LEVEL)
        {
            case 0:
                BPM = 80;
                GetComponent<StudioEventEmitter>().EventInstance.setParameterByNameWithLabel("state", "lvl1");
                GetComponent<StudioEventEmitter>().EventInstance.setParameterByName("rapping", APPSTATE.TUTORIAL_STAGE >= 0 ? 0 : 1);

                break;
                case 1:
                BPM = 100;
                GetComponent<StudioEventEmitter>().EventInstance.setParameterByNameWithLabel("state", "lvl2");
                GetComponent<StudioEventEmitter>().EventInstance.setParameterByName("rapping", 1);
                break;
                case 2:
                BPM = 120;
                GetComponent<StudioEventEmitter>().EventInstance.setParameterByNameWithLabel("state", "lvl3");
                GetComponent<StudioEventEmitter>().EventInstance.setParameterByName("rapping", 1);
                break;
        }
    }

    // ask FMOD where the beat is.
    int GetEventPos_FromEventEmitter(FMODUnity.StudioEventEmitter _eventEmitter)
    {
        RESULT res;
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
        //UnityEngine.Debug.Log(bars);
        if (APPSTATE.TUTORIAL_STAGE == 6)
        {
            GetComponent<StudioEventEmitter>().EventInstance.setParameterByName("rapping", 1);
        }

        if (followingTrack)
        {
            // this is VERY fucking sketch.
            int curr = GetEventPos_FromEventEmitter(GetComponent<FMODUnity.StudioEventEmitter>());

            // if we skip ahead (to a seperate track) offste.
            if (lastCurr == 0 && curr != 0)
            {
                offset = (curr - Time.deltaTime);
            }

            // skip happened in music.. dead reckon now.
            if (Mathf.Abs(curr - lastCurr) > 1000 && lastCurr != 0)
            {
                followingTrack = false;
                // add an extra time.
                lastCurr += (1000 * Time.deltaTime);
                return;
            }

            bars = (0.5f + (((curr - offset) / 1000f) / getBarLen()));
            lastCurr = curr;
        }
        else
        {
            // we lost main track so use unity...
            UnityEngine.Debug.Log("dead reckoning.");
            lastCurr += (1000 * Time.deltaTime);
            bars = ((lastCurr / 1000f) / getBarLen());
        }
    }

        // define phrase as 2 bars
        public float getPhrase()
    {
        // cheating, don't tell
        return SPEEDUP * (bars / 2);
    }
}
