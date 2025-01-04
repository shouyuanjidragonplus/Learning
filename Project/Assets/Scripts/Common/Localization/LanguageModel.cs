/*
 * @file LanguageModel
 * 设置 - 语言
 * @author lu
 */

using System.Collections.Generic;
using Framework.Storage;
using Framework;

public class Language
{
    public string LKey { get; set; }

    public string DisStr { get; set; }

    // 是否已经开放这个语言,具体设置在  LocaleConfigManager.Instance.supportedLocale
    public bool IsEnabled { get; set; }
}

public class LanguageModel : MonoSingleton<LanguageModel>
{
    StorageCommon storageCommon
    {
        get { return StorageManager.Instance.GetStorage<StorageCommon>(); }
    }

    public void Init()
    {
    }


    public void SetLocale(string language)
    {
        storageCommon.Locale = language;
    }

    // 当前语言
    public string GetLocale()
    {
        if (storageCommon != null)
        {
            if (string.IsNullOrEmpty(storageCommon.Locale))
            {
                storageCommon.Locale = LocalType.ENGLISH;
                storageCommon.OrigLocale = LocalType.ENGLISH;
            }

            return storageCommon.Locale;
        }

        return LocalType.ENGLISH;
    }

    // 原语言
    public string GetOrigLLocale()
    {
        if (string.IsNullOrEmpty(storageCommon.OrigLocale))
        {
            storageCommon.OrigLocale = LocalType.ENGLISH;
        }

        return storageCommon.OrigLocale;
    }

    public Dictionary<string, Language> GetCfg()
    {
        var languageCfg = new Dictionary<string, Language>();
        languageCfg[LocalType.ENGLISH] = new Language {LKey = LocalType.ENGLISH, DisStr = "English"};
        languageCfg[LocalType.FRENCH] = new Language {LKey = LocalType.FRENCH, DisStr = "Français"};
        languageCfg[LocalType.GERMAN] = new Language {LKey = LocalType.GERMAN, DisStr = "Deutsch"};
        languageCfg[LocalType.PORTUGUESE] = new Language {LKey = LocalType.PORTUGUESE, DisStr = "Português"};
        languageCfg[LocalType.SPANISH] = new Language {LKey = LocalType.SPANISH, DisStr = "Español"};
        languageCfg[LocalType.ITALIAN] = new Language {LKey = LocalType.ITALIAN, DisStr = "Italiano"};
        languageCfg[LocalType.INDONESIAN] = new Language {LKey = LocalType.INDONESIAN, DisStr = "Bahasa Indonesia"};
        languageCfg[LocalType.RUSSIAN] = new Language {LKey = LocalType.RUSSIAN, DisStr = "Русский"};
        languageCfg[LocalType.VIETNAMESE] = new Language {LKey = LocalType.VIETNAMESE, DisStr = "Tiếng Việt"};
        languageCfg[LocalType.TURKISH] = new Language {LKey = LocalType.TURKISH, DisStr = "Türkçe"};
        languageCfg[LocalType.THAI] = new Language {LKey = LocalType.THAI, DisStr = "ภาษาไทย"};
        languageCfg[LocalType.JAPANESE] = new Language {LKey = LocalType.JAPANESE, DisStr = "日本語"};
        languageCfg[LocalType.KOREA] = new Language {LKey = LocalType.KOREA, DisStr = "한국어"};
        languageCfg[LocalType.CHINESE_SIMPLIFIED] = new Language {LKey = LocalType.CHINESE_SIMPLIFIED, DisStr = "简体中文"};
        languageCfg[LocalType.CHINESE_TRADITION] = new Language {LKey = LocalType.CHINESE_TRADITION, DisStr = "繁體中文"};
        languageCfg[LocalType.HINDI] = new Language {LKey = LocalType.HINDI, DisStr = "हिन्दी"};
        languageCfg[LocalType.DUTCH] = new Language {LKey = LocalType.DUTCH, DisStr = "Nederlands"};
        languageCfg[LocalType.MALAYSIA] = new Language {LKey = LocalType.MALAYSIA, DisStr = "Bahasa Melayu"};

        var LanguageList = LocalizationManager.Instance.supportedLocale;
        foreach (string item in LanguageList)
        {
            languageCfg[item].IsEnabled = true;
        }

        return languageCfg;
    }
}