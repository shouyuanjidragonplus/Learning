//*************************************************//
/*
 * Example
 * 必须且只能初始化一次
 * StorageManager.Instance.Init(new List<StorageBase>());
 *
 * StorageCook cook = StorageManager.Instance.GetStorage<StorageCook>();
 * cook.Coins["island_1"] = 200;
 */
//*************************************************//

namespace Framework.Storage
{
    public class StorageManager : Manager<StorageManager>
    {
        // 本地存档版本
        private ulong _localVersion;

        public ulong LocalVersion
        {
            get { return _localVersion; }
            set { _localVersion = value; }
        }

        // 强制同步，用于存储敏感数据
        public bool SyncForce { get; set; }
        public bool SyncForceRemote { get; set; }
    }
}