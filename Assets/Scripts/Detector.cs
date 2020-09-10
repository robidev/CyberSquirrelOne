using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    public bool DetectorEnabled = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {   
        var player = other.gameObject.GetComponent<PlayerMovement>();
        if(DetectorEnabled && player != null)
        {
            player.Die(PlayerMovement.DieReason.Caught);
        }
    }
}
