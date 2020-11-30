using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerTimeEvent : MonoBehaviour
{
    public UnityEvent Event;
    public string filter = "";
    public LayerMask layer;
    public float Delay = 0f;
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D other)
    {
        if((filter == "" || other.name == filter) && (layer.value == 0 || layer.value == (1 << other.gameObject.layer)))
        {
            StartCoroutine(InvokeEvent());
        }
    }

    public void TriggerEventDelay()
    {
        StartCoroutine(InvokeEvent());
    }

    IEnumerator InvokeEvent()
    {
        yield return new WaitForSeconds(Delay);
        Event.Invoke();
    }

}
