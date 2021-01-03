using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseDialog : MonoBehaviour
{
    public GameObject m_GamePauseUI;
    private float oldTimeScale;
    public AudioSource audioSource;
    private GameObject gameOverMenu;
    void Start()
    {       
        audioSource = GetComponent<AudioSource>();
        gameOverMenu = GetComponent<GameControl>().m_GameOverUI;
    }
    // Update is called once per frame
    void Update()
    {
      if(Input.GetButtonDown ("Cancel") && (gameOverMenu == null || gameOverMenu.activeSelf == false)) {
        if(m_GamePauseUI.activeSelf == false)
        {
          GamePause();
          audioSource.Pause();
        }
        else
        {
          Continue();
          audioSource.UnPause();
        }
      }  
    }

    public void GamePause()
    {
      //showpausescreen
      oldTimeScale = Time.timeScale;
      Time.timeScale = 0;
      m_GamePauseUI.SetActive(true);
    }

    public void Continue()
    {
      //Debug.Log(oldTimeScale);
      Time.timeScale = oldTimeScale;
      m_GamePauseUI.SetActive(false);
    }
}
