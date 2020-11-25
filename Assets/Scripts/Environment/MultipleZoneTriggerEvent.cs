using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class MultipleZoneTriggerEvent : MonoBehaviour
{
    public UnityEvent Event;
    public List<string> TriggerObjects;
    public bool[] triggers;
    void Start()
    {
        triggers = new bool[TriggerObjects.Count];
        for(int i = 0; i< TriggerObjects.Count; i++)
        {
            triggers[i] = false;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        for(int i = 0; i< TriggerObjects.Count; i++)
        {
            if(other.gameObject.name == TriggerObjects[i])
            {
                triggers[i] = true;
                CheckTriggerEvent();
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        for(int i = 0; i< TriggerObjects.Count; i++)
        {
            if(other.gameObject.name == TriggerObjects[i])
            {
                triggers[i] = false;
            }
        }
    }

    public void CheckTriggerEvent()
    {
        for(int i = 0; i< TriggerObjects.Count; i++)
        {
            if(triggers[i] == false)
                return;//if not all triggers have been called
        }
        //invoke if all triggers are satified
        Event.Invoke();
    }
}
