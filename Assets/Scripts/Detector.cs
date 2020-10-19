using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    public bool DetectorEnabled = true;

    SpriteRenderer sprite;
    int detected = 0;
    List<Collider2D> detectedStay;
    IEnumerator coroutine;
    public AudioSource audioSource;
    public AudioClip alertAudio;
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        detectedStay = new List<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {   
        if(DetectorEnabled)
        {
            if(detected == 0)
            {
                PlayerMovement player = other.gameObject.GetComponent<PlayerMovement>();
                if(player == null)
                {
                    player = other.gameObject.GetComponentInChildren<PlayerMovement>();
                }
                if(player != null)
                {
                    sprite.color = Color.red;
                    coroutine = AlarmTimer(5f, player);
                    StartCoroutine(coroutine);
                    //Debug.Log("start");
                    if (audioSource && alertAudio && audioSource.isPlaying == false)
                		audioSource.PlayOneShot(alertAudio);
                }
            }
            detected++;
            detectedStay.Add(other);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(DetectorEnabled)
        {
            if(detectedStay.Contains(other) == false)//if a detected object is not in the list for any reason
            {
                OnTriggerEnter2D(other);//manually call the enter function
            }
        } 
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(detected > 0)
            detected--;
        if(detected == 0)
        {
            sprite.color = Color.white;
            if(coroutine != null)
                StopCoroutine(coroutine);
        }
        detectedStay.Remove(other);
        //Debug.Log("stop");
    }

    IEnumerator AlarmTimer(float duration, PlayerMovement player)
    {
        //Debug.Log("started");
        yield return new WaitForSeconds(duration);
        player.Die(PlayerMovement.DieReason.Caught);
    }
}
