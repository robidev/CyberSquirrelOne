using UnityEngine;
using System.Collections;

public class Grapplinghook : MonoBehaviour {
	public LineRenderer line;
	DistanceJoint2D joint;
	Vector3 targetPos;
	RaycastHit2D hit;
	public float distance=10f;
	public LayerMask mask;
	public float step = 0.02f;
	private PlayerMovement player;
	Collider2D[] colls;

	bool isElectrocuted = false;

    public AudioSource audioSource;
    public AudioClip throwAudio;
	// Use this for initialization
	void Start () {
		joint = GetComponent<DistanceJoint2D> ();
		joint.enabled = false;
		line.enabled = false;
		isElectrocuted = false;
		player = gameObject.GetComponent<PlayerMovement>();
		audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.timeScale > 0.1f)
		{
			if (Input.GetButtonDown ("Action1") && player.selected == true && gameObject.layer == LayerMask.NameToLayer("Player_outside")) {
				//test for object to grapple
				Collider2D coll = null;
				Vector2 ppoint;
				colls = Physics2D.OverlapCircleAll(transform.position, 20f,mask);
				//if object(s) found..,check for the closest one 
				if(colls!=null && colls.Length > 0)
				{
					Vector2 myLocation = new Vector2(transform.position.x,transform.position.y);
					ppoint = Physics2D.ClosestPoint(transform.position, colls[0]);
					Vector2 closest = ppoint - myLocation;
					coll = colls[0];
					
					for(int i = 1; i < colls.Length; i++)
					{
						Vector2 ppoint2 = Physics2D.ClosestPoint(transform.position, colls[i]);
						if((ppoint2 - myLocation).sqrMagnitude < closest.sqrMagnitude)
						{
							closest = ppoint2 - myLocation;
							ppoint = ppoint2;
							coll = colls[i];
						}
					}
				
					//coll is now the closest object
					if(coll.gameObject.GetComponent<Rigidbody2D>()!=null)
					{
						//if it is a powerline
						if (audioSource && throwAudio && audioSource)
            				audioSource.PlayOneShot(throwAudio);
							
						if(coll.gameObject.GetComponent<Powerline>() != null)
						{
							//if line was powered, we lose control and get electrocuted
							bool isPowered = coll.gameObject.GetComponent<Powerline>().isPowered;
							line.GetComponent<Lightningstrike>().linePowered = isPowered;
							isElectrocuted = isPowered;
							
							if(isElectrocuted == true)
								player.Die(PlayerMovement.DieReason.Electrocuted);
						}
						else
						{
							line.GetComponent<Lightningstrike>().linePowered = false;
							isElectrocuted = false;
						}

						joint.enabled=true;
						Vector2 connectPoint = ppoint - new Vector2(coll.transform.position.x,coll.transform.position.y);
						connectPoint.x = connectPoint.x / coll.transform.localScale.x;
						connectPoint.y = connectPoint.y / coll.transform.localScale.y;
						//Debug.Log (connectPoint);
						joint.connectedAnchor = connectPoint;
										
						joint.connectedBody=coll.gameObject.GetComponent<Rigidbody2D>();
						joint.distance= Vector2.Distance(transform.position,ppoint);
						
						line.enabled=true;
						line.SetPosition(0,transform.position);
						line.SetPosition(1,ppoint);
					}
				}
			}
			if(line != null && joint.connectedBody != null && joint.connectedAnchor != null )
			{
				line.SetPosition(1,joint.connectedBody.transform.TransformPoint( joint.connectedAnchor));
			}
			
			//continuous update during e pressed
			if (Input.GetButton ("Action1") && player.selected == true) {
				line.SetPosition(0,transform.position);
			}
				
			//remove line on release
			if ((Input.GetButtonUp ("Action1") || player.selected != true) && isElectrocuted == false) {
				joint.enabled=false;
				line.enabled=false;
				isElectrocuted = false;
				player.controllable = true;
			}
		}
	}

	void FixedUpdate()
	{
		if(Time.timeScale > 0.1f)
		{
			if(isElectrocuted == false)
			{	//pull towards target, until reached
				if (joint.distance > .1f)
					joint.distance -= step;
				else { // position reached
					line.enabled = false;
					joint.enabled=false;				
				}
			}
			else
			{
				player.controllable = false;
			}
		}
	}
}
