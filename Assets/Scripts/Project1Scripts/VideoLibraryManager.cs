using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.Video;

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

    public delegate void LibraryGridSetCallBack();
    public static LibraryGridSetCallBack LibrarySetEvent;

    public delegate void PlayVideoEvent();
    public static PlayVideoEvent PlayVideoCallEvent;

    private void Awake()
    {
        GetAllAvailableVideos();
    }

    private void GetAllAvailableVideos()
    {
        LoadVideoFromAssetBundle();
    }

    private void LoadVideoFromAssetBundle()
    {
        ResourceLoaderUtil.instance.LoadAssetBundle(bundleName);
        ResourceLoaderUtil.AssetBundleLoadedEvent += AssetBundleLoaded;
    }

    private void AssetBundleLoaded(AssetBundle bundle)
    {
        if (bundle != null)
        {
            availableLibraryOfVideosList = bundle.LoadAllAssets<VideoClip>();

            if (availableLibraryOfVideosList != null)
            {
                SetLibraryGrid();
                ResourceLoaderUtil.AssetBundleLoadedEvent -= AssetBundleLoaded;
            }
            else
            {
                Debug.LogError("Failed to load video clip from AssetBundle.");
            }
        }
    }


    private void SetLibraryGrid()
    {
        for (int i = 0; i < availableLibraryOfVideosList.Length; i++)
        {
            GameObject videothumbNailPrefab = ResourceLoaderUtil.instance.LoadPrefab(PrefabType.VideoThumbNailPrefab);
            Button videoThumbnail = Instantiate(videothumbNailPrefab, videoThumbnailHolder.transform).GetComponent<Button>();
            TextMeshProUGUI text = videoThumbnail.transform.GetComponentInChildren<TextMeshProUGUI>();
            text.text = string.Format("Video : {0}", i + 1);
            int j = i;
            videoThumbnail.onClick.AddListener(() => PlayVideoWithIndex(j));
        }
        LibrarySetEvent?.Invoke();
    }

    private void PlayVideoWithIndex(int index)
    {
        Debug.Log("playing video : " + index);
        PlayVideoCallEvent?.Invoke();
        videoPlayerControllerRef.PlayVideo(availableLibraryOfVideosList[index]);
    }
}
