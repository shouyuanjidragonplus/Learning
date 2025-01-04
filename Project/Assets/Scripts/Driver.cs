using Framework.Base;
using Framework.Common;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Driver : MonoBehaviour
{
    //进入主程序类名
    string STARTGAME_NAME = "MMO.UMain";
    public static bool isWhiteListDevice = false;
    private GameObject uRoot = null;
    void Awake()
    {
        Application.runInBackground = DriverConfig.isRunInBg;
        Application.targetFrameRate = DriverConfig.defaultFrame;
        uRoot = GameObject.Find("/URoot");
        if (uRoot == null)
            BaseLogger.Error("Can't find uRoot GameObject!");
        DontDestroyOnLoad(uRoot);

        if (Debug.isDebugBuild)
        {
            FieldInfo fieldInfo = SRDebugger.Settings.Instance.GetType().GetField("_triggerPosition",
                BindingFlags.Instance | BindingFlags.GetField | BindingFlags.NonPublic | BindingFlags.ExactBinding);
            fieldInfo.SetValue(SRDebugger.Settings.Instance, SRDebugger.PinAlignment.TopCenter);

            SRDebug.Init();
        }
    }
    private void Start()
    {
        StartCoroutine(OnStart());
    }

    private IEnumerator OnStart()
    {
        yield return StartCoroutine(HandleDrive_Step2());
    }
    private Assembly _assemblyCSharp = null;
    private IEnumerator HandleDrive_Step2()
    {
        _assemblyCSharp = Assembly.Load("Assembly-CSharp");

        yield return StartCoroutine(GameStart());
    }
    private IEnumerator GameStart()
    {
        string gameComponent = STARTGAME_NAME;
        GameObject mainCamGO = Camera.main.gameObject;
        mainCamGO.AddComponent(_assemblyCSharp.GetType(gameComponent));
        _assemblyCSharp = null;

        yield return new WaitForEndOfFrame();
        Destroy(gameObject);
    }
}
