using System;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zero.Engine.Base;

namespace Zero.Engine.UI
{
    static public class UGUITools
    {
        /// <summary>
        /// 找出相同名字的对象（找到第一个就返回）
        /// </summary>       
        public static Transform GetChildTransform(string childName, Transform parent)
        {
            int childCount = parent.childCount;
            for (int i = 0; i < childCount; ++i)
            {
                Transform curT = parent.GetChild(i);
                if (childName.Equals(curT.name))
                    return curT;
                Transform findT = GetChildTransform(childName, curT);
                if (findT != null)
                    return findT;
            }
            return null;
        }
        /// <summary>
        /// 通过名字找对象
        /// </summary>       
        public static GameObject GetChildByName(string childName, GameObject parent)
        {
            if (parent == null)
            {
                Zlogger.Error("{0} parent is null", childName);
                return null;
            }
            Transform childT = GetChildTransform(childName, parent.transform);
            if (childT != null)
                return childT.gameObject;
            else
            {
                Zlogger.Error("{0} can't find {1}", parent.name, childName);
                return null;
            }
        }
        /// <summary>
        /// 找出这个名字的组件
        /// </summary>      
        public static Component GetComponent(string componentTypeName, string name, GameObject go)
        {
            GameObject obj = GetChildByName(name, go);
            if (obj) return obj.GetComponent(componentTypeName);
            return null;
        }
        public static T GetChildComponent<T>(GameObject parent, string path) where T : Component
        {
            Transform trans = parent.transform.Find(path);
            if (trans != null) return trans.gameObject.GetComponent<T>();
            else return null;
        }
        public static GameObject FindChild(GameObject parent, string path)
        {
            if (parent != null)
            {
                Transform trans = parent.transform.Find(path);
                if (trans) return trans.gameObject;
                else return null;
            }
            else
            {
                Zlogger.Error("UGUITools.FindChild parent is null, and path is {0}", path);
                return null;
            }
        }
        /// <summary>
        /// 查找某路径的GameObject的Component T
        /// </summary>      
        public static Component FindInChild(string componentTypeName, GameObject parent, string path)
        {
            Transform trans = parent.transform.Find(path);
            if (trans == null) return null;
            else return trans.gameObject.GetComponent(componentTypeName);
        }
        /// <summary>
        /// 文本赋值
        /// </summary>            
        public static void SetTextInChild(GameObject parent, string path, string str)
        {
            Transform trans = parent.transform.Find(path);
            if (trans == null) return;
            Text txt = trans.gameObject.GetComponent<Text>();
            TextMeshPro txtTmp = trans.gameObject.GetComponent<TextMeshPro>();
            if (!string.IsNullOrEmpty(str) && txt != null) txt.text = str;
            if (!string.IsNullOrEmpty(str) && txtTmp != null) txtTmp.text = str;
        }
        public static GameObject SimpleClone(GameObject prefab, bool withChildren = true)
        {
            GameObject go = GameObject.Instantiate(prefab) as GameObject;
            Transform parentTrans = prefab.transform.parent;
            if (go != null && parentTrans != null)
            {
                Transform t = go.transform;
                RectTransform newRectTrans = t as RectTransform;
                RectTransform oldRectTrans = prefab.transform as RectTransform;
                t.SetParent(parentTrans);
                if (newRectTrans != null && oldRectTrans != null)
                {
                    newRectTrans.sizeDelta = oldRectTrans.sizeDelta;
                    newRectTrans.pivot = oldRectTrans.pivot;
                }
                t.localPosition = Vector3.zero;
                t.localRotation = Quaternion.identity;
                t.localScale = Vector3.one;
                go.layer = parentTrans.gameObject.layer;
            }
            if (go != null && !withChildren)
            {
                for (int i = go.transform.childCount - 1; i >= 0; i--)
                {
                    var child = go.transform.GetChild(i);
                    GameObject.Destroy(child.gameObject);
                }
            }
            return go;
        }
        public static void SetLayer(GameObject go, int layer)
        {
            go.layer = layer;
            Transform t = go.transform;
            for (int i = 0, imax = t.childCount; i < imax; ++i)
            {
                Transform child = t.GetChild(i);
                SetLayer(child.gameObject, layer);
            }
        }
        public static void Destroy(UnityEngine.Object obj)
        {
            if (obj != null)
            {
                if (Application.isPlaying)
                {
                    if (obj is GameObject)
                    {
                        GameObject go = obj as GameObject;
                        go.transform.parent = null;
                    }
                    UnityEngine.Object.Destroy(obj);
                }
                else
                    UnityEngine.Object.DestroyImmediate(obj);
            }
        }
        /// <summary>
        /// 获取 Assembly-CSharp
        /// </summary>
        static private Assembly _assemblyCSharp = null;
        static public Type GetTypeFromAssemblyCSharp(string typeName)
        {
            if (_assemblyCSharp == null)
            {
                _assemblyCSharp = Assembly.Load("Assembly-CSharp");
            }
            Type t = _assemblyCSharp.GetType(typeName);
            return t;
        }
        /// <summary>
        /// 获取 UnityEngine.UI
        /// </summary>
        static private Assembly _assmeblyUnityEngineUI = null;
        static public Type GetTypeFromAssemblyUnityEngineUI(string typeName)
        {
            if (_assmeblyUnityEngineUI == null)
            {
                _assmeblyUnityEngineUI = Assembly.Load("UnityEngine.UI");
            }
            if (!typeName.StartsWith("UnityEngine.UI"))
            {
                typeName = string.Concat("UnityEngine.UI.", typeName);
            }
            Type t = _assmeblyUnityEngineUI.GetType(typeName);
            return t;
        }
        /// <summary>
        /// 获取 UnityEngine
        /// </summary>
        static private Assembly _assmeblyUnityEngine = null;
        static public Type GetTypeFromAssemblyUnityEngine(string typeName)
        {
            if (_assmeblyUnityEngine == null)
            {
                _assmeblyUnityEngine = Assembly.Load("UnityEngine");
            }
            if (!typeName.StartsWith("UnityEngine"))
            {
                typeName = string.Concat("UnityEngine.", typeName);
            }
            Type t = _assmeblyUnityEngine.GetType(typeName);
            return t;
        }
        /// <summary>
        /// 获取 Plugin
        /// </summary>
        static private Assembly _assmeblyPlugin = null;
        static public Type GetTypeFromAssemblyPlugin(string typeName)
        {
            if (_assmeblyPlugin == null)
            {
                _assmeblyPlugin = Assembly.Load("Assembly-CSharp-firstpass");
            }
            Type t = _assmeblyUnityEngine.GetType(typeName);
            return t;
        }

        static public Type GetTypeFromAssembly(string typeName)
        {
            Type t = GetTypeFromAssemblyCSharp(typeName);
            if (t != null) return t;
            t = GetTypeFromAssemblyUnityEngineUI(typeName);
            if (t != null) return t;
            t = GetTypeFromAssemblyUnityEngine(typeName);
            if (t != null) return t;
            t = GetTypeFromAssemblyPlugin(typeName);
            return t;
        }

        static public ScriptableObject CreateScriptableObject(string typeName)
        {
            Type t = GetTypeFromAssemblyCSharp(typeName);
            if (t == null)
            {
                Zlogger.Error("CreateScriptableObject {0} can't find type.", typeName);
                return null;
            }
            return ScriptableObject.CreateInstance(t);
        }
        /// <summary>
        /// 添加组件
        /// </summary>       
        static public Component AddComponent(GameObject parent, string type)
        {
            return parent.AddComponent(GetTypeFromAssembly(type));
        }
        static public Component AddMissingComponent(GameObject go, string typeName)
        {
            Component comp = go.GetComponent(typeName);
            if (comp == null)
            {
                Type t = GetTypeFromAssembly(typeName);
                if (t != null)
                    comp = go.AddComponent(t);
                else
                {
                    Zlogger.Error("AddMissingComponent {0} can't find type.", typeName);
                    return null;
                }
            }
            return comp;
        }
        /// <summary>
        /// 获取rectTran的四个角对应的屏幕坐标
        /// </summary>         
        static public Vector3[] GetWroldCorners(RectTransform rectTran)
        {
            Vector3[] tmpVect = new Vector3[4];
            rectTran.GetWorldCorners(tmpVect);
            return tmpVect;
        }
        /// <summary>
        /// 触发指定节点下所有粒子系统播放
        /// </summary>                
        static public void PlayParticleSysyems(GameObject root)
        {
            ParticleSystem[] par = root.GetComponentsInChildren<ParticleSystem>();
            for (int i = 0; i < par.Length; ++i)
            {
                par[i].Stop();
                par[i].Play();
            }
        }
        static public void RefreshLayerAndOrder(GameObject targetObj, string sortLayerName, int orderInLayer)
        {
            Renderer[] renders = targetObj.GetComponentsInChildren<Renderer>(true);
            foreach (Renderer render in renders)
            {
                render.sortingOrder = orderInLayer;
                render.sortingLayerName = sortLayerName;
            }
        }
        static public bool PosInCollider(Collider c, Vector3 pos)
        {
            if (c == null) return false;
            return c.bounds.Contains(pos);
        }
        /// <summary>
        /// 获取文字长度
        /// </summary>
        /// <param name="font"> 字体</param>
        /// <param name="fontSize">字号</param>
        /// <param name="style">风格</param>
        /// <param name="str">文字</param>
        /// <returns></returns>
        public static int GetFontLen(Font font, int fontSize, FontStyle style, string str)
        {
            int len = 0;
            font.RequestCharactersInTexture(str, fontSize, style);
            CharacterInfo info;
            for (int i = 0; i < str.Length; i++)
            {
                font.GetCharacterInfo(str[i], out info, fontSize);
                len += info.advance;
            }
            return len;
        }
        public static Vector3 WorldToCanvas(Vector3 worldPos, Camera worldCamera, Camera uiCamera)
        {
            Vector3 screenPos = worldCamera.WorldToScreenPoint(worldPos);
            return uiCamera.ScreenToWorldPoint(screenPos);
        }
        public static GameObject ResourcesLoad(string path)
        {
            return GameObject.Instantiate(Resources.Load(path)) as GameObject;
        }
        /// <summary>
        /// 重播粒子动画
        /// </summary>                 
        public static void RestartParticleSystem(GameObject go)
        {
            if (go != null)
            {
                go.SetActive(true);
                ParticleSystem[] par = go.GetComponentsInChildren<ParticleSystem>();
                foreach (var p in par)
                {
                    var main = p.main;
                    main.simulationSpeed = 1;
                    p.time = 0;
                    p.Play(false);
                }
            }
        }
        /// <summary>
        /// 停止粒子动画
        /// </summary>  
        public static void StopParticleSystem(GameObject go)
        {
            if (go != null)
            {
                ParticleSystem[] par = go.GetComponentsInChildren<ParticleSystem>();
                foreach (var p in par)
                {
                    p.Clear(false);
                    p.Stop(false);
                }
            }
        }
        public static void SetEffectSortingLayer(GameObject go, string layerName)
        {
            if (go == null) return;
            ParticleSystemRenderer[] psRender = go.GetComponentsInChildren<ParticleSystemRenderer>(true);
            for (int i = 0; i < psRender.Length; i++)
            {
                if (psRender[i] == null) continue;
                psRender[i].sortingLayerName = layerName;
            }
        }
    }
}
