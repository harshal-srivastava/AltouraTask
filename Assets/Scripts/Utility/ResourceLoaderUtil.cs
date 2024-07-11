using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System.IO;
using UnityEngine.Networking;

/// <summary>
/// Enum type representation for the type of assets we want to load
/// Each of the asset have their path in the ResourcePathSO scriptable object present in the Resources/Data folder in the project
/// </summary>
public enum PrefabType
{
    VideoThumbNailPrefab,
    GLBModel,
    Room,
    Player,
    Project2UIPrefab,
    None
}

/// <summary>
/// Utility class responsible for loading the assets of different type from the resource folder
/// This is a singleton class, hence provides easy access to other classes to load resources from here
/// </summary>
public class ResourceLoaderUtil : MonoBehaviour
{
    [SerializeField]
    private string pathsScriptableObjectName;

    [SerializeField]
    private ResourcePathsSO pathSO;

    public static ResourceLoaderUtil instance;

    /// <summary>
    /// Delegate coupled with static event to notify the system when asset bundle has loaded
    /// This is done so that the assetbundle loaded can be passed to the respective class making the call
    /// </summary>
    /// <param name="bundle"></param>
    public delegate void AssetBundleLoaded(AssetBundle bundle);
    public static AssetBundleLoaded AssetBundleLoadedEvent;

    private void Awake()
    {
        LoadPathSO();
        //making the class singleton
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

    }

    /// <summary>
    /// Function to load the scriptable object file containing the path of all the resources
    /// </summary>
    private void LoadPathSO()
    {
        pathSO = Resources.Load<ResourcePathsSO>("Data/" + pathsScriptableObjectName);
    }

    /// <summary>
    /// Function to load the asset bundle from the StreamingAssets folder via URL
    /// Reason for putting asset bundles to StreamingAssets is because that way, any remote asset bundle can also be loaded via webrequest
    /// </summary>
    /// <param name="bundleName"></param>
    public void LoadAssetBundle(string bundleName)
    {
        StartCoroutine(LoadAssetBundleRoutine(bundleName));
    }

    /// <summary>
    /// Coroutine to load the asset bundle from the respective URL
    /// </summary>
    /// <param name="assetBundleName"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Function to load the prefab from the resources folder based on the type of prefab
    /// Based on the type of prefab, the system will build the path for the particular resource and then load it
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Function to load a Text file from the resources folder
    /// </summary>
    /// <param name="fileFormat"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Function call to load a particular sprite from the resources folder
    /// </summary>
    /// <param name="spriteName"></param>
    /// <returns></returns>
    public Sprite LoadSprite(string spriteName = "")
    {
        string path = pathSO.Project2DisplaySpritePath;
        Sprite sprite = Resources.Load<Sprite>(path + spriteName);
        return sprite;
    }
}
