using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;
using Debug = UnityEngine.Debug;
using UObject = UnityEngine.Object;

namespace Framework.Asset
{
    public static partial class BuildAssetBundles
    {
        // 版本文件
        private static Dictionary<string, VersionInfo> OutsideAssetVersion;
        private static VersionInfo RemoteVersion;
        private static VersionInfo LocalVersion;
        // assetbundle输出路径
#if UNITY_IOS
        static string exportPath = FilePathTools.assetBundleOutPath + "/" + AssetConfigController.Instance.IOSRootVersion;
#else
        static string exportPath = FilePathTools.assetBundleOutPath + "/" + AssetConfigController.Instance.RootVersion;
#endif
        private static BuildAssetBundleOptions recommandBundleOptions = BuildAssetBundleOptions.ChunkBasedCompression | BuildAssetBundleOptions.DeterministicAssetBundle
#if !UNITY_STANDALONE && !DISABLE_AB_ENCRYPTION
            | BuildAssetBundleOptions.EnableProtection
#endif
            ;

        [MenuItem("AssetBundle/打包所有资源")]
        public static void BuildAllAssetBundleWithJenkinsStyle()
        {
            BuildPre();

            BuildAllAssetBundleWithOptions(recommandBundleOptions);

            BuildPost();

            Debug.Log("BuildAllAssetBundle finish");
        }

        [MenuItem("AssetBundle/打包所有活动资源")]
        public static void BuildAllActivityAssetBundleWithJenkinsStyle()
        {
            BuildPre();
            
            BuildMultipleActivityAssetBundleWithCommandLine();
            
            BuildPost();

            Debug.Log("BuildAllActivityAssetBundle finish");
        }
    }
}