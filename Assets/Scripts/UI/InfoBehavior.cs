using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoBehavior : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            foreach(Toggle t in GetComponentsInChildren<Toggle>())
            {
                Debug.Log(t.name + " " + t.isOn);
                if(t.isOn == false)
                {
                    t.isOn = true;
                    break;
                }
            }
        }
    }
}
