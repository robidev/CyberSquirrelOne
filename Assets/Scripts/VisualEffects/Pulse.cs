using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulse : MonoBehaviour
{
    Color newColor;
    float vv;
    SpriteRenderer m_SpriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        newColor = m_SpriteRenderer.color;
        vv =  Mathf.PingPong(Time.time*2, 1);
        if(vv > 0.8)
        {
            newColor.a = vv;
        }
            
        else
        {
            newColor.a = 0;
        }
        //transform.localScale = new Vector3(Mathf.PingPong(Time.time, 1), Mathf.PingPong(Time.time, 1), transform.localScale.z);
        m_SpriteRenderer.color = newColor;
    }
}
