using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

	public Dialogue dialogue;

	public bool ShowDialogMoreThanOnce = false;
    private bool DialogHasBeenShown = false;
	public void TriggerDialogue ()
	{
		if(ShowDialogMoreThanOnce == true || DialogHasBeenShown == false)
        {
			FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
			DialogHasBeenShown = true;
		}
	}

}
