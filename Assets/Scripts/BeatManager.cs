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
    private float FMOD_START = 0;
    private MusicTrack mt;
    [SerializeField] public float SPEEDUP = 1f;

    private int firstFrame = 60;

    public void Start()
    {
        // get the music player
        mt = FindFirstObjectByType<MusicTrack>();
    }

    public float getBarLen()
    {
        return (4 * (60f / BPM));
    }

    // wait 30 frames.

    // 0.5 second delay maybe..
    public void Update()
    {
        firstFrame -= 1;
        if (firstFrame == 0)
        {
            mt.restartTrackBasedOnLevel();
            BPM = APPSTATE.getBPMForLevel();
            // set BPM manually.
            FMOD_START = Time.time;
        }

        if(firstFrame < 0)
        {
            bars = ((Time.time - FMOD_START) / getBarLen());
        }
    }

        // define phrase as 2 bars
        public float getPhrase()
    {
        // cheating, don't tell
        return SPEEDUP * (bars / 2);
    }
}
