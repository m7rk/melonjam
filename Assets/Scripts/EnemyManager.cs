using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public PlayerAnimtor pa;
    public BeatManager bm;
    public Scorer sc;
    public int lastBar;

    private int spriteOrder = 30000;

    public GameObject enemyPrefab;

    private Queue<Tuple<float,Minion>> minions = new Queue<Tuple<float, Minion>>();

    // measures in phrases
    private const float THRESH_CLOSE = 0.02f; //0 - 0.03
    private const float THRESH_MISS = 0.04f; // 0.03 - 0.06
    private const float THRESH_EARLY = 0.08f; // 0.06 - 0.12


    public List<float> generateRhythms(int count)
    {
        // if tutorial stage = 1
        Debug.Log(APPSTATE.TUTORIAL_STAGE);
        if(APPSTATE.TUTORIAL_STAGE == 0)
        {
            return new List<float> { 8+0,  8+2,  8+4,  8+6, };
        }
        else if (APPSTATE.TUTORIAL_STAGE > 0 && APPSTATE.TUTORIAL_STAGE < 6)
        {
            return new List<float>();
        }

        if(count > 16)
        {
            count = 16;
        }

        // okay, generate all quater notes first.
        var quarts = new List<float>();
        var eigths = new List<float>();
        var sends = new List<float>();

        for (int i = 0; i < 8; i++)
        {
            quarts.Add(i);
            eigths.Add(i + 0.5f);
        }

        // first five notes should always be quarters.
        while(sends.Count < 6 && sends.Count < count)
        {
            // pick randomly
            var idx = UnityEngine.Random.Range(0, quarts.Count);
            sends.Add(quarts[idx]);
            quarts.RemoveAt(idx);
        }

        eigths.AddRange(quarts);
        // otherwise just merge the lists and pick some shit
        while(sends.Count < count)
        {
            var idx = UnityEngine.Random.Range(0, eigths.Count);
            sends.Add(eigths[idx]);
            eigths.RemoveAt(idx);
        }
        sends.Sort();

        return sends;
    }
    public void makeMinionWithDelay(float delay)
    {
        var go = Instantiate(enemyPrefab);
        go.transform.position = new Vector3(-2 + delay, -0.2f, 0);
        var dirs = new string[] { "up", "left", "right", "down" };
        // pick one randomly
        go.GetComponent<Minion>().setDirection(dirs[UnityEngine.Random.Range(0, 4)]);
        go.GetComponent<Minion>().setSpriteOrder(spriteOrder);
        spriteOrder -= 2;
        lastBar = ((int)bm.getPhrase());
        minions.Enqueue(new Tuple<float, Minion>(lastBar + (delay / 8), go.GetComponent<Minion>()));
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            pa.setAnim("left");
            trySwing("left");
        }
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            pa.setAnim("right");
            trySwing("right");
        }
        if(Input.GetKeyDown (KeyCode.UpArrow))
        {
            pa.setAnim("up");
            trySwing("up");
        }
        if(Input.GetKeyDown (KeyCode.DownArrow))
        {
            pa.setAnim("down");
            trySwing("down");
        }


        // spawn an enemy when a new bar starts.
        if(lastBar != ((int)bm.getPhrase()))
        {
            // spawn enemy... the indicator is at x = -3
            // enemies ALWAYS travel 2 unity unit per bar
            // they will arrive in four bars.
            float delay = 16;
            // we are free to add minions in increments of 1/2.

            // 5 is very hard.. use 1/3 when player rapping
            var rhythmCount = ((lastBar % 16) > 5 && (lastBar % 16) < 5+8) ? UnityEngine.Random.Range(4,7) : UnityEngine.Random.Range(1, 2);

            // add difficulty here..
            foreach(var v in generateRhythms(rhythmCount))
            {
                makeMinionWithDelay(delay + v);
            }
        }

        // dequeue all passed minons
        if (minions.Count > 0)
        {
            var first = minions.Peek();
            var diff = first.Item1 - bm.getPhrase();
            if(diff < -THRESH_MISS)
            {
                missFirstMinionInQueue();
            }
            //Debug.Log(first.Item1 + "/" + bm.getPhrase());
        }
    }

    public void missFirstMinionInQueue()
    {
        // dequeue minion, destroy them and hurt player
        Destroy(minions.Peek().Item2.gameObject);
        minions.Dequeue();
        sc.minionMiss();
        pa.setAnim("hurt");
    }

    public void trySwing(string direction)
    {
        if(minions.Count == 0)
        {
            return;
        }

        var curr = minions.Peek();
        var diff = Mathf.Abs(curr.Item1 - bm.getPhrase());

        // good timing
        if(diff < THRESH_CLOSE)
        {
            // solid hit
            if (curr.Item2.direction == direction)
            {
                minions.Peek().Item2.Slay();
                minions.Dequeue();
                sc.minionHit();
            }
            else
            {
                missFirstMinionInQueue();
            }
        }
        // late but valid
        else if(diff < THRESH_MISS)
        {
            if (curr.Item2.direction == direction)
            {
                minions.Peek().Item2.Slay();
                minions.Dequeue();
                sc.minionLate();
            }
            else
            {
                missFirstMinionInQueue();
            }
        }
        // miss
        else if (diff < THRESH_EARLY)
        {
            missFirstMinionInQueue();
        }

    }


}
