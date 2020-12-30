using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class TableScript : MonoBehaviour
{
    public GameObject TableItemPrefab;
    private List<GameObject[]> TableItem = new List<GameObject[]>();
    public Color ColorOdd = Color.gray;
    public Color ColorEven = Color.white;

    void Awake()
    {
        /*Insert(0,new string[] {"A","10:22:32","Amv","RTU","IEC104","Lost connection","On"});
        Insert(0,new string[] {"O","10:22:33","Amv","RTU 2","IED","Received com.","On"});
        Insert(0,new string[] {"A","10:22:34","Amr","IED 1","Protection","trip","Off"});
        Insert(0,new string[] {"B","10:22:34","Amr","IED 1","Protection","trip","Off"});
        Insert(0,new string[] {"C","10:22:34","Amr","IED 1","Protection","trip","Off"});
        Insert(0,new string[] {"D","10:22:34","Amr","IED 1","Protection","trip","Off"});*/
    }

    public int Add(string[] entry)
    {
        return Insert(TableItem.Count, entry);
    }

    public int Add(string entries)//entries delimited by semicolon
    {
        return Add(entries.Split(';'));
    }

    public void Remove(int index)
    {
        Remove(getLine(index));
    }

    public void Remove(GameObject[] line)
    {
        if(line == null)
            return;

        foreach(GameObject item in line)
        {
            Destroy(item);
        }
        if(TableItem.Remove(line) == false)
        {
            Debug.Log("could not remove row from table");
        }
        Recolor();//recolor the table for alternating colors
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }

    public void Recolor() // recolor the table
    {
        int index = 0;
        foreach(GameObject[] line in TableItem)
        {
            foreach(GameObject item in line)
            {
                if(item.transform.GetComponent<Image>().color == ColorEven || item.transform.GetComponent<Image>().color == ColorOdd)
                    item.transform.GetComponent<Image>().color = (index % 2) == 0? ColorEven : ColorOdd;
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
            index++;
        }
    }

    public void Recolor(GameObject[] line, Color colorOdd, Color colorEven) // recolor the line
    {
        int index = 0;
        foreach(GameObject[] lineIdx in TableItem)
        {
            if(lineIdx == line)
            {
                foreach(GameObject item in line)
                {
                    item.transform.GetComponent<Image>().color = (index % 2) == 0? colorEven : colorOdd;
                }
                LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
            }
            index++;
        }
    }

    public int Insert(int index, string[] entry)
    {
        GameObject[] item = new GameObject[transform.childCount];
        if(index >= TableItem.Count)//ensure index is correct
        {
            index = TableItem.Count;
        }
        //add row to table-GameObject
        for(int i = 0; i< transform.childCount; i++)
        {
            Transform column = transform.GetChild(i);
            item[i] = Instantiate(TableItemPrefab,column);
            item[i].transform.SetSiblingIndex(index+1);//+1for header
            item[i].transform.GetComponent<Image>().color = (index % 2) == 0? ColorEven : ColorOdd;

            if(i < entry.Length)
            {
                item[i].GetComponentInChildren<TMP_Text>().text = entry[i];
            }
            else
            {
                item[i].GetComponentInChildren<TMP_Text>().text = "-";
            }
        }
        //add row to private table object
        TableItem.Insert(index, item);
        //if we inserted in the middle, recolor the list of items
        if(index < TableItem.Count)
        {
            Recolor();
        }
        //rebuild the layout for the added item
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        //tell the index where we added the item
        return index;
    }

    public GameObject[] getLine(GameObject itemInLine) // retrieve the line a cell-gameboject is in
    {
        foreach(GameObject[] line in TableItem)
        {
            foreach(GameObject item in line)
            {
                if(item == itemInLine)
                {
                    return line;
                }
            }
        }
        return null;
    }

    public Transform getColumn(GameObject itemInColumn) // retrieve the column-object a cell-gameboject is in
    {   
        GameObject[] item = new GameObject[transform.childCount];
        for(int i = 0; i< transform.childCount; i++)
        {
            Transform column = transform.GetChild(i);
            if(column.IsChildOf(itemInColumn.transform) == true)
            {
                return column;
            }
        }
        return null;
    }

    public List<GameObject[]> getTable()//get the table
    {
        return TableItem;
    }

    public GameObject getCell(int row, int column)
    {
        if(row < TableItem.Count)
        {
            GameObject[] line = TableItem[row];
            if(line != null && column < line.Length)
            {
                return line[column];
            }
            else
            {
                return null;
            }
        }
        else
        {
            return null;
        }
    }

    public GameObject[] getLine(int row)
    {
        if(row < TableItem.Count)
        {
            return TableItem[row];
        }
        else
        {
            return null;
        }
    }

    public void setOnClickEventForAll(UnityAction onClickEvent)
    {
        foreach(GameObject[] line in TableItem)
        {
            foreach(GameObject item in line)
            {
                if(item.GetComponent<Button>() != null)
                {
                    item.GetComponent<Button>().onClick.AddListener( onClickEvent );
                }
            }
        }
    }

    public void setOnClickEvent(int index, UnityAction onClickEvent)
    {
        foreach(GameObject item in TableItem[index])
        {
            if(item.GetComponent<Button>() != null)
            {
                item.GetComponent<Button>().onClick.AddListener( onClickEvent );
            }
        }
    }
    public void setOnHoverEventForAll()
    {
        foreach(GameObject[] line in TableItem)
        {
            foreach(GameObject item in line)
            {
                if(item.GetComponent<TableItemHelper>() != null)
                {
                    item.GetComponent<TableItemHelper>().line = line;
                } 
            }
        }
    }
    public void setOnHoverEvent(int index)
    {
        foreach(GameObject item in TableItem[index])
        {
            if(item.GetComponent<TableItemHelper>() != null)
            {
                item.GetComponent<TableItemHelper>().line = TableItem[index];
            } 
        }
    }
}
