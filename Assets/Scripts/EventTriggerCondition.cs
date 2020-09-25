using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTriggerCondition : DialogueTrigger
{
    public List<string> EventName;
    public bool NeedAllTriggers = true;
    public float delay = 0f;
    private bool[] triggers;
    void Start()
    {
        triggers = new bool[EventName.Count];
        for(int i = 0; i< EventName.Count; i++)
        {
            triggers[i] = false;
        }
    }

    public void EventTrigger(string name)
    {
        //Debug.Log("Trigger! - " + name + " -");
        if(EventName.Contains(name))
        {
            triggers[EventName.IndexOf(name)] = true;
            if(NeedAllTriggers)
            {
                for(int i = 0; i< EventName.Count; i++)
                {
                    if(triggers[i] == false)
                        return;//if not all triggers have been called
                }
                Invoke("TriggerDialogue",delay);
            }
            else
            {
                Invoke("TriggerDialogue",delay);
            }
        }
    }
}
