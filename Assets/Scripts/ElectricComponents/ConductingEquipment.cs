using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

public class ConductingEquipment : MonoBehaviour
{
    public float voltage {
        get { return _voltage; }
        set 
        { 
            _voltage = value;
            if(Destroyed == false)
            {
                if(_voltage > VoltRating)
                {
                    damage += _voltage - VoltRating;
                    Debug.Log(name + ": Overvoltage damage:" + _voltage);
                }
            }
        }
    }
    private float _voltage = 0;
    public float current {
        get { return _current; }
        set 
        {
            _current = value;
            if(Destroyed == false)
            {
                if(_current > AmpRating)
                {
                    damage += _current - AmpRating;
                    Debug.Log(name + ": Overcurrent damage:" + _current);
                }
            }
        }
    }
    private float _current = 0;
    public float damage {
        get { return _damage; }
        set
        {
            if(Destroyed == false)
            {
                _damage = value;
                if(_damage > DestroyRating)
                {
                    Debug.Log(name + ": Exploded");
                    Destroyed = true;
                    OnDestroyed.Invoke();
                }
            }
        }
    }
    private float _damage = 0;
    public float AmpRating = 200;
    public float VoltRating = 1000;
    public float DestroyRating = 1000;
    public bool Destroyed = false;
    public UnityEvent OnDestroyed;
    public virtual void Initialize(ConductingEquipment reference)
    {

    }
    public virtual void Step()
    {
    }
    public bool displayAmp = false;
    public bool displayVolt = false;
    void OnGUI()
    {
        if(displayAmp || displayVolt)
        {
            var pos = transform.position + new Vector3(20,0,-10);
            if(displayAmp)
                GUI.Label(new Rect(pos.x,768-pos.y,100,40),"Amp: " + current + " A");
            if(displayVolt)
                GUI.Label(new Rect(pos.x,768-pos.y-20,100,40),"Volt:" + voltage + " V");
        }
    }

    /*
    void OnDrawGizmos() 
    {
        Handles.Label(transform.position + new Vector3(20,20,0), "Amp: " + current + " A");
        Handles.Label(transform.position + new Vector3(20,-20,0), "Volt:" + voltage + " V");
    }
    */
}
