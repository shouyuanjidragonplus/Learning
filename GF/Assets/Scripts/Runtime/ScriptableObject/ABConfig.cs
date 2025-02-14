using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;

[Serializable]
public class BundleState
{
    // ab包路径
    public string Path;

    // 是否在初始包里
    public bool InInitialPacket = true;
}

[Serializable]
public class BundleGroup
{
    public string GroupName;
    public int GroupIndex;
    public List<BundleState> Paths;
    public bool UpdateWholeAB;

    public List<string> GetBundlePaths()
    {
        List<string> _paths = new List<string>();
        foreach (BundleState item in Paths)
        {
            _paths.Add(item.Path);
        }

        return _paths;
    }

    public bool IsInInitialPacket(string path)
    {
        string partPath = path.Substring(0, path.Length - 3);
        foreach (BundleState item in Paths)
        {
            if (item.Path.ToLower() == partPath)
                return item.InInitialPacket;
        }

        return false;
    }

    public List<string> GetInInitialPacketPaths()
    {
        List<string> _paths = new List<string>();
        foreach (BundleState item in Paths)
        {
            if (item.InInitialPacket)
            {
                _paths.Add(item.Path.ToLower());
            }
        }

        return _paths;
    }
}

[CreateAssetMenu(fileName = "ABConfig", menuName = "ScriptableObject/ABConfig【配置AB包】")]
public class ABConfig : ScriptableObject
{
    private const string configPath = "Core/ABConfig";
    private static ABConfig mInstance = null;
    public BundleGroup[] Groups;

    private void Awake()
    {
        mInstance = this;
    }

#if UNITY_EDITOR
    /// <summary>
    /// 编辑器下获取实例
    /// </summary>
    /// <returns></returns>
    public static ABConfig GetInstanceEditor()
    {
        if (mInstance == null)
        {
            var configAsset = AssetsPath.GetScriptableAsset(configPath);
            mInstance = AssetDatabase.LoadAssetAtPath<ABConfig>(configAsset);
        }

        return mInstance;
    }
#endif
    /// <summary>
    /// 运行时获取实例
    /// </summary>
    /// <returns></returns>
    public static async UniTask<ABConfig> GetInstanceSync()
    {
        var configAsset = AssetsPath.GetScriptableAsset(configPath);
        if (mInstance == null)
        {
            mInstance = await GF.Resource.LoadAssetAwait<ABConfig>(configAsset);
        }

        return mInstance;
    }
}