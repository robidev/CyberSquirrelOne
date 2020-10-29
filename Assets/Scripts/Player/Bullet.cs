using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 50f;
	public int damage = 40;
	public Rigidbody2D rb;
	public GameObject impactEffect;
    public AudioSource audioSource;
    public AudioClip collisionAudio;
	// Use this for initialization
	void Start () {
		rb.velocity = transform.right * speed;
		audioSource = GetComponent<AudioSource>();
	}

	void Update()
	{
		if(rb.IsSleeping()==true)
		{
			Destroy(gameObject);
		}
	}

	void OnTriggerEnter2D (Collider2D hitInfo)
	{
		Enemy enemy = hitInfo.GetComponent<Enemy>();
		if (enemy != null)
		{
			enemy.TakeDamage(damage);
		}

		//Instantiate(impactEffect, transform.position, transform.rotation);
		//Debug.Log(hitInfo.transform.name);
		//Destroy(gameObject);
	}

	void OnCollisionEnter2D(Collision2D hitInfo)
	{
		if(hitInfo.gameObject.tag == "Breakable")
		{
			Debug.Log(hitInfo.transform.name + " is broken");
			hitInfo.gameObject.GetComponent<BreakableEffect>().Break();
		}
		else
		{
			if (audioSource && collisionAudio && audioSource.isPlaying == false)
            	audioSource.PlayOneShot(collisionAudio);
		}
	}
}
