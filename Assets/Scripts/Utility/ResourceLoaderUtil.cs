using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public enum PrefabType
{
    VideoThumbNailPrefab,
    GLBModel,
    Room,
    Player,
    None

}
public class ResourceLoaderUtil : MonoBehaviour
{
    [SerializeField]
    private string pathsScriptableObjectName;

    [SerializeField]
    private ResourcePathsSO pathSO;

    public static ResourceLoaderUtil instance;

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


    public AssetBundle LoadAssetBundle()
    {
        return null;
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
        }

        GameObject obj = Resources.Load<GameObject>(prefabPath);
        return obj;
    }
}
