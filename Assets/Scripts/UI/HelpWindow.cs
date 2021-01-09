using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpWindow : MonoBehaviour
{
    public GameObject HelpDialogPrefab;
    GameObject HelpInstance;
    [TextArea(3, 10)]    public string text = "";
    public void NewHelpDialog()
    {
        if(HelpInstance != null)
        {
            Destroy(HelpInstance);
            HelpInstance = null;
        }  

        HelpInstance = Instantiate(HelpDialogPrefab,transform.parent);
        HelpInstance.GetComponent<RectTransform>().SetAsLastSibling();
        HelpInstance.GetComponent<HMIHelpDialog>().textField.text = text;
    }
}
