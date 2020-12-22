using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SureDialog : MonoBehaviour
{
    public delegate void SureDialogResult(int result);
    SureDialogResult DialogResult;
    public TextMeshProUGUI ControlObject;
    public TextMeshProUGUI Action;
    public GameObject HelpDialogPrefab;
    GameObject HelpInstance;
    public GameObject ModalOverLayPrefab;
    GameObject overlay;
    string HelpText = "";

    public void NewHelpDialog()
    {
        if(HelpInstance != null)
        {
            Destroy(HelpInstance);
            HelpInstance = null;
        }  

        HelpInstance = Instantiate(HelpDialogPrefab,transform.parent);
        HelpInstance.GetComponent<RectTransform>().SetAsLastSibling();
        if(HelpText == "")
        {
            HelpInstance.GetComponent<HMIHelpDialog>().textField.text = 
            "Confirming an action will execute it.\n" +
            "Please take care that it is safe to do so before confirming";
        }
        else
        {
            HelpInstance.GetComponent<HMIHelpDialog>().textField.text = HelpText;
        }
    }

    public void SetHelpText(string text)
    {
        HelpText = text;
    }
    public void ShowDialog(string controlObject, string action, SureDialogResult dialogResult)
    {
        DialogResult = dialogResult;
        ControlObject.text = controlObject;
        Action.text = action;
        
        gameObject.SetActive(true);
    }
    void OnEnable()
    {
        DoOverlay();
        gameObject.GetComponent<RectTransform>().SetAsLastSibling();
    }
    
    public void OkPressed()
    {
        gameObject.SetActive(false);
        Destroy(overlay);
        DialogResult.Invoke(1);
    }
    public void CancelPressed()
    {
        gameObject.SetActive(false);
        Destroy(overlay);
        DialogResult.Invoke(0);
    }
    public void ClosePressed()
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
