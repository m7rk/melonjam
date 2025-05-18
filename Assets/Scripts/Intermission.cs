using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Intermission : MonoBehaviour
{
    public GameObject[] leftItems;
    public GameObject[] rightItems;

    public Sprite GrenOut;
    public Sprite GrenIn;

    public Sprite ClearOut;
    public Sprite ClearIn;

    public Sprite FillOut;
    public Sprite FillIn;

    private int[] bonusesL;
    private int[] bonusesR;
    public Button leftButton;
    public Button rightButton;

    private int sel = 0;

    public GameObject[] powerups;
    public StudioEventEmitter swordHit;

    private bool done = false;

    // Start is called before the first frame update
    void Start()
    {
        bonusesL = new int[2] { Random.Range(0, 3), Random.Range(0, 3)};

        bonusesR = new int[2] { Random.Range(0, 3), Random.Range(0, 3)};

        // make sure that no bonuses are the same
        for (int i = 0; i != 2;)
        {
            if (bonusesL.Contains(bonusesR[i]))
            {
                bonusesR[i] = Random.Range(0, 3);
            }
            else
            {
                i++;
            }
        }

        for(int i = 0; i < bonusesL.Length; i++)
        {
            setItemSprite(bonusesL[i], leftItems[i].GetComponent<Image>(), leftItems[i].transform.GetChild(0).GetComponent<Image>());
        }
        for (int i = 0; i < bonusesR.Length; i++)
        {
            setItemSprite(bonusesR[i], rightItems[i].GetComponent<Image>(), rightItems[i].transform.GetChild(0).GetComponent<Image>());
        }


        FindFirstObjectByType<SceneTransition>().clear();
        FindFirstObjectByType<MusicTrack>().setFlow(50);

        powerups[0].GetComponentsInChildren<TMP_Text>()[0].text = APPSTATE.GRENADE_COUNT.ToString();
        powerups[0].GetComponentsInChildren<TMP_Text>()[1].text = APPSTATE.GRENADE_COUNT.ToString();

        powerups[1].GetComponentsInChildren<TMP_Text>()[0].text = APPSTATE.CLEAR_COUNT.ToString();
        powerups[1].GetComponentsInChildren<TMP_Text>()[1].text = APPSTATE.CLEAR_COUNT.ToString();

        powerups[2].GetComponentsInChildren<TMP_Text>()[0].text = APPSTATE.FILL_COUNT.ToString();
        powerups[2].GetComponentsInChildren<TMP_Text>()[1].text = APPSTATE.FILL_COUNT.ToString();

    }
    public void pointerEnterR()
    {
        sel = 1;
    }

    public void pointerEnterL()
    {
        sel = -1;
    }

    public void pointerExit()
    {
        sel = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(done)
        {
            return;
        }

        if(sel < 0)
        {
            leftButton.transform.localScale = Vector3.Lerp(leftButton.transform.localScale,Vector3.one * 1.1f,2*Time.deltaTime);
            rightButton.transform.localScale = Vector3.Lerp(rightButton.transform.localScale, Vector3.one * 0.7f,2*Time.deltaTime);
        }
        else if (sel > 0)
        {
            leftButton.transform.localScale = Vector3.Lerp(leftButton.transform.localScale, Vector3.one * 0.7f, 2*Time.deltaTime);
            rightButton.transform.localScale = Vector3.Lerp(rightButton.transform.localScale, Vector3.one * 1.1f, 2*Time.deltaTime);
        }
        else
        {
            leftButton.transform.localScale = Vector3.Lerp(leftButton.transform.localScale, Vector3.one * 0.7f, 2*Time.deltaTime);
            rightButton.transform.localScale = Vector3.Lerp(rightButton.transform.localScale, Vector3.one * 0.7f,2*Time.deltaTime);
        }
    }

    public void ToMain()
    {
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }

    public void StartLevel()
    {
        if (done)
        {
            return;
        }
        if (sel > 0)
        {
            for (int i = 0; i != 2; ++i)
            {
                switch(bonusesL[i])
                {
                    case 0:
                        APPSTATE.GRENADE_COUNT += 1;
                        break;
                    case 1:
                        APPSTATE.CLEAR_COUNT += 1;
                        break;
                    case 2:
                        APPSTATE.FILL_COUNT += 1;
                        break;
                }
            }
        }
        if(sel < 0)
        {
            for (int i = 0; i != 2; ++i)
            {
                switch (bonusesR[i])
                {
                    case 0:
                        APPSTATE.GRENADE_COUNT += 1;
                        break;
                    case 1:
                        APPSTATE.CLEAR_COUNT += 1;
                        break;
                    case 2:
                        APPSTATE.FILL_COUNT += 1;
                        break;
                }
            }
        }

        powerups[0].GetComponentsInChildren<TMP_Text>()[0].text = APPSTATE.GRENADE_COUNT.ToString();
        powerups[0].GetComponentsInChildren<TMP_Text>()[1].text = APPSTATE.GRENADE_COUNT.ToString();

        powerups[1].GetComponentsInChildren<TMP_Text>()[0].text = APPSTATE.CLEAR_COUNT.ToString();
        powerups[1].GetComponentsInChildren<TMP_Text>()[1].text = APPSTATE.CLEAR_COUNT.ToString();

        powerups[2].GetComponentsInChildren<TMP_Text>()[0].text = APPSTATE.FILL_COUNT.ToString();
        powerups[2].GetComponentsInChildren<TMP_Text>()[1].text = APPSTATE.FILL_COUNT.ToString();

        swordHit.Play();
        done = true;

        FindFirstObjectByType<SceneTransition>().setState("ready");
        FindFirstObjectByType<MusicTrack>().setMenu(false);
        FindFirstObjectByType<MusicTrack>().setVolume(0);

        Invoke("ToMain", 1);
        
    }

    public void setItemSprite(int id, Image inner, Image outer)
    {
        switch(id)
        {
            case 0:
                outer.sprite = GrenOut;
                inner.sprite = GrenIn;
                break;
            case 1:
                outer.sprite = ClearOut;
                inner.sprite = ClearIn;
                break;
            case 2:
                outer.sprite = FillOut;
                inner.sprite = FillIn;
                break;

        }
    }
}
