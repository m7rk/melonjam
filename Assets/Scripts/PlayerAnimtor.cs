using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimtor : MonoBehaviour
{
    public GameObject idle;
    public GameObject up;
    public GameObject left;
    public GameObject right;
    public GameObject down;
    public GameObject hurt;

    float animTime = 0f;

    // Start is called before the first frame update

    public void setAnim(string anim)
    {
        animTime = 0.15f;
        idle.SetActive(false);
        up.SetActive(false);
        left.SetActive(false);
        right.SetActive(false);
        down.SetActive(false);
        hurt.SetActive(false);

        if (anim == "up")
        {
            up.SetActive(true);
        }
        else if (anim == "left")
        {
            left.SetActive(true);
        }
        else if (anim == "right")
        {
            right.SetActive(true);
        }
        else if (anim == "down")
        {
            down.SetActive(true);
        }
        else if (anim == "hurt")
        {
            hurt.SetActive(true);
        }
    }
    // Update is called once per frame
    void Update()
    {
        animTime -= Time.deltaTime;
        if(animTime <= 0)
        {
            idle.SetActive(true);
            up.SetActive(false);
            left.SetActive(false);
            right.SetActive(false);
            down.SetActive(false);
            hurt.SetActive(false);
        }
    }
}
