using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : MonoBehaviour
{
    public string direction;
    private bool slain = false;

    public GameObject move;
    public GameObject dead;
    // Start is called before the first frame update
    void Start()
    {
        
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

    // Update is called once per frame
    void Update()
    {
        if (!slain)
        {
            this.transform.position += Vector3.left * (4 * Time.deltaTime / FindFirstObjectByType<BeatManager>().getBarLen());
        }
    }
}
