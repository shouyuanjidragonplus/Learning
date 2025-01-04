using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Zero.Engine.Base
{
    public class PathHelper
    {
#if UNITY_EDITOR && !GAME_PUBLISH
        static string _wwwSAPrefix = "file:///";
        static string _wwwPDPrefix = "file:///";
#elif UNITY_IOS
        static string _wwwSAPrefix = "file://";
        static string _wwwPDPrefix = "file://";
#elif UNITY_ANDROID
#if UNITY_EDITOR
        static string _wwwSAPrefix = "file:///";
        static string _wwwPDPrefix = "file:///";
#else
        static string _wwwSAPrefix = "";
        static string _wwwPDPrefix = "file://";
#endif
#else
        static string _wwwSAPrefix = "file:///";
        static string _wwwPDPrefix = "file:///";
#endif

        public static string WWWStreamingAssetsPrefix => _wwwSAPrefix;
        public static string WWWPresistentDataPrefix => _wwwPDPrefix;

        static string _SABasePath = null;

        public static string StreamingAssetBasePath
        {
            get
            {
                if (_SABasePath != null) { }
                else
                {
#if UNITY_EDITOR        
                    _SABasePath = Application.dataPath + "/StreamingAssets/";

#else
#if UNITY_ANDROID
                    _SABasePath = Application.dataPath + "!assets/";
#elif UNITY_IPHONE
                    _SABasePath = string.Concat(Application.streamingAssetsPath, "/");
#else
                    _SABasePath = string.Concat(Application.streamingAssetsPath, "/");
#endif
#endif
                }
                return _SABasePath;
            }
        }
        static string _PDBasePath = null;
        /// <summary>
        /// 获取持久化目录，*路径最后已经加了 '/'*
        /// </summary>
        public static string PersistentDataBasePath => string.Concat(Application.persistentDataPath, "/res/");     //热更资源放在res目录下

        static string _wwwSABasePath = null;
        public static string WWWStreamingAssetsPath => string.Concat(_wwwSAPrefix, Application.streamingAssetsPath, "/");
        static string _wwwPDBasePath = null;
        public static string WWWPersistentDataBasePath => string.Concat(_wwwPDPrefix, Application.persistentDataPath, "/res/");     //热更资源放在res目录下

        public static string GetWWWSARestPath(string resPath)
        {
            return string.Concat(WWWStreamingAssetsPath, resPath);
        }
        public static string GetWWWPDResPath(string resPath)
        {
            return string.Concat(WWWPersistentDataBasePath, resPath);
        }
        public static string GetPDResPath(string resPath)
        {
            return string.Concat(PersistentDataBasePath, resPath);
        }
        public static string GetResPath(string resPath)
        {
            string pdResPath = string.Concat(PersistentDataBasePath, resPath);
            if (File.Exists(pdResPath)) return pdResPath;
            return string.Concat(StreamingAssetBasePath, resPath);
        }
        public static bool IsFileExistInPD(string resPath)
        {
            string pdResPath = string.Concat(PersistentDataBasePath, resPath);
            if (File.Exists(pdResPath)) return true;
            return false;
        }
        public static string GetWWWResPath(string resPath)
        {
            string pdResPath = string.Concat(PersistentDataBasePath, resPath);
            if (File.Exists(pdResPath)) return string.Concat(WWWPersistentDataBasePath, resPath);
            return string.Concat(WWWStreamingAssetsPath, resPath);
        }
    }
}
