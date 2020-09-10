using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HMI : MonoBehaviour
{
    public bool hacked = false;

    public GameObject powerline;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleHacked()
    {
        hacked = !hacked;
        if(hacked == true)
        {
            powerline.GetComponent<Powerline>().isPowered = false;
            Debug.Log("powerline disabled");
        }
        else
        {
            powerline.GetComponent<Powerline>().isPowered = true;
            Debug.Log("powerline enabled");
        }
    }
}
