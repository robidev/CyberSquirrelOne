using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public CharacterController2D controller;
	public Animator animator;
	public float runSpeed = 40f;
	public bool selected = true;

	public bool controllable = true;
	public LayerMask _Door;

	float horizontalMove = 0f;
	bool jumpKey = false;
	bool crouchKey = false;

	RaycastHit2D hitInfo;
	private CharacterController2D characterControl;
	private LayerMask m_WhatIsGround;	// A mask determining what is ground to the character

	public enum DieReason {
		Fallen,
		Electrocuted,
		Caught,
		Hit,
	}

	void Start () {
		characterControl = gameObject.GetComponent<CharacterController2D>();
		m_WhatIsGround = characterControl.m_WhatIsGround;
	}
	
	public void Die(DieReason reason) {
		Debug.Log("I died because of reason: " + reason.ToString());
		controller.GetComponent<Rigidbody2D>().simulated = false;

		animator.SetBool("IsHanging", false);
		animator.SetBool("IsJumping", false);
		animator.SetBool("IsCrouching", false);
		animator.SetBool("IsClimbing", false);
		if(reason == DieReason.Electrocuted)
		{
			animator.SetBool("IsElectrocuted", true);
			FindObjectOfType<GlobalControl>().Invoke("GameOver", 2);//allow for the death-animation
		}
		else
			FindObjectOfType<GlobalControl>().Invoke("GameOver", 0);
	}



	// Update is called once per frame
	void Update () {
		if(selected && controllable) {
			horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
			animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

			//up key
			if (Input.GetButtonDown("Jump"))
			{
				jumpKey = true;
			} else if (Input.GetButtonUp("Jump"))
			{
				jumpKey = false;
			}
			//down key
			if (Input.GetButtonDown("Crouch"))
			{
				crouchKey = true;
			} else if (Input.GetButtonUp("Crouch"))
			{
				crouchKey = false;
			}
			//enter key
			if (Input.GetButtonDown("Submit")) {
				hitInfo = Physics2D.Raycast(transform.position, Vector2.up, 0, _Door);
				if(hitInfo.collider != null && hitInfo.collider.gameObject.GetComponent<DoorLock>().isOpen == true) //we are in front of an open door
				{ 
					if(gameObject.layer == LayerMask.NameToLayer("Player_outside")){ // player_outside
						gameObject.layer = LayerMask.NameToLayer("Player_inside");//11; //switch to inside
						characterControl.m_WhatIsGround &=  ~(1 << LayerMask.NameToLayer("outside"));//~(1 << 15);
						characterControl.m_WhatIsGround |= 1 <<LayerMask.NameToLayer("inside");//1 << 16;
					} else {  // player_inside
						gameObject.layer = LayerMask.NameToLayer("Player_outside");//10;
						characterControl.m_WhatIsGround |= 1 << LayerMask.NameToLayer("outside");//1 << 15;
						characterControl.m_WhatIsGround &=  ~(1 << LayerMask.NameToLayer("inside"));//~(1 << 16);
					}
				}
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
		//hanging
		if(isClimbing) {
			animator.SetBool("IsHanging", false);
			animator.SetBool("IsJumping", false);
			animator.SetBool("IsCrouching", false);
			animator.SetBool("IsClimbing", true);
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
	}

	public void OnLanding ()
	{
		animator.SetBool("IsJumping", false);
		animator.SetBool("IsHanging", false);
		animator.SetBool("IsCrouching", false);
		animator.SetBool("IsClimbing", false);
	}

	public void OnCrouching(bool isCrouching) 
	{
		animator.SetBool("IsCrouching", isCrouching);
	}

	void FixedUpdate ()
	{
		// Move our character
		if(selected) {
			controller.Move(horizontalMove * Time.fixedDeltaTime, crouchKey, jumpKey);
		}
	}
}
