using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public Transform firePoint;
	public GameObject bulletPrefab;
    // Start is called before the first frame update
	private PlayerMovement player;
	
    // Start is called before the first frame update
    void Start()
    {
        player = gameObject.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown (KeyCode.T) && player.selected == true )
		{
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
		}
    }
}
