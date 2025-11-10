using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class VideoManagerScript : MonoBehaviour
{
    public static VideoManagerScript Instance { get; private set; }
    public VideoClip whiteNoiseClip;
    VideoPlayer videoPlayer;
    private GameObject visuals;
    private VideoScript nextVideoScript;
    private VideoScript currentVideoScript;
    private void Awake()
    {
        if(Instance != null && Instance != this) 
        {
            Destroy(gameObject);
        }
        else Instance = this;
    }
    private void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += VideoEnd;
        if (transform.childCount == 1)
        {
            visuals = transform.GetChild(0).gameObject;
            visuals.SetActive(false);
        }
        else Debug.Log("You didnt put a child in the Videoplayer, PEINLI!!");
        videoPlayer.sendFrameReadyEvents = true;
        videoPlayer.frameReady += OnFrameReady;
    }
    private void OnFrameReady(VideoPlayer source, long frameIdx)
    {
        Debug.Log("FRAMMMMMME");
        Delegate[] subscribers = currentVideoScript.OnVideoStart.GetInvocationList();
        foreach (var subscriber in subscribers)
        {
            Debug.Log($"Subscriber Methode: {subscriber.Method.Name}, Ziel: {subscriber.Target}");
        }
        currentVideoScript.OnVideoStart?.Invoke();
        videoPlayer.sendFrameReadyEvents = false;
        // Event abmelden, damit es nicht erneut feuert
        source.frameReady -= OnFrameReady;
    }

    public void PlayVideo(VideoScript videoScript) 
    {
        currentVideoScript = videoScript;
        visuals.gameObject.SetActive(true);
        videoPlayer.clip = videoScript.currentClip;
        nextVideoScript = videoScript.nextVideoScript;
        videoPlayer.Prepare();
        StartCoroutine(StartVideoWhenPrepared());
    }
    public void PlayVideo(VideoClip clip)
    {
        visuals.gameObject.SetActive(true);
        videoPlayer.clip = clip;
        videoPlayer.Prepare();
        StartCoroutine(StartVideoWhenPrepared());
    }
    void OnEnable()
    { 
    }
    
    public void StopVideo()
    {
        if (videoPlayer != null)
            videoPlayer.Stop();
    }
    public void VideoEnd(VideoPlayer videoPlayer) 
    {
        if(nextVideoScript != null) 
        {
            PlayVideo(nextVideoScript);
            return;
        }
        Debug.Log("Its over");
        videoPlayer.clip = null;
        visuals.gameObject.SetActive(false);
    }
    IEnumerator StartVideoWhenPrepared()
    {

        while (!videoPlayer.isPrepared)
        {
            yield return null; // Warten bis Vorbereitung abgeschlossen
        }
        videoPlayer.Play();

    }

    public bool IsVideoDone()
    {
        return !videoPlayer.isPlaying && videoPlayer.clip != null;
    }
}
