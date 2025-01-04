using UnityEditor;
using UnityEngine;

public static class UnitySetHelper
{
    [MenuItem("Tools/打开存档文件夹")]
    public static void OpenSavingPath()
    {
        System.Diagnostics.Process.Start("Explorer.exe", Application.persistentDataPath.Replace("/", "\\"));
    }
    [MenuItem("Tools/清除PlayerPrefs")]
    public static void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}
