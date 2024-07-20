using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public PlayerAnimtor pa;

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
    }
}
