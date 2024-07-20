using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Scorer : MonoBehaviour
{
    public Rhymer rhymer;
    public LyricScoreBox box;
    public TMP_Text previous;
    private List<string> previousWords = new List<string>();

    public Slider scoreSlider;
    public ScoreDecal sd;

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
                    box.newWord(LYRICSCORE.REPEAT);

                    scoreSlider.value += (playerScoring ? 1 : -1) * SCORE_REPEAT;
                    sd.newWord(SCORE_REPEAT);
                }
                else if (rhymed && waspos)
                {
                    // get a nice amount for a match
                    box.newWord(LYRICSCORE.MATCH_BOTH);

                    scoreSlider.value += (playerScoring ? 1 : -1) * SCORE_MATCH_BOTH;
                    sd.newWord(SCORE_MATCH_BOTH);
                }
                else if (rhymed)
                {
                    // get no points for keeping the rhyme alive.
                    box.newWord(LYRICSCORE.RHYME_ONLY);

                    scoreSlider.value += (playerScoring ? 1 : -1) * SCORE_RHYME_ONLY;
                    sd.newWord(SCORE_RHYME_ONLY);
                }
                else if (waspos)
                {
                    // resets the rhyme. If there's at least two words, get points for them.
                    if(previousWords.Count > 1)
                    {
                        scoreSlider.value += (playerScoring ? 1 : -1) * (SCORE_FLOW_BONUS * previousWords.Count);
                        sd.flowBonus((SCORE_FLOW_BONUS * previousWords.Count), previousWords.Count);
                    }

                    box.newWord(LYRICSCORE.POS_ONLY);
                    previous.text = "";
                    previousWords = new List<string>();
                }
                else
                {
                    // lose many points for a nonsenstical word, but the rhyme is at least set..
                    scoreSlider.value += (playerScoring ? 1 : -1) * SCORE_NO_MATCH;
                    sd.newWord(SCORE_NO_MATCH);

                    box.newWord(LYRICSCORE.NO_MATCH);
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
            scoreSlider.value += (playerScoring ? 1 : -1) * SCORE_NOT_WORD;
            previous.text = "";
            previousWords = new List<string>();
            box.newWord(LYRICSCORE.NOT_WORD);
        }

        if(isEnd)
        {
            // cash out words before next turn.
            if (previousWords.Count > 1)
            {
                scoreSlider.value += (playerScoring ? 1 : -1) * (SCORE_FLOW_BONUS * previousWords.Count);
                sd.flowBonus((SCORE_FLOW_BONUS * previousWords.Count), previousWords.Count);
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
}
