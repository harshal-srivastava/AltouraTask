using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    private void Initialize()
    {
        usernameText.text = string.Format("User : {0}", LoginSignUpManager.userName);
    }

    private void AttachEventSpecificListeners()
    {
        VideoLibraryManager.LibrarySetEvent += ShowVideoLibrary;
        VideoPlayerController.VideoPlayerReadyEvent += ShowVideoPlayerScreen;
        VideoLibraryManager.PlayVideoCallEvent += ShowVideoPlayerScreen;
        VideoPlayerController.VideoStopEvent += ShowVideoLibrary;
    }

    private void DetachEventSpecificListeners()
    {
        VideoLibraryManager.LibrarySetEvent -= ShowVideoLibrary;
        VideoPlayerController.VideoPlayerReadyEvent -= ShowVideoPlayerScreen;
        VideoLibraryManager.PlayVideoCallEvent -= ShowVideoPlayerScreen;
        VideoPlayerController.VideoStopEvent -= ShowVideoLibrary;
    }

    private void ShowVideoLibrary()
    {
        videoPlayerScreen.SetActive(false);
        libraryScreen.SetActive(true);
    }

    private void ShowVideoPlayerScreen()
    {
        libraryScreen.SetActive(false);
        videoPlayerScreen.SetActive(true);
    }

    private void DisableAllScreens()
    {
        videoPlayerScreen.SetActive(false);
        libraryScreen.SetActive(false);
    }

    private void OnDestroy()
    {
        DetachEventSpecificListeners();
    }

}
