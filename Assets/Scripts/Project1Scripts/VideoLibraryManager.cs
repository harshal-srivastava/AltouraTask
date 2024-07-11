using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;

/// <summary>
/// Class responsible for loading the videos from the asset bundles
/// Creating the available videos library and populating it with relevant video data
/// </summary>
public class VideoLibraryManager : MonoBehaviour
{
    [SerializeField]
    private VideoClip[] availableLibraryOfVideosList;

    [SerializeField]
    private string bundleName;

    [SerializeField]
    private GameObject videoThumbnailHolder;

    [SerializeField]
    private VideoPlayerController videoPlayerControllerRef;

    /// <summary>
    /// Delegate coupled with static event to send callback when the library is properly set up
    /// </summary>
    public delegate void LibraryGridSetCallBack();
    public static LibraryGridSetCallBack LibrarySetEvent;

    /// <summary>
    /// Delegate coupled with static event to send callback to play a particular video with specific index
    /// </summary>
    public delegate void PlayVideoEvent();
    public static PlayVideoEvent PlayVideoCallEvent;

    private void Awake()
    {
        GetAllAvailableVideos();
    }

    /// <summary>
    /// Function to retreive all the videos present
    /// </summary>
    private void GetAllAvailableVideos()
    {
        LoadVideoFromAssetBundle();
    }

    /// <summary>
    /// Function which makes call to the ResourceLoaderUtil class to load the asset bundle containing the videos
    /// And attach the callback listener to know when the asset bundle is loaded
    /// </summary>
    private void LoadVideoFromAssetBundle()
    {
        ResourceLoaderUtil.instance.LoadAssetBundle(bundleName);
        ResourceLoaderUtil.AssetBundleLoadedEvent += AssetBundleLoaded;
    }

    /// <summary>
    /// Listener to the ResourceLoaderUtil.AssetBundleLoadedEvent
    /// If asset is loaded successfully, then setup the video library
    /// </summary>
    /// <param name="bundle"></param>
    private void AssetBundleLoaded(AssetBundle bundle)
    {
        if (bundle != null)
        {
            availableLibraryOfVideosList = bundle.LoadAllAssets<VideoClip>();
            if (availableLibraryOfVideosList != null)
            {
                SetLibraryGrid();
                //remove the ResourceLoaderUtil.AssetBundleLoadedEvent listener
                //this is done to avoid this function being called everytime if any other class loads any other asset bundle
                ResourceLoaderUtil.AssetBundleLoadedEvent -= AssetBundleLoaded;
            }
            else
            {
                Debug.LogError("Failed to load video clip from AssetBundle.");
            }
        }
    }

    /// <summary>
    /// Set the UI grid with the thumbnails of the videos present in the asset bundle
    /// This mechanism will dynamically handle any number of the videos present in the asset bundle
    /// </summary>
    private void SetLibraryGrid()
    {
        //instantiate the video thumbnail prefab (a UI button) for all the videos
        for (int i = 0; i < availableLibraryOfVideosList.Length; i++)
        {
            GameObject videothumbNailPrefab = ResourceLoaderUtil.instance.LoadPrefab(PrefabType.VideoThumbNailPrefab);
            Button videoThumbnail = Instantiate(videothumbNailPrefab, videoThumbnailHolder.transform).GetComponent<Button>();
            //update the text of the button to indicate video index, scalable to add a thumbnail image to the videos
            TextMeshProUGUI text = videoThumbnail.transform.GetComponentInChildren<TextMeshProUGUI>();
            text.text = string.Format("Video : {0}", i + 1);
            int j = i;
            //Add the listener for the instantiated button onlcick
            //this mechanism will handle any number of video buttons with a single function call
            videoThumbnail.onClick.AddListener(() => PlayVideoWithIndex(j));
        }
        //Once library is set, invoke the function to progress the UI
        LibrarySetEvent?.Invoke();
    }

    /// <summary>
    /// Function called when user presses any video button in the library
    /// </summary>
    /// <param name="index"></param>
    private void PlayVideoWithIndex(int index)
    {
        Debug.Log("playing video : " + index);
        PlayVideoCallEvent?.Invoke();
        videoPlayerControllerRef.PlayVideo(availableLibraryOfVideosList[index]);
    }
}
