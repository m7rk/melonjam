using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationBeatSync : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Animator>().speed = APPSTATE.getBPMForLevel() / 60.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
