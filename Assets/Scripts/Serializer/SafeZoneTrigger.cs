using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SafeZoneTrigger : MonoBehaviour
{
    public string filter = "";
    public LayerMask layer;
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D other)
    {
        if((filter == "" || other.name == filter) && (layer.value == 0 || layer.value == (1 << other.gameObject.layer)))
        {
            SerializedPlayerObject obj = other.GetComponent<SerializedPlayerObject>();
            if(obj != null)
            {
                obj.setSafeLocation();
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if((filter == "" || other.name == filter) && (layer.value == 0 || layer.value == (1 << other.gameObject.layer)))
        {
            SerializedPlayerObject obj = other.GetComponent<SerializedPlayerObject>();
            if(obj != null)
            {
                obj.setSafeLocation();
            }
        }
    }
}
