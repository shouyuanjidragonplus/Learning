using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Base;

/// <summary>
/// 脚本与外部（Android/ios）通讯的中间层
/// 需要分开BaseNativeProxy和NativeProxy是因为plugin代码中要用到，所以讲必须的代码放到这里，扩展代码放到NativeProxy
/// BaseNativeProxy不支持回调到lua中，这个实现在NativePorxy中
/// </summary>
namespace Framework.Utils
{
    public class BaseNativeProxy : MonoBehaviour
    {
        private static BaseNativeProxy _instance;
        public static BaseNativeProxy Instance
        {
            get
            {
                return _instance;
            }
        }
        /// <summary>
        /// 使用前必须先调用Init初始化
        /// </summary>         
        public static BaseNativeProxy Init(GameObject obj)
        {
            _instance = obj.AddComponent<BaseNativeProxy>();
            _instance.CsCallNative("setDefaultCallbackGameObj", obj.name);
            return _instance;
        }
        public void CsCallNative(string funname, params object[] args)
        {
            string msg = "忽略外部调用 " + funname;
            foreach (object item in args)
            {
                msg += "\n    par:" + item.ToString();
            }
            BaseLogger.Info(msg);
        }
        public void exit()
        {
            if (!Application.isEditor)
                Application.Quit();
        }
    }
}
