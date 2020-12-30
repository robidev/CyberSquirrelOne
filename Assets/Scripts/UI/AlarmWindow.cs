using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class AlarmWindow : MonoBehaviour
{
    public TableScript MessageLog;
    public TableScript AlarmLog;
    // Start is called before the first frame update
    void Start()
    {
        SetAlarm("22:00:11","B1","B2","B3","Message","Open");
        SetAlarm("22:00:11","Amr","RTU-1","C101","Lost connection","Open");
        SetAlarm("22:00:11","C1","C2","C3","1234567","Open");
        Invoke("addItemsTest1",3.0f);
        Invoke("addItemsTest2",5.0f);
    }

    void addItemsTest1()
    {
        SetAlarm("22:00:11","D1","D2","D3","test12","Open");
    }

    void addItemsTest2()
    {
        SetAlarm("22:00:11","C1","C2","C3","1234567","Close");
    }

    //set alarm on or off
    public void SetAlarm(string time, string B1, string B2, string B3, string Message, string status)
    {
        //add message to historical message log
        MessageLog.Insert(0, new string[] {time, B1, B2, B3, Message, status});

        //set/modify message from alarm-log
        foreach(GameObject[] lineObject in AlarmLog.getTable())
        {   //find the same object
            if( lineObject[2].GetComponentInChildren<TMP_Text>().text == B1 &&
                lineObject[3].GetComponentInChildren<TMP_Text>().text == B2 &&
                lineObject[4].GetComponentInChildren<TMP_Text>().text == B3 &&
                lineObject[5].GetComponentInChildren<TMP_Text>().text == Message )
            {   //if found, update status
                //lineObject[0].GetComponentInChildren<TMP_Text>().text == time &&
                lineObject[6].GetComponentInChildren<TMP_Text>().text = status;
                AlarmLog.Recolor(lineObject,Color.blue,Color.blue);
                LayoutRebuilder.ForceRebuildLayoutImmediate(AlarmLog.GetComponent<RectTransform>());
                return;
            } 
        }
        //no object found, so add a new one
        int index = AlarmLog.Insert(0,new string[] {"A",time, B1, B2, B3, Message, status});
        AlarmLog.setOnHoverEvent(index);
        AlarmLog.setOnClickEvent(index,onClickEvent);
    }

    void onClickEvent()
    {
        GameObject[] line = EventSystem.current.currentSelectedGameObject.GetComponent<TableItemHelper>().line;
        /*string lineText = "";
        foreach(GameObject item in line)
        {
            lineText += item.GetComponentInChildren<TMP_Text>().text + ";";
        }
        Debug.Log("clicked:" + EventSystem.current.currentSelectedGameObject.transform.parent.name + " in: " + lineText);*/

        string state = line[0].GetComponentInChildren<TMP_Text>().text;
        if(state == "A")
        {
            line[6].GetComponentInChildren<TMP_Text>().text += ",Ack";

            string time = line[1].GetComponentInChildren<TMP_Text>().text;
            string B1 = line[2].GetComponentInChildren<TMP_Text>().text;
            string B2 = line[3].GetComponentInChildren<TMP_Text>().text;
            string B3 = line[4].GetComponentInChildren<TMP_Text>().text;
            string Message = line[5].GetComponentInChildren<TMP_Text>().text;
            string status = line[6].GetComponentInChildren<TMP_Text>().text;

            if(line[6].GetComponentInChildren<TMP_Text>().text.Contains("Close"))
            {
                status += ",Clr";
                AlarmLog.Remove(line);
            }
            else
            {
                line[0].GetComponentInChildren<TMP_Text>().text = "C";
                AlarmLog.Recolor(line,Color.yellow,Color.yellow);
            }
            MessageLog.Insert(0, new string[] {time, B1, B2, B3, Message, status});
        }
        else if(state == "C")
        {
            string time = line[1].GetComponentInChildren<TMP_Text>().text;
            string B1 = line[2].GetComponentInChildren<TMP_Text>().text;
            string B2 = line[3].GetComponentInChildren<TMP_Text>().text;
            string B3 = line[4].GetComponentInChildren<TMP_Text>().text;
            string Message = line[5].GetComponentInChildren<TMP_Text>().text;
            string status = line[6].GetComponentInChildren<TMP_Text>().text + ",Clr";

            AlarmLog.Remove(line);
            MessageLog.Insert(0, new string[] {time, B1, B2, B3, Message, status}); 
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(AlarmLog.GetComponent<RectTransform>());
    }
}

