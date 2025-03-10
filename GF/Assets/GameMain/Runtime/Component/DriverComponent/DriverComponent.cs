using System.Collections;
using System.Collections.Generic;
using GameMain.Asset;
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 编辑器模式下的一些测试
/// </summary>
[DisallowMultipleComponent]
[AddComponentMenu("Game Framework/Driver")]
public class DriverComponent : GameFrameworkComponent
{
    public bool LoadAB = false;
    public static Scheduler Scheduler = null;

    private void Awake()
    {
        if (Debug.isDebugBuild)
        {
            SRDebug.Init();
        }
    }

    private void Start()
    {
        Scheduler = new Scheduler();
    }

    private void Update()
    {
        if (Scheduler)
        {
            Scheduler.Update();
        }
    }

    private void OnDestroy()
    {
        if (Scheduler)
        {
            Scheduler.Dispose();
        }

        Scheduler = null;
    }
}