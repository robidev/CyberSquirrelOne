using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneControl : MonoBehaviour
{
    private PlayerMovement player;
    public bool drone_control = false;
    private bool justSelected = true;
    public Cinemachine.CinemachineVirtualCamera drone_camera;
    public Cinemachine.CinemachineVirtualCamera player_camera;
    // Start is called before the first frame update
    public DroneBehaviour m_Drone;
    private Shoot shoot;
    public Transform listener;
    void Start()
    {
        player = gameObject.GetComponent<PlayerMovement>();
        shoot = gameObject.GetComponent<Shoot>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player.selected == true && Time.timeScale > 0.1f)
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
                    listener.parent = drone_camera.Follow;
                    listener.localPosition = Vector3.zero;
                }
                else
                {
                    player.controllable = true;//enable squirrel movement
                    player_camera.MoveToTopOfPrioritySubqueue();
                    listener.parent = player_camera.Follow;
                    listener.localPosition = Vector3.zero;
                }
            } 
        }
        else
        {
            justSelected = true;//force update next time we select this player
        }
        //ensure drone and shoot also receives the status
        m_Drone.drone_control = drone_control;
        m_Drone.selected = player.selected;
        shoot.canShoot = !drone_control;
    }
}
