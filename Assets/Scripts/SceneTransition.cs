using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    public GameObject bossDown;
    public GameObject YouLose;
    public GameObject YouWin;
    public GameObject PlayerReady;

    private bool fallen = false;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }


    public void setState(string state)
    {
        fallen = true;
        GetComponentInChildren<Animator>().SetTrigger("Fall");
        bossDown.SetActive(state == "bossDown");
        YouLose.SetActive(state == "youLose");
        YouWin.SetActive(state == "youWin");
        PlayerReady.SetActive(state == "ready");
    }

    public void clear()
    {
        if (fallen)
        {
            GetComponentInChildren<Animator>().SetTrigger("Rise");
        }
        fallen = false;
    }
}
