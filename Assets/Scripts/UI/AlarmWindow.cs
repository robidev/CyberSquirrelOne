using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class AlarmWindow : MonoBehaviour
{
    public GameObject HelpDialogPrefab;
    GameObject HelpInstance;
    public void NewHelpDialog()
    {
        if(HelpInstance != null)
        {
            Destroy(HelpInstance);
            HelpInstance = null;
        }  

        HelpInstance = Instantiate(HelpDialogPrefab,transform.parent);
        HelpInstance.GetComponent<RectTransform>().SetAsLastSibling();
        HelpInstance.GetComponent<HMIHelpDialog>().textField.text = 
            "The Alarm window displays the alarm events coming from the field equipment.\n" +
            "Clicking an alarm will acknowledge it, and clicking again will close the alarm.\n" +
            "A closed alarm is removed from the list of alarms.\n\n" +
            "Active Un-acknowledged alarms (On) will show in Red,\n"+
            "an inactive Un-acknowledged alarm (Off) is Blue.\n" +
            "An Active, Acknowledged alarm is Yellow,\n" +
            "and an Inactive alarm, is closed as soon as it is acknowledged.\n\n" +
            "All actions and status changes can be found back in the message window with corresponing timestamps.";
    }
    public static AlarmWindow instance = null;
    public TableScript MessageLog;
    public TableScript AlarmLog;
    AlarmWindow()
    {
        if(instance == null)//get a static pointer to the first instance; not a singleton, but easy to reference
        {
            instance = this;
        }
    }

    //set alarm on or off
    public void SetAlarm(string B1, string B2, string B3, string Message, string status)
    {
        string time = System.DateTime.Now.ToString("dd.MM HH:mm:ss");
        //add message to historical message log
        //MessageLog.Insert(0, new string[] {time, B1, B2, B3, Message, "ALARM:" + status});
        MessageLog.Add(new string[] {time, B1, B2, B3, Message, "ALARM:" + status});

        //set/modify message from alarm-log
        foreach(GameObject[] lineObject in AlarmLog.getTable())
        {   //find the same object
            if( lineObject[2].GetComponentInChildren<TMP_Text>().text == B1 &&
                lineObject[3].GetComponentInChildren<TMP_Text>().text == B2 &&
                lineObject[4].GetComponentInChildren<TMP_Text>().text == B3 &&
                lineObject[5].GetComponentInChildren<TMP_Text>().text == Message )
            {   //if found, update status
                lineObject[6].GetComponentInChildren<TMP_Text>().text = status;
                if(status.Contains("Off"))
                {
                    AlarmLog.Recolor(lineObject,Color.blue,Color.blue);
                }
                else
                {
                    AlarmLog.Recolor(lineObject);
                }
                LayoutRebuilder.ForceRebuildLayoutImmediate(AlarmLog.GetComponent<RectTransform>());
                return;
            } 
        }
        //no object found, so add a new one
        int index = AlarmLog.Insert(0,new string[] {"A",time, B1, B2, B3, Message, status});
        AlarmLog.setOnHoverEvent(index);
        AlarmLog.setOnClickEvent(index,onClickEvent);
        if(status.Contains("Off"))
        {
            GameObject[] lineObject = AlarmLog.getLine(index);
            AlarmLog.Recolor(lineObject,Color.blue,Color.blue);
        }
    }

    public void SetMessage(string B1, string B2, string B3, string Message, string status)
    {
        string time = System.DateTime.Now.ToString("dd.MM HH:mm:ss");
        //add message to historical message log
        MessageLog.Add(new string[] {time, B1, B2, B3, Message, status});
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

            string time = System.DateTime.Now.ToString("dd.MM HH:mm:ss");
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
            MessageLog.Add( new string[] {time, B1, B2, B3, Message, "ALARM:" + status});
        }
        else if(state == "C")
        {
            string time = System.DateTime.Now.ToString("dd.MM HH:mm:ss");
            string B1 = line[2].GetComponentInChildren<TMP_Text>().text;
            string B2 = line[3].GetComponentInChildren<TMP_Text>().text;
            string B3 = line[4].GetComponentInChildren<TMP_Text>().text;
            string Message = line[5].GetComponentInChildren<TMP_Text>().text;
            string status = line[6].GetComponentInChildren<TMP_Text>().text + ",Clr";

            AlarmLog.Remove(line);
            MessageLog.Add( new string[] {time, B1, B2, B3, Message, "ALARM:" + status}); 
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(AlarmLog.GetComponent<RectTransform>());
    }
}

