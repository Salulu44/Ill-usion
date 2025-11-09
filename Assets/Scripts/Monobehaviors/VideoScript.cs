using System;
using UnityEngine;
using UnityEngine.Video;

public class VideoScript : MonoBehaviour
{
   [field: SerializeField] public VideoClip currentClip { get; private set; }
   [field: SerializeField] public VideoScript nextVideoScript { get; private set; }
    public Action OnVideoStart;
    public Action OnVideoStop;
}
