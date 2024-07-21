using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FMODwaiter : MonoBehaviour
{
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
            gameObject.SetActive(false);
            SceneManager.LoadScene("Title", LoadSceneMode.Single);
        }
    }
}
