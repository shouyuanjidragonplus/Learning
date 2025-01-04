using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using UnityEngine;
using Zero.Engine.Base;

namespace Zero.Engine.Game
{
    /// <summary>
    /// 设备信息和效果工具类
    /// </summary>
    public static class DeviceInfo
    {
        public static string device_id = SystemInfo.deviceUniqueIdentifier;
        /// <summary>
        /// 手机操作系统
        /// </summary>
        public static string os = SystemInfo.operatingSystem;
        /// <summary>
        /// 操作系统版本
        /// </summary>
#if UNITY_ANDROID
         public static string os_version = "Android";
#elif UNITY_IOS
        public static string os_version = "IOS";
#elif UNITY_STANDALONE_WIN
        public static string os_version = "Windows";
#elif UNITY_STANDALONE_OSX
        public static string os_version = "macOS";
#else
        public static string os_version = "OtherOS";
#endif
        /// <summary>
        /// 设备信息
        /// </summary>
        public static string device = SystemInfo.deviceModel;
        /// <summary>
        /// 设备名称
        /// </summary>
        public static string device_name = SystemInfo.deviceName;
        /// <summary>
        /// 设备类型
        /// </summary>
        public static string device_type = SystemInfo.deviceType.ToString();
        /// <summary>
        /// 屏幕分辨率
        /// </summary>
        public static string screen = "";
        /// <summary>
        /// 网络运营商
        /// </summary>
        public static string mno = "中国移动";
        /// <summary>
        /// 联网方式
        /// </summary>
        public static string nm = "WIFI";
        /// <summary>
        /// 设备IP
        /// </summary>
        public static string ip = "";
        public static string processInfo = SystemInfo.processorType;

        /// <summary>
        /// 电池电量
        /// </summary>
        public static float Battery = 100;
        /// <summary>
        /// wifi名称
        /// </summary>
        public static string WifiName = "unknown";
        /// <summary>
        /// wifi强度
        /// </summary>
        public static int WifiStrength = 0;
        /// <summary>
        /// wifi速度
        /// </summary>
        public static int WifiSpeed = 0;

        public static void Init()
        {
            try
            {
                screen = string.Format("{0}*{1}", Screen.width, Screen.height);
                processInfo = string.Format("{0}(COUNT:{1})", SystemInfo.processorType, SystemInfo.processorCount);
                nm = GetNetWorkType();
                GetIPAddress();
            }
            catch (Exception ex)
            {
                Zlogger.Error("Create DeviceInfo Error ({0})", ex.Message);
            }
        }
        /// <summary>
        /// 获取联网方式
        /// </summary>
        /// <returns></returns>
        public static string GetNetWorkType()
        {
            if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
            {
                return "WIFI";
            }
            else if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
            {
                return "3G";
            }
            else
                return "NONE";
        }
        public static bool NetworkAbility()
        {
#if UNITY_EDITOR
            Zlogger.Warning("Application.internetReachability = {0},return true in Editor Mode.", Application.internetReachability);
            return true;
#endif
            return Application.internetReachability != NetworkReachability.NotReachable;
        }
        /// <summary>
        /// 获取IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetIPAddress()
        {
            if (string.IsNullOrEmpty(ip))
            {
#if UNITY_2018 || UNITY_2019_1_OR_NEWER
                ip = GetIP();
#else
                ip = Network.player.ipAddress;
#endif
            }
            return ip;
        }
        public static string GetIP(bool bIPv4 = true)
        {
            if (bIPv4 == false && Socket.OSSupportsIPv6 == false) return null;
            string ip = "";
            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
            {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
                if ((item.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || item.NetworkInterfaceType == NetworkInterfaceType.Ethernet) && item.OperationalStatus == OperationalStatus.Up)
#endif
                {
                    foreach (UnicastIPAddressInformation ipInfo in item.GetIPProperties().UnicastAddresses)
                        if (bIPv4 && ipInfo.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            ip = ipInfo.Address.ToString();
                        }
                        else if (ipInfo.Address.AddressFamily == AddressFamily.InterNetworkV6)
                        {
                            ip = ipInfo.Address.ToString();
                        }
                }
            }
            return ip;
        }

        /// <summary>
        /// 获取电池电量
        /// </summary>
        /// <returns></returns>
        public static float GetBattery()
        {
            if (SystemInfo.batteryLevel == -1)
                return Battery;
            return SystemInfo.batteryLevel * 100;
        }
        /// <summary>
        /// 获取电池充电状态
        /// </summary>
        /// <returns></returns>
        public static bool IsBatteryCharge()
        {
            return SystemInfo.batteryStatus == BatteryStatus.Charging;
        }
        /// <summary>
        /// 设置网络类型
        /// </summary>                  
        public static void SetNetWorkType(int type)
        {
            switch (type)
            {
                case 0: nm = "未知"; break;
                case 1: nm = "未连接"; break;
                case 2: nm = "OTHER"; break;
                case 3: nm = "WIFI"; break;
                case 4: nm = "2G"; break;
                case 5: nm = "3G"; break;
                case 6: nm = "4G"; break;
                default: nm = "移动网络:" + type; break;
            }
        }

#if UNITY_IOS
        [DllImport("__Internal")]
        private static extern int getScreenOrientationIOS();
#endif
        public static ScreenOrientation GetScreenOrientation()
        {
#if UNITY_IOS && !UNITY_EDITOR
            int screenOrientation = getScreenOrientationIOS();
            // DebugUtil.Log("iOS Screen orientation : " + screenOrientation);
            switch (screenOrientation)
            {
                case 1:
                    return ScreenOrientation.Portrait;
                case 2:
                    return ScreenOrientation.PortraitUpsideDown;
                case 3:
                    return ScreenOrientation.LandscapeLeft;
                case 4:
                    return ScreenOrientation.LandscapeRight;
                default:
                    return ScreenOrientation.Unknown;
            }
#else
            return Screen.orientation;
#endif

        }
    }
}

