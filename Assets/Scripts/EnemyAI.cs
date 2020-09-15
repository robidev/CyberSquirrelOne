using System.Collections;
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
            //to ensure we are not immediately retargetting
            if(currentWaypoint > 1)
            {
                //scan for squirrels vertical, and attack if seen
                RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.down, 1000f, targetMask);
                if (hitInfo && hitInfo.transform.tag != "drone")
                {
                    //Debug.Log(hitInfo.transform.name);
                    target = hitInfo.transform;
                    patroling = false;
                    reachedEndofPath = false;
                    currentWaypoint = 0;
                }
                else
                {
                    //scan for drones horizontal, and attack if seen
                    Collider2D coll = Physics2D.OverlapBox(transform.position,new Vector2(100,10),0,targetMask);
                    if(coll && coll.gameObject.tag == "drone")
                    {
                        //Debug.Log(coll.transform.name);
                        target = coll.transform;
                        patroling = false;
                        reachedEndofPath = false;
                        currentWaypoint = 0;
                    }
                }
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
