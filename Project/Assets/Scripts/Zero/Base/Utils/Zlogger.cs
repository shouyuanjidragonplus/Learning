using System.Collections.Generic;
using Framework.Base;

namespace Zero.Engine.Base
{
    public class Zlogger
    {
        static Zlogger()
        {
#if GAME_PUBLISH && !DEEP_DEBUG_RESMANAGER
           BaseLogger.SetFilterMsgCallback(HashString);  
#endif
        }
        public static void SetShowStackState(bool showStack)
        {
            BaseLogger.SetShowStackState(showStack);
        }
        public static void SetWriteState(bool whiteState)
        {
            BaseLogger.SetWriteState(whiteState);
        }
#if !GAME_PUBLISH
        public static List<string> infoFilterList = new List<string>();
        private static bool IsInInfoFilterList(string str)
        {
            int infoFilterListCount = infoFilterList.Count;
            if (infoFilterListCount != 0)
            {
                for (int i = 0; i < infoFilterListCount; ++i)
                {
                    if (str.IndexOf(infoFilterList[i]) == 0)
                        return true;
                }
                return false;
            }
            else
                return true;
        }
#endif


        public static void Release()
        {
            BaseLogger.Release();
        }
        public static void RegistLogMsgCallback(BaseLogger.LogMsgCallback lmCB)
        {
            BaseLogger.RegistLogMsgCallback(lmCB);
        }
        public static void UnRegistLogMsgCallback(BaseLogger.LogMsgCallback lmCB)
        {
            BaseLogger.UnRegistLogMsgCallback(lmCB);
        }

        public static void Debug(object message, params object[] argv)
        {
            BaseLogger.Debug(message, argv);
        }
        public static void Info(object message, params object[] argv)
        {
            if (LogLevel.INFO == (BaseLogger.currentLogLevel & LogLevel.INFO))
            {
#if !GAME_PUBLISH
                if (!IsInInfoFilterList(message.ToString())) return;
#endif
                BaseLogger.Info(message, argv);
            }
        }
        public static void Warning(object message, params object[] argv)
        {
            BaseLogger.Warning(message, argv);
        }
        public static void Error(object message, params object[] argv)
        {
            BaseLogger.Error(message, argv);
        }
        public static void Critical(object message, params object[] argv)
        {
            BaseLogger.Critical(message, argv);
        }
        public static void Except(object message, params object[] argv)
        {
            BaseLogger.Except(message, argv);
        }
        public static void Assert(bool comparison, object message = null, params object[] argv)
        {
            BaseLogger.Assert(comparison, message, argv);
        }  

        private static int MAX_CACHE_QUEUE_NUM = 10;
        private static Queue<string> hashqueue = null;
        private static System.Object hashQueueLock = new System.Object();
        private static bool HashString(string str)
        {
            lock (hashQueueLock)
            {
                if (hashqueue == null) hashqueue = new Queue<string>();
                if (hashqueue.Contains(str)) return true;
                hashqueue.Enqueue(str);
                while (hashqueue.Count > MAX_CACHE_QUEUE_NUM) hashqueue.Dequeue();
                return false;
            }
        }
    }
}
