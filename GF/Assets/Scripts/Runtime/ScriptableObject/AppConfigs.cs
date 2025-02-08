using UnityEngine;
using System.Threading.Tasks;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "AppConfigs", menuName = "ScriptableObject/AppConfigs【配置App运行时流程】")]
public class AppConfigs : ScriptableObject
{
    private static AppConfigs mInstance = null;

    [SerializeField] bool m_LoadFromBytes = false;

    public bool LoadFromBytes
    {
        get => m_LoadFromBytes;
        set => m_LoadFromBytes = value;
    }

    [Header("已启用流程列表")] [SerializeField] string[] mProcedures;

    public string[] Procedures => mProcedures;

    private void Awake()
    {
        mInstance = this;
    }

#if UNITY_EDITOR
    /// <summary>
    /// 编辑器下获取实例
    /// </summary>
    /// <returns></returns>
    public static AppConfigs GetInstanceEditor()
    {
        if (mInstance == null)
        {
            var configAsset = AssetsPath.GetScriptableAsset("Core/AppConfigs");
            mInstance = AssetDatabase.LoadAssetAtPath<AppConfigs>(configAsset);
        }

        return mInstance;
    }
#endif
    /// <summary>
    /// 运行时获取实例
    /// </summary>
    /// <returns></returns>
    public static async Task<AppConfigs> GetInstanceSync()
    {
        var configAsset = AssetsPath.GetScriptableAsset("Core/AppConfigs");
        if (mInstance == null)
        {
            mInstance = await GF.Resource.LoadAssetAwait<AppConfigs>(configAsset);
        }

        return mInstance;
    }
}