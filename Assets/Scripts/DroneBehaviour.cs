using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneBehaviour : MonoBehaviour
{
    private Rigidbody2D drone_Rigidbody2D;
    public bool drone_control = false;
    public bool selected = false; 
    public float flySpeed = 40f;
    public LayerMask layer;
    float horizontalMove = 0f;
    float verticalMove = 0f;
    private GameObject oldParent = null;
    private GameObject object1 = null;
    private CircleCollider2D m_ObjectTrigger;
    private Vector3 m_Velocity = Vector3.zero;
    public AudioSource audioSource;
    public AudioClip startDrone;
    public AudioClip flyDrone;
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
    // Start is called before the first frame update
    void Start()
    {
        drone_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_ObjectTrigger = GetComponent<CircleCollider2D>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = startDrone;
        audioSource.loop = false;
        audioSource.Play();
        audioSource.Pause();//make it ready to play on selection
    }

    // Update is called once per frame
    void Update()
    {
        if(drone_control == true && selected == true && Time.timeScale > 0.1f)
        {
            audioSource.UnPause();
            if (Input.GetButtonDown ("Action2") && object1 != null)
            {
                //object1.transform.localPosition = new Vector2(0,-1f);
                var mov = object1.GetComponent<PlayerMovement>();
                if(mov != null)//set the correct animation if it is a player
                {
                    mov.OnHanging(false);
                }
                object1.transform.parent = oldParent.transform;
                object1.GetComponent<Rigidbody2D>().isKinematic = false;
                object1 = null;
                m_ObjectTrigger.isTrigger = true;
            }
            
            horizontalMove = Input.GetAxisRaw("Horizontal") * flySpeed;
            verticalMove = Input.GetAxisRaw("Vertical") * flySpeed;

            if(audioSource.isPlaying == false)//when the start-sound is finished, play the continuous sound
            {
                audioSource.clip = flyDrone;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        else
        {
            if(audioSource.isPlaying == true)
            {
                audioSource.Pause();
            }
        }
    }

    void FixedUpdate ()
	{
		// Move our character
		if(drone_control == true && selected == true) {
            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector2(horizontalMove * 10f * Time.fixedDeltaTime, verticalMove * 10f * Time.fixedDeltaTime);
            // And then smoothing it out and applying it to the character
            drone_Rigidbody2D.velocity = Vector3.SmoothDamp(drone_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
            if(object1 != null)
            {
                object1.transform.localPosition = new Vector2(0,-0.5f);
            }
		}
	}
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(object1 == null && layer.value == (layer.value | 1<<other.gameObject.layer))//TODO add check if object is suitable to pick up
        {
            object1 = other.gameObject;
            oldParent = object1.transform.parent.gameObject;
            object1.transform.parent = drone_Rigidbody2D.transform;
            object1.GetComponent<Rigidbody2D>().isKinematic = true;
            object1.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f);
            object1.GetComponent<Rigidbody2D>().inertia = 0f;

            var mov = object1.GetComponent<PlayerMovement>();
            if(mov != null)//set the correct animation if it is a player
            {
                mov.OnHanging(true);
            }
            //m_ObjectTrigger.isTrigger = false;
        }
    }

    public void Die(PlayerMovement.DieReason reason)
    {
        Debug.Log("I died because of reason: " + reason.ToString());
        FindObjectOfType<GlobalControl>().Invoke("GameOver", 0);
    }
}
