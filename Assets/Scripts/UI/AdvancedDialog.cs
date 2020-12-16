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
    string Action;
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
    public void ShowDialog(string controlObject, string action, AdvancedDialogResult dialogResult)
    {
        DialogResult = dialogResult;
        ControlObject.text = controlObject;
        Action = action;
        if(action=="True")
        {
            //highlight open button
        }
        else
        {
            //highlight close button
        }
        
        gameObject.SetActive(true);
    }
    public void OkPressed()
    {
        Destroy(overlay);
        SureDialog.GetComponent<SureDialog>().ShowDialog(ControlObject.text, Action, dialogResult);
    }

    void dialogResult(int result)
    {
        //Debug.Log("prssed:"+ result);
        //do action or not
        if(result == 1)
        {
            //perform action
            //ControlObjectClass.GetComponent<AnimateSwitch>().SwitchConducting = val;
            gameObject.SetActive(false);
            DialogResult.Invoke(2);
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
