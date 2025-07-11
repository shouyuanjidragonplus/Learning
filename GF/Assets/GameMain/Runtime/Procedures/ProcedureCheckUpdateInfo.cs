using GameFramework.Event;
using GameFramework.Procedure;
using GameMain.Asset;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using Utility = GameFramework.Utility;

namespace GameMain
{
    public class ProcedureCheckUpdateInfo : ProcedureBase
    {
        private bool initComplete = false;
        private UpdateInfo m_UpdateInfo;

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            initComplete = false;
            GameEntry.Event.Subscribe(WebRequestSuccessEventArgs.EventId, OnWebRequestSuccess);
            GameEntry.Event.Subscribe(WebRequestFailureEventArgs.EventId, OnWebRequestFail);
            CheckUpdateInfo();
        }


        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (initComplete)
            {
                if (m_UpdateInfo)
                {
                    ChangeState<ProcedureUpdateResources>(procedureOwner);
                }
            }
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            GameEntry.Event.Unsubscribe(WebRequestSuccessEventArgs.EventId, OnWebRequestSuccess);
            GameEntry.Event.Unsubscribe(WebRequestFailureEventArgs.EventId, OnWebRequestFail);
            base.OnLeave(procedureOwner, isShutdown);
        }

        /// <summary>
        /// 向服务器发送请求获取版本信息 进行版本更新检测
        /// </summary>
        void CheckUpdateInfo()
        {
            Log.Debug(GameEntry.Resource);
            Log.Debug(GameEntry.Resource.ResourceMode);
            // if (GameEntry.Resource.ResourceMode == GameFramework.Resource.ResourceMode.Updatable ||
            //     GameEntry.Resource.ResourceMode == GameFramework.Resource.ResourceMode.UpdatableWhilePlaying)
            // {
            Log.Debug("当前为热更新模式, Web请求最新版本号...");
            Log.Debug("请求版本信息地址:{0}", Assets.UpdateInfoURL);
            GameEntry.WebRequest.AddWebRequest(Assets.UpdateInfoURL, this);
            GameEntry.StartView.ShowLoadingProgress(0);
            // }
            // else
            // {
            //     GameEntry.Resource.InitResources(OnResInitComplete);
            // }
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
            Log.Debug($"最新资源版本信息:{webText}");
            var vinfo = Utility.Json.ToObject<UpdateInfo>(webText);
            CheckUpdateInfo(vinfo);
        }

        private void OnWebRequestFail(object sender, GameEventArgs e)
        {
            GameEntry.StartView.SetLoadingProgress(0, "更新失败，请检查网络连接!");
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
                GameEntry.StartView.SetLoadingProgress(1, "当前版本已是最新!");
            }
            else
            {
                m_UpdateInfo = vinfo;
                GameEntry.StartView.SetLoadingProgress(0, "发现新版本, 准备下载...");
            }

            OnResInitComplete();
        }
    }
}