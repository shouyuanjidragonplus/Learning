using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zero.Base;

namespace MMO
{
    public class UMain : AppBase
    {
        private BaseModule _baseModule = null;
        private SceneFsmModule _sceneFsmModule = null;
        private CameraModule _cameraModule = null;
        private UIModule _uiModule = null;
        private DataModule _dataModule = null;
        private VoiceModule _voiceModule = null;

        //不被删除的放置在DontDestoryOnLoad 的基础BaseGo
        private const string Keepme_Name = "keepme";

        protected override void AwakeProc()
        {
            GameObject keepme = new GameObject(Keepme_Name);
            DontDestroyOnLoad(keepme);
            UMainData.keepme = keepme;

            _baseModule = new BaseModule();
            _baseModule.Init();
            AddModule(_baseModule, needUpdate: true, needLateUpdate: true);

            GameObject canvasGo = GameObject.Find("URoot/Canvas") as GameObject;
            if (canvasGo != null)
            {
                UMainData.canvas = canvasGo;
                CanvasScaler cs = canvasGo.GetComponent<CanvasScaler>();
                _baseModule.SetCanvasScale(cs);
            }
            LightmapSettings.lightmapsMode = LightmapsMode.NonDirectional;
        }
        protected override void StartProc()
        {
            InitModule();
        }
        private void InitModule()
        {
            _sceneFsmModule = new SceneFsmModule();
            _sceneFsmModule.Init();
            AddModule(_cameraModule, needUpdate: true, needLateUpdate: true);

            _dataModule = new DataModule();
            _dataModule.Init();
            AddModule(_dataModule, needUpdate: true);

            _cameraModule = new CameraModule();
            _cameraModule.Init();
            AddModule(_cameraModule, needUpdate: true, needLateUpdate: true);

            _voiceModule = new VoiceModule();
            _voiceModule.Init();
            AddModule(_voiceModule, needUpdate: true);

            _uiModule = new UIModule();
            _uiModule.Init();
            AddModule(_uiModule);
        }

    }
}
