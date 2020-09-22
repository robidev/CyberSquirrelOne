﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    
    public LayerMask targetMask;
    public Transform[] PatrolWaypoints;
    public float speed = 800f;
    public float nextWaypointDistance = 1.2f;
    public Transform enemySprite;

    Transform target;

    Transform _Enemytarget = null;
    List<Transform> _AlternateEnemytarget;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndofPath = false;

    int currentPatrolWaypoint = 0;
    Seeker seeker;
    Rigidbody2D rb;
    bool patroling = false;
    // Start is called before the first frame update



    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, 0.5f);
        if(currentPatrolWaypoint < PatrolWaypoints.Length)
            target = PatrolWaypoints[currentPatrolWaypoint];
        
        _AlternateEnemytarget = new List<Transform>();
    }

    public Transform Enemytarget
    {
        get { return _Enemytarget; }
        set {
            if(_Enemytarget == value)
            {
                return;
            }
            //Debug.Log(value.name);
            // if the proposed target is not the preferred one, we ignore it, provided none has been set
            if(_Enemytarget != null && (((1 << value.gameObject.layer) & targetMask.value) == 0) ) 
            {
                Debug.Log("target: " + value.name + " ignored");
                if(_AlternateEnemytarget.Contains(value) == false)//add the alternate target only once
                {
                    _AlternateEnemytarget.Add(value);//keep a list of all alternate targets in the area
                }
                return;
            }
            Debug.Log("target: " + value.name + " set");
            
            patroling = false;
            reachedEndofPath = false;
            currentWaypoint = 0;
            if(_Enemytarget != null)//store the old target as an alternate
            {
                Debug.Log("old target: " + _Enemytarget.name + " now added to alternate targets");
                if(_AlternateEnemytarget.Contains(_Enemytarget) == false)//add the alternate target only once
                {
                    _AlternateEnemytarget.Add(_Enemytarget);//keep a list of all alternate targets in the area
                }  
            }
            _Enemytarget = value;
            target = value;
            seeker.StartPath(rb.position,target.position, OnPathComplete);//force path refresh
        }
    }

    public void UnsetEnemyTarget(Transform oldTarget)
    {
        //remove old alternate target
        if(_AlternateEnemytarget.Contains(oldTarget))
        {
            Debug.Log("removed " + oldTarget.name + " from alternate targets");
            _AlternateEnemytarget.Remove(oldTarget);
        }
        //remove old primary target
        if(_Enemytarget == oldTarget)
        {   
            if(_AlternateEnemytarget.Count > 0)
            {
                patroling = false;
                reachedEndofPath = false;
                currentWaypoint = 0;
                _Enemytarget = _AlternateEnemytarget[0];
                target = _AlternateEnemytarget[0];
                seeker.StartPath(rb.position,target.position, OnPathComplete);//force path refresh

                Debug.Log("removed " + oldTarget.name + ", now chasing "+ _Enemytarget.name);

                _AlternateEnemytarget.RemoveAt(0);
            }
            else
            {
                Debug.Log("removed " + oldTarget.name + ", now patroling");
                _Enemytarget = null;
                target = PatrolWaypoints[currentPatrolWaypoint];
                patroling = true;
                seeker.StartPath(rb.position,target.position, OnPathComplete);//force path refresh
            }
        }

    }

    void UpdatePath()
    {
        if(seeker.IsDone())
        {
            seeker.StartPath(rb.position,target.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(path == null)
            return;

        if(currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndofPath = true;
        } 
        else
        {
            reachedEndofPath = false;
            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            Vector2 force = direction * speed * Time.deltaTime;

            rb.AddForce(force);

            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
            if(distance < nextWaypointDistance)
            {
                currentWaypoint++;
            }

            if(force.x >= 0.01f)
            {
                enemySprite.localScale = new Vector3(-1f,1f,1f);
            }
            else if (force.x <= -0.01f)
            {
                enemySprite.localScale = new Vector3(1f,1f,1f);
            }
        }

        //check if we are attacking a player
        if(patroling == false)
        {
            if(PathUtilities.IsPathPossible(path.path) == false || reachedEndofPath == true)
            {
                //Debug.Log("reached or gave up");
                target = PatrolWaypoints[currentPatrolWaypoint];
                patroling = true;
            }
        }
        else //if we are patroling
        {
            //if we reached the waypoint, move to the next
            if(reachedEndofPath == true)
            {
                //patrol waypoints
                currentPatrolWaypoint++;
                if(currentPatrolWaypoint >= PatrolWaypoints.Length)
                {
                    currentPatrolWaypoint = 0;
                }
                target = PatrolWaypoints[currentPatrolWaypoint];
            }
        }
    }

    void OnCollisionEnter2D(Collision2D hitInfo)
	{
        //Debug.Log((1 << hitInfo.gameObject.layer) +" : "+ targetMask.value);
		if(((1 << hitInfo.gameObject.layer) & targetMask.value) > 0)
		{
			//Debug.Log(hitInfo.transform.name + " is caught by bird");

            PlayerMovement player = hitInfo.transform.gameObject.GetComponent<PlayerMovement>();
            if(player)
                player.Die(PlayerMovement.DieReason.Caught);

            DroneBehaviour drone = hitInfo.transform.gameObject.GetComponent<DroneBehaviour>();
            if(drone)
                drone.Die(PlayerMovement.DieReason.Caught);
			//hitInfo.gameObject.GetComponent<BreakableEffect>().Break();
		}
        
	}
}
