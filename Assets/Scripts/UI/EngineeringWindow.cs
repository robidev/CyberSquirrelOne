using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EngineeringWindow : MonoBehaviour
{
    public ScrollRect DeviceList;
    public TextMeshProUGUI DeviceName;
    public GameObject deviceItemPrefab;
    public GameObject SettingsTabs;
    public GameObject OverVoltageTabItemPrefab;
    public GameObject OverCurrentTabItemPrefab;
    public GameObject TimeOverCurrentItemPrefab;
    public GameObject DifferentialItemPrefab;
    public GameObject DistanceProtectionItemPrefab;
    // Start is called before the first frame update
    void Start()
    {
        Transform content = DeviceList.viewport.GetChild(0);
        ProtectionFunctions[] devices =  Resources.FindObjectsOfTypeAll<ProtectionFunctions>();
        foreach(ProtectionFunctions device in devices)
        {
            GameObject deviceItem = Instantiate(deviceItemPrefab) as GameObject;
            deviceItem.GetComponentInChildren<TextMeshProUGUI>().text = device.name;
            deviceItem.GetComponent<Toggle>().group = content.GetComponent<ToggleGroup>();
            //deviceItem.GetComponent<Button>().onClick.AddListener();
            deviceItem.GetComponent<Toggle>().onValueChanged.AddListener((bool isOn) => { if(isOn) SetTab(device);});
            deviceItem.transform.SetParent(content,false);
        }
    }

    void SetTab(ProtectionFunctions device)
    {
        for(int i = 0; i < SettingsTabs.transform.childCount; i++)
        {
            Destroy(SettingsTabs.transform.GetChild(i).gameObject);
        }
        DeviceName.text = "Device: " + device.name;

        if(device.OverCurrent)
        {
            GameObject TabItem = Instantiate(OverCurrentTabItemPrefab) as GameObject;
            TabItem.transform.Find("CT").GetComponent<TMP_Text>().text = device.OverCurrentCT.name;
            TabItem.transform.Find("CurrentSettings").GetComponent<TMP_InputField>().text = device.OverCurrentImmediate.ToString();
            TabItem.transform.Find("CurrentSettings").GetComponent<TMP_InputField>().onSubmit.AddListener((string val) => {device.OverCurrentImmediate = float.Parse(val);});

            TabItem.transform.SetParent(SettingsTabs.transform,false);
        }
        if(device.OverVoltage)
        {
            GameObject TabItem = Instantiate(OverVoltageTabItemPrefab) as GameObject;
            TabItem.transform.Find("VT").GetComponent<TMP_Text>().text = device.OverVoltageVT.name;
            TabItem.transform.Find("VoltageSettings").GetComponent<TMP_InputField>().text = device.OverVoltageTreshold.ToString();
            TabItem.transform.Find("VoltageSettings").GetComponent<TMP_InputField>().onSubmit.AddListener((string val) => {device.OverVoltageTreshold = float.Parse(val);});
            TabItem.transform.SetParent(SettingsTabs.transform,false);
        }
        if(device.TimeOverCurrent)
        {
            GameObject TabItem = Instantiate(TimeOverCurrentItemPrefab) as GameObject;
            TabItem.transform.Find("CT").GetComponent<TMP_Text>().text = device.OverCurrentCT.name;
            TabItem.transform.Find("CurrentSettings").GetComponent<TMP_InputField>().text = device.OverCurrentTreshold.ToString();
            TabItem.transform.Find("CurrentSettings").GetComponent<TMP_InputField>().onSubmit.AddListener((string val) => {device.OverCurrentTreshold = float.Parse(val);});

            TabItem.transform.Find("TimeSettings").GetComponent<TMP_InputField>().text = device.OverCurrentTime.ToString();
            TabItem.transform.Find("TimeSettings").GetComponent<TMP_InputField>().onSubmit.AddListener((string val) => {device.OverCurrentTime = float.Parse(val);});

            TabItem.transform.SetParent(SettingsTabs.transform,false);
        }
        if(device.DifferentialProtection)
        {
            GameObject TabItem = Instantiate(DifferentialItemPrefab) as GameObject;
            TabItem.transform.Find("CurrentSettings").GetComponent<TMP_InputField>().text = device.differentialTreshold.ToString();
            //OverCurrentCTsOut;
            //OverCurrentCTsIn;
            TabItem.transform.SetParent(SettingsTabs.transform,false);
        }
        if(device.DistanceProtection)
        {
            GameObject TabItem = Instantiate(DistanceProtectionItemPrefab) as GameObject;
            TabItem.transform.Find("ImpedanceSettings").GetComponent<TMP_InputField>().text = device.ImpedanceTreshold.ToString();
            TabItem.transform.Find("CT").GetComponent<TMP_Text>().text = device.DistanceCT.name;
            TabItem.transform.Find("VT").GetComponent<TMP_Text>().text = device.DistanceVT.name;
            TabItem.transform.SetParent(SettingsTabs.transform,false);
        }
    }
}
