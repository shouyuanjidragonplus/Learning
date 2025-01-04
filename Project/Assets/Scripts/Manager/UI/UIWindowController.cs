using Framework.Base;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zero.Engine.Base;


public abstract class UIWindowController : UIWindow
{
    public GameObject GetItem(string key, GameObject parObj = null)
    {
        if (parObj == null)
        {
            parObj = this.gameObject;
        }

        var obj = FindObj(key, parObj);
        if (obj == null)
        {
            Zlogger.Error("GetItem failed, window controller name : {0},  key = {1}", GetType().ToString(), key);
        }

        return obj;
    }

    public bool TryGetItem(string key, out GameObject go, GameObject parObj = null)
    {
        if (parObj == null)
        {
            parObj = this.gameObject;
        }

        var trans = parObj.transform.Find(key);
        go = trans ? trans.gameObject : null;
        return go != null;
    }

    public T GetItem<T>(string key, GameObject parObj = null)
    {
        var go = GetItem(key, parObj);
        return GetItem<T>(go);
    }

    public T GetItem<T>(GameObject go)
    {
        if (go != null)
        {
            var com = go.GetComponent<T>();
            if (com == null)
            {
                Zlogger.Error("GetItem failed, window controller name : {0},  game object name = {1}, Component type:{2}",
                    GetType().ToString(), go.name, typeof(T).ToString());
            }

            return com;
        }

        return default(T);
    }

    public Toggle BindValueChange(string key, UnityAction<bool> onValueChanged, GameObject parObj = null)
    {
        var toggle = GetItem<Toggle>(key, parObj);
        if (toggle)
        {
            toggle.onValueChanged.RemoveAllListeners();
            toggle.onValueChanged.AddListener(onValueChanged);
        }

        return toggle;
    }
}