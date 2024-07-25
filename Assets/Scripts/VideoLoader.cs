using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoLoader : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public RawImage videoImage;
    public string videoUrl = "https://videos.pexels.com/video-files/6924608/6924608-hd_1080_1920_24fps.mp4";

    // Start is called before the first frame update
    void Start()
    {
        PlayVideo();
    }

    void PlayVideo()
    {
        videoPlayer.url = videoUrl;
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        videoPlayer.EnableAudioTrack(0, true);
        videoPlayer.SetDirectAudioVolume(0, 0.3f);
        videoPlayer.Prepare();
    }

    public void SetNewVideoURL(string url)
    {
        videoUrl = url;
        PlayVideo();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            videoImage.enabled = !videoImage.enabled;
        }
    }
}
