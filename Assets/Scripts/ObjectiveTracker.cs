using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveTracker : MonoBehaviour
{
    // Start is called before the first frame update
    int size = 0;
    void Start()
    {
        size = gameObject.transform.childCount;
        /*for (int i = 0; i < size; i++)
        {
            Debug.Log(gameObject.transform.GetChild(i).name);
        }*/
    }

    public void ShowNextDialogue()
    {
        for (int i = 0; i < size; i++)
        {
            var trigger = gameObject.transform.GetChild(i).GetComponent<DialogueTrigger>();
            if(trigger != null && trigger.DialogHasBeenShown == false)
            {
                trigger.TriggerDialogue();
                break;
            }
        }        
    }
}
