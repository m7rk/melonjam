using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public BeatManager bm;
    public GameObject[] tutorialObjects;

    // Start is called before the first frame update
    void Start()
    {
        if(APPSTATE.TUTORIAL_STAGE == 0)
        {
            foreach(var v in FindObjectsByType<TutorialHider>(FindObjectsSortMode.None))
            {
                v.HideForTutorial();
            }
        }
        else
        {
            // hide all text.
            foreach (var v in tutorialObjects)
            {
                v.gameObject.SetActive(false);
            }
        }
    }

    public void setTutorialWindow()
    {
        if (APPSTATE.TUTORIAL_STAGE >= 0)
        {
            foreach (var v in tutorialObjects)
            {
                v.gameObject.SetActive(false);
            }
            tutorialObjects[APPSTATE.TUTORIAL_STAGE].SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // transitions.
        if (APPSTATE.TUTORIAL_STAGE >= 0)
        {   
            if (bm.getPhrase() > 11)
            {
                tutorialObjects[APPSTATE.TUTORIAL_STAGE].SetActive(false);
                APPSTATE.TUTORIAL_STAGE = -1;
            }
            else if (bm.getPhrase() > 10)
            {
                // score
                APPSTATE.TUTORIAL_STAGE = 9;
            }
            else if (bm.getPhrase() > 9)
            {
                // now 
                APPSTATE.TUTORIAL_STAGE = 8;
            }

            else if (bm.getPhrase() > 8)
            {
                // keep it up
                APPSTATE.TUTORIAL_STAGE = 7;
            }

            else if (bm.getPhrase() > 7)
            {
                // fill
                APPSTATE.TUTORIAL_STAGE = 6;
            }

            else if (bm.getPhrase() > 6)
            {
                // robot
                APPSTATE.TUTORIAL_STAGE = 5;
            }

            else if (bm.getPhrase() > 5)
            {
                // robot
                APPSTATE.TUTORIAL_STAGE = 4;
                foreach (var v in FindObjectsByType<TutorialHider>(FindObjectsSortMode.None))
                {
                    v.ShowForTutorial();
                }
            }
            else if (bm.getPhrase() > 3)
            {
                // beat.
                APPSTATE.TUTORIAL_STAGE = 3;
            }
            else if (bm.getPhrase() > 2)
            {
                // enemies!
                APPSTATE.TUTORIAL_STAGE = 2;
            }
            else if (bm.getPhrase() > 1)
            {
                // keyboard
                APPSTATE.TUTORIAL_STAGE = 1;
            }

        }
        setTutorialWindow();
    }
}
