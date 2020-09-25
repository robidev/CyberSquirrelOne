using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BreakableEffect : MonoBehaviour
{
    GameObject child;
    public UnityEvent BreakEvent;

    void Start()
    {
        child = transform.GetChild(0).gameObject;
    }

    public void Break()
    {
        child.SetActive(false);
        if(BreakEvent != null)
			BreakEvent.Invoke();
    }
}
