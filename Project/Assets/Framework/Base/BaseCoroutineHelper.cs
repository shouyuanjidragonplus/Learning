using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Base
{
    /// <summary>
    /// 提供基本 协程 服务
    /// 可指定字符串做区分（一个字符串对应一个GameObject的MonoBehaviour进行处理）
    /// </summary>
    public class BaseCoroutineHelper
    {
        private static BaseCoroutineHelper _instance = null;
        public static BaseCoroutineHelper Instance
        {
            get
            {
                if (_instance != null) { }
                else _instance = new BaseCoroutineHelper();
                return _instance;
            }
        }
        sealed class CoroutineCore : MonoBehaviour
        {
            private void OnDestroy()
            {
                StopAllCoroutines();
            }
        }
        private static string COROUTINEHELPER_GONAME = "CoroutineHelper";
        private GameObject _coroutineHelperObj = null;
        private static string DEFAULT_COROUTINEHELPER = "MainCoroutine";
        private Dictionary<string, CoroutineCore> _cores = new Dictionary<string, CoroutineCore>();
        private BaseCoroutineHelper()
        {
            _coroutineHelperObj = new GameObject(COROUTINEHELPER_GONAME);
            Object.DontDestroyOnLoad(_coroutineHelperObj);
        }
        public Coroutine StartCoroutine(string coreName, IEnumerator routine)
        {
            CoroutineCore curCore = GetOrCreateCoroutineCore(coreName);
            return curCore.StartCoroutine(routine);
        }
        public void StopCoroutine(string coreName, Coroutine routine)
        {
            CoroutineCore curCore = GetCoroutineCore(coreName);
            if (curCore != null)
                curCore.StopCoroutine(routine);
            else
                BaseLogger.Error("You StopCoroutine with eroor coreName:{0}", coreName);
        }
        public void StopCoroutine(string coreName, IEnumerator routine)
        {
            CoroutineCore curCore = GetCoroutineCore(coreName);
            if (curCore != null)
                curCore.StopCoroutine(routine);
            else
                BaseLogger.Error("You StopCoroutine with eroor coreName:{0}", coreName);
        }
        public void StopAllCoroutines(string coreName)
        {
            CoroutineCore curCore = GetCoroutineCore(coreName);
            if (curCore != null) curCore.StopAllCoroutines();
            else
                BaseLogger.Error("Not have this coreName:{0}", coreName);
        }
        public void StopAllCoroutines()
        {
            var iter = _cores.GetEnumerator();
            while (iter.MoveNext())
            {
                iter.Current.Value.StopAllCoroutines();
            }
        }
        public Coroutine StartCoroutine(IEnumerator routine)
        {
            return StartCoroutine(DEFAULT_COROUTINEHELPER, routine);
        }
        public void StopCoroutine(Coroutine routine)
        {
            StopCoroutine(DEFAULT_COROUTINEHELPER, routine);
        }
        public void StopCoroutine(IEnumerator routine)
        {
            StopCoroutine(DEFAULT_COROUTINEHELPER, routine);
        }
        public void DestroyCoroutineCore(string coreName)
        {
            if (_cores.ContainsKey(coreName))
            {
                CoroutineCore curCore = _cores[coreName];
                curCore.StopAllCoroutines();
                GameObject.DestroyImmediate(curCore.gameObject);
            }
        }
        private CoroutineCore GetCoroutineCore(string coreName)
        {
            CoroutineCore curCC = null;
            _cores.TryGetValue(coreName, out curCC);
            return curCC;
        }
        private CoroutineCore CreateCoroutineCore(string coreName)
        {
            GameObject curCoreObj = new GameObject(coreName);
            curCoreObj.transform.parent = _coroutineHelperObj.transform;

            CoroutineCore curCore = curCoreObj.AddComponent<CoroutineCore>();
            _cores.Add(coreName, curCore);
            return curCore;
        }
        private CoroutineCore GetOrCreateCoroutineCore(string coreName)
        {
            CoroutineCore curCore = GetCoroutineCore(coreName);
            if (curCore != null) { }
            else curCore = CreateCoroutineCore(coreName);
            return curCore;
        }
    }
}
