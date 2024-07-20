using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreDecal : MonoBehaviour
{
    public TMP_Text text;

    private Vector3 start;
    public void Start()
    {
        start = this.transform.position;
    }

    public void newWord(int score)
    {
        var stext = (score > 0 ? "+" : "") + score;
        text.text = stext;
        text.color = Color.black;
        this.transform.position = start;
    }

    public void flowBonus(int score, int cnt)
    {
        var stext = cnt + " rhyme flow! " + ((score > 0 ? "+" : "") + score);
        text.text = stext;
        text.color = Color.black;
        this.transform.position = start;
    }


    public void Update()
    {
        text.color = Color.Lerp(text.color, new Color(0,0,0,0), 0.7f * Time.deltaTime);
        text.transform.position = Vector3.MoveTowards(text.transform.position, text.transform.position + Vector3.up.normalized, 100 * Time.deltaTime);
    }
}
