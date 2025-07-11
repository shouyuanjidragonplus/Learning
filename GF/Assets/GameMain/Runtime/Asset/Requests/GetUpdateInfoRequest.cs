using GameFramework;
using UnityEngine;
namespace GameMain.Asset
{
    public sealed class GetUpdateInfoRequest : Request
    {
        private int _requestId;
        public UpdateInfo info { get; private set; }

        protected override void OnStart()
        {
            if (!Assets.Updatable)
            {
                SetResult(Result.Failed);
                return;
            }

            _requestId = GameEntry.WebRequest.AddWebRequest(Assets.UpdateInfoURL);
        }

        protected override void OnUpdated()
        {
            TaskInfo _taskInfo = GameEntry.WebRequest.GetWebRequestInfo(_requestId);
            if (_taskInfo.Status != TaskStatus.Done)
                return;

            if (_taskInfo.IsValid)
            {
                info = Utility.LoadFromJson<UpdateInfo>(_taskInfo.UserData.ToString());
                // Web GL 直接读取 Player Data Path
                if (Application.isEditor || !Assets.IsWebGLPlatform)
                    Assets.DownloadURL = info.downloadURL; // 更新下载地址
                // 版本文件未发生更新
                if (info.timestamp <= Assets.Versions.timestamp)
                {
                    SetResult(Result.Failed, "Nothing to update.");
                    return;
                }

                SetResult(Result.Success);
                return;
            }

            SetResult(Result.Failed);
        }
        // public VersionsRequest GetVersionsAsync()
        // {
        //     var request = new VersionsRequest
        //         { url = Assets.GetDownloadURL(info.file), hash = info.hash, size = info.size };
        //     request.SendRequest();
        //     return request;
        // }
    }
}