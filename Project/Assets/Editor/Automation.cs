using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using Framework.Asset;

namespace Framework
{
    public partial class Automation : Editor
    {
        public static void CreateAsset<T>(string path) where T : ScriptableObject
        {
            var folder = Path.GetDirectoryName(Application.dataPath + "/Resources/" + path);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            T ac = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(ac, "Assets/Resources/" + path + ".asset");
        }

        [MenuItem("资源配置/生成AssetConfig")]
        private static void CreateController()
        {
            CreateAsset<AssetConfigController>(AssetConfigController.AssetConfigPath);
        }

        [MenuItem("Assets/资源配置/AtlasConfig", false, 0)]
        static public void GameCofig_Atlas()
        {
            string[] filePaths = GetFilePaths();
            if (filePaths == null || filePaths.Length == 0)
                return;

            foreach (var value in AtlasConfigController.Instance.AtlasPathNodeList)
            {
                if (value.AtlasName.Equals(filePaths[0]))
                {
                    UnityEditor.EditorUtility.DisplayDialog("AtlasFile 【Error】", "图集文件中已经存在 [ " + filePaths[0] + " ] 请手动检查 \n", "确定");

                    return;
                }
            }

            AtlasPathNode pathNode = new AtlasPathNode();
            pathNode.AtlasName = filePaths[0];
            pathNode.HdPath = filePaths[1];
            pathNode.SdPath = filePaths[2];

            AtlasConfigController.Instance.AtlasPathNodeList.Add(pathNode);

            EditorUtility.SetDirty(AtlasConfigController.Instance);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            UnityEditor.EditorUtility.DisplayDialog("AtlasConfig 【Success】", filePaths[0] + "\n" + filePaths[1] + "\n" + filePaths[2], "确定");
        }

        [MenuItem("Assets/资源配置/AssetConfig-Activity", false, 1)]
        static public void GameCofig_Asset_Activity()
        {
            string[] filePaths = GetFilePaths();
            if (filePaths == null || filePaths.Length == 0)
                return;

            foreach (string path in AssetConfigController.Instance.ActivityResPaths)
            {
                if (path.Equals(filePaths[1]))
                {
                    UnityEditor.EditorUtility.DisplayDialog("Activity 【Error】", "图集文件中已经存在 [ " + filePaths[1] + " ] 请手动检查 \n", "确定");
                    return;
                }
                else if (path.Equals(filePaths[2]))
                {
                    UnityEditor.EditorUtility.DisplayDialog("Activity 【Error】", "图集文件中已经存在 [ " + filePaths[2] + " ] 请手动检查 \n", "确定");
                    return;
                }
            }

            List<string> tempList = new List<string>(AssetConfigController.Instance.ActivityResPaths);
            tempList.Add(SubString(filePaths[1], "Hd"));
            tempList.Add(SubString(filePaths[2], "Sd"));

            AssetConfigController.Instance.ActivityResPaths = tempList.ToArray();

            EditorUtility.SetDirty(AssetConfigController.Instance);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            UnityEditor.EditorUtility.DisplayDialog("Activity 【Success】", filePaths[1] + "\n" + filePaths[2], "确定");
        }

        private static string[] GetFilePaths()
        {
            if (Selection.assetGUIDs == null || Selection.assetGUIDs.Length != 1)
            {
                UnityEditor.EditorUtility.DisplayDialog("GetAtlasFilePath 【Error】", "选择一个图集文件夹", "确定");
                return null;
            }

            string selectionPath = AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0]);
            int childCount = Directory.GetDirectories(selectionPath).Length;
            if (childCount != 2)
            {
                UnityEditor.EditorUtility.DisplayDialog("GetAtlasFilePath 【Error】", "图集路径选择错误，该文件夹下必须包含Hd Sd \n ErrorCode: 2", "确定");
                return null;
            }

            bool haveHd = false;
            bool haveSd = false;
            string[] hsFilePath = { "", "" };
            foreach (string fileDir in Directory.GetDirectories(selectionPath))
            {
                if (fileDir.EndsWith("Hd"))
                {
                    haveHd = true;
                    hsFilePath[0] = fileDir;
                }

                if (fileDir.EndsWith("Sd"))
                {
                    haveSd = true;
                    hsFilePath[1] = fileDir;
                }
            }

            if (!haveHd || !haveSd)
            {
                UnityEditor.EditorUtility.DisplayDialog("GetAtlasFilePath 【Error】", "图集路径选择错误，该文件夹下必须包含Hd Sd \n ErrorCode: 3", "确定");
                return null;
            }

            string[] spPath = selectionPath.Split('/');
            if (spPath == null || spPath.Length == 0)
            {
                UnityEditor.EditorUtility.DisplayDialog("GetAtlasFilePath 【Error】", "图集路径选择错误，该文件夹下必须包含Hd Sd \n ErrorCode: 4", "确定");
                return null;
            }

            string atlasName = spPath[spPath.Length - 1];
            int index = 0;
            for (int i = 0; i < hsFilePath.Length; i++)
            {
                foreach (string fileName in Directory.GetFiles(hsFilePath[i]))
                {
                    if (!fileName.EndsWith(".spriteatlas"))
                        continue;

                    hsFilePath[i] = fileName;
                    index = i;
                }
            }

            if (index != hsFilePath.Length - 1)
            {
                UnityEditor.EditorUtility.DisplayDialog("GetAtlasFilePath 【Error】", "图集路径选择错误，该文件夹下必须包含Hd Sd \n ErrorCode: 5", "确定");
                return null;
            }

            for (int i = 0; i < hsFilePath.Length; i++)
            {
                hsFilePath[i] = hsFilePath[i].Replace("Assets/Export/", "");
                hsFilePath[i] = hsFilePath[i].Replace(".spriteatlas", "");
            }

            string[] filePaths = { "", "", "" };
            filePaths[0] = atlasName;
            filePaths[1] = hsFilePath[0];
            filePaths[2] = hsFilePath[1];

            return filePaths;
        }
        private static string SubString(string orgStr, string subStr)
        {
            int index = orgStr.IndexOf(subStr);
            if (index < 0)
                return orgStr;

            string sub = orgStr.Substring(0, index + subStr.Length);

            return sub;
        }
    }
}