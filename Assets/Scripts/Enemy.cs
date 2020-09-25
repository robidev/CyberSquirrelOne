using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
	public int health = 100;

	public GameObject deathEffect;

	public UnityEvent deathEvent;

	public void TakeDamage (int damage)
	{
		health -= damage;

		if (health <= 0)
		{
			Die();
		}
	}

	void Die ()
	{
		Instantiate(deathEffect, transform.position, Quaternion.identity);
		Destroy(gameObject);
		if(deathEvent != null)
			deathEvent.Invoke();
	}
}
