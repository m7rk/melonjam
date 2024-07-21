using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialHider : MonoBehaviour
{
    public Vector3 offscreenPos;
    private Vector3 originalPos;

    private bool hide = false;
    // Start is called before the first frame update
    void Awake()
    {
        originalPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(!hide)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, originalPos, Time.deltaTime);
        }
    }

    public void HideForTutorial()
    {
        hide= true;
        this.transform.position = offscreenPos;
    }

    public void ShowForTutorial()
    {
        hide = false;
    }
}
