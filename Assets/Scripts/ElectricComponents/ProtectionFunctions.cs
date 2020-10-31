using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

public class ProtectionFunctions : MonoBehaviour
{
    //protections
    [HideInInspector]
    public bool OverVoltage = false;
    [HideInInspector]
    public float OverVoltageTreshold = 1000;
    [HideInInspector]
    public ConductingEquipment OverVoltageVT;
    [HideInInspector]
    public bool OverCurrent = false;
    [HideInInspector]
    public float OverCurrentImmediate = 100;
    [HideInInspector]
    public ConductingEquipment OverCurrentCT;
    [HideInInspector]
    public bool TimeOverCurrent = false;
    [HideInInspector]
    public float OverCurrentTreshold = 50;
    [HideInInspector]
    public float OverCurrentTime = 2;//2 sec

    public bool DifferentialProtection = false;
    private float differentialTreshold = 0;
    private List<ConductingEquipment> OverCurrentCTsIn;
    private List<ConductingEquipment> OverCurrentCTsOut;
    public bool DistanceProtection = false;
    private float ImpedanceTreshold = 0;
    private ConductingEquipment DistanceVT;
    private ConductingEquipment DistanceCT;
    
    public UnityEvent OnTrip;
    private float OC = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PerformProtectionFunction();
    }

    void PerformProtectionFunction()
    {
        //overvoltage
        if(OverVoltage && OverVoltageVT.voltage > OverVoltageTreshold)
        {
            OnTrip.Invoke();
            Debug.Log(name + ": overvoltage trip");
            return;
        }
        //overcurrent
        if(OverCurrent && OverCurrentCT.current > OverCurrentImmediate)
        {
            OnTrip.Invoke();
            Debug.Log(name + ": overcurrent immediate trip");  
            return;          
        }
        
        //time overcurrent
        if(TimeOverCurrent)
        {
            if(OverCurrentCT.current > OverCurrentTreshold)
            {
                float delta = (OverCurrentImmediate - OverCurrentTreshold) / OverCurrentTime;
                OC += (OverCurrentCT.current - OverCurrentTreshold) * Time.deltaTime;
                if(OC > delta)
                {
                    OnTrip.Invoke();
                    Debug.Log(name + ": time overcurrent trip"); 
                }
            }
            else
            {
                if(OC > 0)
                {
                    OC -= (OverCurrentTreshold - OverCurrentCT.current) * Time.deltaTime; //subtract current with similar falloff rate
                }
                else //ensure we clamp negative values to 0
                {
                    OC = 0;
                }
            }
        }
        //distance protection - measure impedance VT/CT < x
        if(DistanceProtection)
        {
            float impedance = DistanceVT.voltage / DistanceCT.current;
            if(impedance < ImpedanceTreshold)
            {
                OnTrip.Invoke();
                Debug.Log(name + ": distance protection trip");                 
            }
        }
        //differential protection - measure ingressing and egressing current CT-CT > x
        //busbar protection - variant of differential protection scheme with mutiple sources
        if(DifferentialProtection)
        {
            float totalcurrent = 0;
            foreach(ConductingEquipment ct in OverCurrentCTsIn)
            {
                totalcurrent += ct.current;
            }
            foreach(ConductingEquipment ct in OverCurrentCTsOut)
            {
                totalcurrent -= ct.current;
            }
            if(Mathf.Abs(totalcurrent) > differentialTreshold)
            {
                OnTrip.Invoke();
                Debug.Log(name + ": differential protection trip"); 
            }
        }
    }
}

 [CustomEditor(typeof(ProtectionFunctions))]
 public class MyScriptEditor : Editor
 {
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector ();

        var protectionFunctions = target as ProtectionFunctions;

        protectionFunctions.OverVoltage = GUILayout.Toggle(protectionFunctions.OverVoltage, "OverVoltage");
        
        if(protectionFunctions.OverVoltage)
        {
            protectionFunctions.OverVoltageTreshold = EditorGUILayout.FloatField("    OverVoltage Treshold", protectionFunctions.OverVoltageTreshold);
            protectionFunctions.OverVoltageVT = (ConductingEquipment)EditorGUILayout.ObjectField("    OverVoltage VT", protectionFunctions.OverVoltageVT, typeof(ConductingEquipment),true);
        }


        protectionFunctions.OverCurrent = GUILayout.Toggle(protectionFunctions.OverCurrent, "OverCurrent");
        if(protectionFunctions.OverCurrent)
        {
            protectionFunctions.OverCurrentImmediate = EditorGUILayout.FloatField("    OverCurrent Immediate Treshold", protectionFunctions.OverCurrentImmediate);
            protectionFunctions.OverCurrentCT = (ConductingEquipment)EditorGUILayout.ObjectField("    OverCurrent CT", protectionFunctions.OverCurrentCT, typeof(ConductingEquipment),true);
            protectionFunctions.TimeOverCurrent = GUILayout.Toggle(protectionFunctions.TimeOverCurrent, "    Time OverCurrent");
            
            if(protectionFunctions.TimeOverCurrent)
            {
                protectionFunctions.OverCurrentTreshold = EditorGUILayout.FloatField("        OverCurrent Treshold", protectionFunctions.OverCurrentTreshold);
                protectionFunctions.OverCurrentTime = EditorGUILayout.FloatField("        OverCurrent Time", protectionFunctions.OverCurrentTime);
            }

        }       
        
    }
 }