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

using System.Collections.Generic;
using UnityEngine;

namespace Framework.Storage
{
    public class StorageManager : MonoSingleton<StorageManager>
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

        static Dictionary<System.Type, string> gType2Name = new Dictionary<System.Type, string>();
        Dictionary<string, StorageBase> storageMap;

        public T GetStorage<T>() where T : StorageBase
        {
            System.Type storage_type = typeof(T);
            if (!gType2Name.TryGetValue(storage_type, out string name))
            {
                name = storage_type.Name;
                gType2Name[storage_type] = name;
            }

            return (T)storageMap[name];
        }

        public bool Inited { get; private set; }

        public void Init(List<StorageBase> storages)
        {
            if (!Inited)
            {
                storageMap = new Dictionary<string, StorageBase>();
                foreach (var storage in storages)
                {
                    var type = storage.GetType().Name;
                    storageMap[type] = storage;
                }

                Inited = true;
                //读取本地存档
                //ReadFromLocal();
            }
            else
            {
                Debug.Assert(false, "Error Init Storage !!!");
            }
        }
        public void SaveToLocal()
        { 
        }
    }
}