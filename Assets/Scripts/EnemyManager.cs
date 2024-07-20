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

    public GameObject enemyPrefab;

    private Queue<Tuple<float,Minion>> minions = new Queue<Tuple<float, Minion>>();

    // measures in phrases
    private const float THRESH_CLOSE = 0.02f; //0 - 0.03
    private const float THRESH_MISS = 0.04f; // 0.03 - 0.06
    private const float THRESH_EARLY = 0.08f; // 0.06 - 0.12


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
            float delay = 12;

            var go = Instantiate(enemyPrefab);
            go.transform.position = new Vector3(-2 + delay ,-0.5f, 0);
            lastBar = ((int)bm.getPhrase());

            minions.Enqueue(new Tuple<float, Minion>(lastBar + delay / 8, go.GetComponent<Minion>()));
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
