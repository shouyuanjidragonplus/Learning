using System.Collections.Generic;
using UnityEngine;

namespace Framework.Asset
{
    [System.Serializable]
    public class VersionInfo
    {
        public string Version;

        public Dictionary<string, VersionItemInfo> ResGroups;

        public string UniqueID;

        public string UnityVersion;

        public List<ResPublicLibraryCommit> ResPublicLibraryCommits;

        public VersionInfo()
        {
            ResGroups = new Dictionary<string, VersionItemInfo>();
            ResPublicLibraryCommits = new List<ResPublicLibraryCommit>();
        }

        public VersionInfo Clone()
        {
            var versionInfo = new VersionInfo();
            versionInfo.Version = Version;
            versionInfo.UniqueID = UniqueID;
            foreach (var key in ResGroups.Keys)
            {
                versionInfo.Add(key, ResGroups[key]);
            }

            versionInfo.UnityVersion = UnityVersion;
            foreach (var p in ResPublicLibraryCommits)
            {
                versionInfo.ResPublicLibraryCommits.Add(p);
            }

            return versionInfo;
        }

        public void Add(string key, VersionItemInfo value)
        {
            if (this.ResGroups.ContainsKey(key))
            {
                Debug.Log("资源组重名:" + key);
            }
            else
            {
                this.ResGroups.Add(key, value);
            }
        }

        public void Refresh(string key, VersionItemInfo value)
        {
            VersionItemInfo versionItemInfo = GetVersionItemInfo(key);
            if (null == versionItemInfo)
            {
                Add(key, value);
            }
            else
            {
                foreach (var p in value.AssetBundles)
                {
                    versionItemInfo.Refresh(p.Key, p.Value);
                }
            }
        }

        public List<string> GetAssetBundlesByKey(string key)
        {
            List<string> names = new List<string>();
            if (ResGroups.ContainsKey(key))
            {
                int count = ResGroups[key].AssetBundles.Count;
                foreach (string name in ResGroups[key].AssetBundles.Keys)
                {
                    names.Add(name);
                }
            }

            return names;
        }

        public string GetAssetBundleByKeyAndName(string key, string name)
        {
            string abname = string.Empty;


            if (ResGroups.ContainsKey(key) && !string.IsNullOrEmpty(name))
            {
                if (ResGroups[key].AssetBundles.ContainsKey(name))
                {
                    abname = ResGroups[key].AssetBundles[name].AssetBundleName;
                }
            }

            return abname;
        }

        public string GetAssetBundleHash(string key, string name)
        {
            string hash = "";
            if (ResGroups.ContainsKey(key) && !string.IsNullOrEmpty(name))
            {
                if (ResGroups[key].AssetBundles.ContainsKey(name))
                {
                    hash = ResGroups[key].AssetBundles[name].HashString;
                }
                else
                {
                    Debug.LogError("GetAssetBundleHash error2:" + key + "->" + name);
                }
            }
            else
            {
                Debug.LogError("GetAssetBundleHash error:" + key + "->" + name);
            }

            return hash;
        }

        public string GetAssetBundleMd5(string key, string name)
        {
            string md5 = "";
            if (ResGroups.ContainsKey(key) && !string.IsNullOrEmpty(name))
            {
                if (ResGroups[key].AssetBundles.ContainsKey(name))
                {
                    md5 = ResGroups[key].AssetBundles[name].Md5;
                }
                else
                {
                    Debug.LogError("GetAssetBundleMd5 error2:" + key + "->" + name);
                }
            }
            else
            {
                Debug.LogError("GetAssetBundleMd5 error:" + key + "->" + name);
            }

            return md5;
        }

        /*
        public AssetState GetAssetBundleState(string key, string name)
        {
            if (ResGroups.ContainsKey(key) && !string.IsNullOrEmpty(name))
            {
                if (ResGroups[key].AssetBundles.ContainsKey(name))
                {
                    return ResGroups[key].AssetBundles[name].State;
                }
                else
                {
                    DebugUtil.LogError("GetAssetBundleState error2:" + key + "->" + name);
                    return AssetState.NotExist;
                }
            }
            else
            {
                DebugUtil.LogError("GetAssetBundleState error:" + key + "->" + name);
                return AssetState.NotExist;
            }
        }
        */

        public bool GetAssetBundleFromOutside(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                foreach (var group in ResGroups)
                {
                    if (group.Value.AssetBundles.ContainsKey(name))
                    {
                        return group.Value.AssetBundles[name].FromOutside;
                    }
                }
            }

            //DebugUtil.LogError(string.Format("can not find : {0} in GetAssetBundleFromOutside", name));
            return false;
        }

        public string GetAssetBundleNameOutside(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                foreach (var group in ResGroups)
                {
                    if (group.Value.AssetBundles.ContainsKey(name))
                    {
                        return group.Value.AssetBundles[name].AssetBundleNameOutside;
                    }
                }
            }

            Debug.LogError(string.Format("can not find : {0} in GetAssetBundleNameOutside", name));
            return "";
        }

        public VersionItemInfo GetVersionItemInfo(string key)
        {
            VersionItemInfo temp;
            if (ResGroups.TryGetValue(key, out temp))
            {
                return temp;
            }

            return null;
        }
    }
}