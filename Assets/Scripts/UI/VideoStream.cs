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
    void Awake()
    {
        videoPlayer.loopPointReached += EndReached;
        videoPlayer.started += StartReached;        
        #if UNITY_WEBGL && !UNITY_EDITOR
            Debug.Log("WEBGL movie handling (URL: '" + Application.streamingAssetsPath + "/" + playOnStartUrl + "')");
            videoPlayer.source = VideoSource.Url;
        #else
            Debug.Log("normal movie handling (Clip)");
            videoPlayer.source = VideoSource.VideoClip;
        #endif
        videoPlayer.url = Application.streamingAssetsPath + "/" + playOnStartUrl;
        videoPlayer.clip = playOnStart;
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
            videoPlayer.url = Application.streamingAssetsPath + "/" + playOnEndUrl;
            DestroyOverlay();
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
        videoPlayer.Stop();
        //StartedCheck = true;
        EndReachedCheck = true;
        videoPlayer.clip = playOnEvent;
        videoPlayer.url = Application.streamingAssetsPath + "/" + playOnEventUrl;
        videoPlayer.isLooping = false;
        videoPlayer.Play();
        DoOverlay();
        Debug.Log("Video Triggered");
    }

    public GameObject ModalOverLayPrefab;
    GameObject overlay;
    void DoOverlay()
    {
        overlay = Instantiate(ModalOverLayPrefab,transform.parent.parent);
        overlay.GetComponent<RectTransform>().anchorMin = Vector2.zero;
        overlay.GetComponent<RectTransform>().anchorMax = Vector2.one;
        overlay.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
        overlay.GetComponent<RectTransform>().SetAsLastSibling();//set top background
    }//Destroy(overlay);

    void DestroyOverlay()
    {
        Destroy(overlay);
    }
}