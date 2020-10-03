using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneDialogueTrigger : DialogueTrigger
{
    public string filter = "";
    public LayerMask layer;
    public float delay = 0f;
    private bool ZoneDialogHasBeenShown = false;
    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("ontrigger:" + other.name);
        if(ZoneDialogHasBeenShown == false)
        {
            //Debug.Log("hasbeenshown:" + other.name);
            if((filter == "" || other.name == filter) && (layer.value == 0 || layer.value == (1 << other.gameObject.layer)))
            {
                //Debug.Log("triggered");
                Invoke("TriggerDialogue",delay);
                ZoneDialogHasBeenShown = true;
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        //Debug.Log("ontrigger:" + other.name);
        if(ZoneDialogHasBeenShown == false)
        {
            //Debug.Log("hasbeenshown:" + other.name);
            if((filter == "" || other.name == filter) && (layer.value == 0 || layer.value == (1 << other.gameObject.layer)))
            {
                //Debug.Log("triggered");
                Invoke("TriggerDialogue",delay);
                ZoneDialogHasBeenShown = true;
            }
        }
    }
}
