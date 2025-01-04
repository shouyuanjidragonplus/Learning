using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Zero.Base
{
    public class AppBase : MonoBehaviour
    {
        protected List<CAppModule> _allModules = new List<CAppModule>();
        protected List<CAppModule> _updateModules = new List<CAppModule>();
        protected List<CAppModule> _fixedUpdateModules = new List<CAppModule>();
        protected List<CAppModule> _lateUpdateModules = new List<CAppModule>();

        public static bool s_bPauseAll = false;
        protected virtual void AwakeProc() { }
        protected virtual void StartProc() { }
        protected virtual void OnApplicationFocusProc(bool focusStatus) { }
        protected virtual void OnApplicationPauseProc(bool pauseStatus) { }

        protected virtual void OnApplicationQuitProc()
        {
            Destory();
        }
        protected virtual void Destory()
        {
            int mCount = _allModules.Count;
            for (int i = mCount - 1; i >= 0; --i)
            {
                _allModules[i].Destory();
            }
            _allModules.Clear();
            _updateModules.Clear();
            _fixedUpdateModules.Clear();
            _lateUpdateModules.Clear();
        }
        protected void AddModule(CAppModule m, bool needUpdate = false, bool needLateUpdate = false, bool needFixedUpdate = false)
        {
            _allModules.Add(m);
            if (needUpdate) _updateModules.Add(m);
            if (needFixedUpdate) _fixedUpdateModules.Add(m);
            if (needLateUpdate) _lateUpdateModules.Add(m);
        }
        bool IsModuleAlreadyExist(List<CAppModule> ms, CAppModule m)
        {
            return (ms.IndexOf(m) >= 0);
        }
        private void Awake()
        {
            AwakeProc();
        }
        private void Start()
        {
            StartProc();
        }
        private void Update()
        {
            if (s_bPauseAll) return;

            int mCount = _updateModules.Count;
            for (int i = 0; i < mCount; ++i)
            {
                _updateModules[i].Update();
            }
        }
        private void FixedUpdate()
        {
            if (s_bPauseAll) return;

            int mCount = _fixedUpdateModules.Count;
            for (int i = 0; i < mCount; ++i)
            {
                _fixedUpdateModules[i].Update();
            }
        }
        private void LateUpdate()
        {
            if (s_bPauseAll) return;

            int mCount = _lateUpdateModules.Count;
            for (int i = 0; i < mCount; ++i)
            {
                _lateUpdateModules[i].Update();
            }
        }
        private void OnApplicationFocus(bool focus)
        {
            OnApplicationFocusProc(focus);
        }
        private void OnApplicationPause(bool pause)
        {
            OnApplicationPauseProc(pause);
        }
        private void OnApplicationQuit()
        {
            OnApplicationQuitProc();
        }
        protected float _lastTs = 0.0f;
        protected Stopwatch sw = new Stopwatch();
        protected void ShowTimeStamp(string content)
        {
            if (Driver.isWhiteListDevice == false || string.IsNullOrEmpty(content)) return;

            UnityEngine.Debug.Log(content);
            _lastTs = Time.realtimeSinceStartup;
            sw.Restart();
        }
    }
}
