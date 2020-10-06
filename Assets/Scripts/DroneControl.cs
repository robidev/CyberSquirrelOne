using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneControl : MonoBehaviour
{
    private PlayerMovement player;
    private bool drone_control = false;
    private bool justSelected = true;
    public Cinemachine.CinemachineVirtualCamera drone_camera;
    public Cinemachine.CinemachineVirtualCamera player_camera;
    // Start is called before the first frame update
    public DroneBehaviour m_Drone;
    void Start()
    {
        player = gameObject.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player.selected == true)
        {
            if (Input.GetButtonDown ("Action1"))
            {
                drone_control = !drone_control;
                justSelected = true;//force update
            }
            
            if(justSelected) {
                justSelected = false;
                if(drone_control == true)
                {   
                    player.controllable = false;//disable squirrel movement
                    drone_camera.MoveToTopOfPrioritySubqueue();
                }
                else
                {
                    player.controllable = true;//enable squirrel movement
                    player_camera.MoveToTopOfPrioritySubqueue();
                }
            } 
        }
        else
        {
            justSelected = true;//force update next time we select this player
        }
        //ensure drone also receives the status
        m_Drone.drone_control = drone_control;
        m_Drone.selected = player.selected;
    }
}
