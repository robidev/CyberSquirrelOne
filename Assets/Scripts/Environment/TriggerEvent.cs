using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvent : MonoBehaviour
{
    public UnityEvent Event;
    public string filter = "";
    public LayerMask layer;
    bool eventHasbeenTriggered = false;
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D other)
    {
        if((filter == "" || other.name == filter) && (layer.value == 0 || layer.value == (1 << other.gameObject.layer)))
        {
            Event.Invoke();
            eventHasbeenTriggered = true;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        //Debug.Log("ontrigger:" + other.name);
        if(eventHasbeenTriggered == false)
        {
            //Debug.Log("hasbeenshown:" + other.name);
            if((filter == "" || other.name == filter) && (layer.value == 0 || layer.value == (1 << other.gameObject.layer)))
            {
                //Debug.Log("triggered");
                Event.Invoke();
                eventHasbeenTriggered = true;
            }
        }
    }

    public void TriggerEventNow()
    {
        Event.Invoke();
        eventHasbeenTriggered = true;
    }

}
