using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EngineeringWindow : MonoBehaviour
{
    public ScrollRect DeviceList;
    public GameObject deviceItemPrefab;
    // Start is called before the first frame update
    void Start()
    {
        Transform content = DeviceList.viewport.GetChild(0);
        ProtectionFunctions[] devices =  Resources.FindObjectsOfTypeAll<ProtectionFunctions>();
        foreach(ProtectionFunctions device in devices)
        {
            GameObject deviceItem = Instantiate(deviceItemPrefab) as GameObject;
            deviceItem.GetComponentInChildren<TextMeshProUGUI>().text = device.name;
            deviceItem.transform.SetParent(content,false);
        }
    }
}
