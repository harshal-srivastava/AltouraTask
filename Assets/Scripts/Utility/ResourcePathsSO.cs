using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

}
