using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Helper class to enable an asset building pipeline for the application
/// This will make asset bundles in the StreamingAssets folder which can be used in the application to load asset bundles
/// </summary>
public class AssetBundleBuilder : MonoBehaviour
{
    [MenuItem("Assets/Build AssetBundles")]
    private static void BuildAllAssetBundles()
    {
        BuildPipeline.BuildAssetBundles("Assets/StreamingAssets/AssetBundles", BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
    }
}
