using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EditorWindow : MonoBehaviour
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
            "The Editor can be used to view documents and take notes. You do not have permission to save documents.";
    }    
    public TMP_InputField TextField;

    public void DisplayText(string text)
    {
        TextField.text = text;
        gameObject.SetActive(true);
    }

    public void DisplayTextResourceByName(string resourceName)
    {
        TextAsset text = Resources.Load(resourceName) as TextAsset;
        TextField.text = text.text;
        gameObject.SetActive(true);
    }

    public void DisplayTextResourceObject(TextAsset TextResource)
    {
        TextField.text = TextResource.text;
        //TextField.verticalScrollbar.Rebuild()
        gameObject.SetActive(true);
    }
}
