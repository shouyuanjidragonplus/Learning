namespace Framework.Common
{
    public static class DriverConfig
    {
        //默认帧数
        public static int defaultFrame = 30;
        //待机帧数
        public static int lowFrame = 10;

        //游戏设定的分辨率
        public static readonly int GameResolutionWidth = 720;
        public static readonly int GameResolutionHeight = 1280;

        public static byte[] urlListContent = null;
        public static UrlList urlList = null;

        public static bool isCheckUpdate = false;

        //默认后台运行
        public static bool isRunInBg = true;
    }
}
