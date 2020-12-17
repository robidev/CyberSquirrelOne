using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OperateDialog : MonoBehaviour
{
    public GameObject HelpDialogPrefab;
    GameObject HelpInstance;
    public TextMeshProUGUI ControlObject;
    public TextMeshProUGUI ErrorMessage;
    GameObject ControlObjectClass;
    public GameObject SureDialog;
    public GameObject AdvancedDialog;
    public GameObject ModalOverLayPrefab;
    public Button open;
    public Button close;
    GameObject overlay;
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
            "The Breaker Control dialog is for opening and closing the respective switch.\n" +
            "Please take care that it is safe to do so before confirming";
    }
    public void ShowDialog(GameObject controlObjectClass)
    {
        ControlObjectClass = controlObjectClass;
        ControlObject.text = "Object: " + controlObjectClass.name;

        if(ControlObjectClass.GetComponent<AnimateSwitch>().SwitchConducting == true)
        {
            //highlight open button
            open.interactable = true;
            close.interactable = false;
        }
        else
        {
            open.interactable = false;
            close.interactable = true;
            //highlight close button
        }
        ErrorMessage.color = Color.black;
        ErrorMessage.text = "status: Ready";
        gameObject.SetActive(true);
    }
    void OnEnable()
    {
        DoOverlay();
        gameObject.GetComponent<RectTransform>().SetAsLastSibling();
    }

    public void OperatePressed(bool state)
    {
        Destroy(overlay);
        SureDialog.GetComponent<SureDialog>().ShowDialog(ControlObject.text, ControlObjectClass.GetComponent<AnimateSwitch>().SwitchConducting.ToString(), dialogResult);
    }
    public void MorePressed()
    {
        Destroy(overlay);
        AdvancedDialog.GetComponent<AdvancedDialog>().ShowDialog(ControlObjectClass, dialogResult);
    }
     void dialogResult(int result)
    {
        Debug.Log("pressed:"+ result);
        //do action or not
        if(result == 1)
        {
            ErrorMessage.color = Color.red;//assume error
            //perform action
            if(open.interactable == true)
            {
                ControlObjectClass.GetComponent<AnimateSwitch>().SwitchConducting = false;
            }
            else
            {
                ControlObjectClass.GetComponent<AnimateSwitch>().SwitchConducting = true;
            }
        }
        else if(result == 2)
        {
            gameObject.SetActive(false);
        }
        else
        {
            DoOverlay();
            gameObject.GetComponent<RectTransform>().SetAsLastSibling();
        }
    }
    public void CancelPressed()
    {
        Destroy(overlay);
        gameObject.SetActive(false);
    }

    void DoOverlay()
    {
        overlay = Instantiate(ModalOverLayPrefab,transform.parent);
        overlay.GetComponent<RectTransform>().anchorMin = Vector2.zero;
        overlay.GetComponent<RectTransform>().anchorMax = Vector2.one;
        overlay.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
        overlay.GetComponent<RectTransform>().SetAsLastSibling();//set top background
    }//Destroy(overlay);

    public void SetOperateResult(int result)
    {
        if(result == 0) //success
        {
            ErrorMessage.text = "status: success";
            ErrorMessage.color = Color.green;
            if(ControlObjectClass.GetComponent<AnimateSwitch>().SwitchConducting == true)
            {
                //highlight open button
                open.interactable = true;
                close.interactable = false;
            }
            else
            {
                open.interactable = false;
                close.interactable = true;
                //highlight close button
            }
        }
        else if(result == -1)
        {
            ErrorMessage.color = Color.red;
            ErrorMessage.text = "status: blocked by interlock";
        }
        else if(result == -2)
        {
            ErrorMessage.color = Color.red;
            ErrorMessage.text = "status: equipment failure";
        }
        else
        {
            ErrorMessage.color = Color.red;
            ErrorMessage.text = "operate failed with code: " + result;
        }
    }
}
