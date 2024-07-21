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
    private List<string> baseBars = new List<string>();


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

    public Material sideMaterial;


    // Start is called before the first frame update
    void Start()
    {
        foreach (var v in songAsset.text.Split("\n"))
        {
            baseBars.Add(v);
        }
        shuffleBars();

    }


    void shuffleBars()
    {
        bars.Clear();
        // pick 8
        while(bars.Count < 8)
        {
            var bar = baseBars[Random.Range(0, baseBars.Count)];
            if (!bars.Contains(bar))
            {
                bars.Add(bar);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        sideMaterial.SetFloat("_WhoPlays01", Random.RandomRange(0f,1f));
        if (APPSTATE.TUTORIAL_STAGE >= 0 && APPSTATE.TUTORIAL_STAGE < 5)
        {
            // keep up.
            totalBarIndex = (int)bm.getPhrase();
            return;
        }

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
            scorer.submitWord(word.ToUpper(), targetPOS, !bossBars, totalBarIndex % 8 == 0, rhymer.getSyllableCount(word.ToUpper()));
            word = "";
            if (totalBarIndex % 8 == 0)
            {
                // switch
                shuffleBars();
                bossBars = !bossBars;
                sideMaterial.SetFloat("WhoPlays01", bossBars ? 1 : 0);
                lyricBarIndex = 0;
            }
            return;

        }

        // is the current typed word valid?
        var validWord = rhymer.validWord(word.ToUpper());

        // subsitute [A] with typed word. green if the word's valid
        wordTextBox.text = (bossBars ? "<color=#999999>" : (validWord ? "<color=green>" : "<color=red>")) + word + "<color=orange>";

        // supported so far - nouns, adjectives, transitive verbs
        var textSubbed = currentBar.Replace("[n.]", "");
        textSubbed = textSubbed.Replace("[a.]", "");
        textSubbed = textSubbed.Replace("[v. i.]", "");

        // not yet!
        textSubbed = textSubbed.Replace("[v. t.]", "");
        textSubbed = textSubbed.Replace("[prep.]", "");
        textSubbed = textSubbed.Replace("[conj.]", "");
        textSubbed = textSubbed.Replace("[pron.]", "");

        lyricTextBox.text = (bossBars ? "<color=#999999>" : "<color=black>") + textSubbed.Substring(0, barCharacter) + (bossBars ? "<color=#bbbbbb>" : "<color=orange>") + textSubbed.Substring(barCharacter);
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
                var dice = Random.Range(0, 6);

                if (dice == 1)
                {
                    string[] confuse = new string[] { "ERRR", "UHHH", "UMMM", "HMMM" };
                    word = confuse[Random.Range(0, confuse.Length)];
                }
                // throw in a noun randomly
                else if (dice == 2)
                {
                    word = rhymer.getRandomWord("n.");
                }
                // start a new rhyme
                else if (dice == 3)
                {
                    word = rhymer.getRandomWord(targetPOS);
                }
                else
                {
                    word = rhymer.getRandomWordRhymesWith(targetPOS, scorer.getLastRhyme());
                }

            }
        }
    }

}
