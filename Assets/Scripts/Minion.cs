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

    public ParticleSystem upParticle;
    public ParticleSystem downParticle;
    public ParticleSystem leftParticle;
    public ParticleSystem rightParticle;

    public Color colourUp;
    public Color colourDown;
    public Color colourLeft;
    public Color colourRight;

    public SpriteRenderer[] srEyes;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void setSpriteOrder(int idx)
    {
        srOutline[0].sortingOrder = idx + 1;
        srOutline[1].sortingOrder = idx + 1;

        srEyes[0].sortingOrder = idx;
        srEyes[1].sortingOrder = idx;

        srColor[0].sortingOrder = idx - 1;
        srColor[1].sortingOrder = idx - 1;
    }

    public void Slay()
    {
        slain = true;
        Invoke("delete", 0.2f);
        move.SetActive(false);
        dead.SetActive(true);




        if (direction == "up")
        {
            upParticle.Play();
        }
        else if (direction == "down")
        {
            downParticle.Play();
        }
        else if (direction == "left")
        {
            leftParticle.Play();
        }
        else if (direction == "right")
        {
            rightParticle.Play();
        }

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
            srEyes[0].color = colourUp;
            srEyes[1].color = colourUp;

            upArrow.SetActive(true);
        }
        else if(d == "down")
        {
            srEyes[0].color = colourDown;
            srEyes[1].color = colourDown;

            downArrow.SetActive(true);
        }
        else if(d == "left")
        {
            srEyes[0].color = colourLeft;
            srEyes[1].color = colourLeft;

            leftArrow.SetActive(true);
        }
        else if(d == "right")
        {
            srEyes[0].color = colourRight;
            srEyes[1].color = colourRight;

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
