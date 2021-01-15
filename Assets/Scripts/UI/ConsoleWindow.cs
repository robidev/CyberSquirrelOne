using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConsoleWindow : MonoBehaviour
{
    public GameObject HelpDialogPrefab;
    GameObject HelpInstance;
    private int response = 0;
    private int sudoresponse = 0;

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
        //Debug.Log(text);
        //print comand
        string input = text.Substring(TextVariable.Length);
        PrintText(input + "\n");

        //handle command
        if(input == "ls")
        {
            PrintText(".\n..\nprivate");
        }
        else if(input == "exit")
        {
            CloseSession();
        }
        else if(input == "help")
        {
            PrintText("ACDE shell, version 1.3.12(1)-release\n" +
             "These shell commands are defined internally.  Type `help' to see this list.\n" +
             "\nA star (*) next to a name means that the command is disabled.\n\n" +
             "  *cd [dir]\t\t\tchange current directory\n" +
             "  exit\t\t\t\tclose this prompt\n" +
             "  help\t\t\t\tprint this help\n" +
             "  ls\t\t\t\tlist the current directory\n" +
             "  sudo [command]\t\texecute command as super user\n");
        }
        else if(input.StartsWith("hack"))
        {
            PrintText("no!");
        }
        else if(input.StartsWith("sudo"))
        {
            if(input == "sudo")
            {
                PrintText("what are you trying to sudo?");
            }
            else if(input.StartsWith("sudo "))
            {
                string subcommand = input.Substring(5);
                if(subcommand == "hack")
                {
                    PrintText("Cool, if you want to cheat by hacking. Please type yes if you would like to cheat, and just finish this level");
                    
                }
                else if(response > 6)
                {
                    if(sudoresponse == 0)
                    {
                        PrintText("Why do you want to sudo '" + subcommand + "'?");
                    }
                    else if(sudoresponse == 1)
                    {
                        PrintText("Have you tried 'sudo hack' instead of '" + subcommand + "'?");
                    }                    
                    sudoresponse++;
                }
                else
                {
                    PrintText("You cannot sudo '" + subcommand + "'");
                    response++;
                }
            }
        }
        else
        {
            if(response < 3)
            {
                PrintText(input + ": command not found");
            }
            else if(response == 3 || response == 4)
            {
                PrintText(input + ": command still not found");
            }
            else if(response == 5)
            {
                PrintText(".");
            }
            else if(response == 6)
            {
                PrintText("..");
            }
            else if(response == 7)
            {
                PrintText("what are you trying to do?");
            }
            else if(response == 8)
            {
                PrintText("you are persistent!");
            }
            else if(response >= 9)
            {
                PrintText("have you tried sudo?");
            }
            
            response++;
        }
        //print commandline
        PrintText("\nbin-bash: # ");
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
