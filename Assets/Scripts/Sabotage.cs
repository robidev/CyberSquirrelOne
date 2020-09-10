using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sabotage : MonoBehaviour
{
    private PlayerMovement player;
    // Start is called before the first frame update
    void Start()
    {
        player = gameObject.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown (KeyCode.E) && player.selected == true) {
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.up, 1);
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit2D hit = hits[i];
                if(hit.collider != null && hit.collider.gameObject.tag == "HV") //we are at an HMI
                { 
                    Debug.Log("HV equipment: " + hit.collider.gameObject.name + " sabotaged!");
                }	
            }	
        }
    }
}
