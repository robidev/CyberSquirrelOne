using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

public class VideoStream : MonoBehaviour
{
    public bool StartedCheck = false;
    public float DelayTime = 1f;
    public bool EndReachedCheck = false;
    public UnityEvent StartTrigger;
    public UnityEvent DelayedTrigger;
    public UnityEvent EndTrigger;


    public VideoPlayer videoPlayer;
    public VideoClip playOnStart;
    public string playOnStartUrl;
    public VideoClip playOnEvent;
    public string playOnEventUrl;
    public VideoClip playOnEnd;
    public string playOnEndUrl;
    void Start()
    {
        videoPlayer.loopPointReached += EndReached;
        videoPlayer.started += StartReached;        
        #if UNITY_WEBGL && !UNITY_EDITOR
            Debug.Log("WEBGL movie handling (URL: '" + Application.streamingAssetsPath + "/" + playOnStartUrl + "')");
            videoPlayer.source = VideoSource.Url;
            videoPlayer.url = Application.streamingAssetsPath + "/" + playOnStartUrl;
        #else
            Debug.Log("normal movie handling (Clip)");
            videoPlayer.source = VideoSource.VideoClip;
            videoPlayer.clip = playOnStart;
        #endif
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
            videoPlayer.clip = playOnEnd;
            #if UNITY_WEBGL && !UNITY_EDITOR
                videoPlayer.url = Application.streamingAssetsPath + "/" + playOnEndUrl;
            #else
                videoPlayer.clip = playOnEnd;
            #endif
            
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

    public void TriggerEventVideo()
    {
        StartedCheck = true;
        EndReachedCheck = true;
        #if UNITY_WEBGL && !UNITY_EDITOR
            videoPlayer.url = Application.streamingAssetsPath + "/" + playOnEventUrl;
        #else
            videoPlayer.clip = playOnEvent;
        #endif
        videoPlayer.isLooping = false;
    }
}