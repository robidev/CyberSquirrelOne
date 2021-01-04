using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Outro : MonoBehaviour
{
    public GameObject Title;
    public GameObject Credits;
    public float Rate;
    private bool move = false;
    public float MoveDelay = 2f;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("StartMove",MoveDelay);
    }

    void StartMove()
    {
        move = true;
    }

    // Update is called once per frame
    void Update()
    {
        if( Input.GetButtonUp("Submit") || Input.GetButtonUp ("Cancel") )
        {
            SceneManager.LoadScene(0);
        }
    }

    void FixedUpdate()
    {
        if(move)
        {
            Vector3 pos1 = Title.transform.position;
            pos1.y += Rate;
            Title.transform.position = pos1;
            Vector3 pos2 = Credits.transform.position;
            pos2.y += Rate;
            Credits.transform.position = pos2;
        }
    }
}
