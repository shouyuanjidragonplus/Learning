namespace Framework.Common
{
    public static class DriverCommPaths
    {
        public static readonly string ABFilePostfix = ".assetbundle";
#if UNITY_EDITOR
        public static readonly string UrlListFilePath = "UrlList.txt";
#elif UNITY_ANDROID
        public static readonly string UrlListFilePath = "UrlList_Android.txt";
#elif UNITY_IOS
        public static readonly string UrlListFilePath = "UrlList_IOS.txt";
#else
        public static readonly string UrlListFilePath = "UrlList.txt";
#endif

        //UI
        public static readonly string URootPath = "/URoot";
        public static readonly string URootCanvasPath = "/URoot/Canvas";
        public static readonly string UIEventSystemPath = "/URoot/EventSystem";
        public static readonly string UICameraPath = "/URoot/UICamera";

        public static readonly string UILowLayerPath = "/URoot/Canvas/LowLayer";
        public static readonly string UIMiddleLayerPath = "/URoot/Canvas/MiddleLayer";
        public static readonly string UIHighLayerPath = "/URoot/Canvas/HighLayer";
        public static readonly string UITopLayerPath = "/URoot/Canvas/TopLayer";
        public static readonly string UITopLayerLoadingBgPath = "/URoot/Canvas/TopLayer/LoadingBg";
        public static readonly string UINpcDialogueLayerPath = "/URoot/Canvas/NpcDialogueLayer";
        public static readonly string UICartoonLayerPath = "/URoot/Canvas/CartoonLayer";
        public static readonly string UINickLayerPath = "/URoot/Canvas/NickLayer";
        public static readonly string UIJoyStickRectPath = "/URoot/Canvas/NickLayer/JoyStickRect";

        public static readonly string GameStartDataFilePath = "Zero/Driver/GameStartData.assetbundle";
    }
}
