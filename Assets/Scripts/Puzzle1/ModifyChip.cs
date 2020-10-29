using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//[ExecuteInEditMode]
public class ModifyChip : MonoBehaviour
{
    public Puzzle1 puzzle1;
    // Start is called before the first frame update
    void Start()
    {
        //int i = 1;
        foreach(Toggle toggle in GetComponentsInChildren<Toggle>())
        {
            //toggle.name = i.ToString();
            //i++;
            //Add listener for when the state of the Toggle changes, to take action
            toggle.onValueChanged.AddListener(delegate {
                ToggleValueChanged(toggle);
            });
        }
    }

    void ToggleValueChanged(Toggle toggle)
    {
        Transform obj = toggle.transform.Find("Background/Checkmark");
        if(obj.GetComponent<Image>().color == Color.green && toggle.isOn == false)
        {
            obj.GetComponent<Image>().color = Color.red;
            toggle.isOn = true;
        }
        else if(obj.GetComponent<Image>().color == Color.red && toggle.isOn == false)
        {
            obj.GetComponent<Image>().color = Color.green;
        }   
        Debug.Log("pin:" + toggle.name + " is " + toggle.isOn.ToString() );

        //EEPROM and Microcontroller are connected, so essentially the same pin
        int pin = 0;
        int.TryParse(toggle.name, out pin);
        if(pin == 45 || pin == 46)
        {
            setState(10,getState(45));
            setState(11,getState(46));
        }
        if(pin == 16 || pin == 17)
        {
            setState(45,getState(10));
            setState(46,getState(11));
        }
        puzzle1.chipModifier(pin);
    }

    public int getState(int pin)
    {
        foreach(Transform child in transform)
        {
            Transform res = child.Find(pin.ToString());
            if(res != null)
            {
                Toggle toggle = res.GetComponent<Toggle>();
                if(toggle.isOn == true)
                {
                    Transform obj = toggle.transform.Find("Background/Checkmark");
                    if(obj.GetComponent<Image>().color == Color.green)
                    {
                        return 1;
                    }
                    else //obj.GetComponent<Image>().color == Color.red
                    {
                        return 2;
                    }   
                }
                else //toggle.isOn == false
                {
                    return 0;
                }
            }
        }
        return -1; // pin not found
    }

    public void setState(int pin, int state)
    {
        foreach(Transform child in transform)
        {
            Transform res = child.Find(pin.ToString());
            if(res != null)
            {
                Toggle toggle = res.GetComponent<Toggle>();
                if(state > 0)
                {
                    Transform obj = toggle.transform.Find("Background/Checkmark");
                    if(state == 1)
                    {
                        obj.GetComponent<Image>().color = Color.green;
                    }
                    else //obj.GetComponent<Image>().color == Color.red
                    {
                        obj.GetComponent<Image>().color = Color.red;
                    }   
                    toggle.isOn = true;
                }
                else //state is off
                {
                    toggle.isOn = false;
                }
            }
        }
    }

    public void setSolution()
    {
        foreach(Toggle toggle in GetComponentsInChildren<Toggle>())
        {
            if(toggle.interactable == true)
            {
                //Debug.Log(toggle.name);
                Transform obj = toggle.transform.Find("Background/Checkmark");
                obj.GetComponent<Image>().color = Color.red;
                toggle.isOn = false;
            }
            
        }
        setState(47, 1);//set WP high
    }
}
