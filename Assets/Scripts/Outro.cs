using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Outro : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.H) || Input.GetButtonUp("Submit") || Input.GetButtonUp("Action1") || Input.GetButtonUp("Action2") )
        {
            SceneManager.LoadScene(0);
        }
    }
}
