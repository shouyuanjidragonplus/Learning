using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Framework.Base;
using Framework.Common;

namespace Framework.Utils
{
    /// <summary>
    /// 简单加载支援辅助类
    /// </summary>
    public class SimpleLoader
    {
        public delegate void OnLoadFinished(System.Object obj);
        public delegate void OnProgress(float progress);
        public static IEnumerator Load(string filePath, OnLoadFinished onLoadFinished = null, OnProgress onProgress = null, bool isFindInPresistentDataPath = true)
        {
#if LOAD_ASSETBUNDLE
            string fileRealPath = null;
            if (isFindInPresistentDataPath)
                fileRealPath = PathUtils.ToABResFilePathForWWW(filePath);
            else
            {
                string pathInStreamingAssetsPath = string.Concat(Application.streamingAssetsPath, "/", filePath);
                fileRealPath = PathUtils.TranFilePathForWWW_AtStreamingAssetsPath(pathInStreamingAssetsPath);
            }
            if (Driver.isWhiteListDevice)
                BaseLogger.Warning("SimpleLoader:begin to load {0},curTS is {1} sec.", fileRealPath, Time.realtimeSinceStartup.ToString("F3"));
            UnityWebRequest webRequest = null;
            bool bABFile = fileRealPath.EndsWith(DriverCommPaths.ABFilePostfix);
            if (bABFile)
                webRequest = UnityWebRequestAssetBundle.GetAssetBundle(fileRealPath, 0);
            else
                webRequest = UnityWebRequest.Get(fileRealPath);
            webRequest.SendWebRequest();
            if (null == onProgress)
            {
                while (webRequest.isDone == false)
                    yield return null;
            }
            else
            {
                while (true)
                {
                    try
                    {
                        onProgress(webRequest.downloadProgress);
                    }
                    catch (Exception ex)
                    {
                        BaseLogger.Error("SimpleLoader->onProgress call error ({0}) ({1})", fileRealPath, ex.Message);
                    }
                    if (webRequest.isDone) break;
                    yield return null;
                }
            }
            long _ts = DateTime.Now.Ticks;

            if (null == webRequest.error)
            {
                System.Object asset = null;
                AssetBundle ab = null;
                if (bABFile)
                {
                    try
                    {
                        ab = DownloadHandlerAssetBundle.GetContent(webRequest);
                    }
                    catch (Exception ex)
                    {
                        BaseLogger.Error("SimpleLoader GetContent error ({0}) ({1})", fileRealPath, ex.Message);
                    }
                    if (null != ab)
                        asset = ab.GetMainAsset();
                    else
                        asset = webRequest;
                }
                else
                {
                    asset = webRequest;
                }
                if (Driver.isWhiteListDevice)
                {
                    BaseLogger.Warning("load {0},used/curTS is {1} ms/{2} sec.", fileRealPath, (DateTime.Now.Ticks - _ts) / 10000, Time.realtimeSinceStartup.ToString("F3"));
                }
                if (null != onLoadFinished)
                {
                    try
                    {
                        onLoadFinished(asset);
                    }
                    catch (Exception ex)
                    {
                        BaseLogger.Error("SimpleLoader onLoadFinished call error ({0}) ({1})", fileRealPath, ex.Message);
                    }
                }
                if (null != ab)
                {
                    ab.Unload(false);
                }
            }
            else
            {
                BaseLogger.Error("SimpleLoader->load failed ({0}) ({1})", fileRealPath, webRequest.error);

                if (null != onLoadFinished)
                {
                    try
                    {
                        onLoadFinished(null);
                    }
                    catch (Exception ex)
                    {
                        BaseLogger.Error("SimpleLoader->onLoadFinished call error ({0}) ({1})", fileRealPath, ex.Message);
                    }
                }
            }
            webRequest.Dispose();
            webRequest = null;
#else
            //去除后缀
            string fileRealPath = filePath;
            int extBeginPos = filePath.LastIndexOf(".");
            if (extBeginPos > 0)
            {
                int slashPos = filePath.LastIndexOf("/");
                if (slashPos < extBeginPos)
                    fileRealPath = filePath.Substring(0, extBeginPos);
                int backslashPos = filePath.LastIndexOf("\\");
                if (backslashPos < extBeginPos)
                    fileRealPath = filePath.Substring(0, extBeginPos);
            }
            UnityEngine.Object obj = Resources.Load<UnityEngine.Object>(fileRealPath);
            if (null != onLoadFinished)
            {
                try
                {
                    onLoadFinished(obj);
                }
                catch (Exception ex)
                {
                    BaseLogger.Error("SimpleLoader onLoadFinished [{0}],error is {1}", fileRealPath, ex.Message);
                }
            }
#endif
            yield return true;
        }
    }
}
