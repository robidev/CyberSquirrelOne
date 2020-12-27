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
        SureDialog.GetComponent<SureDialog>().ShowDialog(ControlObject.text, "Action: " + ControlObjectClass.GetComponent<AnimateSwitch>().SwitchConducting.ToString(), dialogResult);
    }
    public void MorePressed()
    {
        Destroy(overlay);
        AdvancedDialog.GetComponent<AdvancedDialog>().ShowDialog(ControlObjectClass, dialogResult);
    }
     void dialogResult(int result)
    {
        //Debug.Log("pressed:"+ result);
        //do action or not
        if(result == 1)
        {
            //perform action
            if(open.interactable == true)
            {
                Debug.Log("1:"+ result);
                ErrorMessage.color = Color.black;
                ControlObjectClass.GetComponent<AnimateSwitch>().SwitchConducting = false;
            }
            else if (close.interactable == true)
            {
                Debug.Log("2:"+ result);
                ErrorMessage.color = Color.black;
                ControlObjectClass.GetComponent<AnimateSwitch>().SwitchConducting = true;
            }
            else
            {
                Debug.Log("3:"+ result);
                ErrorMessage.color = Color.red;
                ErrorMessage.text = "status: could not send operate, try again";
            }
        }
        else if(result == 2)
        {
            ErrorMessage.color = Color.black;
            ErrorMessage.text = "status: ";
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
        if(gameObject.activeInHierarchy == false)
        {
            return;
        }
        if(result == 2) //moving
        {
            ErrorMessage.text = "status: ready";
            ErrorMessage.color = Color.black;
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
        if(result == 1) //moving
        {
            ErrorMessage.text = "status: moving";
            ErrorMessage.color = Color.yellow;
            open.interactable = false;
            close.interactable = false;
        }
        else if(result == 0) //success
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
        else if(result == -3)
        {
            ErrorMessage.color = Color.red;
            ErrorMessage.text = "status: switch is allready moving"; 
        }
        else
        {
            ErrorMessage.color = Color.red;
            ErrorMessage.text = "operate failed with code: " + result;
        }
    }
}
