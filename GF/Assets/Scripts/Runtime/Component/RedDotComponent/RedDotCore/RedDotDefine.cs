using System.Collections.Generic;

/// <summary>
/// 红点路径定义
/// </summary>
public static class RedDotDefine
{
    public const char split = '/';

    /// <summary>
    /// 红点树的根节点
    /// </summary>
    public const string rdRoot = "Root";

    // ---------- 业务红点 ----------
    public const string MailBox = "Root/Mail";
    public const string MailBox_System = "Root/Mail/System";
    public const string MailBox_Team = "Root/Mail/Team";

    /// <summary>
    /// 红点路径的表（每次 E_RedDotDefine 添加完后此处也必须添加）
    /// </summary>
    public static List<string> lstRedDotTreeList = new List<string>
    {
        rdRoot,

        MailBox,
        MailBox_System,
        MailBox_Team,
    };
}