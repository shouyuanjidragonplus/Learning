namespace Framework.Common
{
    /// <summary>
    /// 各种服务器地址的配置对象，将序列化为json文件
    /// </summary>
    [System.Serializable]
    public sealed class UrlList
    {
        //配置文件 用于定位更新等地址
        public string baseUrl = "";
        public string configFileUrl = "";
        public string updateUrl = "";
        public string updatePackageUrl = "";

        public string versionCustomContent = "";
        public string gameReleaseVersion = "";
        //...
    }
}
