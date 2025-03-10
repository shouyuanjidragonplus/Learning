using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace GameMain.Editor
{
    [CreateAssetMenu(fileName = nameof(Build), menuName = "ScriptableObject/BuildAsset")]
    public class Build : ScriptableObject
    {
        public Object[] groups;
        private static Build instance;

        public static Build Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = AssetDatabase.LoadAssetAtPath<Build>("Assets/Config/Builds/Build.asset");
                }

                return instance;
            }
        }
    }

    public class BuildAB
    {
        [MenuItem("Assets/BuildAB")]
        public static void CreateAsset()
        {
            List<AssetBundleBuild> buildList = new List<AssetBundleBuild>();
            // foreach (var group in Build.Instance.groups)
            // {
            //     var path = AssetDatabase.GetAssetPath(group);
            //     Debug.Log($"打包地址 {path}");
            //     var abName = group.name.ToLower() + ".ab";
            //     Debug.Log($"包名 {abName}");
            //     AssetBundleBuild build = new AssetBundleBuild
            //     {
            //         assetBundleName = abName,
            //         assetNames = new string[] { path },
            //     };
            //     buildList.Add(build);
            // }
            AssetBundleBuild build = new AssetBundleBuild();
            build.assetBundleName = "test.ab";
            build.assetNames = new string[]
            {
                "Assets/Export/TestBuild/Texture/Common_cycle.png",
                "Assets/Export/TestBuild/Pre/Cube.prefab",
                "Assets/Export/TestBuild/Mat/cubMat.mat",
            };
            buildList.Add(build);
            BuildPipeline.BuildAssetBundles("Assets", buildList.ToArray(), BuildAssetBundleOptions.None,
                EditorUserBuildSettings.activeBuildTarget);
            AssetDatabase.Refresh();
            Debug.Log("打包成功");
        }
    }
}