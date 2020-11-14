using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VideoStream : MonoBehaviour
{
    public bool StartedCheck = false;
    public float DelayTime = 1f;
    public bool EndReachedCheck = false;
    public UnityEvent StartTrigger;
    public UnityEvent DelayedTrigger;
    public UnityEvent EndTrigger;


    public UnityEngine.Video.VideoPlayer videoPlayer;
    void Start()
    {
        videoPlayer.loopPointReached += EndReached;
        videoPlayer.started += StartReached;
    }
    void StartReached(UnityEngine.Video.VideoPlayer vp)
    {
        if(StartedCheck == true)
        {
            Debug.Log("started");
            StartTrigger.Invoke();
            StartCoroutine("startDelay");
        }
    }

    IEnumerator startDelay()
    {
        yield return new WaitForSeconds(DelayTime);
        Debug.Log("delayTrigger");
        DelayedTrigger.Invoke();
    }
    void EndReached(UnityEngine.Video.VideoPlayer vp)
    {
        if(EndReachedCheck == true)
        {
             Debug.Log("endreached");
            EndTrigger.Invoke();
        }
    }

    public void SetEndReachedCheck(bool value)
    {
        EndReachedCheck = value;
    }

    public void SetStartedCheck(bool value)
    {
        StartedCheck = value;
    }
}