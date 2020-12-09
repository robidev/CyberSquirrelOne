using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hacking : MonoBehaviour
{
    private PlayerMovement player;
    public LayerMask _Door;
    RaycastHit2D hitInfo;
    public AudioSource audioSource;
    public AudioClip hackingAudio;

    bool startListening = false;
    // Start is called before the first frame update
    void Start()
    {
        player = gameObject.GetComponent<PlayerMovement>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {   
        if( (Input.GetButtonDown ("Action1") | Input.GetButtonDown ("Action2") ) && player.selected == true && Time.timeScale > 0.1f)
        {
            startListening = true;
        }
        if ( startListening == true && ( Input.GetButtonUp ("Action1") | Input.GetButtonUp ("Action2") ) ) {
            startListening = false;
            if(player.selected == true && Time.timeScale > 0.1f)
            {
                hitInfo = Physics2D.Raycast(transform.position, Vector2.up, 0, _Door);
                if(hitInfo.collider != null) //we are in front of an open door
                { 
                    hitInfo.collider.gameObject.GetComponent<DoorLock>().DisableDialog();
                    if(hitInfo.collider.gameObject.GetComponent<DoorLock>().isOpen == false)
                    {
                        //Debug.Log("Door hacked!");
                        //we try to open the door, and start the hacking game
                        hitInfo.collider.gameObject.GetComponent<DoorLock>().isOpen = true;
                    }
                }	
                RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.up, 1);
                for (int i = 0; i < hits.Length; i++)
                {
                    RaycastHit2D hit = hits[i];
                    if(hit.collider != null && hit.collider.gameObject.tag == "HMI") //we are at an HMI
                    { 
                        //Debug.Log("HMI hacked!");
                        hit.collider.gameObject.GetComponent<HMI>().ToggleHacked();
                        if (audioSource && hackingAudio && audioSource.isPlaying == false)
                            audioSource.PlayOneShot(hackingAudio);
                    }	
                }	
            }
        }
    }
}
