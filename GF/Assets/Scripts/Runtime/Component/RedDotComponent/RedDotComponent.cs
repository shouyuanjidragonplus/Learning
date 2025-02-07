using RedDot;
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 红点系统
/// </summary>
[DisallowMultipleComponent]
[AddComponentMenu("Game Framework/RedDotComponent")]
public class RedDotComponent : GameFrameworkComponent
{
    private RedDotSystem m_RedDotManager;

    /// <summary>
    /// 红点管理器
    /// </summary>
    public RedDotSystem RedDotManager
    {
        get
        {
            if (m_RedDotManager == null)
                m_RedDotManager = new RedDotSystem();
            return m_RedDotManager;
        }
    }
}