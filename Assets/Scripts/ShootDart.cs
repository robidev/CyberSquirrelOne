using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootDart : MonoBehaviour
{
    public Transform firePoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown (KeyCode.T) )
		{
            StartCoroutine(ShootRay());
		}
    }

	public LineRenderer lineRenderer;
	public LayerMask hitMask;
    IEnumerator ShootRay ()
	{
		//RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, firePoint.right, 100f, hitMask.value);
		RaycastHit2D hitInfo = Physics2D.CircleCast(firePoint.position, 10f, Vector2.zero, 0f,hitMask.value);
		
		if (hitInfo)
		{
			Debug.Log(hitInfo.transform.name);
			Enemy enemy = hitInfo.transform.GetComponent<Enemy>();
			if (enemy != null)
			{
				//enemy.TakeDamage(damage);
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
