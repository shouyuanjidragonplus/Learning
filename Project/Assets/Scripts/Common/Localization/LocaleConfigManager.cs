using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using DragonU3DSDK;
using Framework.Asset;
using Newtonsoft.Json;
using UnityEngine;

namespace Framework
{
    public class LocaleConfigManager : Manager<LocaleConfigManager>
    {
        Dictionary<string, Dictionary<string, string>> localeConfigs = new Dictionary<string, Dictionary<string, string>>();

        // 备份版多语言，从app里读取的
        Dictionary<string, Dictionary<string, string>> bakLocaleConfigs = new Dictionary<string, Dictionary<string, string>>();

        private bool loadedFromApp = false;
        string sep = " = ";

        public bool IsInit
        {
            get { return localeConfigs.Count > 0; }
        }

        /// <summary>
        /// 读取一份本地的多语言
        /// </summary>
        public void LoadBakLocale()
        {
            if (loadedFromApp)
            {
                return;
            }

            foreach (string locale in LocalizationManager.Instance.supportedLocale)
            {
                var configPath = "Export/Configs/LocaleConfig/locale_loading_" + locale;
                var ta = Resources.Load<TextAsset>(configPath);
                string content = ta.text;

                List<KVConfig> localeConfig = JsonConvert.DeserializeObject<List<KVConfig>>(content);
                if (localeConfig != null && localeConfig.Count > 0)
                {
                    Dictionary<string, string> configs = new Dictionary<string, string>();
                    foreach (KVConfig c in localeConfig)
                    {
                        configs[c.Key] = c.Value;
                    }

                    bakLocaleConfigs[locale] = configs;
                }

                Resources.UnloadAsset(ta);
            }

            loadedFromApp = true;
        }

        public void InitConfigs(bool isClear = false)
        {
            if (isClear)
                localeConfigs.Clear();

            foreach (string locale in LocalizationManager.Instance.supportedLocale)
            {
                if (localeConfigs.ContainsKey(locale))
                    continue;

                var configPath = "Configs/LocaleConfig/locale_" + locale;
                var ta = ResourcesManager.Instance.LoadResource<TextAsset>(configPath, addToCache: false);
                string content = ta.text;
                List<KVConfig> localeConfig = JsonConvert.DeserializeObject<List<KVConfig>>(content);
                Debug.Log("多语言配置 count : " + localeConfig.Count);
                if (localeConfig != null && localeConfig.Count > 0)
                {
                    Dictionary<string, string> configs = new Dictionary<string, string>();
                    foreach (KVConfig c in localeConfig)
                    {
                        if (string.IsNullOrEmpty(c.Key))
                        {
                            Debug.LogError("key is " + c.Key + "  value is " + c.Value + "\t local " + locale);
                            continue;
                        }

                        //DebugUtil.Log("key : "+c.Key);
                        if (configs.ContainsKey(c.Key))
                        {
                            configs[c.Key] = c.Value;
                        }
                        else
                        {
                            configs.Add(c.Key, c.Value);
                        }
                    }

                    localeConfigs[locale] = configs;
                }

                ResourcesManager.Instance.ReleaseRes(configPath, true);
            }
        }

        // 对嵌套的key进行处理
        string ReplaceKeyVlaue(Dictionary<string, Dictionary<string, string>> tempLocaleConfigs, string locale,
            string key)
        {
            string value = tempLocaleConfigs[locale][key];
            if (string.IsNullOrEmpty(value))
                return "";

            MatchCollection Matches = Regex.Matches(value, "%{(.+?)}");
            foreach (Match match in Matches)
            {
                TryGetLocalString(match.Groups[1].Value, locale, out var result);
                value = value.Replace("%{" + match.Groups[1].Value + "}", result);
            }

            return value;
        }

        public bool TryGetLocalString(string key, string locale, out string result)
        {
            var replaceLocalConfig = ReplaceLocalConfigManager.Instance.GetLocaleConfigs();
            if (replaceLocalConfig.ContainsKey(locale)
                && replaceLocalConfig[locale].ContainsKey(key)) //是否有热更的内容
            {
                result = ReplaceKeyVlaue(replaceLocalConfig, locale, key);
                return true;
            }

            if (localeConfigs.ContainsKey(locale) && localeConfigs[locale].ContainsKey(key)) //从远端多语言里get到了
            {
                result = ReplaceKeyVlaue(localeConfigs, locale, key);
                return true;
            }

            if (bakLocaleConfigs.ContainsKey(locale) && bakLocaleConfigs[locale].ContainsKey(key)) //从备份版多语言里get到了
            {
                result = ReplaceKeyVlaue(bakLocaleConfigs, locale, key);
                return true;
            }

            result = key;
            return false;
        }

        public string AllConfigs(int index, int total)
        {
            if (index < 1) index = 1;
            if (index > total) index = total;
            var locale = LanguageModel.Instance.GetLocale();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            foreach (var localeConfig in localeConfigs)
            {
                if (localeConfig.Key == locale)
                {
                    var dict = localeConfig.Value;
                    var firstHalf = new Dictionary<string, string>();
                    int start = dict.Count * (index - 1) / total;
                    int end = dict.Count * index / total;
                    int count = 0;
                    foreach (var item in dict)
                    {
                        count++;
                        if (count >= start && count <= end)
                        {
                            firstHalf.Add(item.Key, item.Value);
                        }
                    }

                    foreach (var item in firstHalf)
                    {
                        sb.Append(item.Key)
                            .Append(sep)
                            .Append(item.Value)
                            .AppendLine();
                    }

                    return $"<start>{sb}</end>";
                }
            }

            return $"<start>{sb}</end>";
        }

        public string match(string str)
        {
            Regex regex = new Regex("([\\u4e00-\\u9fa5]{1,})");
            return regex.Replace(str, $"<color=green>{str}</color>");
        }
    }
}