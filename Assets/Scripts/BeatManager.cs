using FMOD;
using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatManager : MonoBehaviour
{
    // use a getter if you need this!
    private float BPM = 90;

    private float bars = 0;
    private bool followingTrack = true;
    private float FMOD_START = 0;
    [SerializeField] public float SPEEDUP = 1f;

    public void Start()
    {

    }

    public float getBarLen()
    {
        return (4 * (60f / BPM));
    }

    bool firstFrame = true;
    bool isPlaying = false;
    public void Update()
    {
        PLAYBACK_STATE state;
        GetComponent<StudioEventEmitter>().EventInstance.getPlaybackState(out state);

        if (!isPlaying && state == PLAYBACK_STATE.PLAYING)
        {
            isPlaying = true;
            FMOD_START = Time.time;

            // start right music and BPM
            switch (APPSTATE.LEVEL)
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

        if(firstFrame)
        {       
            firstFrame = false;
            GetComponent<StudioEventEmitter>().Play();
        }

        //UnityEngine.Debug.Log(bars);
        if (APPSTATE.TUTORIAL_STAGE == 5)
        {
            GetComponent<StudioEventEmitter>().EventInstance.setParameterByName("rapping", 1);
        }
        bars = (FMOD_START == 0 ? 0 : ((Time.time - FMOD_START) / getBarLen()));
    }

        // define phrase as 2 bars
        public float getPhrase()
    {
        // cheating, don't tell
        return SPEEDUP * (bars / 2);
    }
}
