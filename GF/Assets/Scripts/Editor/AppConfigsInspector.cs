#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
using GameFramework;
using GameFramework.Procedure;

[CustomEditor(typeof(AppConfigs))]
public class AppConfigsInspector : UnityEditor.Editor
{
    public const int ONE_LINE_SHOW_COUNT = 3;

    private class ItemData
    {
        public bool isOn;
        public string excelName { get; private set; }

        public ItemData(bool isOn, string dllName)
        {
            this.isOn = isOn;
            this.excelName = dllName;
        }
    }

    AppConfigs appConfig;
    bool procedureFoldout = true;
    Vector2 procedureScrollPos;
    ItemData[] procedures;
    private GUIStyle normalStyle;
    private GUIStyle selectedStyle;
    GUIContent procedureTitleContent;

    GUIContent loadFromBytesContent;
    GUIContent designResolutionContent;
    GUIContent designResolutionBtnContent;

    private void OnEnable()
    {
        appConfig = target as AppConfigs;
        normalStyle = new GUIStyle();
        normalStyle.normal.textColor = Color.white;
        selectedStyle = new GUIStyle();
        selectedStyle.normal.textColor = ColorUtility.TryParseHtmlString("#2BD988", out var textCol) ? textCol : Color.green;

        procedureTitleContent = new GUIContent("流程(Procedures)", "勾选的流程在有限状态机中有效");
        ReloadProcedures(appConfig);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        procedureFoldout = EditorGUILayout.Foldout(procedureFoldout, procedureTitleContent);
        var perItemWidth = GUILayout.Width(Mathf.Max(EditorGUIUtility.currentViewWidth / ONE_LINE_SHOW_COUNT - 20, 100));
        if (procedureFoldout)
        {
            EditorGUILayout.BeginVertical();
            {
                procedureScrollPos = EditorGUILayout.BeginScrollView(procedureScrollPos, "box", GUILayout.Height(200));
                {
                    EditorGUI.BeginChangeCheck();
                    for (int i = 0; i < procedures.Length; i++)
                    {
                        if (i % ONE_LINE_SHOW_COUNT == 0)
                            EditorGUILayout.BeginHorizontal();
                        var item = procedures[i];
                        item.isOn = EditorGUILayout.ToggleLeft(item.excelName, item.isOn, item.isOn ? selectedStyle : normalStyle, perItemWidth);
                        if (i % ONE_LINE_SHOW_COUNT == ONE_LINE_SHOW_COUNT - 1)
                            EditorGUILayout.EndHorizontal();
                    }
                    if (EditorGUI.EndChangeCheck())
                    {
                        SaveConfig(appConfig);
                    }
                    if (procedures.Length % ONE_LINE_SHOW_COUNT != 0)
                        EditorGUILayout.EndHorizontal();
                    EditorGUILayout.EndScrollView();
                }
                EditorGUILayout.EndVertical();
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
    private void OnDisable()
    {
        SaveConfig(appConfig);
    }
    private void SaveConfig(AppConfigs cfg)
    {
        List<string> selectedProcedures = new List<string>();
        foreach (var item in procedures)
        {
            if (!item.isOn) continue;
            selectedProcedures.Add(item.excelName);
        }
        cfg.GetType().GetField("mProcedures", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(cfg, selectedProcedures.ToArray());
        EditorUtility.SetDirty(cfg);
        EditorUtility.SetDirty(AppSettings.Instance);
    }
    private void ReloadProcedures(AppConfigs cfg)
    {
        procedures ??= new ItemData[0];
        ArrayUtility.Clear(ref procedures);
        var hotfixDlls = Utility.Assembly.GetAssemblies().Where(dll =>
            HybridCLR.Editor.SettingsUtil.HotUpdateAssemblyNamesIncludePreserved.Contains(dll.GetName().Name)).ToArray();

        foreach (var item in hotfixDlls)
        {
            var proceClassArr = item.GetTypes().Where(tp => tp.BaseType == typeof(ProcedureBase)).ToArray();
            foreach (var proceClass in proceClassArr)
            {
                var proceName = proceClass.FullName;
                ArrayUtility.Add(ref procedures, new ItemData(cfg.Procedures.Contains(proceName), proceName));
            }
        }
    }
}

#endif