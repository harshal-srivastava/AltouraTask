using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Class responsible for the user interaction with the library as well as the video player screen
/// </summary>
public class VideoPlayerUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject libraryScreen;

    [SerializeField]
    private GameObject videoPlayerScreen;

    [SerializeField]
    private TextMeshProUGUI usernameText;

    private void Awake()
    {
        AttachEventSpecificListeners();
        DisableAllScreens();
        Initialize();
    }

    /// <summary>
    /// Function to initialize the respective values
    /// </summary>
    private void Initialize()
    {
        usernameText.text = string.Format("User : {0}", LoginSignUpManager.userName);
    }

    /// <summary>
    /// Function to attach game event listeners
    /// Helps in decoupling the referencing to multiple classes
    /// </summary>
    private void AttachEventSpecificListeners()
    {
        VideoLibraryManager.LibrarySetEvent += ShowVideoLibrary;
        VideoPlayerController.VideoPlayerReadyEvent += ShowVideoPlayerScreen;
        VideoLibraryManager.PlayVideoCallEvent += ShowVideoPlayerScreen;
        VideoPlayerController.VideoStopEvent += ShowVideoLibrary;
    }

    /// <summary>
    /// Function to detach listeners to respective class game events
    /// This is done as a safe keeping in future if a scene reload is required
    /// Static events couple with delegates don't work so well on scene reloads
    /// So detach them if object is destroyed and it will be attached again when instance of class is created
    /// </summary>
    private void DetachEventSpecificListeners()
    {
        VideoLibraryManager.LibrarySetEvent -= ShowVideoLibrary;
        VideoPlayerController.VideoPlayerReadyEvent -= ShowVideoPlayerScreen;
        VideoLibraryManager.PlayVideoCallEvent -= ShowVideoPlayerScreen;
        VideoPlayerController.VideoStopEvent -= ShowVideoLibrary;
    }

    /// <summary>
    /// Listener to VideoLibraryManager.LibrarySetEvent
    /// </summary>
    private void ShowVideoLibrary()
    {
        videoPlayerScreen.SetActive(false);
        libraryScreen.SetActive(true);
    }

    /// <summary>
    /// Listener to VideoPlayerController.VideoPlayerReadyEvent
    /// Navigates from the library screen to video player screen
    /// </summary>
    private void ShowVideoPlayerScreen()
    {
        libraryScreen.SetActive(false);
        videoPlayerScreen.SetActive(true);
    }

    /// <summary>
    /// Function to disable all screens
    /// </summary>
    private void DisableAllScreens()
    {
        videoPlayerScreen.SetActive(false);
        libraryScreen.SetActive(false);
    }

    /// <summary>
    /// Function to quit the application
    /// </summary>
    public void QuitApplication()
    {
        Application.Quit();
    }

    private void OnDestroy()
    {
        DetachEventSpecificListeners();
    }

}
