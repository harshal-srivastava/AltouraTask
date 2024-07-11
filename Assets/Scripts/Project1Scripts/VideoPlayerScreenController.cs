using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Class responsible for updating the visual UI elements for the video player screen while user interacts with it
/// </summary>
public class VideoPlayerScreenController : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup videoPlayBackControlsGroup;

    [SerializeField]
    private Button pauseButton;

    [SerializeField]
    private GameObject playAgainButton;

    [SerializeField]
    private Sprite pauseVideoSprite;

    [SerializeField]
    private Sprite playVideoSprite;

    [SerializeField]
    private TextMeshProUGUI videoStartingTimeText;

    [SerializeField]
    private TextMeshProUGUI videoEndingTimeText;

    [SerializeField]
    private TextMeshProUGUI videoCurrentTimeText;


    private void Awake()
    {
        AttachEventListeners();
    }

    /// <summary>
    /// Function to update the pause button sprite based on the state of the video player
    /// </summary>
    /// <param name="isPaused"></param>
    private void PlayPauseButtonPressed(bool isPaused)
    {
        if (isPaused)
        {
            pauseButton.GetComponent<Image>().sprite = playVideoSprite;
        }
        else
        {
            pauseButton.GetComponent<Image>().sprite = pauseVideoSprite;
        }
    }

    /// <summary>
    /// Function to attach game event listeners
    /// Helps in decoupling the referencing to multiple classes
    /// </summary>
    private void AttachEventListeners()
    {
        VideoPlayerController.VideoPlayPausedEvent += PlayPauseButtonPressed;
        VideoPlayerController.UpdateVideoTimeEvent += UpdateVideoPlayBackTime;
        VideoPlayerController.VideoStopEvent += StopVideoAndDisableVideoScreen;
        VideoPlayerController.VideoEndedEvent += EnablePlayAgainButton;
    }

    /// <summary>
    /// Listener to the VideoPlayerController.VideoStopEvent
    /// This will stop the video playback and reset the elements
    /// </summary>
    private void StopVideoAndDisableVideoScreen()
    {
        videoStartingTimeText.text = "";
        videoEndingTimeText.text = "";
        videoCurrentTimeText.text = "";
        pauseButton.GetComponent<Image>().sprite = pauseVideoSprite;
        playAgainButton.SetActive(false);
    }

    /// <summary>
    /// Listener to VideoPlayerController.UpdateVideoTimeEvent
    /// This updates the current video time to show the user time elapsed since video started running
    /// </summary>
    /// <param name="time"></param>
    /// <param name="doOnce"></param>
    private void UpdateVideoPlayBackTime(string time, bool doOnce)
    {
        if (doOnce)
        {
            //condition added to use the same function to set the initial start and end time
            //whenever a new video is playing
            videoStartingTimeText.text = time.Length == 5 ? "00:00" :"00:00:00";
            videoEndingTimeText.text = time;
            if (videoPlayBackControlsGroup.alpha == 0)
            {
                videoPlayBackControlsGroup.alpha = 1;
                playAgainButton.SetActive(false);
            }
        }
        else
        {
            videoCurrentTimeText.text = time;
        }
    }

    /// <summary>
    /// Listener to VideoPlayerController.VideoEndedEvent
    /// Will enable the play again button when a video is finished playing
    /// </summary>
    private void EnablePlayAgainButton()
    {
        playAgainButton.SetActive(true);
        videoPlayBackControlsGroup.alpha = 0;
    }

    /// <summary>
    /// Function to detach listeners to respective class game events
    /// This is done as a safe keeping in future if a scene reload is required
    /// Static events couple with delegates don't work so well on scene reloads
    /// So detach them if object is destroyed and it will be attached again when instance of class is created
    /// </summary>
    private void DetachEventListeners()
    {
        VideoPlayerController.VideoPlayPausedEvent -= PlayPauseButtonPressed;
        VideoPlayerController.UpdateVideoTimeEvent -= UpdateVideoPlayBackTime;
        VideoPlayerController.VideoStopEvent -= StopVideoAndDisableVideoScreen;
        VideoPlayerController.VideoEndedEvent -= EnablePlayAgainButton;
    }

    private void OnDestroy()
    {
        DetachEventListeners();
    }
}
