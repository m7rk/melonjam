using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//60,000 words
public class Rhymer : MonoBehaviour
{
    public TextAsset cmufile;
    public TextAsset partofspeech;
    public TextAsset mostcommonwords;

    private Dictionary<string, List<string[]>> tocmu = new Dictionary<string, List<string[]>>();
    private Dictionary<string, string[]> topos = new Dictionary<string, string[]>();
    private Dictionary<string, int> mcw = new Dictionary<string, int>();

    // Start is called before the first frame update
    void Start()
    {
        createPOS();
        createCMU();
        createMCW();
    }

    private void createMCW()
    {
        var lines = mostcommonwords.text.Split('\n');
        int i = 0;
        foreach (var line in lines)
        {
            if (!mcw.ContainsKey(line))
            {
                mcw[line.ToUpper()] = i;
            }
            ++i;
        }
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
        foreach (var cmu1 in tocmu[word1.ToUpper()])
        {
            foreach (var cmu2 in tocmu[word2.ToUpper()])
            {
                // for each prononciation of both words...
                int cmp = 1;
                while(true)
                {
                    // if OOB it's a rhyme.
                    if(cmp == 1+cmu1.Length || cmp == 1+cmu2.Length)
                    {
                        return true;
                    }

                    var pho1 = cmu1[cmu1.Length - cmp];
                    var pho2 = cmu2[cmu2.Length - cmp];

                    // staring from the end, compare syllables until both match on a stress.
                    if (pho1 == pho2)
                    {
                        // matched phoneme... if it's a stress phoneme it's a rhyme!
                        if (pho1.Contains("0") || pho1.Contains("1") || pho1.Contains("2") || pho1.Contains("3") || pho1.Contains("4"))
                        {
                            return true;
                        }
                        // continue
                        cmp += 1;
                    }
                    else
                    {
                        break;
                        // no rhyme
                    }
                }

            }
        }
        return false;
    }

    // strict - don't use words that have more than one POS (good for AI)
    public bool isPOS(string word1, string target, bool strict)
    {

        if(strict && topos[word1.ToUpper()].Length > 2)
        {
            return false;
        }

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

    public string getRandomWord(string pos, int intelligence)
    {
        var keys = tocmu.Keys.ToList();
        var random = new System.Random();
        while (true)
        {
            var word = keys[random.Next(keys.Count)];
            if (isPOS(word, pos, true) && aiKnowsWord(word,intelligence))
            {
                return word;
            }
        }

    }

    public bool aiKnowsWord(string word, int intelligence)
    {
        return mcw.ContainsKey(word) && mcw[word] < intelligence;
    }

    public string getRandomWordRhymesWith(string pos, string lastWord, int intelligence)
    {
        var keys = tocmu.Keys.ToList();
        List<string> words = new List<string>();
        foreach(var word in keys) 
        {
            if (isPOS(word, pos, true) && rhymes(word, lastWord) && aiKnowsWord(word, intelligence) && word != lastWord)
            {
                words.Add(word);
            }
        }
        if(words.Count > 0)
        {
            // return random from list
            var random = new System.Random();
            return words[random.Next(words.Count)];
        }
        return getRandomWord(pos, intelligence);
    }
}
