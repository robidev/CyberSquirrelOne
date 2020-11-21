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
        StartCoroutine(BlinkText());
    }

    // Update is called once per frame
    void Update()
    {
        ButtonText.color = Color.Lerp(Color.white, Color.grey, Mathf.PingPong(Time.time, 1));
        if(Input.GetKey("space"))
        {
            NextLevel();
        }
    }

    public IEnumerator BlinkText(){
        //blink it forever. You can set a terminating condition depending upon your requirement
        while(true){
            //set the Text's text to blank
            //flashingText.text= "";
            //display blank text for 0.5 seconds
            yield return new WaitForSeconds(.5f);
            //display “I AM FLASHING TEXT” for the next 0.5 seconds
            //flashingText.text= "I AM FLASHING TEXT!";
            yield return new WaitForSeconds(.5f);
        }
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}

