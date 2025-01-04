using UnityEngine;

namespace MMO
{
    public class UMainData
    {
        private static GameObject _keepme = null;
        public static GameObject keepme
        {
            get { return _keepme; }
            set { _keepme = value; _keepme_tr = _keepme.transform; }
        }
        private static Transform _keepme_tr = null;
        public static Transform keepme_tr
        {
            get { return _keepme_tr; }
        }
        private static GameObject _canvas = null;
        public static GameObject canvas
        {
            get { return _canvas; }
            set { _canvas = value; _canvas_tr = _canvas.transform; }
        }
        private static Transform _canvas_tr = null;
        public static Transform canvas_tr
        {
            get { return _canvas_tr; }
        }
    }
}
