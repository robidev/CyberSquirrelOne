using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootDart : MonoBehaviour
{
    public Transform firePoint;
	private PlayerMovement player;
	private CharacterController2D character;
	
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerMovement>();
		character = GetComponent<CharacterController2D>();
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetButtonDown ("Action2") && player.selected == true)
		{
			//Debug.Log("piew");
            StartCoroutine(ShootRay());
		}
    }

	public LineRenderer lineRenderer;
	public LayerMask hitMaskOutside;
	public LayerMask hitMaskInside;
    IEnumerator ShootRay ()
	{
		//RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, firePoint.right, 100f, hitMask.value);
		RaycastHit2D hitInfo;
		if(gameObject.layer == LayerMask.NameToLayer("Player_outside"))
			hitInfo = Physics2D.CircleCast(firePoint.position, 10f, Vector2.zero, 0f,hitMaskOutside.value);
		else
			hitInfo = Physics2D.CircleCast(firePoint.position, 10f, Vector2.zero, 0f,hitMaskInside.value);

		if (hitInfo && (hitInfo.transform.position.x > firePoint.position.x) == character.FacingRight) //in range and facing the player
		{
			Debug.Log(hitInfo.transform.name);
			Enemy enemy = hitInfo.transform.GetComponent<Enemy>();
			if (enemy != null)
			{
				enemy.TakeDamage(50);
			}

			//Instantiate(impactEffect, hitInfo.point, Quaternion.identity);
			//Debug.Log("hit " + hitInfo.transform.name);

			lineRenderer.SetPosition(0, firePoint.position);
			lineRenderer.SetPosition(1, hitInfo.point);
		} else
		{
			lineRenderer.SetPosition(0, firePoint.position);
			lineRenderer.SetPosition(1, firePoint.position + firePoint.right * 100);
		}

		lineRenderer.enabled = true;

		yield return 0;

		lineRenderer.enabled = false;
	}
}
