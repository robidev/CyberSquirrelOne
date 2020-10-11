using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using System.Text.RegularExpressions;


public class DialogueManager : MonoBehaviour {

	public GameObject dialog;
	public Text nameText;
	public Text dialogueText;
	public UnityEvent m_StartDialogsEvent;
	//public Animator animator;

	private Queue<string> sentences;
	private float oldTimeScale;

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
		if( dialog.activeSelf == true && (Input.GetKeyUp(KeyCode.Return) 
										|| Input.GetKeyUp(KeyCode.Space) 
										|| Input.GetKeyUp(KeyCode.H)
										|| Input.GetButtonUp("Submit") 
										|| Input.GetButtonUp("Action1") 
										|| Input.GetButtonUp("Action2")) )
		{
			DisplayNextSentence ();
		}
	}

	public void StartDialogue (Dialogue dialogue)
	{
		dialog.SetActive(true);
		oldTimeScale = Time.timeScale;
		Time.timeScale = 0;
		nameText.text = dialogue.name;

		sentences.Clear();

		foreach (string sentence in dialogue.sentences)
		{
			sentences.Enqueue(sentence);
		}

		DisplayNextSentence();
	}

	private IEnumerable<string> GetSubStrings(string input, string start, string end)
	{
		Regex r = new Regex(Regex.Escape(start) + "(.*?)"  + Regex.Escape(end));
		MatchCollection matches = r.Matches(input);
		foreach (Match match in matches)
			yield return match.Groups[1].Value;
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

		var keys = GetSubStrings(sentence,"{","}");
		foreach(string key in keys)
		{
			sentence = sentence.Replace("{" + key + "}", key); // for future reference
		}

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
		Time.timeScale = oldTimeScale;
	}

}
