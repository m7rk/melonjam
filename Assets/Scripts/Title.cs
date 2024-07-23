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
        FindFirstObjectByType<SceneTransition>().clear();
        FindFirstObjectByType<MusicTrack>().setMenu(true);
        FindFirstObjectByType<MusicTrack>().setFlow(50);

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ToMain()
    {
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }

    public void StartLevel()
    {
        FindFirstObjectByType<SceneTransition>().setState("ready");
        FindFirstObjectByType<MusicTrack>().setMenu(false);
        FindFirstObjectByType<MusicTrack>().setVolume(0);
        Invoke("ToMain", 1);
    }
}
