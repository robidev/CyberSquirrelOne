using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using TMPro;

public class Intro : MonoBehaviour
{
    public TextMeshProUGUI ButtonText;
    public UnityEvent OnStart;
    // Start is called before the first frame update
    void Start()
    {
        OnStart.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        ButtonText.color = Color.Lerp(Color.white, Color.grey, Mathf.PingPong(Time.unscaledTime, 1));
        if(Input.GetKeyUp("space"))
        {
            NextLevel();
        }
    }

    public void NextLevel()
    {
        System.GC.Collect();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}

