using System;
using System.Collections;
using System.Collections.Generic;
using GameMain;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameEntry = GameMain.GameEntry;

public class GF : GameEntry
{
    private void Start()
    {
        InitGameEntry();
        Debug.Log("Game Framework Start");
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