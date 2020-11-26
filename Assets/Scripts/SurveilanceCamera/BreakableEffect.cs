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
    private bool isBroken = false;
    public bool BreakOnlyOnce = false;
    void Start()
    {
        if(transform.childCount > 0)
            child = transform.GetChild(0).gameObject;

        audioSource = GetComponent<AudioSource>();
    }

    public void Break()
    {
        if(BreakOnlyOnce == true && isBroken == true)
        {
            return;
        }

        if(child != null)
            child.SetActive(false);

        if(BreakEvent != null)
			BreakEvent.Invoke();
            
        if (audioSource && breakAudio)
        	audioSource.PlayOneShot(breakAudio);

        isBroken = true;
    }
}
