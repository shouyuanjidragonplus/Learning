using Zero.Base;
using UnityEngine;
using UnityEngine.UI;
using Framework.Storage;

//各个模块
namespace MMO
{
    public class BaseModule : CAppModule
    {
        private CanvasScaler _canvasScaler = null;
        public override void Init()
        {

        }
        public override void Destory()
        {
            //ScreenResizeHelper.RemoveCallback(UpdateCanvasScale);
        }
        public override void Update()
        {
            //ScreenResizeHelper.Update();
        }
        public override void LateUpdate()
        {

        }
        public void SetCanvasScale(CanvasScaler cs)
        {
            //_canvasScaler = cs;
            //if (_canvasScaler != null)
            //{
            //    UpdateCanvasScale();
            //    ScreenResizeHelper.AddCallback(UpdateCanvasScale);
            //}
        }
        private void UpdateCanvasScale()
        {
            //if (ScreenResizeHelper.isFitWidth)
            //    _canvasScaler.matchWidthOrHeight = 1.0f;
            //else
            //    _canvasScaler.matchWidthOrHeight = 0.0f;
        }
    }
    public class SceneFsmModule : CAppModule
    {
        SceneFsm instance = null;
        public override void Init()
        {
            GameObject mainCamGo = Camera.main.gameObject;
            instance = mainCamGo.GetOrCreateComponent<SceneFsm>();
            instance.AwakeProc();
        }
        public override void Update()
        {
            instance.Update();
        }
        public override void LateUpdate()
        {
            instance.LateUpdate();
        }
    }
    public class CameraModule : CAppModule
    {
        //AroundAlignCamera instance = null;
        public override void Init()
        {
            //GameObject mainCamGo = Camera.main.gameObject;
            //instance = mainCamGo.GetComponent<AroundAlignCamera>();
            //instance.AwakeProc();
        }
        public override void LateUpdate()
        {
            //instance.LateUpdateProc();
        }
    }
    public class UIModule : CAppModule
    {
        UIManager instance = null;
        public override void Init()
        {
            GameObject mainCamGo = Camera.main.gameObject;
            instance = mainCamGo.AddComponent<UIManager>();
            instance.AwakeProc();
        }
    }
    public class DataModule : CAppModule
    {
        //DataManager instance = null;
        //public override void Init()
        //{
        //    GameObject mainCamGo = Camera.main.gameObject;
        //    instance = mainCamGo.AddComponent<DataManager>();
        //    instance.AwakeProc();
        //}
        //public override void Update()
        //{
        //    instance.MUpdate();
        //}
        public override void Destory()
        {
            PlayerPrefs.Save();
            StorageManager.Instance.SaveToLocal();
        }
    }
    public class ModelModule : CAppModule
    {
        //ModelManager instance = null;
        //public override void Init()
        //{
        //    GameObject mainCamGo = Camera.main.gameObject;
        //    instance = mainCamGo.AddComponent<ModelManager>();
        //    instance.AwakeProc();
        //}
        //public override void Update()
        //{
        //    instance.MUpdate();
        //}
    }
    public class VoiceModule : CAppModule
    {
        //VoiceManager instance = null;
        //public override void Init()
        //{
        //    GameObject mainCamGo = Camera.main.gameObject;
        //    instance = mainCamGo.AddComponent<VoiceManager>();
        //    instance.AwakeProc();
        //}
        //public override void Update()
        //{
        //    instance.VoiceUpdate();
        //}
    }
    public class AnimationModule : CAppModule
    {
        //AniManager instance = null;
        //public override void Init()
        //{
        //    GameObject mainCamGo = Camera.main.gameObject;
        //    instance = mainCamGo.AddComponent<AniManager>();
        //    instance.AwakeProc();
        //}
        //public override void Update()
        //{
        //    instance.AniUpdate();
        //}
    }
    public class StepModule : CAppModule
    {
        //StepEventManager instance = null;
        //public override void Init()
        //{
        //    GameObject mainCamGo = Camera.main.gameObject;
        //    instance = mainCamGo.AddComponent<StepEventManager>();
        //    instance.AwakeProc();
        //}
        //public override void Update()
        //{
        //    instance.EventUpdate();
        //}
    }
}