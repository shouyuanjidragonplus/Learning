using GameFramework;
using UnityEngine;

public class AssetsPath : MonoBehaviour
{
     public static string GetCombinePath(params string[] args)
        {
            return Utility.Path.GetRegularPath(System.IO.Path.Combine(args));
        }
        public static string GetDataTablePath(string name, bool useBytes)
        {
            return Utility.Text.Format("Assets/Export/DataTable/{0}.{1}", name, useBytes ? "bytes": "txt");
        }

        public static string GetSoundPath(string name)
        {
            return Utility.Text.Format("Assets/Export/Audio/{0}", name);
        }

        public static string GetScenePath(string name)
        {
            return Utility.Text.Format("Assets/Export/Scene/{0}.unity", name);
        }
        public static string GetEntityPath(string name)
        {
            return Utility.Text.Format("Assets/Export/Prefabs/Entity/{0}.prefab", name);
        }

        public static string GetUIFormPath(string v)
        {
            return Utility.Text.Format("Assets/Export/Prefabs/UI/{0}.prefab", v);
        }

        public static string GetTexturePath(string fileName)
        {
            return Utility.Text.Format("Assets/Export/Textures/{0}", fileName);
        }
        public static string GetSpritesPath(string fileName)
        {
            return Utility.Text.Format("Assets/Export/Sprites/{0}", fileName);
        }
        public static string GetConfigPath(string v, bool useBytes)
        {
            return Utility.Text.Format("Assets/Export/Config/{0}.{1}", v, useBytes ? "bytes" : "txt");
        }
        public static string GetScriptableConfigPath(string v)
        {
            return Utility.Text.Format("Assets/Export/ScriptableAssets/{0}.asset", v);
        }

        public static string GetPrefab(string v)
        {
            return Utility.Text.Format("Assets/Export/Prefabs/{0}.prefab", v);
        }

        public static string GetLanguagePath(string v, bool useBytes)
        {
            return Utility.Text.Format("Assets/Export/Language/{0}.{1}", v, useBytes ? "bytes" : "json");
        }
        public static string GetMaterialPath(string v)
        {
            return Utility.Text.Format("Assets/Export/Material/{0}.mat", v);
        }

        public static string GetHotfixDll(string dllName)
        {
            return Utility.Text.Format("Assets/Export/HotfixDlls/{0}.bytes", dllName);
        }

        public static string GetScriptableAsset(string v)
        {
            return Utility.Text.Format("Assets/Export/ScriptableAssets/{0}.asset", v);
        }
}
