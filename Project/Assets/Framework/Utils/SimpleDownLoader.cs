using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Framework.Base;

namespace Framework.Utils
{
    /// <summary>
    /// 简易的www下载器
    /// 提供：
    ///     1,常规www下载 
    ///     2,超时检查
    ///     3,下载地址可附带随机版本号
    /// </summary>
    public class SimpleDownLoader
    {
#if UNITY_ANDROID
        private static int Max_Retrytime = 3;
        private static float Retry_Waitime = 0.2f;
        public static float timeOutLimit = 3.0f;
#else
        private static int Max_Retrytime = 3;
        private static float Retry_Waitime = 0.2f;
        public static float timeOutLimit = 3.0f;
#endif
        /// <summary>
        /// 冲CDN 备源的最大重试次数
        /// </summary>
        private static int Max_Retry_Time_From_CDN_BAK = 3;

        public static IEnumerator DownloadFile(string url, bool isAppendRandomVer, Action<bool, UnityWebRequest> onDownloadFinished, Action<float> onDownloadProgress = null, WWWForm wwwForm = null)
        {
            int retryTime = 0;
            bool bUseCDNBakUrl = false;
            for (; retryTime < Max_Retrytime + Max_Retry_Time_From_CDN_BAK; ++retryTime)
            {
                string realUrl = Uri.EscapeUriString(url);
                if (isAppendRandomVer)
                    realUrl = string.Concat(realUrl, string.Format("?v={0:F0}{1:F0}", GenTotalMillseconds(), UnityEngine.Random.Range(100000f, 999999f)));
                BaseLogger.Info("SimpleDownloader->retry [{0}]: begin download ({1}, FrameNo.[{2}])", retryTime + 1, realUrl, Time.frameCount);

                bool isDownloadSucc = false;
                float downloadLastCheckTime = Time.realtimeSinceStartup;
                float lastCheckProcess = 0f;

                UnityWebRequest webRequest;
                if (wwwForm == null)
                    webRequest = UnityWebRequest.Get(realUrl);
                else
                    webRequest = UnityWebRequest.Post(realUrl, wwwForm);

                webRequest.SendWebRequest();
                while (true)
                {
                    if (webRequest.isDone)
                    {
                        isDownloadSucc = (null == webRequest.error);
                        break;
                    }
                    if (null != webRequest.error)
                        break;
                    if (null != onDownloadProgress)
                    {
                        try
                        {
                            onDownloadProgress(webRequest.downloadProgress);
                        }
                        catch (Exception ex)
                        {
                            BaseLogger.Error("SimpleDownLoader -> onDownloadProgress call ({0},error ({1}),FrameNo.[{2}])", realUrl, ex.Message, Time.frameCount);
                        }
                    }
                    //Android 也开放超时间检测，强制结束此下载（安卓后面不能进行Dispose）
                    if (Mathf.Abs(webRequest.downloadProgress - lastCheckProcess) >= 1e-4)
                    {
                        lastCheckProcess = webRequest.downloadProgress;
                        downloadLastCheckTime = Time.realtimeSinceStartup;
                    }
                    else
                    {
                        if (Time.realtimeSinceStartup - downloadLastCheckTime >= timeOutLimit)
                        {
                            BaseLogger.Warning("SimpleDownloader->retry [{0}]:download timeout ({1})", retryTime, realUrl);
                            break;
                        }
                    }
                    yield return null;
                }
                if (isDownloadSucc)
                {
                    if (null != onDownloadFinished)
                    {
                        try
                        {
                            onDownloadFinished(true, webRequest);
                        }
                        catch (Exception ex)
                        {
                            BaseLogger.Error("SimpleDownloader-> onDownloadFinished call ({0}),error ({1}),FrameNo.[{2}]", realUrl, ex.Message, Time.frameCount);
                        }
                    }

                    webRequest.Dispose();
                    webRequest = null;
                    BaseLogger.Info("SimpleDownloader-> No.[{0}]:download successful ({1}),and Dispose,FrameNo.[{2}]", retryTime + 1, realUrl, Time.frameCount);
                    break;
                }
                else
                {
                    BaseLogger.Warning("SimpleDownloader->retry [{0}] times:download failed ({1})-({2})", retryTime + 1, realUrl, webRequest.error);

                    if (retryTime >= Max_Retrytime + Max_Retry_Time_From_CDN_BAK - 1)
                    {
                        if (null != onDownloadFinished)
                        {
                            try
                            {
                                onDownloadFinished(false, webRequest);
                            }
                            catch (Exception ex)
                            {
                                BaseLogger.Error("SimpleDownloader->onDownloadFinishied call ({0}),error ({1})", realUrl, ex.Message);
                            }
                        }
#if UNITY_ANDROID
                        //此逻辑路径不会是isDown，增加保险判断确保进行Dispose
                         if (webRequest.isDone)
                        {
                            webRequest.Dispose();
                            webRequest = null;
                        }

#else
                        webRequest.Dispose();
                        webRequest = null;
#endif
                        break;
                    }
                    else
                    {
#if UNITY_ANDROID
                         //此逻辑路径不会是isDown，增加保险判断确保进行Dispose
                         if (webRequest.isDone)
                        {
                            webRequest.Dispose();
                            webRequest = null;
                        }
#else
                        webRequest.Dispose();
#endif
                        BaseLogger.Info("SimpleDownloader->retry wait begin");
                        yield return new WaitForSeconds(Retry_Waitime);
                        BaseLogger.Info("SimpleDownloader->retry wait end");

                        //重试 Max_Retrytime 后，使用 CDN 备源下载
                        if (retryTime >= Max_Retrytime - 1)
                        {
                            if (bUseCDNBakUrl == false)
                            {

                            }
                        }
                    }
                }
            }
            yield return true;
        }
        /// <summary>
        /// 下载新的url格式文件 （http://zzxx-cdnres.xxxx.com/zzxx{0}/...）
        /// </summary>
        /// <returns></returns>
        public static IEnumerator DownloadURLFile(string url, object randomVerStr, Action<bool, UnityWebRequest> onDownloadFinished, Action<float> onDownloadProgress = null)
        {
            if (randomVerStr == null)
            {
                randomVerStr = string.Format("{0:F0}{1:F0}", GenTotalMillseconds(), UnityEngine.Random.Range(100000f, 999999f));
            }
            try
            {
                if (url.ToLower().Contains("cdnres."))
                    url = string.Format(url, randomVerStr);
                else
                    url = string.Format(url, "res");
            }
            catch (Exception ex)
            {
                BaseLogger.Error("DownloadURLFile rewrite url {0} error:{1}", url, ex.Message);
                if (onDownloadFinished != null)
                {
                    UnityWebRequest webRequest = UnityWebRequest.Get(url);
                    webRequest.SendWebRequest();
                    onDownloadFinished(false, webRequest);
                }
                return null;
            }
            return DownloadFile(url, false, onDownloadFinished, onDownloadProgress);
        }
        private static DateTime _startDT = new DateTime(1970, 1, 1);
        /// <summary>
        /// 获取1970-01-01至今所有过的毫秒数
        /// </summary>           
        private static double GenTotalMillseconds()
        {
            TimeSpan ts = DateTime.Now.Subtract(_startDT);
            return ts.TotalMilliseconds;
        }
    }
}
