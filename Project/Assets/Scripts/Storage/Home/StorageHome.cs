/************************************************
 * Storage class : StorageHome
 * This file is can not be modify !!!
 * If there is some problem, ask bin.guo.
 ************************************************/

using System;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Framework.Storage
{
    [System.Serializable]
    public class StorageHome : StorageBase
    {
         
        // 玩家身上金币数
        [JsonProperty]
        StorageDictionary<string,int> currency = new StorageDictionary<string,int>(true);
        [JsonIgnore]
        public StorageDictionary<string,int> Currency
        {
            get
            {
                return currency;
            }
        }
        // ---------------------------------//
        
        // EXP
        [JsonProperty]
        int exp;
        [JsonIgnore]
        public int Exp
        {
            get
            {
                return exp;
            }
            set
            {
                if(exp != value)
                {
                    exp = value;
                    StorageManager.Instance.LocalVersion++;
                    StorageManager.Instance.SyncForce = true;
                    
                }
            }
        }
        // ---------------------------------//
        
        // 等级
        [JsonProperty]
        int level;
        [JsonIgnore]
        public int Level
        {
            get
            {
                return level;
            }
            set
            {
                if(level != value)
                {
                    level = value;
                    StorageManager.Instance.LocalVersion++;
                    StorageManager.Instance.SyncForce = true;
                    
                }
            }
        }
        // ---------------------------------//
        
        // 第一次登陆
        [JsonProperty]
        bool isFirstLogin;
        [JsonIgnore]
        public bool IsFirstLogin
        {
            get
            {
                return isFirstLogin;
            }
            set
            {
                if(isFirstLogin != value)
                {
                    isFirstLogin = value;
                    StorageManager.Instance.LocalVersion++;
                    
                    
                }
            }
        }
        // ---------------------------------//
    }
}