using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AssetBundleBuilder : MonoBehaviour
{
    [MenuItem("Assets/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        BuildPipeline.BuildAssetBundles("Assets/StreamingAssets/AssetBundles", BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
    }
}
