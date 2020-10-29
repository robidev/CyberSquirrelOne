using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flickering : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine (waiter());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator waiter()
     {
         float wait_time = Random.Range (0f, 2f);
         yield return new WaitForSeconds (wait_time);
         GetComponent<Renderer>().enabled = !GetComponent<Renderer>().enabled;
         StartCoroutine (waiter());
     }
}
