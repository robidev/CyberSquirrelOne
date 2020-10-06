using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class DialogueManager : MonoBehaviour {

	public GameObject dialog;
	public Text nameText;
	public Text dialogueText;
	public UnityEvent m_StartDialogsEvent;
	//public Animator animator;

	private Queue<string> sentences;

	// Use this for initialization
	void Start () {
		sentences = new Queue<string>();
		Invoke("StartEvent", 1f);
	}

	void StartEvent()
	{
		if(m_StartDialogsEvent != null)
			m_StartDialogsEvent.Invoke();
	}

	void Update()
	{
		if(Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.T) )//Input.anyKey)//
		{
			DisplayNextSentence ();
		}
	}

	public void StartDialogue (Dialogue dialogue)
	{
		//animator.SetBool("IsOpen", true);
		dialog.SetActive(true);
		Time.timeScale = 0;
		nameText.text = dialogue.name;

		sentences.Clear();

		foreach (string sentence in dialogue.sentences)
		{
			sentences.Enqueue(sentence);
		}

		DisplayNextSentence();
	}

	public void DisplayNextSentence ()
	{
		if (sentences.Count == 0)
		{
			EndDialogue();
			return;
		}

		string sentence = sentences.Dequeue();
		StopAllCoroutines();
		StartCoroutine(TypeSentence(sentence));
	}

	IEnumerator TypeSentence (string sentence)
	{
		dialogueText.text = "";
		foreach (char letter in sentence.ToCharArray())
		{
			dialogueText.text += letter;
			yield return null;
		}
	}

	void EndDialogue()
	{
		dialog.SetActive(false);
		Time.timeScale = 1;
		//animator.SetBool("IsOpen", false);
	}

}
