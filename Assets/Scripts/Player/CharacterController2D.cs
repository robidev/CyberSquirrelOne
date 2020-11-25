using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private float m_JumpForce = 250f;							// Amount of force added when the player jumps.
	[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .4f;			// Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;							// Whether or not a player can steer while jumping;
	[SerializeField] public LayerMask m_WhatIsGround;							// A mask determining what is ground to the character

	[SerializeField] public LayerMask m_WhatIsHangable;							// A mask determining what is ground to the character
	[SerializeField] public Transform m_GroundCheck;							// A position marking where to check if the player is grounded.
	[SerializeField] public Transform m_CeilingCheck;							// A position marking where to check for ceilings
	[SerializeField] public Collider2D m_CrouchDisableCollider;				// A collider that will be disabled when crouching

	const float k_GroundedRadius = .1f; // Radius of the overlap circle to determine if grounded
	public bool m_Grounded = true;            // Whether or not the player is grounded.
	const float k_CeilingRadius = .1f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	public bool FacingRight	{ get { return m_FacingRight;}	}
	private Vector3 m_Velocity = Vector3.zero;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;
	public UnityEvent OnJumpEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }
	public BoolEvent OnCrouchEvent;
	public BoolEvent OnHangEvent;
	public BoolEvent OnLadderEvent;
	private bool m_wasCrouching = false;
	private float normalGravity;
	public bool isHanging = false;
	public bool isOnLadder = false;
	public LayerMask _Ladder;
	RaycastHit2D hitInfo;
	bool climbOnWire = false;
	bool dropFromWire = false;

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		normalGravity = m_Rigidbody2D.gravityScale;
		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

		if (OnCrouchEvent == null)
			OnCrouchEvent = new BoolEvent();
	}

	private void FixedUpdate()
	{
		bool wasGrounded = m_Grounded;
		bool wasHanging = isHanging;
		bool wasOnLadder = isOnLadder;

		m_Grounded = false;
		isHanging = false;
		isOnLadder = false;
		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject && m_Rigidbody2D.velocity.y < m_JumpForce * 0.55f)
			{
				m_Grounded = true;
				if (wasGrounded == false)
					OnLandEvent.Invoke();
			}
		}

		//hang hitbox
		if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsHangable))
		{
			isHanging = true;
			if(wasHanging == false){
				m_Rigidbody2D.gravityScale = 0;
				OnHangEvent.Invoke(true);
				//Debug.Log("hang");
			}
		}
		if(wasHanging == true && isHanging == false){
			OnHangEvent.Invoke(false);
			climbOnWire = false;
			dropFromWire = false;
			m_Rigidbody2D.gravityScale = normalGravity;
		}

		//ladder/stairs hitbox
		hitInfo = Physics2D.Raycast(transform.position, Vector2.up, 0, _Ladder);
		if(hitInfo.collider != null && gameObject.layer == 11) //we are inside
		{ 
			isOnLadder = true;
			if(wasOnLadder == false)
			{
				m_Rigidbody2D.gravityScale = 0;
				//OnLadderEvent.Invoke(true);
				//Debug.Log("ladder");
			}
		}
		if(wasOnLadder == true && isOnLadder == false)
		{
			//OnLadderEvent.Invoke(false);
			m_Rigidbody2D.gravityScale = normalGravity;
		}

		if(climbOnWire){
			m_Rigidbody2D.velocity = new Vector2(0,0);
			m_Rigidbody2D.MovePosition(Vector3.Lerp(transform.position, m_CeilingCheck.position, Time.fixedDeltaTime * 10));
			//transform.position = Vector3.Lerp(transform.position, m_CeilingCheck.position, Time.fixedDeltaTime * 10);
		}
		if(dropFromWire){
			m_Rigidbody2D.velocity = new Vector2(0,0);
			m_Rigidbody2D.MovePosition(Vector3.Lerp(transform.position, m_GroundCheck.position, Time.fixedDeltaTime * 10));
			//transform.position = Vector3.Lerp(transform.position, m_GroundCheck.position, Time.fixedDeltaTime * 10);
		}
	}


	public void Move(float move, bool crouchKey, bool jumpKey)
	{
		//if player is hanging
		if(isHanging) 
		{
			if (jumpKey)
			{
				climbOnWire = true;
			}
			else if (crouchKey)
			{
				dropFromWire = true;
			}	
			else 
			{
				m_Rigidbody2D.velocity = new Vector2(0f, 0f);
			}
			return;
		}
		//if player is on ladder
		else if(isOnLadder) 
		{
			if (jumpKey)
			{
				//m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 2f);
				m_Rigidbody2D.velocity = new Vector2(0,0);
				m_Rigidbody2D.MovePosition(Vector3.Lerp(transform.position, m_CeilingCheck.position, Time.fixedDeltaTime * 10));
				OnLadderEvent.Invoke(true);
			}
			else if (crouchKey){
				//m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, -2f);
				m_Rigidbody2D.velocity = new Vector2(0,0);
				m_Rigidbody2D.MovePosition(Vector3.Lerp(transform.position, m_GroundCheck.position, Time.fixedDeltaTime * 10));
				OnLadderEvent.Invoke(true);
			}	
			else {
				m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0f);
				OnLadderEvent.Invoke(false);
			}
		} 
		//only control the player if grounded or airControl is turned on, and not hanging
		else if (m_Grounded || m_AirControl)
		{
			// If crouching, check to see if the character can stand up
			if (!crouchKey)
			{
				// If the character has a ceiling preventing them from standing up, keep them crouching
				if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
				{
					crouchKey = true;
				}
			}
			// If crouching
			if (crouchKey)
			{
				if (!m_wasCrouching)
				{
					m_wasCrouching = true;
					OnCrouchEvent.Invoke(true);
				}
				// Reduce the speed by the crouchSpeed multiplier
				move *= m_CrouchSpeed;

				// Disable one of the colliders when crouching
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = false;
			} else
			{
				// Enable the collider when not crouching
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = true;

				if (m_wasCrouching)
				{
					m_wasCrouching = false;
					OnCrouchEvent.Invoke(false);
				}
			}
			// If the player should jump...
			if (m_Grounded && jumpKey && m_Rigidbody2D.velocity.y < m_JumpForce * 0.55f)
			{
				// Add a vertical force to the player.
				//Debug.Log(m_Rigidbody2D.velocity.y);
				m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0);//reset the Y velocity before jumping
				m_Grounded = false;
				m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce),ForceMode2D.Impulse);
				OnJumpEvent.Invoke();
			}			
	
		}
		//only move if we are allowed to
		if(m_Rigidbody2D.isKinematic == false)
		{
			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
		}
	}

	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		/*Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;*/
		transform.Rotate(0f,180f,0f);
	}
}