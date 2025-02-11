#if UNITY_EDITOR
using System.IO;
using UnityEngine;

/// <summary>
/// 默认编辑器配置项
/// </summary>
public class ConstEditor
{
    public const bool AutoScriptUTF8 = true; //新建脚本时自动修改脚本编码方式为utf-8以支持中文
    public const string UIViewScriptFile = "Assets/Scripts/Runtime/UI/Core/UIViews.cs";
    public const string UISerializeFieldDir = "Assets/Export/Scripts/UI/UIVariables"; //生成UI变量代码目录
    public const string UIItemSerializeFiledDir = "Assets/Export/Scripts/UI/UIItemVariables";
    public const string UITableExcel = "Core/UITable.xlsx";
    public static string UITableExcelFullPath => AssetsPath.GetCombinePath(DataTableExcelPath, UITableExcel);

    public const string EntityGroupTableExcel = "Core/EntityGroupTable.xlsx";
    public static string EntityGroupTableExcelFullPath => AssetsPath.GetCombinePath(DataTableExcelPath, EntityGroupTableExcel);

    public const string SoundGroupTableExcel = "Core/SoundGroupTable.xlsx";
    public static string SoundGroupTableExcelFullPath => AssetsPath.GetCombinePath(DataTableExcelPath, SoundGroupTableExcel);

    public const string UIGroupTableExcel = "Core/UIGroupTable.xlsx";
    public static string UIGroupTableExcelFullPath => AssetsPath.GetCombinePath(DataTableExcelPath, UIGroupTableExcel);

    public const string ConstGroupScriptFileFullName = "Assets/Export/Scripts/Common/Core/Const.Groups.cs";

    public static readonly string PrefabsPath = "Assets/Export/Prefabs";
    public static readonly string ScenePath = "Assets/Export/Scene";

    public const string DataTableCodeTemplate = "Assets/GameMain/Editor/DataTableGenerator/DataTableCodeTemplate/DataTableCodeTemplate.txt"; //生成配置表代码的模板文件
    public const string BuiltinAssembly = "Assets/GameMain/Runtime/GameMain.Runtime.asmdef";
    public const string HotfixAssembly = "Assets/Scripts/Runtime/HotUpdate.asmdef";


    public const string SharedAssetBundleName = "SharedAssets"; //AssetBundle分包共用资源
    public static readonly string[] DefaultLayers = { "UI" };
    internal static readonly string KeystorePass = "topgames";
    internal static readonly string KeyAliasPass = "topgames";
    internal static string KeystoreName => AssetsPath.GetCombinePath(Directory.GetParent(Application.dataPath).FullName, "user.keystore");
    internal static readonly string KeyAliasName = "release";
    internal static string AssetBundleOutputPath => AssetsPath.GetCombinePath(Directory.GetParent(Application.dataPath).FullName, "AB");
    public static readonly string UpdatePrefixUri = "https://gitcode.net/topgamesopen/gf_hybridclr_hotfix/-/raw/master"; //默认资源下载地址
    internal static readonly string AppUpdateUrl = "https://play.google.com/store/apps/details?id=";

    /// <summary>
    /// 数据表Excel目录
    /// </summary>
    public static string DataTableExcelPath => AssetsPath.GetCombinePath(Directory.GetParent(Application.dataPath).FullName, "ExcelData/DataTables");

    /// <summary>
    /// 配置表Excel目录
    /// </summary>
    public static string ConfigExcelPath => AssetsPath.GetCombinePath(Directory.GetParent(Application.dataPath).FullName, "ExcelData/Configs");

    /// <summary>
    /// 语言国际化Excel目录
    /// </summary>
    public static string LanguageExcelPath => AssetsPath.GetCombinePath(Directory.GetParent(Application.dataPath).FullName, "Export/Languages");

    public static string ToolsPath = AssetsPath.GetCombinePath(Directory.GetParent(Application.dataPath).FullName, "Tools");
    public const string DataTablePath = "Assets/Export/DataTable";
    public const string GameConfigPath = "Assets/Export/Config";
    public const string LanguagePath = "Assets/Export/Language";
    public const string DataTableCodePath = "Assets/Scripts/Runtime/DataTable";
    public const string UIFormTemplate = "Assets/Export/ScriptsBuiltin/Editor/UI/Templates/UIFormTemplate.prefab";
    public const string UIDialogTemplate = "Assets/Export/ScriptsBuiltin/Editor/UI/Templates/UIDialogTemplate.prefab";
}
#endif