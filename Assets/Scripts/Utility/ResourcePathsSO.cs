using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scriptable object class to store the resources path in order to load it using the ResourceLoaderUtil
/// This mechanism is to easily load new elements by simply adding their path over here or changing the path of existing objects
/// </summary>
[CreateAssetMenu(fileName = "PathSO", menuName = "CustomObject/Path")]
public class ResourcePathsSO : ScriptableObject
{
    [SerializeField]
    private string videosFolderPath;

    [SerializeField]
    private string videoThumbnailPrefabPath;

    [SerializeField]
    private string assetBundlePath;

    [SerializeField]
    private string glbModelPath;

    [SerializeField]
    private string roomPrefabPath;

    [SerializeField]
    private string playerControlPrefabPath;

    [SerializeField]
    private string project2UIPath;

    [SerializeField]
    private string userDataFilePath;

    [SerializeField]
    private string project2DisplaySpritePath;

    public string VideosFolderPath
    {
        get
        {
            return videosFolderPath;
        }
    }

    public string VideoThumbnailPrefabPath
    {
        get
        {
            return videoThumbnailPrefabPath;
        }
    }

    public string AssetBundlePath
    {
        get
        {
            return assetBundlePath;
        }
    }

    public string GLBModelPath
    {
        get
        {
            return glbModelPath;
        }
    }

    public string RoomPrefabPath
    {
        get
        {
            return roomPrefabPath;
        }
    }

    public string PlayerControlPrefabPath
    {
        get
        {
            return playerControlPrefabPath;
        }
    }

    public string UserDataFilePath
    {
        get
        {
            return userDataFilePath;
        }
    }

    public string Project2UIPath
    {
        get
        {
            return project2UIPath;
        }
    }

    public string Project2DisplaySpritePath
    {
        get
        {
            return project2DisplaySpritePath;
        }
    }

}
