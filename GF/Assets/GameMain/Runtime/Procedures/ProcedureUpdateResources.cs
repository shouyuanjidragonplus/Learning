using GameFramework.Event;
using GameFramework.Procedure;
using GameMain.Asset;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using Utility = GameFramework.Utility;

namespace GameMain
{
    public class ProcedureUpdateResources : ProcedureBase
    {
        private bool initComplete = false;
        public Versions versions { get; private set; }
        public string url { get; set; }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            initComplete = false;
            GameEntry.Event.Subscribe(WebRequestSuccessEventArgs.EventId, OnWebRequestSuccess);
            GameEntry.Event.Subscribe(WebRequestFailureEventArgs.EventId, OnWebRequestFail);
            CheckVersion();
        }


        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            GameEntry.Event.Unsubscribe(WebRequestSuccessEventArgs.EventId, OnWebRequestSuccess);
            GameEntry.Event.Unsubscribe(WebRequestFailureEventArgs.EventId, OnWebRequestFail);
        }

        /// <summary>
        /// 向服务器发送请求获取版本信息
        /// </summary>
        private void CheckVersion()
        {
            Log.Debug(Application.temporaryCachePath);
            
            url = Assets.GetDownloadURL(Versions.Filename);
            Log.Debug("版本地址:{0}", url);
            GameEntry.WebRequest.AddWebRequest(url, this);
            GameEntry.StartView.ShowLoadingProgress(0);
        }

        void OnResInitComplete()
        {
            initComplete = true;
            Log.Debug("All Resource Completed!");
        }

        private void OnWebRequestSuccess(object sender, GameEventArgs e)
        {
            var arg = e as WebRequestSuccessEventArgs;
            if (arg.UserData != this)
            {
                return;
            }

            var webText = Utility.Converter.GetString(arg.GetWebResponseBytes());
            Debug.Log($"最新资源版本信息:{webText}");
            var vinfo = Utility.Json.ToObject<UpdateInfo>(webText);
            CheckUpdateInfo(vinfo);
        }

        private void OnWebRequestFail(object sender, GameEventArgs e)
        {
            Log.Debug("Web请求失败, 请检查网络连接!");
        }

        private void CheckUpdateInfo(UpdateInfo vinfo)
        {
            if (vinfo == null)
            {
                Log.Error("热更失败: 解析version.json信息失败!");
                return;
            }

            if (Application.isEditor || !Assets.IsWebGLPlatform)
                Assets.DownloadURL = vinfo.downloadURL; // 更新下载地址
            // 版本文件未发生更新
            if (vinfo.timestamp <= Assets.Versions.timestamp)
            {
                Log.Debug("当前版本已是最新!");
                return;
            }
        }
    }
}