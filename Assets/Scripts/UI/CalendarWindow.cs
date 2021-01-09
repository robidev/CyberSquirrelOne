using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CalendarWindow : MonoBehaviour
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
            "The Calender is only to display the current month, week and (week)day. Currently it servers no other purpose.";
    }
    public Transform Calendar;
    public TMP_Text MonthYear;
    public GameObject DayPrefab;
    private GameObject[] Days = new GameObject[40];
    // Start is called before the first frame update
    void Awake()
    {
        MonthYear.text = System.DateTime.Now.ToString("MMMM, yyyy");
        int year = System.DateTime.Now.Year;
        int month = System.DateTime.Now.Month;

        System.DateTime cc = new System.DateTime(year,month,1);
        int weekday = (int)cc.DayOfWeek;
        int day = 1;
        for(int i = 0; i < 40; i++)
        {
            Days[i] = Instantiate(DayPrefab,Calendar);
            if(i < weekday)
            {
                Days[i].transform.Find("Number").gameObject.SetActive(false);
            }
            else
            {
                try
                {
                    System.DateTime dd = new System.DateTime(year,month,day);
                    Days[i].transform.Find("Number").GetComponentInChildren<TMP_Text>().text = dd.Day.ToString();
                    if(dd.DayOfYear == System.DateTime.Now.DayOfYear)
                    {
                        Days[i].GetComponent<Image>().color = Color.white;
                    }
                }
                catch 
                {
                    Days[i].SetActive(false);
                }

                day++;
            }
        }
    }
}
