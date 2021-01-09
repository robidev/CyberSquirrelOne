using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EngineeringWindow : MonoBehaviour
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
            "The Engineering program is to alter device settings of the substation components.\n"+
            "You need to save the settings before they take effect.\n" +
            "Restore settings will reload the device settings in the current dialog";
    }
    public ScrollRect DeviceList;
    public Sprite SelectedButtonImg;
    public Sprite UnSelectedButtonImg;
    public GameObject SaveDialog;
    public TextMeshProUGUI DeviceName;
    public GameObject deviceItemPrefab;
    public GameObject SettingsTabs;
    public GameObject OverVoltageTabItemPrefab;
    public GameObject OverCurrentTabItemPrefab;
    public GameObject TimeOverCurrentItemPrefab;
    public GameObject DifferentialItemPrefab;
    public GameObject DistanceProtectionItemPrefab;

    bool SettingsDirty = false;
    ProtectionFunctions currentDevice = null;
    // Start is called before the first frame update
    void Start()
    {
        Transform content = DeviceList.viewport.GetChild(0);
        ProtectionFunctions[] devices =  Resources.FindObjectsOfTypeAll<ProtectionFunctions>();
        foreach(ProtectionFunctions device in devices)
        {
            GameObject deviceItem = Instantiate(deviceItemPrefab) as GameObject;
            deviceItem.GetComponentInChildren<TextMeshProUGUI>().text = device.name;
            //deviceItem.GetComponent<Button>().onClick.AddListener();
            deviceItem.GetComponent<Button>().onClick.AddListener(() => { 
                if(SettingsDirty == true)
                {
                    //display modal save dialog
                    SureDialog();
                }
                else
                {
                    foreach(Image img in content.GetComponentsInChildren<Image>())  
                    {
                        img.sprite = UnSelectedButtonImg;
                    }
                    deviceItem.GetComponent<Image>().sprite = SelectedButtonImg;
                    currentDevice = device;
                    SetTab();
                }
                });
            deviceItem.transform.SetParent(content,false);
        }
    }

    public void SureDialog()
    {
        if(currentDevice && SettingsDirty)
        {
            SaveDialog.GetComponent<SureDialog>().SetHelpText("This dialog is to ensure that saving was intended. The impact of applying settings can be high, and can have a direct effect");
            SaveDialog.GetComponent<SureDialog>().ShowDialog(currentDevice.name, "There are unsaved modified settings<br>Press OK to save, or Cancel to go back", SureDialogResult);
        }
    }

    public void RevertDialog()
    {
        if(currentDevice && SettingsDirty)
        {
            SaveDialog.GetComponent<SureDialog>().SetHelpText("This dialog is to ensure that reverting was intended. Work can be lost if OK is pressed accidentally");
            SaveDialog.GetComponent<SureDialog>().ShowDialog(currentDevice.name, "There are unsaved modified settings<br>Press OK to revert, or Cancel to go back", RevertDialogResult);
        }
    }
    void SureDialogResult(int result)
    {
        if(result == 1)//ok pressed
        {
            SaveSettings();
            SettingsDirty = false;
            SetTab();
        }
    }
    void RevertDialogResult(int result)
    {
        if(result == 1)//ok pressed
        {
            SetTab();
            SettingsDirty = false;
        }
    }

    void SaveSettings()
    {
        if(currentDevice == null)
            return;
        int i = 0;
        if(currentDevice.OverCurrent)
        {
            string val = SettingsTabs.transform.GetChild(i++).Find("CurrentSettings").GetComponent<TMP_InputField>().text;
            currentDevice.OverCurrentImmediate = float.Parse(val);
        }
        if(currentDevice.OverVoltage)
        {
            string val = SettingsTabs.transform.GetChild(i++).Find("VoltageSettings").GetComponent<TMP_InputField>().text;
            currentDevice.OverVoltageTreshold = float.Parse(val);
        }
        if(currentDevice.TimeOverCurrent)
        {
            string val1 = SettingsTabs.transform.GetChild(i).Find("TmCurrentSettings").GetComponent<TMP_InputField>().text;
            currentDevice.OverCurrentTreshold = float.Parse(val1);
            string val2 = SettingsTabs.transform.GetChild(i++).Find("TimeSettings").GetComponent<TMP_InputField>().text;
            currentDevice.OverCurrentTime = float.Parse(val2);
        }
        if(currentDevice.DifferentialProtection)
        {

        }
        if(currentDevice.DistanceProtection)
        {

        }
        Debug.Log("items saved");
    }

    void SetTab()
    {
        SettingsDirty = false;

        for(int i = 0; i < SettingsTabs.transform.childCount; i++)
        {
            Destroy(SettingsTabs.transform.GetChild(i).gameObject);
        }
        DeviceName.text = "Device: " + currentDevice.name;

        if(currentDevice.OverCurrent)
        {
            GameObject TabItem = Instantiate(OverCurrentTabItemPrefab) as GameObject;
            TabItem.transform.Find("CT").GetComponent<TMP_Text>().text = currentDevice.OverCurrentCT.name;
            TabItem.transform.Find("CurrentSettings").GetComponent<TMP_InputField>().text = currentDevice.OverCurrentImmediate.ToString();
            TabItem.transform.Find("CurrentSettings").GetComponent<TMP_InputField>().onValueChanged.AddListener((string val) => {
                SettingsDirty = true; 
                //device.OverCurrentImmediate = float.Parse(val);
                });

            TabItem.transform.SetParent(SettingsTabs.transform,false);
        }
        if(currentDevice.OverVoltage)
        {
            GameObject TabItem = Instantiate(OverVoltageTabItemPrefab) as GameObject;
            TabItem.transform.Find("VT").GetComponent<TMP_Text>().text = currentDevice.OverVoltageVT.name;
            TabItem.transform.Find("VoltageSettings").GetComponent<TMP_InputField>().text = currentDevice.OverVoltageTreshold.ToString();
            TabItem.transform.Find("VoltageSettings").GetComponent<TMP_InputField>().onValueChanged.AddListener((string val) => {
                SettingsDirty = true; 
                //device.OverVoltageTreshold = float.Parse(val);
                });

            TabItem.transform.SetParent(SettingsTabs.transform,false);
        }
        if(currentDevice.TimeOverCurrent)
        {
            GameObject TabItem = Instantiate(TimeOverCurrentItemPrefab) as GameObject;
            TabItem.transform.Find("CT").GetComponent<TMP_Text>().text = currentDevice.OverCurrentCT.name;
            TabItem.transform.Find("TmCurrentSettings").GetComponent<TMP_InputField>().text = currentDevice.OverCurrentTreshold.ToString();
            TabItem.transform.Find("TmCurrentSettings").GetComponent<TMP_InputField>().onValueChanged.AddListener((string val) => {
                SettingsDirty = true; 
                //device.OverCurrentTreshold = float.Parse(val);
                });

            TabItem.transform.Find("TimeSettings").GetComponent<TMP_InputField>().text = currentDevice.OverCurrentTime.ToString();
            TabItem.transform.Find("TimeSettings").GetComponent<TMP_InputField>().onValueChanged.AddListener((string val) => {
                SettingsDirty = true; 
                //device.OverCurrentTime = float.Parse(val);
                });

            TabItem.transform.SetParent(SettingsTabs.transform,false);
        }
        if(currentDevice.DifferentialProtection)//TODO
        {
            GameObject TabItem = Instantiate(DifferentialItemPrefab) as GameObject;
            TabItem.transform.Find("CurrentSettings").GetComponent<TMP_InputField>().text = currentDevice.differentialTreshold.ToString();
            //OverCurrentCTsOut;
            //OverCurrentCTsIn;
            TabItem.transform.SetParent(SettingsTabs.transform,false);
        }
        if(currentDevice.DistanceProtection)//TODO
        {
            GameObject TabItem = Instantiate(DistanceProtectionItemPrefab) as GameObject;
            TabItem.transform.Find("ImpedanceSettings").GetComponent<TMP_InputField>().text = currentDevice.ImpedanceTreshold.ToString();
            TabItem.transform.Find("CT").GetComponent<TMP_Text>().text = currentDevice.DistanceCT.name;
            TabItem.transform.Find("VT").GetComponent<TMP_Text>().text = currentDevice.DistanceVT.name;
            TabItem.transform.SetParent(SettingsTabs.transform,false);
        }
    }
}
