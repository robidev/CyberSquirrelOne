 // Touchable component
 
 using UnityEngine;
 using UnityEngine.UI;
using UnityEditor;
 public class Touchable : Text
 {
     protected override void Awake()
     {
         base.Awake();
     }
 }
 
 // Touchable_Editor component, to prevent treating the component as a Text object.
 

 
 [CustomEditor(typeof(Touchable))]
 public class Touchable_Editor : Editor
 {
     public override void OnInspectorGUI ()
     {
         // Do nothing
     }
 }