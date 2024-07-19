using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Scorer : MonoBehaviour
{
    public Rhymer rhymer;
    public LyricScoreBox box;
    public TMP_Text previous;

    string lastWord = null;

    public enum LYRICSCORE
    {
        MATCH_BOTH,
        RHYME_ONLY, // wrong POS
        POS_ONLY, // doesn't rhyme
        NO_MATCH, // neither
        NOT_WORD // didnt finish or not a word
    }


    public void submitWord(string word, string targetpos)
    {
        if (rhymer.validWord(word))
        {
            if (lastWord != null)
            {
                var rhymed = rhymer.rhymes(lastWord, word);
                var waspos = rhymer.isPOS(word, targetpos);

                if (rhymed && waspos)
                {
                    box.newWord(LYRICSCORE.MATCH_BOTH);
                }
                else if (rhymed)
                {
                    box.newWord(LYRICSCORE.RHYME_ONLY);
                }
                else if (waspos)
                {
                    box.newWord(LYRICSCORE.POS_ONLY);
                    previous.text = "";
                }
                else
                {
                    box.newWord(LYRICSCORE.NO_MATCH);
                    previous.text = "";
                }
            }
            // this is the new word.
            lastWord = word;
            previous.text += (word + "\n");
        }
        else
        {
            box.newWord(LYRICSCORE.NOT_WORD);
        }
    }
}
