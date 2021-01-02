﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
    public GameObject m_GameOverUI;
    public AudioSource audioSource;
    private float oldTimeScale;
    public StateSerializer serializer;

    void Start()
    {       
        audioSource = GetComponent<AudioSource>();

        PlayerPrefs.SetString("LastLevel",SceneManager.GetActiveScene().name);
        if(SceneManager.GetActiveScene().name == "Level1_A")
        {
          string filename = PlayerPrefs.GetString("LastCheckPoint");
          serializer.LoadFromFile(filename);
        }
        else
        {
          //string filename = PlayerPrefs.GetString("LastCheckPoint");
          //serializer.LoadFromFile(filename);
        }
    }
    public void GameOver()
    {
      Time.timeScale = 0;
      m_GameOverUI.SetActive(true);
      audioSource.Stop();
    }

    public void Quit()
    {
      SceneManager.LoadScene( 0 );//back to main
    }

    public void Restart()
    {
      Time.timeScale = 1;
      PlayerPrefs.SetString("LastCheckPoint","");
      SceneManager.LoadScene( SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    public void LastCheckPoint()
    {
      Time.timeScale = 1;
      SceneManager.LoadScene( SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }
}
