using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<SceneTransition>().clear();

    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<StudioEventEmitter>().Stop();
    }

    public void StartLevel()
    {
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }
}
