using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// handles the rap UI (user input + prompts the user to type the lyrics, and boss)
public class RapManager : MonoBehaviour
{
    // rhymer
    public Rhymer rhymer;
    // scorer
    public Scorer scorer;
    // beat
    public BeatManager bm;

    // current song textfile
    public TextAsset songAsset;

    // text box for lyrics
    public TMP_Text lyricTextBox;

    // text box for typed word
    public TMP_Text wordTextBox;

    // bars of song
    private List<string> bars = new List<string>();

    // current word typed
    private string word = "";

    // which total bar are we on?
    int totalBarIndex = 0;

    // which bar from the lyrics?
    int lyricBarIndex = 0;

    bool bossBars = false;

    public SpriteBar playerSlider;
    public SpriteBar bossSlider;

    private const int INTEL = 10000;


    // Start is called before the first frame update
    void Start()
    {
        foreach (var v in songAsset.text.Split("\n"))
        {
            bars.Add(v);
        }

    }

    // Update is called once per frame
    void Update()
    {
        // get the current bar and bar character.
        var currentBar = bars[lyricBarIndex];
        var barProgress = bm.getPhrase() % 1;
        int barCharacter = (int)(barProgress * (float)currentBar.IndexOf("["));

        // get text inside []
        var targetPOS = currentBar.Substring(currentBar.IndexOf("[") + 1, currentBar.IndexOf("]") - currentBar.IndexOf("[") - 1);


        playerSlider.set( 1 - (bm.getPhrase() % 8 / 8f));
        bossSlider.set(1 - (bm.getPhrase() % 8 / 8f));

        if (!bossBars)
        {
            bossSlider.set(0);
        }
        else
        {
            playerSlider.set(0);

        }

        if (!bossBars)
        {
            checkKeyPress();
        }
        else
        {
            AIWord(targetPOS);
        }


        // submit word and go to next bar.
        if (totalBarIndex != (int)bm.getPhrase())
        {
            totalBarIndex += 1;
            lyricBarIndex += 1;
            scorer.submitWord(word.ToUpper(), targetPOS, !bossBars, totalBarIndex % 8 == 0);
            word = "";
            if (totalBarIndex % 8 == 0)
            {
                // switch
                bossBars = !bossBars;
                lyricBarIndex = 0;
            }
            return;

        }

        // is the current typed word valid?
        var validWord = rhymer.validWord(word.ToUpper());

        // subsitute [A] with typed word. green if the word's valid
        wordTextBox.text = (validWord ? "<color=green>" : "<color=red>") + word + "<color=orange>";

        // supported so far - nouns, adjectives, transitive verbs
        var textSubbed = currentBar.Replace("[n.]", "");
        textSubbed = textSubbed.Replace("[a.]", "");
        textSubbed = textSubbed.Replace("[v. i.]", "");

        // not yet!
        textSubbed = textSubbed.Replace("[v. t.]", "");
        textSubbed = textSubbed.Replace("[prep.]", "");
        textSubbed = textSubbed.Replace("[conj.]", "");
        textSubbed = textSubbed.Replace("[pron.]", "");

        lyricTextBox.text = "<color=black>" + textSubbed.Substring(0, barCharacter) + "<color=orange>" + textSubbed.Substring(barCharacter);
    }

    void checkKeyPress()
    {
        // this is filthy.
        for(int i = (int)KeyCode.A; i <= (int)KeyCode.Z; i++)
        {
            if (Input.GetKeyDown((KeyCode)i))
            {
                word += (char)i;
            }
        }

        if(Input.GetKeyDown(KeyCode.Backspace) && word.Length > 0)
        {
            word = word.Substring(0, word.Length - 1);
        }
    }

    // fine for now...
    void AIWord(string targetPOS)
    {
        if (word == "" && bm.getPhrase() % 1 > 0.5f)
        {
            if (scorer.getLastRhyme() == null)
            {
                word = rhymer.getRandomWord(targetPOS);
            }
            else if (word == "")
            {
                word = rhymer.getRandomWordRhymesWith(targetPOS, scorer.getLastRhyme());

            }
        }
    }

}
