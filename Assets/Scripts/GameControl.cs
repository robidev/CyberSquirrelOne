using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
    public GameObject m_GameOverUI;
    public AudioSource audioSource;
    private float oldTimeScale;
    public StateSerializer serializer;
    private GameObject gamePauseMenu;

    void Start()
    {
      Time.timeScale = 1;       
      audioSource = GetComponent<AudioSource>();
      gamePauseMenu = GetComponent<PauseDialog>().m_GamePauseUI;

      //save current level as the last level loaded
      PlayerPrefs.SetString("LastLevel",SceneManager.GetActiveScene().name);
      //load game settings if available
      string filename = PlayerPrefs.GetString("LastCheckPoint");
      if(filename.StartsWith(SceneManager.GetActiveScene().name))
      {
        Debug.Log("Loading:" + filename);
        serializer.LoadFromFile(filename);
      }
    }
    public void GameOver()
    {
      Time.timeScale = 0;
      m_GameOverUI.SetActive(true);
      if(gamePauseMenu != null)
      {
        gamePauseMenu.SetActive(false);
      }
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

    public void NextLevel()
    {
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
