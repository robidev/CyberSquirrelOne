using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Clock : MonoBehaviour
{
    public Text TimeObj;
    public Text DateObj;

    // Update is called once per frame
    void Update()
    {
        TimeObj.text = System.DateTime.Now.ToString("HH:mm:ss");
        DateObj.text = System.DateTime.Now.ToString("ddd. MMM dd, yyyy");
    }
}
