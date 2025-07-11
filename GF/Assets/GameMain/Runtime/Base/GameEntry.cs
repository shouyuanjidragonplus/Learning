//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System;
using System.Reflection;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain
{
    /// <summary>
    /// 游戏入口。
    /// </summary>
    public partial class GameEntry : MonoBehaviour
    {
        protected void Start()
        {
            InitBuiltinComponents();
            InitCustomComponents();
            if (Resource != null)
            {
                var resTp = Resource.GetType();
                var m_ResourceMode = resTp.GetField("m_ResourceMode", BindingFlags.Instance | BindingFlags.NonPublic);
                m_ResourceMode.SetValue(Resource, AppSettings.Instance.ResourceMode);
                Log.Info($"------------设置资源模式： ResourceMode:{AppSettings.Instance.ResourceMode}------------");
            }
        }
    }
}