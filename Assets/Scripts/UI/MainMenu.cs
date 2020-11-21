using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartNewGame()
    {
        PlayerPrefs.SetString("LastCheckPoint","");
        PlayerPrefs.SetString("LastLevel","");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ContinueGame()
    {
        string level = PlayerPrefs.GetString("LastLevel");
        if(level != "")
            SceneManager.LoadScene(level);
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
