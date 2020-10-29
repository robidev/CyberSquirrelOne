using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurveillanceCameraControl : MonoBehaviour
{
    public bool scanning_enabled = false;
    public float rotFL = 1.0f;
    public float speed = 1.0f;
    public float offset = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(scanning_enabled)
        {
            transform.rotation = Quaternion.Euler(0,0,Mathf.PingPong(Time.time * speed, rotFL*2)-rotFL + offset);
        }
    }
}
