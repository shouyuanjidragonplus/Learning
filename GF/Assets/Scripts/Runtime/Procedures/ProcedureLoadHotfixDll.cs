using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GameFramework;
using GameFramework.Event;
using GameFramework.Procedure;
using GameFramework.Resource;
using GameMain;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

public class ProcedureLoadHotfixDll : ProcedureBase
{
    /// <summary>
    /// 全部的预加载热更脚本dll
    /// </summary>
    private List<string> hotfixDlls;

    private bool hotfixListIsLoaded;
    private int totalProgress;
    private int loadedProgress;

    protected override void OnEnter(ProcedureOwner procedureOwner)
    {
        base.OnEnter(procedureOwner);
#if !DISABLE_HYBRIDCLR
        GF.Event.Subscribe(LoadHotfixDllEventArgs.EventId, OnLoadHotfixDllCallback);
#endif
        PreloadAndInitData();
    }

    protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
    {
#if !DISABLE_HYBRIDCLR
        GF.Event.Unsubscribe(LoadHotfixDllEventArgs.EventId, OnLoadHotfixDllCallback);
#endif
        base.OnLeave(procedureOwner, isShutdown);
    }
    protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
        if (!hotfixListIsLoaded)
        {
            return;
        }
        //加载热更新Dll完成,进入热更逻辑
        if (loadedProgress >= totalProgress)
        {
            loadedProgress = -1;
            var entryFunc = Utility.Assembly.GetType("HotfixEntry")?.GetMethod("StartHotfixLogic", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            if (entryFunc == null)
            {
                Log.Fatal("游戏启动失败, 未找到HotfixEntry.StartHotfixLogic入口函数");
                return;
            }
#if !DISABLE_HYBRIDCLR
            entryFunc.Invoke(null, new object[] { true });
#else
            entryFunc.Invoke(null, new object[] { false });
#endif
        }
    }
    /// <summary>
    /// 加载热更新dll
    /// </summary>
    private void PreloadAndInitData()
    {
        //显示进度条
        GF.StartView.ShowLoadingProgress();
        totalProgress = 0;
        loadedProgress = 0;
        hotfixListIsLoaded = true;

#if !UNITY_EDITOR && !DISABLE_HYBRIDCLR
        hotfixListIsLoaded = false;
        LoadAotDlls();
        LoadHotfixDlls();
#endif
    }
    private void OnLoadHotfixDllCallback(object sender, GameEventArgs e)
    {
        var args = e as LoadHotfixDllEventArgs;
        if (args.UserData != this)
        {
            return;
        }

        if (args.Assembly == null)
        {
            Log.Info($"加载dll失败:{args.DllName}");
            return;
        }

        loadedProgress++;
        GF.StartView.SetLoadingProgress(loadedProgress / (float)totalProgress);

        //所有依赖dll加载完成后再加载Hotfix.dll
        if (hotfixDlls.Contains(args.DllName))
        {
            hotfixDlls.Remove(args.DllName);
            if (hotfixDlls.Count == 1)
            {
                var mainDll = Utility.Text.Format("Assets/Export/HotfixDlls/{0}.bytes", hotfixDlls.Last());
                LoadHotfixDll(mainDll, this);
            }
        }
    }

    /// <summary>
    /// 加载热更文件
    /// </summary>
    /// <param name="dllAssetName"></param>
    /// <param name="userData"></param>
    public void LoadHotfixDll(string dllAssetName, object userData)
    {
        GF.Resource.LoadAsset(dllAssetName, typeof(TextAsset), new LoadAssetCallbacks(OnLoadDllSuccess, OnLoadDllFail), userData);
    }

    private void OnLoadDllFail(string assetName, LoadResourceStatus status, string errorMessage, object userData)
    {
        Log.Error("加载{0}失败! Error:{1}", assetName, errorMessage);
        GF.Event.Fire(this, ReferencePool.Acquire<LoadHotfixDllEventArgs>().Fill(Path.GetFileNameWithoutExtension(assetName), null, userData));
    }

    private void OnLoadDllSuccess(string assetName, object asset, float duration, object userData)
    {
        var dllTextAsset = asset as TextAsset;
        System.Reflection.Assembly dllAssembly = null;
        if (dllTextAsset != null)
        {
            try
            {
                dllAssembly = System.Reflection.Assembly.Load(dllTextAsset.bytes);
            }
            catch (Exception e)
            {
                Log.Error("Assembly.Load加载热更dll失败:{0},Error:{1}", assetName, e.Message);
                throw;
            }
        }

        var dllName = Path.GetFileNameWithoutExtension(assetName);
        GF.Event.Fire(this, ReferencePool.Acquire<LoadHotfixDllEventArgs>().Fill(dllName, dllAssembly, userData));
    }
}