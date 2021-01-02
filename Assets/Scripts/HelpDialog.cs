using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpDialog : MonoBehaviour
{
    public GameObject dialog;
    private float oldTimeScale;
    // Update is called once per frame
    void Update()
    {
        if(dialog.activeSelf == false && Time.timeScale > 0.1 && Input.GetButtonDown("Help"))
        {
            oldTimeScale = Time.timeScale;
            Time.timeScale = 0;
            dialog.SetActive(true);
        }
        else if(dialog.activeSelf == true && (Input.GetButtonDown("Cancel") || Input.GetButtonDown("Help")))
        {
            dialog.SetActive(false);
            Time.timeScale = oldTimeScale;
        }
    }
}
