using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// handles the rap UI (user input + prompts the user to type the lyrics, and boss)
public class RapUI : MonoBehaviour
{
    // rhymer
    public Rhymer rhymer;
    // scorer
    public Scorer scorer;

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

    // which bar are we on?
    int barIndex = 0;

    // when did this bar start?
    float barStartTime;
    
    //how long are bars? (const'd for now)
    const float barLength = 2f;

    bool bossBars = true;

    // for AI
    private string lastWord = "rap";


    // Start is called before the first frame update
    void Start()
    {
        foreach (var v in songAsset.text.Split("\n"))
        {
            bars.Add(v);
        }
        barStartTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        // get the current bar and bar character.
        var currentBar = bars[barIndex];
        var barProgress = ((Time.time - barStartTime) / barLength);
        int barCharacter = (int)(barProgress * (float)currentBar.IndexOf("["));

        // get text inside []
        var targetPOS = currentBar.Substring(currentBar.IndexOf("[") + 1, currentBar.IndexOf("]") - currentBar.IndexOf("[") - 1);

        if (!bossBars)
        {
            checkKeyPress();
        }
        else
        {
            AIWord(targetPOS);
        }



        // submit word and go to next bar.
        if (Time.time - barStartTime >= barLength)
        {
            lastWord = word;
            scorer.submitWord(word.ToUpper(),targetPOS);
            barIndex += 1;
            barStartTime += barLength;
            word = "";
            return;
        }

        // is the current typed word valid?
        var validWord = rhymer.validWord(word.ToUpper());

        // subsitute [A] with typed word. green if the word's valid
        wordTextBox.text = (validWord ? "<color=green>" : "<color=red>") + word + "<color=orange>";
        var textSubbed = currentBar.Replace("[n.]", "");
        textSubbed = textSubbed.Replace("[v.]", "");
        textSubbed = textSubbed.Replace("[adj.]", "");

        // do we use these?
        textSubbed = textSubbed.Replace("[adv.]", "");
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
        if (word == "")
        {
            if (Random.Range(0, 4) == 2)
            {
                word = rhymer.getRandomWord(targetPOS);
            }
            else if (word == "")
            {
                // lots of attempts.
                word = rhymer.getRandomWordRhymesWith(targetPOS, lastWord, 100000);

            }
        }
    }

}
