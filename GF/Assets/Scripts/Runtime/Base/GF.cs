using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using GameMain;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameEntry = GameMain.GameEntry;

public class GF : GameEntry
{
    public static RedDotComponent RedDot { get; private set; }

    private void Start()
    {
        RedDot = UnityGameFramework.Runtime.GameEntry.GetComponent<RedDotComponent>();
    }

    private void OnApplicationQuit()
    {
        OnExitGame();
    }

    private void OnApplicationPause(bool pause)
    {
        if (Application.isMobilePlatform && pause)
        {
            OnExitGame();
        }
    }

    private void OnExitGame()
    {
        GF.Event.FireNow(this, GFEventArgs.Create(GFEventType.ApplicationQuit));
        var exit_time = DateTime.UtcNow.ToString();
        GF.Setting.SetString(Constant.Setting.QuitAppTime, exit_time);
        GF.Setting.Save();
        Log.Info("Application Quit:{0}", exit_time);
    }
}