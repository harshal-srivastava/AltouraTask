using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System.IO;

public class VideoPlayerController : MonoBehaviour
{
    public VideoPlayer player;

    public int fastForwardTime = 5;

    public Slider videoSlider;

    private float currVideoLength;

    public delegate void VideoPlayerReady();
    public static VideoPlayerReady VideoPlayerReadyEvent;

    public delegate void VideoPlayPaused(bool status);
    public static VideoPlayPaused VideoPlayPausedEvent;

    public delegate void UpdateVideoPlaybackTime(string endingtime, bool doOnce);
    public static UpdateVideoPlaybackTime UpdateVideoTimeEvent;

    public delegate void VideoStop();
    public static VideoStop VideoStopEvent;

    public delegate void VideoEnded();
    public static VideoEnded VideoEndedEvent;

    private bool isPaused = false;

    private VideoClip currVideo;

    public void PlayVideo(VideoClip video)
    {
        if (player.clip != null)
        {
            player.clip = null;
        }
        player.clip = video;
        player.Prepare();
        player.prepareCompleted += PlayVideoOnPlayer;
        player.loopPointReached += DisableVideoPlayer;
        currVideo = video;
    }

    void DisableVideoPlayer(VideoPlayer player)
    {
        VideoEndedEvent?.Invoke();
    }

    public void PlayCurrentVideoAgain()
    {
        PlayVideo(currVideo);
    }

    void PlayVideoOnPlayer(VideoPlayer source)
    {
        VideoPlayerReadyEvent?.Invoke();
        if (source != null)
        {
            source.Play();
        }
        SetScreenVariables(source);
    }

    void SetScreenVariables(VideoPlayer source)
    {
        currVideoLength = source.frameCount / source.frameRate;
        videoSlider.minValue = 0;
        videoSlider.maxValue = currVideoLength;
        videoSlider.onValueChanged.AddListener(ChangeMovieRuntime);
        UpdateVideoTimeEvent?.Invoke(VideoUtility.GetTimeStampFromTotalTime(currVideoLength), true);
    }


    private void Update()
    {
        UpdateVideoSlider();
    }

    void UpdateVideoSlider()
    {
        videoSlider.SetValueWithoutNotify((float)player.time);
        UpdateVideoTimeEvent?.Invoke(VideoUtility.GetTimeStampFromTotalTime((float)player.time), false);
    }

    public void ChangeMovieRuntime(float value)
    {
        player.time = value;
        UpdateVideoTimeEvent?.Invoke(VideoUtility.GetTimeStampFromTotalTime((float)player.time), false);
    }

    public void ToggleVideoPlayPause()
    {
        isPaused = !isPaused;
        UpdateVideoPlayBack();
        VideoPlayPausedEvent?.Invoke(isPaused);
    }

    private void UpdateVideoPlayBack()
    {
        if (isPaused)
        {
            PauseVideo();
        }
        else
        {
            PlayVideo();
        }
    }

    private void PauseVideo()
    {
        if (player.isPlaying)
        {
            player.Pause();
        }
    }

    private void PlayVideo()
    {
        if (player.isPaused)
        {
            player.Play();
        }
    }

    public void FastForward()
    {
        float playerTime = (float)player.time;
        playerTime += fastForwardTime;
        playerTime = Mathf.Clamp(playerTime, 0, currVideoLength);
        player.time = playerTime;
    }

    public void Rewind()
    {
        float playerTime = (float)player.time;
        playerTime -= fastForwardTime;
        playerTime = Mathf.Clamp(playerTime, 0, currVideoLength);
        player.time = playerTime;
    }

    bool IsVideoPlayerRunning()
    {
        return player.isPlaying;
    }
    
    public void StopVideo()
    {
        Debug.Log("stop video called");
         player.Stop();
         VideoStopEvent?.Invoke();
         isPaused = false;
    }
}
