using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hacking : MonoBehaviour
{
    private PlayerMovement player;
    public LayerMask _Door;
    RaycastHit2D hitInfo;
    // Start is called before the first frame update
    void Start()
    {
        player = gameObject.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown (KeyCode.E) && player.selected == true) {
            hitInfo = Physics2D.Raycast(transform.position, Vector2.up, 0, _Door);
            if(hitInfo.collider != null && hitInfo.collider.gameObject.GetComponent<DoorLock>().isOpen == false) //we are in front of an open door
            { 
                //Debug.Log("Door hacked!");
                //we try to open the door, and start the hacking game
                hitInfo.collider.gameObject.GetComponent<DoorLock>().isOpen = true;
            }	
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.up, 1);
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit2D hit = hits[i];
                if(hit.collider != null && hit.collider.gameObject.tag == "HMI") //we are at an HMI
                { 
                    //Debug.Log("HMI hacked!");
                    hit.collider.gameObject.GetComponent<HMI>().ToggleHacked();
                }	
            }	
        }
    }
}
