using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TableItemHelper : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject[] line;
    public virtual void OnPointerEnter(PointerEventData e)
    {
        foreach(GameObject item in line)
        {
            item.GetComponent<Button>().OnPointerEnter(e);
        }
    }

    public virtual void OnPointerExit(PointerEventData e)
    {
        foreach(GameObject item in line)
        {
            item.GetComponent<Button>().OnPointerExit(e);
        }
    }
}
