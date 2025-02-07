using GameFramework.Resource;
using UnityEngine;

public enum VersionStatus
{
    RELEASE = 0,
    DEBUG = 1,
}
[CreateAssetMenu(fileName = "AppSettings", menuName = "ScriptableObject/AppSettings【App内置配置参数】")]
public class AppSettings : ScriptableObject
{
    private static AppSettings mInstance = null;
    public static AppSettings Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = Resources.Load<AppSettings>("Settings/AppSettings");
            }
            return mInstance;
        }
    }
    public VersionStatus version = VersionStatus.DEBUG;
    [Space(5)]
    [Header("[True:使用AB包  False:使用编辑器里资源  真机下应为True]")]
    public bool LoadAB = false;
}
