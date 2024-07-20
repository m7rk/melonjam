using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public PlayerAnimtor pa;
    public BeatManager bm;
    public int lastBar;

    public GameObject enemyPrefab;


    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            pa.setAnim("left");
        }
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            pa.setAnim("right");
        }
        if(Input.GetKeyDown (KeyCode.UpArrow))
        {
            pa.setAnim("up");
        }
        if(Input.GetKeyDown (KeyCode.DownArrow))
        {
            pa.setAnim("down");
        }


        // spawn an enemy to the left of the origin when a new bar starts.
        if(lastBar != ((int)bm.getPhrase()))
        {
            // spawn enemy... the indicator is at x = -3

            // enemies ALWAYS travel 2 unity unit per bar??

            var go = Instantiate(enemyPrefab);
            go.transform.position = new Vector3(-3 + 16,-0.5f, 0);
            lastBar = ((int)bm.getPhrase());
        }
    }


}
