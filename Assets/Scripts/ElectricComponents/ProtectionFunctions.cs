using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


#if UNITY_EDITOR
using UnityEditor;
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
        
        protectionFunctions.DistanceProtection = GUILayout.Toggle(protectionFunctions.DistanceProtection, "DistanceProtection");
        if(protectionFunctions.DistanceProtection)
        {
            protectionFunctions.ImpedanceTreshold = EditorGUILayout.FloatField("    Impedance Treshold", protectionFunctions.ImpedanceTreshold);
            protectionFunctions.DistanceVT = (ConductingEquipment)EditorGUILayout.ObjectField("    Distance VT", protectionFunctions.DistanceVT, typeof(ConductingEquipment),true);
            protectionFunctions.DistanceCT = (ConductingEquipment)EditorGUILayout.ObjectField("    Distance CT", protectionFunctions.DistanceCT, typeof(ConductingEquipment),true);
        }  

        protectionFunctions.DifferentialProtection = GUILayout.Toggle(protectionFunctions.DifferentialProtection, "DifferentialProtection");
        if(protectionFunctions.DifferentialProtection)
        {
            protectionFunctions.differentialTreshold = EditorGUILayout.FloatField("    Differential Treshold", protectionFunctions.differentialTreshold);
            //protectionFunctions.OverCurrentCTsIn = (ConductingEquipment)EditorGUILayout.("    OverCurrent CT incoming", protectionFunctions.OverCurrentCTsIn, typeof(ConductingEquipment),true);
            //protectionFunctions.OverCurrentCTsOut = (ConductingEquipment)EditorGUILayout.ObjectField("    OverCurrent CT outgoing", protectionFunctions.OverCurrentCTsOut, typeof(ConductingEquipment),true);
            var listint = serializedObject.FindProperty("OverCurrentCTsIn");
            EditorGUILayout.PropertyField(listint, new GUIContent("    OverCurrent CT incoming"), true);
            var listout = serializedObject.FindProperty("OverCurrentCTsOut");
            EditorGUILayout.PropertyField(listout, new GUIContent("    OverCurrent CT outgoing"), true);
            serializedObject.ApplyModifiedProperties();
        }          


    }
 }
#endif


public class ProtectionFunctions : MonoBehaviour
{
    //protections
    [HideInInspector]    public bool OverVoltage = false;
    [HideInInspector]    public float OverVoltageTreshold = 1000;
    [HideInInspector]    public ConductingEquipment OverVoltageVT;
    [HideInInspector]    public bool OverCurrent = false;
    [HideInInspector]    public float OverCurrentImmediate = 100;
    [HideInInspector]    public ConductingEquipment OverCurrentCT;
    [HideInInspector]    public bool TimeOverCurrent = false;
    [HideInInspector]    public float OverCurrentTreshold = 50;
    [HideInInspector]    public float OverCurrentTime = 2;//2 sec
    [HideInInspector]    public bool DifferentialProtection = false;
    [HideInInspector]    public float differentialTreshold = 0;
    [SerializeField] public List<ConductingEquipment> OverCurrentCTsIn;
    [SerializeField] public List<ConductingEquipment> OverCurrentCTsOut;
    [HideInInspector]    public bool DistanceProtection = false;
    [HideInInspector]    public float ImpedanceTreshold = 0;
    [HideInInspector]    public ConductingEquipment DistanceVT;
    [HideInInspector]    public ConductingEquipment DistanceCT;
    
    public UnityEvent OnTrip;
    private float OC = 0;
    [SerializeField] public new string name = "P1";

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
            Debug.Log(name + ": overvoltage trip:" + OverVoltageVT.voltage);
            return;
        }
        //overcurrent
        if(OverCurrent && OverCurrentCT.current > OverCurrentImmediate)
        {
            OnTrip.Invoke();
            Debug.Log(name + ": overcurrent immediate trip:" + OverCurrentCT.current);  
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
                    Debug.Log(name + ": time overcurrent trip:" + OverCurrentCT.current); 
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
                Debug.Log(name + ": distance protection trip:" + impedance);                 
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
                Debug.Log(name + ": differential protection trip:" + totalcurrent); 
            }
        }
    }
}
