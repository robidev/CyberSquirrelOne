using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BreakableEffect : MonoBehaviour
{
    GameObject child;
    public UnityEvent BreakEvent;
    public AudioSource audioSource;
    public AudioClip breakAudio;
    // Start is called before the first frame update
    void Start()
    {
        child = transform.GetChild(0).gameObject;
        audioSource = GetComponent<AudioSource>();
    }

    public void Break()
    {
        child.SetActive(false);
        if(BreakEvent != null)
			BreakEvent.Invoke();
            
        if (audioSource && breakAudio)
        	audioSource.PlayOneShot(breakAudio);
    }
}
