using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System.IO;

/// <summary>
/// Class responsible for the video player functionalities
/// Which includes playing video, pause, rewind, fastforward, seek, etc
/// </summary>
public class VideoPlayerController : MonoBehaviour
{
    public VideoPlayer player;

    public int fastForwardTime = 5;

    public Slider videoSlider;

    private float currVideoLength;

    /// <summary>
    /// Delegate coupled with static event to be called when videoplayer is ready
    /// </summary>
    public delegate void VideoPlayerReady();
    public static VideoPlayerReady VideoPlayerReadyEvent;

    /// <summary>
    /// Delegate coupled with static event to be called when video is paused
    /// Added to notify the related UI class to update when user interacts with video player
    /// </summary>
    public delegate void VideoPlayPaused(bool status);
    public static VideoPlayPaused VideoPlayPausedEvent;

    /// <summary>
    /// Delegate coupled with static event to be update video playback time
    /// This will give the dynamic playtime for any video which is to be played
    /// </summary>
    public delegate void UpdateVideoPlaybackTime(string endingtime, bool doOnce);
    public static UpdateVideoPlaybackTime UpdateVideoTimeEvent;

    /// <summary>
    /// Delegate coupled with static event to be called when video is stopped
    /// </summary>
    public delegate void VideoStop();
    public static VideoStop VideoStopEvent;

    /// <summary>
    /// Delegate coupled with static event to be called when video is finished
    /// </summary>
    public delegate void VideoEnded();
    public static VideoEnded VideoEndedEvent;

    private bool isPaused = false;

    private VideoClip currVideo;

    /// <summary>
    /// Function to assign the clip to the video player component
    /// Prepare and then play the video
    /// </summary>
    /// <param name="video"></param>
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

    /// <summary>
    /// Function to disable the video player
    /// </summary>
    /// <param name="player"></param>
    void DisableVideoPlayer(VideoPlayer player)
    {
        VideoEndedEvent?.Invoke();
    }

    /// <summary>
    /// Function to play the current video again
    /// </summary>
    public void PlayCurrentVideoAgain()
    {
        PlayVideo(currVideo);
    }

    /// <summary>
    /// Listener to VideoPlayer.prepareCompleted event
    /// </summary>
    /// <param name="source"></param>
    private void PlayVideoOnPlayer(VideoPlayer source)
    {
        VideoPlayerReadyEvent?.Invoke();
        if (source != null)
        {
            source.Play();
        }
        SetScreenVariables(source);
    }

    /// <summary>
    /// Function to set the screen visual elements whenever a new video is to be played
    /// </summary>
    /// <param name="source"></param>
    private void SetScreenVariables(VideoPlayer source)
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

    /// <summary>
    /// Update the video player slider value according to the video playback
    /// </summary>
    private void UpdateVideoSlider()
    {
        videoSlider.SetValueWithoutNotify((float)player.time);
        UpdateVideoTimeEvent?.Invoke(VideoUtility.GetTimeStampFromTotalTime((float)player.time), false);
    }

    /// <summary>
    /// Function called when user tries to seek through the video
    /// </summary>
    /// <param name="value"></param>
    public void ChangeMovieRuntime(float value)
    {
        player.time = value;
        UpdateVideoTimeEvent?.Invoke(VideoUtility.GetTimeStampFromTotalTime((float)player.time), false);
    }

    /// <summary>
    /// Function called when user presses pause video button while video is playing
    /// And to play the video again if it is paused
    /// </summary>
    public void ToggleVideoPlayPause()
    {
        isPaused = !isPaused;
        UpdateVideoPlayBack();
        //Invoking this event so that the UI class gets notified when video is paused or resumed
        VideoPlayPausedEvent?.Invoke(isPaused);
    }

    /// <summary>
    /// Function to pause the video if it is playing and vice versa
    /// </summary>
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

    /// <summary>
    /// Function to pause the video if it is playing
    /// </summary>
    private void PauseVideo()
    {
        if (player.isPlaying)
        {
            player.Pause();
        }
    }

    /// <summary>
    /// Function to resume the video if it is paused
    /// </summary>
    private void PlayVideo()
    {
        if (player.isPaused)
        {
            player.Play();
        }
    }

    /// <summary>
    /// Function called when user presses the fast forward button while video is playing
    /// </summary>
    public void FastForward()
    {
        float playerTime = (float)player.time;
        playerTime += fastForwardTime;
        playerTime = Mathf.Clamp(playerTime, 0, currVideoLength);
        player.time = playerTime;
    }

    /// <summary>
    /// Function called when user presses the rewind button while video is playing
    /// </summary>
    public void Rewind()
    {
        float playerTime = (float)player.time;
        playerTime -= fastForwardTime;
        playerTime = Mathf.Clamp(playerTime, 0, currVideoLength);
        player.time = playerTime;
    }

    /// <summary>
    /// Function to check if the video player is playing/running
    /// </summary>
    /// <returns></returns>
    private bool IsVideoPlayerRunning()
    {
        return player.isPlaying;
    }
    
    /// <summary>
    /// Function called when user presses the stop button while the video is playing
    /// </summary>
    public void StopVideo()
    {
        Debug.Log("stop video called");
         player.Stop();
         VideoStopEvent?.Invoke();
         isPaused = false;
    }
}
