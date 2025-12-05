using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class VideoManagerScript : MonoBehaviour
{
    public static VideoManagerScript Instance { get; private set; }
    public VideoClip whiteNoiseClip;
    public VideoClip jumpscareClip;
    VideoPlayer videoPlayer;
    private GameObject visuals;
    private VideoScript nextVideoScript;
    private VideoScript currentVideoScript;
    [SerializeField]float jumpScareTimerActivate;
    [SerializeField]float jumpScareTimerDeactivate;
    float jumpScaretimer;
    bool jumpScareRunning;
    public bool startJumpScare;
    private void Awake()
    {
        if(Instance != null && Instance != this) 
        {
            Destroy(gameObject);
        }
        else Instance = this;
    }
    private void Update()
    {
        if (startJumpScare) 
        {
            JumpScare();
        }

    }
    public void StartJumpScare() 
    {
        //Thought i could implement a Method where i can control when to start the JumpScare actually
        //So that I can jumpscare immediately or with an offset
    }
    void JumpScare()
    {

        if (!jumpScareRunning)
        {
            jumpScaretimer -= Time.deltaTime;
            if (jumpScaretimer <= 0)
            {
                PlayVideo(jumpscareClip);
                jumpScareRunning = true;
                jumpScaretimer = UnityEngine.Random.Range(0,jumpScareTimerDeactivate);

            }
            return;
        }
        jumpScaretimer -= Time.deltaTime;
        if(jumpScaretimer <= 0) 
        {
            StopVideo();
            jumpScareRunning = false;
            jumpScaretimer = UnityEngine.Random.Range(0, jumpScareTimerActivate);
            startJumpScare = false;
        }

    }
    private void Start()
    {
        jumpScaretimer = UnityEngine.Random.Range(0, jumpScareTimerActivate);
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
        //Delegate[] subscribers = currentVideoScript.OnVideoStart.GetInvocationList();
        //foreach (var subscriber in subscribers)
        //{
        //    Debug.Log($"Subscriber Methode: {subscriber.Method.Name}, Ziel: {subscriber.Target}");
        //}
        if(currentVideoScript != null) 
        currentVideoScript.OnVideoStart?.Invoke();
        videoPlayer.sendFrameReadyEvents = false;
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
        {
            videoPlayer.Stop();
            visuals.SetActive(false);
        }


    }
    public void VideoEnd(VideoPlayer videoPlayer) 
    {
        if(nextVideoScript != null) 
        {
            PlayVideo(nextVideoScript);
            return;
        }
        Debug.Log("Its over");
        currentVideoScript.OnVideoStop?.Invoke();
        videoPlayer.clip = null;
        visuals.SetActive(false);
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
