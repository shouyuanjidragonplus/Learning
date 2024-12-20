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
        [MenuItem("AssetBundle/打包所有资源")]
        public static void BuildAllAssetBundleWithJenkinsStyle()
        {
            BuildPre();

            // if (AssetConfigController.Instance.ProjectPretreatmentDelegate)
            //     AssetConfigController.Instance.ProjectPretreatmentDelegate.ExecutePretreatment();
            // BuildAllAssetBundleWithOptions(recommandBundleOptions);
            //
            // BuildPost();

            Debug.Log("BuildAllAssetBundle finish");
        }
        
        [MenuItem("AssetBundle/打包所有活动资源")]
        public static void BuildAllActivityAssetBundleWithJenkinsStyle()
        {
            BuildPre();

            // if (AssetConfigController.Instance.ProjectPretreatmentDelegate)
            //     AssetConfigController.Instance.ProjectPretreatmentDelegate.ExecutePretreatment();
            // BuildMultipleActivityAssetBundleWithCommandLine();
            //
            // BuildPost();

            Debug.Log("BuildAllActivityAssetBundle finish");
        }
    }
}