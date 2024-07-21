using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : MonoBehaviour
{
    public string direction;
    private bool slain = false;

    public GameObject move;
    public GameObject dead;

    public GameObject upArrow;
    public GameObject downArrow;
    public GameObject leftArrow;
    public GameObject rightArrow;

    public SpriteRenderer[] srOutline;
    public SpriteRenderer[] srColor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void setSpriteOrder(int idx)
    {
        srOutline[0].sortingOrder = idx;
        srOutline[1].sortingOrder = idx;

        srColor[0].sortingOrder = idx - 1;
        srColor[1].sortingOrder = idx - 1;
    }

    public void Slay()
    {
        slain = true;
        Invoke("delete", 0.2f);
        move.SetActive(false);
        dead.SetActive(true);
    }

    public void delete()
    {
        Destroy(this.gameObject);
    }

    public void setDirection(string d)
    {
        direction = d;
        if(d == "up")
        {
            upArrow.SetActive(true);
        }
        else if(d == "down")
        {
            downArrow.SetActive(true);
        }
        else if(d == "left")
        {
            leftArrow.SetActive(true);
        }
        else if(d == "right")
        {
            rightArrow.SetActive(true);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (!slain)
        {
            this.transform.position += Vector3.left * (4 * Time.deltaTime / FindFirstObjectByType<BeatManager>().getBarLen());
        }
    }
}
