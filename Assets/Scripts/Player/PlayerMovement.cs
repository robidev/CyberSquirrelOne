using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public Animator animator;
	public float runSpeed = 40f;
	public bool selected {
		get{ 
			return m_Selected; 
		}
		set{ 
			m_Selected = value; 
		}
	}
	private bool m_Selected = true;
	public bool controllable = true;
	public LayerMask _Door;

	float horizontalMove = 0f;
	bool jumpKey = false;
	bool crouchKey = false;
	public GameObject detected;
	RaycastHit2D hitInfo;
	public CharacterController2D characterControl;
	private LayerMask m_WhatIsGround;	// A mask determining what is ground to the character
	public AudioSource audioSource;
	public AudioClip jumpAudio;
	public AudioClip openAudio;
	private AudioClip walkingAudio;
	public AudioClip outsideWalkingAudio;
	public AudioClip insideWalkingAudio;
	private bool _playerInside = false;
	public bool playerInside {
		set {
			if(value == true)
			{
				walkingAudio = insideWalkingAudio;
			}
			else
			{
				walkingAudio = outsideWalkingAudio;
			}
			_playerInside = value;
		}
	}
	public AudioClip dropAudio;
	public AudioClip electrocutedAudio;
	private AudioListener listener = null;
	public enum DieReason {
		Detected,
		Electrocuted,
		Caught,
		Hit,
	}

	void Start () {
		characterControl = gameObject.GetComponent<CharacterController2D>();
		m_WhatIsGround = characterControl.m_WhatIsGround;
		audioSource = GetComponent<AudioSource>();
		listener = transform.GetComponent<AudioListener>();
		playerInside = false;
	}
	
	public void Die(DieReason reason) {
		Debug.Log("I died because of reason: " + reason.ToString());
		characterControl.GetComponent<Rigidbody2D>().simulated = false;

		animator.SetBool("IsHanging", false);
		animator.SetBool("IsJumping", false);
		animator.SetBool("IsCrouching", false);
		animator.SetBool("IsClimbing", false);
		if(reason == DieReason.Electrocuted)
		{
			animator.SetBool("IsElectrocuted", true);
			if (audioSource && electrocutedAudio)
            	audioSource.PlayOneShot(electrocutedAudio);
			FindObjectOfType<GlobalControl>().Invoke("GameOver", 2);//allow for the death-animation
			return;
		}
		if(reason == DieReason.Detected)
		{
			if(detected != null)
				detected.SetActive(true);
			FindObjectOfType<GlobalControl>().Invoke("GameOver", 2);//allow for the death-animation
			return;
		}
		FindObjectOfType<GlobalControl>().Invoke("GameOver", 0);
	}



	// Update is called once per frame
	void Update () {
		if(Time.timeScale > 0.1f)
		{
			if(selected && controllable ) {
				horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
				animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

				//up key
				jumpKey = Input.GetButton("Jump");
				crouchKey = Input.GetButton("Crouch");
				//enter key
				if (Input.GetButtonDown("Open")) {
					hitInfo = Physics2D.Raycast(transform.position, Vector2.up, 0, _Door);
					if(hitInfo.collider != null && hitInfo.collider.gameObject.GetComponent<DoorLock>().isOpen == true) //we are in front of an open door
					{ 
						if(gameObject.layer == LayerMask.NameToLayer("Player_outside")){ // player_outside
							gameObject.layer = LayerMask.NameToLayer("Player_inside");//11; //switch to inside
							characterControl.m_WhatIsGround &=  ~(1 << LayerMask.NameToLayer("outside"));//~(1 << 15);
							characterControl.m_WhatIsGround |= 1 <<LayerMask.NameToLayer("inside");//1 << 16;
							playerInside = true;
						} else {  // player_inside
							gameObject.layer = LayerMask.NameToLayer("Player_outside");//10;
							characterControl.m_WhatIsGround |= 1 << LayerMask.NameToLayer("outside");//1 << 15;
							characterControl.m_WhatIsGround &=  ~(1 << LayerMask.NameToLayer("inside"));//~(1 << 16);
							playerInside = false;
						}
						if (audioSource && openAudio)
							audioSource.PlayOneShot(openAudio);
					}
				}	
				if (audioSource && walkingAudio && audioSource.isPlaying == false 
					&& (characterControl.m_Grounded == true || characterControl.isOnLadder)
					&& Mathf.Abs(horizontalMove) > 0.01f)
					audioSource.PlayOneShot(walkingAudio);			
			}
			else
			{
				animator.SetFloat("Speed", 0f);
				crouchKey = false;
				jumpKey = false;
			}
		}
	}

	public void OnHanging(bool isHanging)
	{
		//hanging
		if(isHanging) {
			animator.SetBool("IsClimbing", false);
			animator.SetBool("IsJumping", false);
			animator.SetBool("IsCrouching", false);
			animator.SetBool("IsHanging", true);
		}
		else {
			animator.SetBool("IsHanging", false);
		}
	}

	public void OnClimbing(bool isClimbing)
	{
		//on stairs
		if(isClimbing) {
			animator.SetBool("IsHanging", false);
			animator.SetBool("IsJumping", false);
			animator.SetBool("IsCrouching", false);
			animator.SetBool("IsClimbing", true);
			if (audioSource && walkingAudio && audioSource.isPlaying == false)
				audioSource.PlayOneShot(walkingAudio);	
		}
		else {
			animator.SetBool("IsClimbing", false);
		}
	}

	public void OnJumping ()
	{
		animator.SetBool("IsHanging", false);
		animator.SetBool("IsCrouching", false);
		animator.SetBool("IsClimbing", false);
		animator.SetBool("IsJumping", true);
		if (audioSource && jumpAudio)
           	audioSource.PlayOneShot(jumpAudio);
	}

	public void OnLanding ()
	{
		animator.SetBool("IsJumping", false);
		animator.SetBool("IsHanging", false);
		animator.SetBool("IsCrouching", false);
		animator.SetBool("IsClimbing", false);
		if (audioSource && dropAudio)
           	audioSource.PlayOneShot(dropAudio);
	}

	public void OnCrouching(bool isCrouching) 
	{
		animator.SetBool("IsCrouching", isCrouching);
	}

	void FixedUpdate ()
	{
		// Move our character
		if(selected && controllable) {
			characterControl.Move(horizontalMove * Time.fixedDeltaTime, crouchKey, jumpKey);
		}
	}
}