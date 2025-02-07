using UnityEngine;

namespace GameMain
{
    /// <summary>
    /// 游戏入口。
    /// </summary>
    public partial class GameEntry : MonoBehaviour
    {
        public static DriverComponent Driver { get; private set; }
        public static StartComponent StartView { get; private set; }
        private static void InitCustomComponents()
        {
            Debug.Log("Game Framework Start");
            Driver = UnityGameFramework.Runtime.GameEntry.GetComponent<DriverComponent>();
            StartView = UnityGameFramework.Runtime.GameEntry.GetComponent<StartComponent>();
        }
    }
}
