using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//60,000 words
public class Rhymer : MonoBehaviour
{
    public TextAsset cmufile;
    public TextAsset partofspeech;

    private Dictionary<string, List<string[]>> tocmu = new Dictionary<string, List<string[]>>();
    private Dictionary<string, string[]> topos = new Dictionary<string, string[]>();

    // Start is called before the first frame update
    void Start()
    {
        createPOS();
        createCMU();
    }

    private void createPOS()
    {
        var lines = partofspeech.text.Split('\n');
        foreach (var line in lines)
        {
            var split = line.Split("|");
            var word = split[0];
            var pos = split[1].Split(",");
            topos[word.ToUpper()] = pos;
        }
    }

    int hitCount = 0;
    private void createCMU()
    {
        // read the whole text file
        var lines = cmufile.text.Split('\n');
        foreach (var line in lines)
        {
            var split = line.Split("  ");
            var word = split[0];
            if (topos.ContainsKey(word))
            {
                if (word.Contains("("))
                {
                    word = word.Split("(")[0];
                }

                var cmu = split[1].Split(" ");
                if (!tocmu.ContainsKey(word))
                {
                    tocmu[word] = new List<string[]>();
                }
                tocmu[word].Add(cmu);


                hitCount++;
            }
        }
    }

    public bool validWord(string word)
    {
        return tocmu.ContainsKey(word) && topos.ContainsKey(word);
    }

    public bool rhymes(string word1, string word2)
    {

        foreach (var cmu in tocmu[word1.ToUpper()])
        {
            foreach (var cmu2 in tocmu[word2.ToUpper()])
            {
                // if the last syllables are the same, it's a rhyme.
                if (cmu[cmu.Length - 1] == cmu2[cmu2.Length - 1])
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool isPOS(string word1, string target)
    {
        foreach(var v in topos[word1.ToUpper()])
        {
            // corpus has extra space.
            if(v.Trim() == target.Trim())
            {
                return true;
            }
        }
        return false;
    }


}
