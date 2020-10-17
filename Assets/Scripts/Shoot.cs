using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Shoot : MonoBehaviour
{
    public Transform firePoint;
	public GameObject bulletPrefab;
    // Start is called before the first frame update
	private PlayerMovement player;

    private float throwspeed = 0;
    public float MaxThrowSpeed = 50f;
	
    public bool canShoot = true;
    public CinemachineVirtualCamera m_Camera;

    public CinemachineVirtualCamera m_CameraZoomOut;
    float screenX;
    // Start is called before the first frame update
    public AudioSource audioSource;
    public AudioClip throwAudio;
    void Start()
    {
        player = gameObject.GetComponent<PlayerMovement>();
        screenX = m_CameraZoomOut.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale > 0.1f)
        {
            if (Input.GetButtonDown ("Action2") && player.selected == true && canShoot == true)
            {
                throwspeed = 0;
                if(player.characterControl.FacingRight == true)
                {
                    m_CameraZoomOut.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX = screenX;
                }
                else
                {
                    m_CameraZoomOut.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX = 1 - screenX;
                }
                
                m_CameraZoomOut.MoveToTopOfPrioritySubqueue();
            }
            if (Input.GetButton ("Action2") && player.selected == true && canShoot == true)
            {
                if(throwspeed < MaxThrowSpeed)
                {
                    throwspeed += Time.deltaTime * (MaxThrowSpeed / 1f);
                }
            }
            if (Input.GetButtonUp ("Action2") && player.selected == true && canShoot == true)
            {
                Debug.Log(throwspeed);
                GameObject obj = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                Bullet bullet = obj.GetComponent<Bullet>();
                bullet.speed = throwspeed;
                Invoke("ResetCam",0.5f);
                if (audioSource && throwAudio)
                	audioSource.PlayOneShot(throwAudio);
            }
        }
    }

    void ResetCam()
    {
        m_Camera.MoveToTopOfPrioritySubqueue();
    }
}
