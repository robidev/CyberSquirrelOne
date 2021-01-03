using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using Unity.VectorGraphics;
using UnityEngine.UI;

public class ConductingEquipment : MonoBehaviour
{
    private bool AlarmState = false;
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
            if(mimic != null)
            {
                if(_voltage < PowerLossTreshold)
                {
                    mimic.color = PowerlossColor;
                }
                else
                {
                    mimic.color = NormalColor;
                }
            }
            if((displayVolt || displayAmp) && AlarmWindow.instance != null)//only for sensors (VT's+CT's)
            {
                if(AlarmState == false && _voltage < PowerLossTreshold)//power lost
                {
                    AlarmState = true;
                    AlarmWindow.instance.SetAlarm("Sub_S42",gameObject.name,"VT","Undervoltage Alarm","On"); 
                }
                if(AlarmState == true && _voltage >= PowerLossTreshold)//power restored
                {
                    AlarmState = false;
                    AlarmWindow.instance.SetAlarm("Sub_S42",gameObject.name,"VT","Undervoltage Alarm","Off"); 
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
                    AlarmWindow.instance.SetAlarm("Sub_S42",gameObject.name,this.GetType().Name,"Component failed","On"); 
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
    public virtual void Initialize(ConductingEquipment reference)  {   }
    public virtual void Step() {  }
    public bool displayAmp = false;
    public bool displayVolt = false;
    private SVGImage mimic;
    public float PowerLossTreshold = 100;
    public Color PowerlossColor = Color.white;
    private Color NormalColor;

    private GameObject AmpText;
    private GameObject VoltText;

    void Awake()
    {
        mimic = GetComponent<SVGImage>();
        if(mimic != null)
            NormalColor = mimic.color;
        else
            Debug.Log("cannot find SVG Image");
    }

    void Start()
    {
        if(displayVolt)
        {
            VoltText = new GameObject("VoltText");
            VoltText.transform.position = transform.position + new Vector3(70,-33,0);
            VoltText.transform.SetParent(this.transform);
            
            VoltText.AddComponent<Text>().text = "Volt: - KV";
            VoltText.GetComponent<Text>().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;            
        }
        if(displayAmp)
        {
            AmpText = new GameObject("AmpText");
            AmpText.transform.position = transform.position + new Vector3(70,-47,0);
            AmpText.transform.SetParent(this.transform);

            AmpText.AddComponent<Text>().text = "Amp: - A";
            AmpText.GetComponent<Text>().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        }
    }

    void Update()
    {
        if(displayVolt)
        {
            VoltText.GetComponent<Text>().text =  "Volt:" + voltage.ToString("0") + " kV";      
        }
        if(displayAmp)
        {
            AmpText.GetComponent<Text>().text = "Amp: " + current.ToString("0") + " A";
        } 
    }
}
