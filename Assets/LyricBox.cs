using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// handles user input and prompts the user to type the lyrics.
public class LyricBox : MonoBehaviour
{
    // rhymer
    public Rhymer rhymer;
    // scorer
    public Scorer scorer;

    // current song textfile
    public TextAsset songAsset;
    // text box
    public TMP_Text text;

    // bars of song
    private List<string> bars = new List<string>();

    // current word typed
    private string word = "";

    // which bar are we on?
    int barIndex = 0;

    // when did this bar start?
    float barStartTime;
    
    //how long are bars? (const'd for now)
    const float barLength = 5f;


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
        checkKeyPress();
        var currentBar = bars[barIndex];

        // get the current bar and bar character.
        var barProgress = ((Time.time - barStartTime) / barLength);
        int barCharacter = (int)(barProgress * (float)currentBar.IndexOf("["));

        // get text inside []
        var targetPOS = currentBar.Substring(currentBar.IndexOf("[") + 1, currentBar.IndexOf("]") - currentBar.IndexOf("[") - 1);


        // submit word and go to next bar.
        if (Time.time - barStartTime >= barLength)
        {
            scorer.submitWord(word.ToUpper(),targetPOS);
            barIndex += 1;
            barStartTime += barLength;
            word = "";
            return;
        }

        // is the current typed word valid?
        var validWord = rhymer.validWord(word.ToUpper());

        // subsitute [A] with typed word. green if the word's valid
        var wordText = (validWord ? "<color=green>" : "<color=red>") + word + "<color=orange>";
        var textSubbed = currentBar.Replace("[n.]", wordText);
        textSubbed = textSubbed.Replace("[v.]", wordText);
        textSubbed = textSubbed.Replace("[adj.]", wordText);

        // do we use these?
        textSubbed = textSubbed.Replace("[adv.]", wordText);
        textSubbed = textSubbed.Replace("[prep.]", wordText);
        textSubbed = textSubbed.Replace("[conj.]", wordText);
        textSubbed = textSubbed.Replace("[pron.]", wordText);

        text.text = "<color=black>" + textSubbed.Substring(0, barCharacter) + "<color=orange>" + textSubbed.Substring(barCharacter);
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

}
