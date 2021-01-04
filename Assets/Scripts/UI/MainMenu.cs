using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public GameObject areyousure;
    public TMP_InputField code;
    public void StartNewGame()
    {
        PlayerPrefs.SetString("LastCheckPoint","");
        PlayerPrefs.SetString("LastLevel","");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void TryStartNewGame()
    {
        if( PlayerPrefs.GetString("LastCheckPoint") == "" || PlayerPrefs.GetString("LastLevel") == "")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            areyousure.SetActive(true);
        }
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

    public void CheckPassCode()
    {
        if(code.text.Equals("AARDVARK"))
        {
            SceneManager.LoadScene(5);
        }
        else if(code.text.Equals("SPSY?!?"))
        {
            SceneManager.LoadScene(7);
        }
    }
}
