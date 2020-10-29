using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class StringEvent : UnityEvent<string>
{
}
public class IsVisibleTrigger : MonoBehaviour
{
    public StringEvent IsVisibleEvent;
    void OnBecameVisible()
    {
        //Debug.Log("Visible!");
        IsVisibleEvent.Invoke(name);
    }
}
