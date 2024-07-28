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
    public BeatManager beatManager;

    // words already used by player and boss
    private List<string> playerWords = new List<string>();
    private List<string> bossWords = new List<string>();

    // previous rhymes for the current string
    public TMP_Text previousRhymesText;
    private List<string> previousRhymes = new List<string>();

    public SpriteBar scoreSlider;
    public ScoreDecal scoreDecal;
    public ScoreDecal minionDecal;


    public GameObject[] bossEnable;
    public GameObject[] defeatEnable;

    public StudioEventEmitter swordHit;
    public StudioEventEmitter winJingle;
    public StudioEventEmitter loseJingle;

    public void Start()
    {
        bossEnable[APPSTATE.LEVEL].SetActive(true);
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

    private const int SCORE_REPEAT = -400;
    private const int SCORE_MATCH_BOTH = 400;
    private const int SCORE_MATCH_BOTH_LONG = 800;
    private const int SCORE_RHYME_ONLY = 0;
    private const int SCORE_FLOW_BONUS = 200;
    private const int SCORE_NO_MATCH = -400;
    private const int SCORE_NOT_WORD = -500;

    private const int SCORE_MINION_HIT = 50;
    private const int SCORE_MINION_LATE = 25;
    private const int SCORE_MINION_MISS = -100;

    public const int SCORE_MAX = 10000;
    private int currentScore = SCORE_MAX / 2;

    public bool roundOver = false;

    public void gameLoseAnimStart()
    {
        FindObjectOfType<SceneTransition>().setState("youLose");
        FindFirstObjectByType<MusicTrack>().setMenu(true);
        Invoke("toTitle", 4f);
    }

    public void roundWinAnimStart()
    {
        FindObjectOfType<SceneTransition>().setState("bossDown");
        Invoke("toIntermission", 2f);
    }

    public void gameWinAnimStart()
    {
        FindObjectOfType<SceneTransition>().setState("youWin");
        Invoke("toTitle", 2f);
    }

    public void toTitle()
    {
        SceneManager.LoadScene("Title", LoadSceneMode.Single);
    }

    public void toIntermission()
    {
        SceneManager.LoadScene("Intermission", LoadSceneMode.Single);
    }

    public void applyScore(int amt, bool playerScoring)
    {
        if(roundOver)
        {
            return;
        }

        currentScore += (playerScoring ? 1 : -1) * amt;
        scoreSlider.set((float)(currentScore / (float)SCORE_MAX));
        var currflow = (float)(currentScore / (float)SCORE_MAX);
        // bias a little lower
        var currflowbias = Mathf.Pow(currflow, 1.5f);
        FindFirstObjectByType<MusicTrack>().setFlow(100 * currflowbias);

        if (currentScore <= 0)
        {
            loseJingle.Play();
            FindFirstObjectByType<MusicTrack>().setRapping(false);
            FindFirstObjectByType<MusicTrack>().setMenu(true);
            roundOver = true;
            gameLoseAnimStart();
            APPSTATE.LEVEL = 0;
            //lose. go to title

        }
        if(currentScore >= SCORE_MAX)
        {
            winJingle.Play();
            FindFirstObjectByType<MusicTrack>().setMenu(true);
            FindFirstObjectByType<MusicTrack>().setRapping(false);
            roundOver = true;
            bossEnable[APPSTATE.LEVEL].SetActive(false);
            defeatEnable[APPSTATE.LEVEL].SetActive(true);
            if (APPSTATE.LEVEL == 2)
            {
                // game reset.
                APPSTATE.LEVEL = 0;
                Invoke("gameWinAnimStart", 3f);
            }
            else
            {

                APPSTATE.LEVEL++;
                //win. go to intermission
                Invoke("roundWinAnimStart", 3f);
            }


        }
    }

    public void Update()
    {
        if(APPSTATE.TUTORIAL_STAGE == 1)
        {
            currentScore = 6000;
        }
    }

    public List<string> getUsedWordList(bool playerScoring)
    {
        return playerScoring ? playerWords : bossWords;
    }



    public void submitWord(string word, string targetpos, bool playerScoring, bool isEnd, int syllableCount)
    {
        // check if word in dict
        if (rhymer.validWord(word))
        {
            bool wordWasRepeat = false;
            FindFirstObjectByType<MusicTrack>().setSyllables(syllableCount);

            // check if rhyme scheme exists
            if (getLastRhyme() != null)
            {
                var rhymed = rhymer.rhymes(getLastRhyme(), word);
                var waspos = rhymer.isPOS(word, targetpos,false);

                // the word is repeated. Don't add it to any list and penalize player
                if(getUsedWordList(playerScoring).Contains(word))
                {
                    wordWasRepeat = true;
                    // lose some points for a repeat, but keep combo.
                    feedbackBox.newWord(LYRICSCORE.REPEAT);
                    applyScore(SCORE_REPEAT, playerScoring);
                    scoreDecal.newWord(SCORE_REPEAT, playerScoring);
                    FindFirstObjectByType<MusicTrack>().setRhyme("Incorrect");
                }
                else if (rhymed && waspos)
                {
                    // it's a rhyme that makes sense.
                    if (syllableCount <= 3)
                    {
                        feedbackBox.newWord(LYRICSCORE.MATCH_BOTH);
                        applyScore(SCORE_MATCH_BOTH, playerScoring);
                        scoreDecal.newWord(SCORE_MATCH_BOTH, playerScoring);
                    }
                    else
                    {
                        // bonus for 4 syl. word
                        feedbackBox.newWord(LYRICSCORE.MATCH_BOTH_LONG);
                        applyScore(SCORE_MATCH_BOTH_LONG, playerScoring);
                        scoreDecal.newWord(SCORE_MATCH_BOTH_LONG, playerScoring);
                    }

                    FindFirstObjectByType<MusicTrack>().setRhyme("Rhyme");
                }
                // it's a rhyme, but not right POS.
                else if (rhymed)
                {
                    // get no points for keeping the rhyme alive.
                    feedbackBox.newWord(LYRICSCORE.RHYME_ONLY);
                    applyScore(SCORE_RHYME_ONLY, playerScoring);
                    scoreDecal.newWord(SCORE_RHYME_ONLY, playerScoring);
                    FindFirstObjectByType<MusicTrack>().setRhyme("Incorrect");
                }
                // resets the rhyme. If there's at least two words, get points for them.
                else if (waspos)
                {
                    if(previousRhymes.Count > 1)
                    {
                        applyScore(SCORE_FLOW_BONUS * previousRhymes.Count, playerScoring);
                        scoreDecal.flowBonus((SCORE_FLOW_BONUS * previousRhymes.Count), previousRhymes.Count, playerScoring);
                    }

                    FindFirstObjectByType<MusicTrack>().setRhyme("New_Rhyme");
                    feedbackBox.newWord(LYRICSCORE.POS_ONLY);
                    previousRhymesText.text = "";
                    previousRhymes = new List<string>();
                }
                else
                {
                    // lose many points for a nonsenstical word, but the rhyme is at least set..
                    applyScore(SCORE_NO_MATCH, playerScoring);
                    scoreDecal.newWord(SCORE_NO_MATCH, playerScoring);
                    FindFirstObjectByType<MusicTrack>().setRhyme("Incorrect");

                    feedbackBox.newWord(LYRICSCORE.NO_MATCH);
                    previousRhymesText.text = "";
                    previousRhymes = new List<string>();
                }
            }
            else
            {
                // we set a new rhyme.
                FindFirstObjectByType<MusicTrack>().setRhyme("New_Rhyme");
                feedbackBox.newWord(LYRICSCORE.POS_ONLY);
            }

            if (!wordWasRepeat)
            {
                previousRhymesText.text += (word + " ");
                previousRhymes.Add(word);
                getUsedWordList(playerScoring).Add(word);
            }
        }
        else
        {
            // lose everything.
            applyScore(SCORE_NOT_WORD, playerScoring);
            scoreDecal.newWord(SCORE_NOT_WORD, playerScoring);
            previousRhymesText.text = "";
            previousRhymes = new List<string>();
            feedbackBox.newWord(LYRICSCORE.NOT_WORD);
            FindFirstObjectByType<MusicTrack>().setRhyme("Incorrect");
        }

        if(isEnd)
        {
            // cash out words before next turn.
            if (previousRhymes.Count > 1)
            {
                applyScore(SCORE_FLOW_BONUS * previousRhymes.Count, playerScoring);
                scoreDecal.flowBonus((SCORE_FLOW_BONUS * previousRhymes.Count), previousRhymes.Count, playerScoring);
            }
            previousRhymesText.text = "";
            previousRhymes = new List<string>();
        }
    }

    public string getLastRhyme()
    {
        if (previousRhymes.Count > 0)
        {
            return previousRhymes[previousRhymes.Count - 1];
        }
        else
        {
            return null;
        }
    }
    public void minionHit()
    {
        applyScore(SCORE_MINION_HIT, true);
        swordHit.Play();
        minionDecal.newWord(SCORE_MINION_HIT, true);
    }

    public void minionLate()
    {
        applyScore(SCORE_MINION_LATE, true);
        swordHit.Play();
        minionDecal.newWord(SCORE_MINION_LATE, true);
    }

    public void minionMiss()
    {
        applyScore(SCORE_MINION_MISS, true);
        minionDecal.newWord(SCORE_MINION_MISS, true);

    }
}
