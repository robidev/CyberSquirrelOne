using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSfx : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip hoverAudio;
    public AudioClip clickAudio;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnMouseEnter()
    {
        Debug.Log ("hover Play_Audio");
        audioSource.PlayOneShot(hoverAudio);
    }

    void OnMouseClick()
    {
        Debug.Log ("click Play_Audio");
        audioSource.PlayOneShot(clickAudio);        
    }
}
