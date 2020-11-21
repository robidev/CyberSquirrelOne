using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MoveAlongModifier : MonoBehaviour
{
    public UnityEvent OnEnterEvents;
    public UnityEvent OnReachedEvents;

    public Transform SetPositionTarget;
    public Transform SetPositionAtLocation;
    public void OnEnter()
    {
        OnEnterEvents.Invoke();
    }

    public void OnReached()
    {
        OnReachedEvents.Invoke();
    }

    public void SetPosition()
    {
        SetPositionTarget.position = SetPositionAtLocation.position;
    }    
}
