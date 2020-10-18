using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvent : MonoBehaviour
{
    public UnityEvent Event;
    public string filter = "";
    public LayerMask layer;
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D other)
    {
        if((filter == "" || other.name == filter) && (layer.value == 0 || layer.value == (1 << other.gameObject.layer)))
        {
            Event.Invoke();
        }
    }
}
