using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConsoleWindow : MonoBehaviour
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
            "The Console is currently limited in function, but can be used to issue commands such as ls and exit.";
    }
    private TMP_InputField TextField = null;
    private string TextVariable = "";

    void Awake()
    {
        TextField = GetComponent<TMP_InputField>();
        PrintText("bin-bash: # ",true);
        TextField.ActivateInputField();
        //TextField.caretPosition = TextVariable.Length;
        TextField.stringPosition = TextVariable.Length;
    }

    void OnEnable()
    {
        if(TextField != null)
        {
            TextField.ActivateInputField();
            //TextField.caretPosition = TextVariable.Length;
            TextField.stringPosition = TextVariable.Length;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(TextField.stringPosition < TextVariable.Length)
        {
            //TextField.caretPosition = TextVariable.Length;
            TextField.stringPosition = TextVariable.Length;
        }
    }

    public void filterInput(string text)
    {
        if(text.Length <= TextVariable.Length)
        {
            TextField.text = TextVariable;
            return;
        }
        text = text.Replace("\r","");
        int totalFinds = 0;
        int found = 0;
        int oldTextLenght = TextVariable.Replace("\r","").Length;
        for (int i = oldTextLenght-1; i < text.Length; i++)
        {
            found = text.IndexOf("\n", i);
            if (found > 0)
            {
                totalFinds++;
                i = found; 
                OnEnter(text.Substring(0,found));
                oldTextLenght = TextVariable.Replace("\r","").Length;
            }
        }
    }

    public void OnEnter(string text)
    {
        Debug.Log(text);
        string input = text.Substring(TextVariable.Length);
        
        if(input == "ls")
        {
            PrintText(input + "\n.\n..\nhome\nbin-bash: # ");
        }
        else if(input == "exit")
        {
            CloseSession();
        }
        else
        {
            PrintText(input + "\n" + input + ": command not found\nbin-bash: # ");
        }
        TextField.ActivateInputField();
    }

    void PrintText(string text, bool clear = false)
    {
        if(clear == true)
        {
            TextVariable = text;
        }
        else
        {
            TextVariable += text;
        }
        TextField.text = TextVariable;
    }

    public void CloseSession()
    {
        PrintText("bin-bash: # ",true);
        transform.parent.gameObject.SetActive(false);
    }
}
