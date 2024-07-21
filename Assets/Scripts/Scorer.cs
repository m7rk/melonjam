using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scorer : MonoBehaviour
{
    public Rhymer rhymer;
    public LyricScoreBox feedbackBox;
    public TMP_Text previous;
    private List<string> previousWords = new List<string>();
    public BeatManager beatManager;

    public SpriteBar scoreSlider;
    public ScoreDecal scoreDecal;


    public void Start()
    {
    }

    public enum LYRICSCORE
    {
        MATCH_BOTH,
        MATCH_BOTH_LONG,
        RHYME_ONLY, // wrong POS
        POS_ONLY, // doesn't rhyme
        REPEAT, 
        NO_MATCH, // neither
        NOT_WORD // didnt finish or not a word
    }

    private const int SCORE_REPEAT = -300;
    private const int SCORE_MATCH_BOTH = 500;
    private const int SCORE_MATCH_BOTH_LONG = 700;
    private const int SCORE_RHYME_ONLY = 0;
    private const int SCORE_FLOW_BONUS = 200;
    private const int SCORE_NO_MATCH = -400;
    private const int SCORE_NOT_WORD = -500;

    private const int SCORE_MINION_HIT = 100;
    private const int SCORE_MINION_LATE = 25;
    private const int SCORE_MINION_MISS = -25;

    public const int SCORE_MAX = 10000;
    private int currentScore = SCORE_MAX / 2;

    public void applyScore(int amt, bool playerScoring)
    {
        currentScore += (playerScoring ? 1 : -1) * amt;
        scoreSlider.set((float)(currentScore / (float)SCORE_MAX));
        beatManager.GetComponent<StudioEventEmitter>().EventInstance.setParameterByName("Flow_Bar", 100 * (float)(currentScore / (float)SCORE_MAX));

        if (currentScore <= 0)
        {
            //lose. go to title
            SceneManager.LoadScene("Title", LoadSceneMode.Single);
        }
        if(currentScore >= SCORE_MAX)
        {
            //win. go to intermission
            SceneManager.LoadScene("Intermission", LoadSceneMode.Single);

        }
    }

    public void Update()
    {
        if(APPSTATE.TUTORIAL_STAGE == 1)
        {
            currentScore = 7000;
        }
    }

    public void submitWord(string word, string targetpos, bool playerScoring, bool isEnd, int syllableCount)
    {
        if (rhymer.validWord(word))
        {
            beatManager.GetComponent<StudioEventEmitter>().EventInstance.setParameterByName("Rhyme", syllableCount);
            if (getLastRhyme() != null)
            {
                var rhymed = rhymer.rhymes(getLastRhyme(), word);
                var waspos = rhymer.isPOS(word, targetpos,false);

                if(previousWords.Contains(word))
                {
                    // lose some points for a repeat, but keep combo.
                    feedbackBox.newWord(LYRICSCORE.REPEAT);
                    applyScore(SCORE_REPEAT, playerScoring);
                    scoreDecal.newWord(SCORE_REPEAT, playerScoring);
                    beatManager.GetComponent<StudioEventEmitter>().EventInstance.setParameterByNameWithLabel("Rhyme", "Incorrect");
                }
                else if (rhymed && waspos)
                {
                    if (syllableCount <= 3)
                    {
                        // get a nice amount for a match
                        feedbackBox.newWord(LYRICSCORE.MATCH_BOTH);
                        applyScore(SCORE_MATCH_BOTH, playerScoring);
                        scoreDecal.newWord(SCORE_MATCH_BOTH, playerScoring);
                    }
                    else
                    {
                        // get a nice amount for a match
                        feedbackBox.newWord(LYRICSCORE.MATCH_BOTH_LONG);
                        applyScore(SCORE_MATCH_BOTH_LONG, playerScoring);
                        scoreDecal.newWord(SCORE_MATCH_BOTH_LONG, playerScoring);
                    }

                    beatManager.GetComponent<StudioEventEmitter>().EventInstance.setParameterByNameWithLabel("Rhyme", "Rhyme");
                }
                else if (rhymed)
                {
                    // get no points for keeping the rhyme alive.
                    feedbackBox.newWord(LYRICSCORE.RHYME_ONLY);
                    applyScore(SCORE_RHYME_ONLY, playerScoring);
                    scoreDecal.newWord(SCORE_RHYME_ONLY, playerScoring);

                    beatManager.GetComponent<StudioEventEmitter>().EventInstance.setParameterByNameWithLabel("Rhyme", "Incorrect");
                }
                else if (waspos)
                {
                    // resets the rhyme. If there's at least two words, get points for them.
                    if(previousWords.Count > 1)
                    {
                        applyScore(SCORE_FLOW_BONUS * previousWords.Count, playerScoring);
                        scoreDecal.flowBonus((SCORE_FLOW_BONUS * previousWords.Count), previousWords.Count, playerScoring);
                    }

                    beatManager.GetComponent<StudioEventEmitter>().EventInstance.setParameterByNameWithLabel("Rhyme", "New_Rhyme");
                    feedbackBox.newWord(LYRICSCORE.POS_ONLY);
                    previous.text = "";
                    previousWords = new List<string>();
                }
                else
                {
                    // lose many points for a nonsenstical word, but the rhyme is at least set..
                    applyScore(SCORE_NO_MATCH, playerScoring);
                    scoreDecal.newWord(SCORE_NO_MATCH, playerScoring);
                    beatManager.GetComponent<StudioEventEmitter>().EventInstance.setParameterByNameWithLabel("Rhyme", "Incorrect");

                    feedbackBox.newWord(LYRICSCORE.NO_MATCH);
                    previous.text = "";
                    previousWords = new List<string>();
                }
            }
            else
            {
                // we set a new rhyme.
                beatManager.GetComponent<StudioEventEmitter>().EventInstance.setParameterByNameWithLabel("Rhyme", "New_Rhyme");
                feedbackBox.newWord(LYRICSCORE.POS_ONLY);
            }
            previous.text += (word + " ");
            previousWords.Add(word);
        }
        else
        {
            // lose everything.
            applyScore(SCORE_NOT_WORD, playerScoring);
            previous.text = "";
            previousWords = new List<string>();
            feedbackBox.newWord(LYRICSCORE.NOT_WORD);
            beatManager.GetComponent<StudioEventEmitter>().EventInstance.setParameterByNameWithLabel("Rhyme", "Incorrect");
        }

        if(isEnd)
        {
            // cash out words before next turn.
            if (previousWords.Count > 1)
            {
                applyScore(SCORE_FLOW_BONUS * previousWords.Count, playerScoring);
                scoreDecal.flowBonus((SCORE_FLOW_BONUS * previousWords.Count), previousWords.Count, playerScoring);
            }
            previous.text = "";
            previousWords = new List<string>();
        }
    }

    public string getLastRhyme()
    {
        if (previousWords.Count > 0)
        {
            return previousWords[previousWords.Count - 1];
        }
        else
        {
            return null;
        }
    }
    public void minionHit()
    {
        applyScore(SCORE_MINION_HIT, true);
    }

    public void minionLate()
    {
        applyScore(SCORE_MINION_LATE, true);
    }

    public void minionMiss()
    {
        applyScore(SCORE_MINION_MISS, true);

    }
}
