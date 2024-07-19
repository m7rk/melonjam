using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LyricScoreBox : MonoBehaviour
{
    public TMP_Text text;


    public void newWord(Scorer.LYRICSCORE s)
    {
        var stext = "";
        switch(s)
        {
            case Scorer.LYRICSCORE.MATCH_BOTH:
                stext = "<color=green>GREAT!";
                break;
            case Scorer.LYRICSCORE.POS_ONLY:
                stext = "<color=blue>NEW RHYME.";
                break;
            case Scorer.LYRICSCORE.RHYME_ONLY:
                stext = "<color=orange>SKETCH";
                break;
            case Scorer.LYRICSCORE.NO_MATCH:
                stext = "<color=red>WHAT?";
                break;
            case Scorer.LYRICSCORE.NOT_WORD:
                stext = "<color=red>TOO SLOW!";
                break;

        }
        text.text = stext;
        text.color = Color.white;
    }

    public void Update()
    {
        text.color = Color.Lerp(text.color, Color.clear, Time.deltaTime);
    }
}
