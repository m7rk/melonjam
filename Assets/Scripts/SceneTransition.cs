using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    public GameObject bossDown;
    public GameObject YouLose;
    public GameObject YouWin;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }


    public void setState(string state)
    {
        GetComponentInChildren<Animator>().SetBool("fall", true);
        bossDown.SetActive(state == "bossDown");
        YouLose.SetActive(state == "youLose");
        YouWin.SetActive(state == "youWin");
    }

    public void clear()
    {
        GetComponentInChildren<Animator>().SetBool("rise", true);
    }
}
