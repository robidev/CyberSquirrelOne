using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
 {
     private bool mouse_over = false;
     private float hoverTime = 0f;
     private bool toolTipNotShown = true;
     public GameObject toolTipPrefab;
     private GameObject toolTipInstance;
     public string ToolTipText = "";
     public Transform ToolTipParent;
     public Vector3 offset = new Vector3(10f,0f,0f);
     void Awake()
     {
         if(ToolTipText == "")
         {
             ToolTipText = name;
         }
     }
     void Update()
     {
         if (mouse_over)
         {
            if(Time.time - hoverTime > 1f && toolTipNotShown)//hover still for longer then a second
            {
                toolTipNotShown = false;
                toolTipInstance = Instantiate(toolTipPrefab,Input.mousePosition + offset,Quaternion.identity,ToolTipParent);
                toolTipInstance.GetComponentInChildren<TMP_Text>().text = ToolTipText;
                Destroy(toolTipInstance,30f);//to ensure a tooltip does not stay forever
            }
         }
     }
 
     public void OnPointerEnter(PointerEventData eventData)
     {
         Destroy(toolTipInstance);//destroy any old tooltip
         mouse_over = true;
         hoverTime = Time.time;
         toolTipNotShown = true;
     }
 
     public void OnPointerExit(PointerEventData eventData)
     {
         mouse_over = false;
         Destroy(toolTipInstance);
     }
 }