using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayerOnTrigger : MonoBehaviour
{
    public PlayerMovement.DieReason reason;
    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerMovement player = other.transform.gameObject.GetComponent<PlayerMovement>();
        if(player)
            player.Die(reason);
    }
}
