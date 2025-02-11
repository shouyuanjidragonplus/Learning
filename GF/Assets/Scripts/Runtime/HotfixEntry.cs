using GameFramework;
using GameFramework.Fsm;
using GameFramework.Procedure;
using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 热更逻辑入口
/// </summary>
public class HotfixEntry
{
    public static async void StartHotfixLogic(bool enableHotfix)
    {
        Debug.LogFormat("Hotfix Enable:{0}", enableHotfix);
        AwaitExtension.SubscribeEvent();

        GF.Fsm.DestroyFsm<IProcedureManager>();
        var procManager = GameFrameworkEntry.GetModule<IProcedureManager>();
        var appConfig = await AppConfigs.GetInstanceSync();

        ProcedureBase[] procedures = new ProcedureBase[appConfig.Procedures.Length];
        for (int i = 0; i < appConfig.Procedures.Length; i++)
        {
            procedures[i] = (ProcedureBase)Activator.CreateInstance(Type.GetType(appConfig.Procedures[i]));
        }

        procManager.Initialize(GameFrameworkEntry.GetModule<IFsmManager>(), procedures);
        await UniTask.DelayFrame(1);
        procManager.StartProcedure<ProcedurePreload>();
    }
}