using UnityEngine;

namespace Framework.Utils
{
    public class PathUtils
    {
        /// <summary>
        /// 转换文件路径，供WWW对象加载处理（仅针对streamingAssets目录中的资源）
        /// </summary> 
        public static string TranFilePathForWWW_AtStreamingAssetsPath(string path)
        {
            string filePrefix = "";
#if UNITY_EDITOR
            filePrefix = "file:///";
#elif UNITY_IOS
            filePrefix = "file://";
#elif UNITY_ANDROID
             filePrefix = "";
#else
            filePrefix = "file:///";
#endif
            string filePath = (string.IsNullOrEmpty(filePrefix) ? path : string.Concat(filePrefix, path));
            return filePath;
        }
        /// <summary>
        /// 转换文件路径，供WWW对象加载处理（仅针对Persistent目录中的资源）
        /// </summary> 
        public static string TranFilePathForWWW_AtPersistentPath(string path)
        {
            string filePrefix = "";
#if UNITY_EDITOR
            filePrefix = "file:///";
#elif UNITY_IOS
            filePrefix = "file://";
#elif UNITY_ANDROID
             filePrefix = "file://";
#else
            filePrefix = "file:///";
#endif
            string filePath = (string.IsNullOrEmpty(filePrefix) ? path : string.Concat(filePrefix, path));
            return filePath;
        }
        public static string ToABResFilePathForWWW(string filePath)
        {
#if GAME_PUBLISH
            string path = string.Concat(FileUtils.resRoot, "/", filePath);
            if (!FileUtils.IsFileExists(path))
            {
                string pathInStreamingAssetPath = string.Concat(Application.streamingAssetsPath, "/", filePath);
                path = TranFilePathForWWW_AtStreamingAssetsPath(pathInStreamingAssetPath);
            }
            else
            {
                path = TranFilePathForWWW_AtPersistentPath(path);
            }
            return path;
#else
            string pathInStreamingAssetPath = string.Concat(Application.streamingAssetsPath, "/", filePath);
            string path = TranFilePathForWWW_AtStreamingAssetsPath(pathInStreamingAssetPath);
            return path;
#endif
        }
    }
}
