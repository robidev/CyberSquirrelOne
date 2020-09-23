using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneDialogueTrigger : DialogueTrigger
{
    public string filter = "";
    public float delay = 0f;
    private bool ZoneDialogHasBeenShown = false;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(ZoneDialogHasBeenShown == false)
        {
            if(filter == "" || other.name == filter)
            {
                Invoke("TriggerDialogue",delay);
                ZoneDialogHasBeenShown = true;
            }
        }
    }
}
