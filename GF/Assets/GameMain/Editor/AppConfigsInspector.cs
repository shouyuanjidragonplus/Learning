#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Linq;
using GameFramework;
using GameFramework.Procedure;
using UnityEditorInternal;

namespace UGF.EditorTools
{
    public class ItemData
    {
        public bool isOn;
        public string excelName { get; private set; }

        public ItemData(bool isOn, string dllName)
        {
            this.isOn = isOn;
            this.excelName = dllName;
        }
    }

    [CustomEditor(typeof(AppConfigs))]
    public class AppConfigsInspector : UnityEditor.Editor
    {
        public const int ONE_LINE_SHOW_COUNT = 3;
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
        GUIContent editorConstSettingsContent;

        GameDataScrollView[] svDataArr;

        private void OnEnable()
        {
            appConfig = target as AppConfigs;
            normalStyle = new GUIStyle();
            normalStyle.normal.textColor = Color.white;
            selectedStyle = new GUIStyle();
            selectedStyle.normal.textColor = ColorUtility.TryParseHtmlString("#2BD988", out var textCol) ? textCol : Color.green;
            editorConstSettingsContent = EditorGUIUtility.TrTextContentWithIcon("Path Settings [设置DataTable/Config导入/导出路径]", "Settings");
            procedureTitleContent = new GUIContent("流程(Procedures)", "勾选的流程在有限状态机中有效");

            loadFromBytesContent = new GUIContent("Load from bytes(勾选:二进制模式; 不勾选:文本模式)", "数据表/配置表/多语言表使用二进制模式");

            svDataArr = new GameDataScrollView[]
            {
                new GameDataScrollView(appConfig, GameDataType.Config, 20),
                new GameDataScrollView(appConfig, GameDataType.Language, 50),
                new GameDataScrollView(appConfig, GameDataType.DataTable, 100),
            };
            ReloadScrollView(appConfig);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            if (GUILayout.Button(editorConstSettingsContent))
            {
                InternalEditorUtility.OpenFileAtLineExternal(Path.Combine(Path.GetDirectoryName(ConstEditor.BuiltinAssembly), "../Editor/Common/ConstEditor.cs"), 0);
            }
            EditorGUILayout.Space(10);
            appConfig.LoadFromBytes = EditorGUILayout.ToggleLeft(loadFromBytesContent, appConfig.LoadFromBytes);
            EditorGUILayout.Space(10);
            DrawProcedure();
            EditorGUILayout.Space(10);
            DrawExcelData();
            EditorGUILayout.Space(10);
            EditorGUILayout.BeginHorizontal("box");
            {
                if (GUILayout.Button("Reload", GUILayout.Height(30)))
                {
                    ReloadScrollView(appConfig);
                }

                if (GUILayout.Button("Save", GUILayout.Height(30)))
                {
                    SaveConfig(appConfig);
                }
            }
            EditorGUILayout.EndHorizontal();
            serializedObject.ApplyModifiedProperties();
        }

        private void ReloadScrollView(AppConfigs cfg)
        {
            foreach (var item in svDataArr)
            {
                item.Reload();
            }

            ReloadProcedures(cfg);
        }

        private void DrawExcelData()
        {
            var perItemWidth = GUILayout.Width(Mathf.Max(EditorGUIUtility.currentViewWidth / ONE_LINE_SHOW_COUNT - 20, 100));
            foreach (var item in svDataArr)
            {
                if (item.DrawPanel(perItemWidth))
                {
                    SaveConfig(appConfig);
                }
            }
        }

        private void DrawProcedure()
        {
            float itemHeight = 100;
            procedureFoldout = EditorGUILayout.Foldout(procedureFoldout, procedureTitleContent);
            var perItemWidth = GUILayout.Width(Mathf.Max(EditorGUIUtility.currentViewWidth / ONE_LINE_SHOW_COUNT - 20, 100));
            if (procedureFoldout)
            {
                EditorGUILayout.BeginVertical();
                {
                    procedureScrollPos = EditorGUILayout.BeginScrollView(procedureScrollPos, "box", GUILayout.Height(itemHeight));
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
        }

        private void OnDisable()
        {
            SaveConfig(appConfig);
        }

        private void SaveConfig(AppConfigs cfg)
        {
            SaveProcedures(cfg);
            SaveExcelData(cfg);
            EditorUtility.SetDirty(cfg);
            EditorUtility.SetDirty(AppSettings.Instance);
        }

        private void SaveExcelData(AppConfigs cfg)
        {
            foreach (var svData in svDataArr)
            {
                if (svData.CfgType == GameDataType.DataTable)
                {
                    cfg.GetType().GetField("mDataTables", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                        .SetValue(cfg, svData.GetSelectedItems());
                }
                else if (svData.CfgType == GameDataType.Config)
                {
                    cfg.GetType().GetField("mConfigs", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                        .SetValue(cfg, svData.GetSelectedItems());
                }
                else if (svData.CfgType == GameDataType.Language)
                {
                    cfg.GetType().GetField("mLanguages", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                        .SetValue(cfg, svData.GetSelectedItems());
                }
            }
        }

        private void SaveProcedures(AppConfigs cfg)
        {
            List<string> selectedProcedures = new List<string>();
            foreach (var item in procedures)
            {
                if (!item.isOn) continue;
                selectedProcedures.Add(item.excelName);
            }

            cfg.GetType().GetField("mProcedures", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(cfg, selectedProcedures.ToArray());
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
}
#endif