using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FMODwaiter : MonoBehaviour
{
    public GameObject musicSystem;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (FMODUnity.RuntimeManager.HasBankLoaded("Master"))
        {
            Debug.Log("Master Bank Loaded");
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Menu", 1);
            gameObject.SetActive(false);
            musicSystem.SetActive(true);
            SceneManager.LoadScene("Title", LoadSceneMode.Single);
        }
    }
}
