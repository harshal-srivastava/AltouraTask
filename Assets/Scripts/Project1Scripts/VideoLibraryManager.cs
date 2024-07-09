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

    private string videosAssetBundleURL = "";

    [SerializeField]
    private GameObject videoThumbnailPrefab;

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
        //videosAssetBundleURL = "file://" + Application.dataPath + "/AssetBundles/videos";
        //videosAssetBundleURL = string.Format("file:///{0}/AssetBundles/{1}", Application.streamingAssetsPath, bundleName);
        videosAssetBundleURL = Path.Combine(Application.streamingAssetsPath, bundleName);
        Debug.Log("path : " + videosAssetBundleURL);
        StartCoroutine(LoadVideoFromBundle());
    }


    private IEnumerator LoadVideoFromBundle()
    {
        var www = UnityWebRequestAssetBundle.GetAssetBundle(videosAssetBundleURL,0);
        
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
                yield break;
            }

            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);

            availableLibraryOfVideosList = bundle.LoadAllAssets<VideoClip>();

            if (availableLibraryOfVideosList != null)
            {
                SetLibraryGrid();
            }
            else
            {
                Debug.LogError("Failed to load video clip from AssetBundle.");
            }
        
    }

    private void SetLibraryGrid()
    {
        for (int i = 0; i < availableLibraryOfVideosList.Length; i++)
        {
            Button videoThumbnail = Instantiate(videoThumbnailPrefab, videoThumbnailHolder.transform).GetComponent<Button>();
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
