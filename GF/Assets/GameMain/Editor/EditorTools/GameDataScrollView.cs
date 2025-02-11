using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;


namespace UGF.EditorTools
{
    [Flags]
    public enum GameDataType
    {
        Config = 1,
        Language = 2,
        DataTable = 4,
    }

    public class GameDataScrollView
    {
        public const int ONE_LINE_SHOW_COUNT = 3;
        public bool foldout = true;
        public GameDataType CfgType { get; private set; }
        public Vector2 scrollPos; //记录滚动列表位置
        public string excelDir;
        public string excelOuputDir;
        private string newExcelName;
        private AppConfigs appConfig;
        public List<ItemData> ExcelItems { get; private set; }
        private GUIStyle normalStyle;
        private GUIStyle selectedStyle;
        GUIContent titleContent;
        private float itemHeight;

        public GameDataScrollView(AppConfigs cfg, GameDataType configTp, float itemHeight = 200)
        {
            normalStyle = new GUIStyle();
            normalStyle.normal.textColor = Color.white;
            selectedStyle = new GUIStyle();
            selectedStyle.normal.textColor = ColorUtility.TryParseHtmlString("#2BD988", out var textCol) ? textCol : Color.green;
            this.CfgType = configTp;
            this.excelDir = GameDataGenerator.GetGameDataExcelDir(configTp);
            this.excelOuputDir = GameDataGenerator.GetGameDataExcelOutputDir(configTp);
            this.appConfig = cfg;
            this.itemHeight = itemHeight;
            titleContent = new GUIContent(configTp.ToString());
            switch (configTp)
            {
                case GameDataType.DataTable:
                    titleContent.tooltip = "选择项目需要用到的数据表";
                    break;
                case GameDataType.Config:
                    titleContent.tooltip = "选择项目需要用到的常量配置表";
                    break;
                case GameDataType.Language:
                    titleContent.tooltip = "选择项目需要用到的多语言表";
                    break;
                default:
                    break;
            }
        }

        public void Reload()
        {
            if (!Directory.Exists(excelDir) || appConfig == null) return;

            var mainExcels = GameDataGenerator.GetAllGameDataExcels(this.CfgType, GameDataExcelFileType.MainFile);

            if (ExcelItems == null) ExcelItems = new List<ItemData>();
            else ExcelItems.Clear();

            string[] desArr = GetGameDataList();
            if (desArr != null)
            {
                foreach (var mainExcelFile in mainExcels)
                {
                    var mainExcelRelativePath = GameDataGenerator.GetGameDataExcelRelativePath(this.CfgType, mainExcelFile);
                    var isOn = ArrayUtility.Contains(desArr, mainExcelRelativePath);
                    ExcelItems.Add(new ItemData(isOn, mainExcelRelativePath));
                }
            }
        }

        public string[] GetSelectedItems()
        {
            var selectedList = ExcelItems.Where(dt => dt.isOn).ToArray();
            string[] resultArr = new string[selectedList.Length];
            for (int i = 0; i < selectedList.Length; i++)
            {
                resultArr[i] = selectedList[i].excelName;
            }

            return resultArr;
        }

        internal void SetSelectAll(bool v)
        {
            foreach (var item in ExcelItems)
            {
                item.isOn = v;
            }
        }

        internal bool DrawPanel(GUILayoutOption perItemWidth)
        {
            bool dataChanged = false;
            var dataTypeStr = this.CfgType.ToString();
            this.foldout = EditorGUILayout.Foldout(this.foldout, titleContent);
            if (foldout)
            {
                EditorGUILayout.BeginVertical();
                {
                    scrollPos = EditorGUILayout.BeginScrollView(scrollPos, "box", GUILayout.MaxHeight(itemHeight));
                    {
                        EditorGUI.BeginChangeCheck();
                        for (int i = 0; i < ExcelItems.Count; i++)
                        {
                            if (i % ONE_LINE_SHOW_COUNT == 0)
                                EditorGUILayout.BeginHorizontal();
                            var item = ExcelItems[i];
                            item.isOn = EditorGUILayout.ToggleLeft(item.excelName, item.isOn, item.isOn ? selectedStyle : normalStyle, perItemWidth);
                            if (i % ONE_LINE_SHOW_COUNT == ONE_LINE_SHOW_COUNT - 1)
                                EditorGUILayout.EndHorizontal();
                        }

                        if (EditorGUI.EndChangeCheck())
                        {
                            dataChanged = true;
                        }

                        if (ExcelItems.Count % ONE_LINE_SHOW_COUNT != 0)
                            EditorGUILayout.EndHorizontal();
                        EditorGUILayout.EndScrollView();
                    }
                    EditorGUILayout.BeginHorizontal("box");
                    {
                        if (GUILayout.Button("All", GUILayout.Width(50)))
                        {
                            SetSelectAll(true);
                            dataChanged = true;
                        }

                        if (GUILayout.Button("None", GUILayout.Width(50)))
                        {
                            SetSelectAll(false);
                            dataChanged = true;
                        }

                        GUILayout.FlexibleSpace();

                        if (GUILayout.Button("Reveal", GUILayout.Width(70)))
                        {
                            EditorUtility.RevealInFinder(excelDir);
                            GUIUtility.ExitGUI();
                        }

                        if (GUILayout.Button("Export", GUILayout.Width(70)))
                        {
                            Export();
                        }

                        EditorGUILayout.EndHorizontal();
                    }
                    if (this.CfgType == GameDataType.DataTable || this.CfgType == GameDataType.Config)
                    {
                        EditorGUILayout.BeginHorizontal("box");
                        {
                            newExcelName = EditorGUILayout.TextField(newExcelName);
                            if (GUILayout.Button($"New {dataTypeStr}", GUILayout.Width(100)))
                            {
                                CreateExcel(newExcelName);
                            }

                            EditorGUILayout.EndHorizontal();
                        }
                    }

                    EditorGUILayout.EndVertical();
                }
            }

            return dataChanged;
        }

        private void Export()
        {
            switch (this.CfgType)
            {
                case GameDataType.DataTable:
                    GameDataGenerator.RefreshAllDataTable(GameDataGenerator.GameDataExcelRelative2FullPath(CfgType, GetGameDataList()));
                    GameDataGenerator.GenerateUIViewScript();
                    break;
                case GameDataType.Config:
                    GameDataGenerator.RefreshAllConfig(GameDataGenerator.GameDataExcelRelative2FullPath(CfgType, GetGameDataList()));
                    break;
                case GameDataType.Language:
                    GameDataGenerator.RefreshAllLanguage(GameDataGenerator.GameDataExcelRelative2FullPath(CfgType, GetGameDataList()));
                    break;
            }
        }

        private string[] GetGameDataList()
        {
            switch (this.CfgType)
            {
                case GameDataType.DataTable:
                    return appConfig.DataTables;
                case GameDataType.Config:
                    return appConfig.Configs;
                case GameDataType.Language:
                    return appConfig.Languages;
                default:
                    return null;
            }
        }

        private void CreateExcel(string newExcelName)
        {
            switch (this.CfgType)
            {
                case GameDataType.DataTable:
                    CreateDataTableExcel(newExcelName);
                    break;
                case GameDataType.Config:
                    CreateConfigExcel(newExcelName);
                    break;
            }
        }

        private void CreateDataTableExcel(string v)
        {
            if (string.IsNullOrWhiteSpace(v))
            {
                return;
            }

            var excelPath = AssetsPath.GetCombinePath(excelDir, v + ".xlsx");
            if (File.Exists(excelPath))
            {
                Debug.LogWarning($"创建DataTable失败, 文件已存在:{excelPath}");
                return;
            }

            if (GameDataGenerator.CreateDataTableExcel(excelPath))
            {
                Reload();
                EditorUtility.RevealInFinder(excelPath);
                GUIUtility.ExitGUI();
            }
        }

        private void CreateConfigExcel(string v)
        {
            if (string.IsNullOrWhiteSpace(v))
            {
                return;
            }

            var excelPath = AssetsPath.GetCombinePath(excelDir, v + ".xlsx");
            if (File.Exists(excelPath))
            {
                Debug.LogWarning($"创建Config失败, 文件已存在:{excelPath}");
                return;
            }

            if (GameDataGenerator.CreateGameConfigExcel(excelPath))
            {
                Reload();
                EditorUtility.RevealInFinder(excelPath);
                GUIUtility.ExitGUI();
            }
        }
    }
}