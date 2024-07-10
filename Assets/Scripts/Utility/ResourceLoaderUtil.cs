using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System.IO;
using UnityEngine.Networking;

public enum PrefabType
{
    VideoThumbNailPrefab,
    GLBModel,
    Room,
    Player,
    Project2UIPrefab,
    None

}
public class ResourceLoaderUtil : MonoBehaviour
{
    [SerializeField]
    private string pathsScriptableObjectName;

    [SerializeField]
    private ResourcePathsSO pathSO;

    public static ResourceLoaderUtil instance;

    public delegate void AssetBundleLoaded(AssetBundle bundle);
    public static AssetBundleLoaded AssetBundleLoadedEvent;

    private void Awake()
    {
        LoadPathSO();
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

    }

    private void LoadPathSO()
    {
        pathSO = Resources.Load<ResourcePathsSO>("Data/" + pathsScriptableObjectName);
    }


    public void LoadAssetBundle(string bundleName)
    {
        StartCoroutine(LoadAssetBundleRoutine(bundleName));
    }

    private IEnumerator LoadAssetBundleRoutine(string assetBundleName)
    {
        string path = Application.streamingAssetsPath + pathSO.AssetBundlePath + assetBundleName;
        var www = UnityWebRequestAssetBundle.GetAssetBundle(path, 0);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
            yield break;
        }

        AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);
        if (bundle != null)
        {
            AssetBundleLoadedEvent?.Invoke(bundle);
        }
        else
        {
            Debug.LogError("Could not load asset bundle");
        }
    }

    public VideoClip LoadVideo(string videoName)
    {
        return null;
    }

    public GameObject LoadPrefab(PrefabType type)
    {
        string prefabPath = "";
        switch (type)
        {
            case PrefabType.GLBModel:
                prefabPath = pathSO.GLBModelPath;
                break;
            case PrefabType.VideoThumbNailPrefab:
                prefabPath = pathSO.VideoThumbnailPrefabPath;
                break;
            case PrefabType.Room:
                prefabPath = pathSO.RoomPrefabPath;
                break;
            case PrefabType.Player:
                prefabPath = pathSO.PlayerControlPrefabPath;
                break;
            case PrefabType.Project2UIPrefab:
                prefabPath = pathSO.Project2UIPath;
                break;
        }

        GameObject obj = Resources.Load<GameObject>(prefabPath);
        return obj;
    }

    public TextAsset LoadFile(string fileFormat = "")
    {
        string path = pathSO.UserDataFilePath;
        string filePath = Path.Combine(Application.dataPath + "/Resources/", (path + fileFormat));
        Debug.Log("file path from resource loader : " + filePath);
        if (File.Exists(filePath))
        {
            Debug.Log("File exists resource loader");
            TextAsset file = Resources.Load<TextAsset>(path);
            return file;
        }
        else
        {
            Debug.Log("file not found resource loader");
        }
        return null;
    }

    public Sprite LoadSprite(string spriteName = "")
    {
        string path = pathSO.Project2DisplaySpritePath;
        Sprite sprite = Resources.Load<Sprite>(path + spriteName);
        return sprite;
    }
}
