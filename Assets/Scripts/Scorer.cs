using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Scorer : MonoBehaviour
{
    public Rhymer rhymer;
    public LyricScoreBox feedbackBox;
    public TMP_Text previous;
    private List<string> previousWords = new List<string>();

    public SpriteBar scoreSlider;
    public ScoreDecal scoreDecal;

    public enum LYRICSCORE
    {
        MATCH_BOTH,
        RHYME_ONLY, // wrong POS
        POS_ONLY, // doesn't rhyme
        REPEAT, 
        NO_MATCH, // neither
        NOT_WORD // didnt finish or not a word
    }

    private const int SCORE_REPEAT = -300;
    private const int SCORE_MATCH_BOTH = 500;
    private const int SCORE_RHYME_ONLY = 0;
    private const int SCORE_FLOW_BONUS = 200;
    private const int SCORE_NO_MATCH = -400;
    private const int SCORE_NOT_WORD = -500;

    private const int SCORE_MINION_HIT = 50;
    private const int SCORE_MINION_LATE = 25;
    private const int SCORE_MINION_MISS = -25;

    public const int SCORE_MAX = 10000;
    private int currentScore = SCORE_MAX / 2;

    public void applyScore(int amt, bool playerScoring)
    {
        currentScore += (playerScoring ? 1 : -1) * amt;
        scoreSlider.set((float)(currentScore / (float)SCORE_MAX));
    }

    public void submitWord(string word, string targetpos, bool playerScoring, bool isEnd)
    {
        if (rhymer.validWord(word))
        {
            if (getLastRhyme() != null)
            {
                var rhymed = rhymer.rhymes(getLastRhyme(), word);
                var waspos = rhymer.isPOS(word, targetpos,false);

                if(previousWords.Contains(word))
                {
                    // lose some points for a repeat, but keep combo.
                    feedbackBox.newWord(LYRICSCORE.REPEAT);
                    applyScore(SCORE_REPEAT, playerScoring);
                    scoreDecal.newWord(SCORE_REPEAT);
                }
                else if (rhymed && waspos)
                {
                    // get a nice amount for a match
                    feedbackBox.newWord(LYRICSCORE.MATCH_BOTH);
                    applyScore(SCORE_MATCH_BOTH, playerScoring);
                    scoreDecal.newWord(SCORE_MATCH_BOTH);
                }
                else if (rhymed)
                {
                    // get no points for keeping the rhyme alive.
                    feedbackBox.newWord(LYRICSCORE.RHYME_ONLY);
                    applyScore(SCORE_RHYME_ONLY, playerScoring);
                    scoreDecal.newWord(SCORE_RHYME_ONLY);
                }
                else if (waspos)
                {
                    // resets the rhyme. If there's at least two words, get points for them.
                    if(previousWords.Count > 1)
                    {
                        applyScore(SCORE_FLOW_BONUS * previousWords.Count, playerScoring);
                        scoreDecal.flowBonus((SCORE_FLOW_BONUS * previousWords.Count), previousWords.Count);
                    }

                    feedbackBox.newWord(LYRICSCORE.POS_ONLY);
                    previous.text = "";
                    previousWords = new List<string>();
                }
                else
                {
                    // lose many points for a nonsenstical word, but the rhyme is at least set..
                    applyScore(SCORE_NO_MATCH, playerScoring);
                    scoreDecal.newWord(SCORE_NO_MATCH);

                    feedbackBox.newWord(LYRICSCORE.NO_MATCH);
                    previous.text = "";
                    previousWords = new List<string>();
                }
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
        }

        if(isEnd)
        {
            // cash out words before next turn.
            if (previousWords.Count > 1)
            {
                applyScore(SCORE_FLOW_BONUS * previousWords.Count, playerScoring);
                scoreDecal.flowBonus((SCORE_FLOW_BONUS * previousWords.Count), previousWords.Count);
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
