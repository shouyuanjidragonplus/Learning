using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMain
{
    public class NativeBridge
    {
#if UNITY_IOS
        [DllImport("__Internal")]
        public static extern void copy(string text);

        [DllImport("__Internal")]
        public static extern string paste();
#endif
#if UNITY_ANDROID && !UNITY_EDITOR
        private static AndroidJavaClass aj = null;
        private static string unityPlayerClass = "com.unity3d.player.UnityPlayer";

        public static AndroidJavaObject GetActivity()
        {
            AndroidJavaClass jc = new AndroidJavaClass(unityPlayerClass);
            if (jc == null)
            {
                Debug.LogErrorFormat("Failed to get Unity Player class, {0}", unityPlayerClass);
                return null;
            }

            AndroidJavaObject activity = jc.GetStatic<AndroidJavaObject>("currentActivity");
            if (activity == null)
            {
                Debug.LogError("Failed to obtain Android Activity from Unity Player class.");
                return null;
            }

            return activity;
        }

#endif
        static NativeBridge()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            aj = new AndroidJavaClass("com.gamemain.NativeBridge");
#endif
        }

        public static void Copy(string text)
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            TextEditor textEditor = new TextEditor();
            textEditor.text = text;
            textEditor.OnFocus();
            textEditor.Copy();
#elif UNITY_IOS
            copy(text);
#elif UNITY_ANDROID
            aj.CallStatic("copy", text);
#endif
        }

        public static string Paste()
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            TextEditor textEditor = new TextEditor();
            textEditor.Paste();
            return textEditor.text;
#elif UNITY_IOS
            return paste();
#elif UNITY_ANDROID
            return aj.CallStatic<string>("paste");
#else
            return "";
#endif
        }
    }
}