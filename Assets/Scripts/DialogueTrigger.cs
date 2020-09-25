using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

	public Dialogue dialogue;
	public bool ShowDialogMoreThanOnce = false;
    private bool _DialogHasBeenShown = false;

	private bool DialogueEnabled = true;
	public bool DialogHasBeenShown {
		get {return _DialogHasBeenShown;}
		set {_DialogHasBeenShown = value;}
	}

	public void EnableDialogue()
	{
		DialogueEnabled = true;
	}

	public void DisableDialogue()
	{
		DialogueEnabled = false;
	}

	public void TriggerDialogue ()
	{
		if(DialogueEnabled == true)
		{
			if(ShowDialogMoreThanOnce == true || _DialogHasBeenShown == false)
			{
				FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
				_DialogHasBeenShown = true;
			}
		}
	}

}
