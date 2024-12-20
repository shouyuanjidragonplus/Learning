using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

namespace Framework.Asset
{
    public static partial class BuildAssetBundles
    {
        public static readonly string AtlasConfigName = "AtlasConfigController";

        [MenuItem("AssetBundle/生成图集配置")]
        public static void CreateAtlasConfig()
        {
            AtlasConfigController asset = ScriptableObject.CreateInstance<AtlasConfigController>();
            asset.ParseAtlasPath("/Export/SpriteAtlas");

            AssetDatabase.CreateAsset(asset, $"Assets/Resources/Settings/{AtlasConfigName}.asset");
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
        }
    }
}