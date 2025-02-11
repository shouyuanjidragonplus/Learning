using UnityEditor;
using UnityEngine;

namespace UGF.EditorTools
{
    /// <summary>
    /// 资源生成器。
    /// </summary>
    public class AppBuildEidtor : EditorWindow
    {
        public static void Open()
        {
            AppBuildEidtor window = GetWindow<AppBuildEidtor>("App Builder", true);
#if UNITY_2019_3_OR_NEWER
            window.minSize = new Vector2(800f, 800f);
#else
            window.minSize = new Vector2(800f, 750f);
#endif
        }
    }
}
