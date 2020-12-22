using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AdvancedDialog : MonoBehaviour
{
    public delegate void AdvancedDialogResult(int result);
    AdvancedDialogResult DialogResult;
    public GameObject HelpDialogPrefab;
    GameObject HelpInstance;
    public TextMeshProUGUI ControlObject;
    public GameObject SureDialog;
    public GameObject ModalOverLayPrefab;
    GameObject overlay;
    public TMP_Dropdown CtlVal;
    public TMP_InputField CtlNum;
    public TMP_InputField Origin;
    public TMP_Dropdown Test;
    public TMP_InputField T;
    public TMP_Dropdown check;
    private GameObject ControlObjectClass;
    public TextMeshProUGUI Status;

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
            "The Advanced dialog is for controlling the parameters of a switch command.\n" +
            "Please note that a wrong value can have unforseen consequences";
    }

    void OnEnable()
    {
        DoOverlay();
        gameObject.GetComponent<RectTransform>().SetAsLastSibling();
    }
    public void ShowDialog(GameObject controlObject, AdvancedDialogResult dialogResult)
    {
        ControlObjectClass = controlObject;
        DialogResult = dialogResult;
        ControlObject.text = "Object: " + ControlObjectClass.name;
        if(ControlObjectClass.GetComponent<AnimateSwitch>().SwitchConducting==true)
        {
            //highlight open button
            CtlVal.value = 0;
        }
        else
        {
            //highlight close button
            CtlVal.value = 1;
        }
        CtlNum.text = ControlObjectClass.GetComponent<AnimateSwitch>().CtlNum.ToString();
        Origin.text = "Station:0.1";
        Test.value = 0;
        T.text = System.DateTime.Now.ToString();
        check.value = 3;
        Status.color = Color.black;
        Status.text = "status: Ready";
        gameObject.SetActive(true);
    }
    public void OkPressed()
    {
        Destroy(overlay);
        SureDialog.GetComponent<SureDialog>().ShowDialog(ControlObject.text, "Action: " + ControlObjectClass.GetComponent<AnimateSwitch>().SwitchConducting.ToString(), dialogResult);
    }

    void dialogResult(int result)
    {
        //Debug.Log("prssed:"+ result);
        //do action or not
        if(result == 1)
        {
            Status.color = Color.red;
            if((CtlVal.value == 0 && ControlObjectClass.GetComponent<AnimateSwitch>().SwitchConducting == false) ||
                (CtlVal.value == 1 && ControlObjectClass.GetComponent<AnimateSwitch>().SwitchConducting == true))
            {
                Status.text = "status: CtlVal: allready at value";
                DoOverlay();
                gameObject.GetComponent<RectTransform>().SetAsLastSibling();
                return;
            }

            try
            {
                ControlObjectClass.GetComponent<AnimateSwitch>().CtlNum = int.Parse(CtlNum.text);
            }
            catch
            {
                Status.text = "status: CtlNum: invalid, must be digit";
                DoOverlay();
                gameObject.GetComponent<RectTransform>().SetAsLastSibling();
                return;
            }       
            if(Test.value != 0)
            {
                Status.text = "status: Test: true, operate not executed";
                DoOverlay();
                gameObject.GetComponent<RectTransform>().SetAsLastSibling();
                return;
            }
            if(check.value == 1 || check.value == 3)//interlock check enabled, or both
            {
                if(CtlVal.value == 0)
                {
                    ControlObjectClass.GetComponent<AnimateSwitch>().SwitchConducting = false;
                }
                else
                {
                    ControlObjectClass.GetComponent<AnimateSwitch>().SwitchConducting = true;
                }
            }
            else//none or synchrocheck enabled
            {
                if(CtlVal.value == 0)
                {
                    ControlObjectClass.GetComponent<AnimateSwitch>().OperateOverrideChecks(false);
                }
                else
                {
                    ControlObjectClass.GetComponent<AnimateSwitch>().OperateOverrideChecks(true);
                }
            }

            gameObject.SetActive(false);
            DialogResult.Invoke(0);
        }
        else
        {
            DoOverlay();
            gameObject.GetComponent<RectTransform>().SetAsLastSibling();
        }
    }
    public void CancelPressed()
    {
        gameObject.SetActive(false);
        Destroy(overlay);
        DialogResult.Invoke(0);
    }


    void DoOverlay()
    {
        overlay = Instantiate(ModalOverLayPrefab,transform.parent);
        overlay.GetComponent<RectTransform>().anchorMin = Vector2.zero;
        overlay.GetComponent<RectTransform>().anchorMax = Vector2.one;
        overlay.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
        overlay.GetComponent<RectTransform>().SetAsLastSibling();//set top background
    }//Destroy(overlay);
}
