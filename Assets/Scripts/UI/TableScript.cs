using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TableScript : MonoBehaviour
{
    public GameObject TableItemPrefab;
    private List<GameObject[]> TableItem;
    public Color ColorOdd = Color.gray;
    public Color ColorEven = Color.white;

    void Awake()
    {
        TableItem = new List<GameObject[]>();
        Add(new string[] {"A","10:22:32","Amv","RTU","IEC104","Lost connection","On"});
        Add(new string[] {"O","10:22:33","Amv","RTU 2","IED","Received com.","On"});
        Add(new string[] {"A","10:22:34","Amr","IED 1","Protection","trip","Off"});
    }

    public void Add(string[] entry)
    {
        GameObject[] item = new GameObject[transform.childCount];
        for(int i = 0; i< transform.childCount; i++)
        {
            Transform column = transform.GetChild(i);
            item[i] = Instantiate(TableItemPrefab,column);
            item[i].transform.GetComponent<Image>().color = (TableItem.Count % 2) == 0? ColorEven : ColorOdd;

            if(i < entry.Length)
            {
                item[i].GetComponentInChildren<TMP_Text>().text = entry[i];
            }
            else
            {
                item[i].GetComponentInChildren<TMP_Text>().text = "-";
            }
            
        }
        TableItem.Add(item);
    }

    public void Add(string entries)//entries delimited by semicolon
    {
        Add(entries.Split(';'));
    }
}
