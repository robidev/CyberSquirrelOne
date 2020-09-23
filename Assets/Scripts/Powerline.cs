using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Powerline : MonoBehaviour
{
    private bool _isPowered = true;
    public UnityEvent m_PowerLossEvent;
    public bool isPowered 
    {
        get{ return _isPowered; }
        set
        {
            if(_isPowered == true && value == false)
            {
                m_PowerLossEvent.Invoke();
            }
            _isPowered = value;

        }
    }
}
