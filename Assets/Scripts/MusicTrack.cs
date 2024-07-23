using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// the theory is.. this track should always be playing. just pause it to avoid buffering problems.
public class MusicTrack : MonoBehaviour
{
    public StudioEventEmitter music;
    public StudioEventEmitter ambient;
    private bool trackStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        if(!trackStarted)
        {
            music.Play();
            trackStarted = true;
        }
        DontDestroyOnLoad(gameObject);
    }

    public void restartTrackBasedOnLevel()
    {
        music.EventInstance.setTimelinePosition(0);
        music.EventInstance.setVolume(1);
        switch (APPSTATE.LEVEL)
        {
            case 0:
                music.EventInstance.setParameterByNameWithLabel("state", "lvl1");
                setRapping(APPSTATE.TUTORIAL_STAGE < 0);
                break;
            case 1:
                music.EventInstance.setParameterByNameWithLabel("state", "lvl2");
                setRapping(true);
                break;
            case 2:
                music.EventInstance.setParameterByNameWithLabel("state", "lvl3");
                setRapping(true);
                break;
        }
        FMODUnity.RuntimeManager.StudioSystem.flushCommands();
    }


    public void setRapping(bool rapping)
    {
        music.EventInstance.setParameterByName("rapping", rapping ? 1 : 0);
    }

    public void setMenu(bool amb)
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Menu", amb ? 1 : 0);
    }

    public void setVolume(float volume)
    {
        music.EventInstance.setVolume(volume);
    }

    public void setFlow(float flow)
    {
        music.EventInstance.setParameterByName("Flow_bar", flow);
    }

    public void setSyllables(float syllables)
    {
        music.EventInstance.setParameterByName("Syllables", syllables);
    }

    public void setRhyme(string rhyme)
    {
        music.EventInstance.setParameterByNameWithLabel("Rhyme", rhyme);
    }



    //UnityEngine.Debug.Log(bars);




}
