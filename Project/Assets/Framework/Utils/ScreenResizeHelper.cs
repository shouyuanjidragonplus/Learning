using System;
using UnityEngine;
using Framework.Base;
using Framework.Common;

namespace Framework.Utils
{
    public class ScreenResizeHelper
    {
        public delegate void OnScreenResize();

        private static int _width = 0;
        public static int width { get { return _width; } }
        private static int _height = 0;
        public static int height { get { return _height; } }
        private static OnScreenResize _onScreeenResize;
        public static void Init()
        {
            _width = Screen.width;
            _height = Screen.height;
            _onScreeenResize = null;
        }
        public static void AddCallback(OnScreenResize osr)
        {
            if (_onScreeenResize == null)
                _onScreeenResize = osr;
            else
            {
                Delegate[] ds = _onScreeenResize.GetInvocationList();
                int dsCount = ds.Length;
                for (int i = 0; i < dsCount; ++i)
                {
                    if (ds[i].Equals(osr))
                        BaseLogger.Warning("ScreenResizeHelper AddCallback duplicate ({0})", osr);
                }
                _onScreeenResize += osr;
            }
        }
        public static void RemoveCallback(OnScreenResize osr)
        {
            if (_onScreeenResize != null)
                _onScreeenResize -= osr;
        }
        public static void ClearupCallback()
        {
            if (_onScreeenResize != null)
            {
                OnScreenResize newOnScreenResize = null;
                Delegate[] ds = _onScreeenResize.GetInvocationList();
                int dsCount = ds.Length;
                if (dsCount > 0)
                    newOnScreenResize = ds[0] as OnScreenResize;
                for (int i = 1; i < dsCount; ++i)
                {
                    newOnScreenResize += (ds[i] as OnScreenResize);
                }
                _onScreeenResize = newOnScreenResize;
            }
        }
        public static void RemoveAllCallback()
        {
            _onScreeenResize = null;
        }
        public static void Update()
        {
            int curWidth = Screen.width;
            int curHeight = Screen.height;
            if (curWidth != _width || curHeight != _height)
            {
                _width = curWidth;
                _height = curHeight;
                if (_onScreeenResize != null)
                    _onScreeenResize();
            }
        }
        public static bool isFitWidth
        {
            get
            {
                return (float)_width / _height <= ((float)DriverConfig.GameResolutionWidth / DriverConfig.GameResolutionHeight);
            }
        }
        public static Vector2 ScreenSize()
        {
            return new Vector2(((float)_width / _height) * DriverConfig.GameResolutionHeight, DriverConfig.GameResolutionHeight);
        }
    }
}
