using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectiveTracker : MonoBehaviour
{
    [TextArea(3, 10)]
	public string startScentence;
    public TextMeshProUGUI dialogueText;
    int size = 0;
    void Start()
    {
        size = gameObject.transform.childCount;
        /*for (int i = 0; i < size; i++)
        {
            Debug.Log(gameObject.transform.GetChild(i).name);
        }*/
    }

    public void ShowNextDialogue()
    {
        for (int i = 0; i < size; i++)
        {
            var trigger = gameObject.transform.GetChild(i).GetComponent<DialogueTrigger>();
            if(trigger != null && trigger.DialogHasBeenShown == false)
            {
                trigger.TriggerDialogue();
                break;
            }
        }        
    }

    public void GetShownDialogue()
    {
        dialogueText.text = startScentence;
        for (int i = 0; i < size; i++)
        {
            var trigger = gameObject.transform.GetChild(i).GetComponent<DialogueTrigger>();
            if(trigger != null && trigger.DialogHasBeenShown == true)
            {
                foreach(string scentence in trigger.dialogue.sentences)
                {
                    dialogueText.text += scentence + "\n\n";
                }
            }
        }        
    }
}
