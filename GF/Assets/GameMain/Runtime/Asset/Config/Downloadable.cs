using System;

namespace GameMain.Asset
{
    [Serializable]
    public class Downloadable
    {
        public string name = string.Empty;
        public string hash;
        public ulong size;
        public string file { get; set; }
    }
}