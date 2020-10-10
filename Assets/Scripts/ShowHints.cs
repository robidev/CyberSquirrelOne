using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHints : MonoBehaviour
{
    int size = 0;
    int currentHint = 0;
    bool nextpressed = false;
    // Start is called before the first frame update
    void Start()
    {
        size = gameObject.transform.childCount;
        /*for (int i = 0; i < size; i++)
        {
            Debug.Log(gameObject.transform.GetChild(i).name);
        }*/
    }

    // enable the next hint    
    public void NextHint()
    {
        StartCoroutine(nextHint());
    }

    IEnumerator nextHint()
    {
        if(nextpressed == true)
        {
            yield break;
        }
        nextpressed = true;
        while(currentHint < size)
        {
            int _currentHint = currentHint++;
            if(gameObject.transform.GetChild(_currentHint).gameObject.activeSelf == false)
            {
                gameObject.transform.GetChild(_currentHint).gameObject.SetActive(true);
                yield return new WaitForSecondsRealtime(2f);
                nextpressed = false;
                yield break;
            }
        }
        nextpressed = false;
    }
}
