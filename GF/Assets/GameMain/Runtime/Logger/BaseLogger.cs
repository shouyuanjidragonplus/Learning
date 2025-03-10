using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;

namespace GameMain.Base
{
    [Flags]
    public enum LogLevel
    {
        NONE = 0,
        DEBUG = 1,
        INFO = 2,
        WARNING = 4,
        ERROR = 8,
        EXCEPT = 16,
        CRITICAL = 32,
    }
    public class BaseLogger
    {
        public static string LOGGER_PREFIX = "['Logger']";
        public static LogLevel currentLogLevel = LogLevel.DEBUG | LogLevel.INFO | LogLevel.WARNING | LogLevel.ERROR | LogLevel.EXCEPT | LogLevel.CRITICAL;

        public delegate bool FilterMsgCallback(string msg);
        private static FilterMsgCallback _filterMsgCallback = null;

        public delegate void LogMsgCallback(string msg, LogLevel level);
        private static LogMsgCallback _logMsgCallback = null;

        private static bool _isShowStack = true;
        private static bool _isWrite = true;
        private static LogWriter _logWriter = null;

        static BaseLogger()
        {
            Init();
            Application.logMessageReceived += ProcessExceptionReport;
        }
        public static void Init()
        {
            if (_logWriter == null)
            {
                _logWriter = new LogWriter();
            }
        }
        public static void SetShowStackState(bool showStack)
        {
            _isShowStack = showStack;
        }
        public static void SetWriteState(bool whiteState)
        {
            _isWrite = whiteState;
        }

        public static void Release()
        {
            Application.logMessageReceived -= ProcessExceptionReport;

            _filterMsgCallback = null;
            _logMsgCallback = null;

            _logWriter.Release();
            _logWriter = null;
        }
        public static void SetFilterMsgCallback(FilterMsgCallback fmCB)
        {
            _filterMsgCallback = fmCB;
        }
        public static void RegistLogMsgCallback(LogMsgCallback lmCB)
        {
            if (_logMsgCallback == null) _logMsgCallback = lmCB;
            else
            {
                Delegate[] ds = _logMsgCallback.GetInvocationList();
                int dsCount = ds.Length;
                for (int i = 0; i < dsCount; ++i)
                {
                    if (ds[i].Equals(lmCB))
                    {
                        Warning("BaseLogger RegistLogMsgCallback duplicate ({0})", lmCB);
                        return;
                    }
                }
                _logMsgCallback += lmCB;
            }
        }
        public static void UnRegistLogMsgCallback(LogMsgCallback lmCB)
        {
            if (_logMsgCallback != null)
                _logMsgCallback -= lmCB;
        }

        public static void Debug(object message, params object[] argv)
        {
            if (LogLevel.DEBUG == (currentLogLevel & LogLevel.DEBUG))
            {
                string messageFormat = LoggerStringFormat(message.ToString(), argv);
                Log(string.Concat(" [DEBUG]：", _isShowStack ? GetStackInfo() : "", messageFormat), LogLevel.DEBUG);
            }
        }
        public static void Info(object message, params object[] argv)
        {
            if (LogLevel.INFO == (currentLogLevel & LogLevel.INFO))
            {
                string messageFormat = LoggerStringFormat(message.ToString(), argv);
                Log(string.Concat(" [INFO]：", _isShowStack ? GetStackInfo() : "", messageFormat), LogLevel.INFO);
            }
        }
        public static void Warning(object message, params object[] argv)
        {
            if (LogLevel.WARNING == (currentLogLevel & LogLevel.WARNING))
            {
                string messageFormat = LoggerStringFormat(message.ToString(), argv);
                Log(string.Concat(" [WARNING]：", _isShowStack ? GetStackInfo() : "", messageFormat), LogLevel.WARNING);
            }
        }
        public static void Error(object message, params object[] argv)
        {
            if (LogLevel.ERROR == (currentLogLevel & LogLevel.ERROR))
            {
                string messageFormat = LoggerStringFormat(message.ToString(), argv);
                Log(string.Concat(" [ERROR]：", messageFormat, "\n", _isShowStack ? GetStacksInfo() : ""), LogLevel.ERROR);
            }
        }
        public static void Critical(object message, params object[] argv)
        {
            if (LogLevel.CRITICAL == (currentLogLevel & LogLevel.CRITICAL))
            {
                string messageFormat = LoggerStringFormat(message.ToString(), argv);
                Log(string.Concat(" [CRITICAL]：", messageFormat, "\n", _isShowStack ? GetStacksInfo() : ""), LogLevel.CRITICAL);
            }
        }
        public static void Except(object message, params object[] argv)
        {
            if (LogLevel.EXCEPT == (currentLogLevel & LogLevel.EXCEPT))
            {
                string messageFormat = LoggerStringFormat(message.ToString(), argv);
                Log(string.Concat(" [EXCEPT]：", messageFormat, "\n", _isShowStack ? GetStacksInfo() : ""), LogLevel.EXCEPT);
            }
        }
        public static void Assert(bool comparison, object message = null, params object[] argv)
        {
            if (comparison) return;
            if (LogLevel.ERROR == (currentLogLevel & LogLevel.ERROR))
            {
                string messageFormat = "";
                if (message != null)
                    messageFormat = LoggerStringFormat(message.ToString(), argv);
                else
                    messageFormat = "comparison not pass!";
                Error("Assert {0} #", messageFormat);
            }
        }

        public static string LoggerStringFormat(object message, params object[] argv)
        {
            string logMsg = "logMsg";
            try
            {
                if (argv == null || argv.Length == 0)
                    logMsg = message.ToString();
                else
                    logMsg = string.Format(message.ToString(), argv);
            }
            catch (Exception ex)
            {
                logMsg = ex.Message;
            }
            return logMsg;
        }

        private static string GetStacksInfo()
        {
            StringBuilder sb = new StringBuilder();
            StackTrace st = new StackTrace(true);
            StackFrame[] sf = st.GetFrames();
            for (int i = 2; i < sf.Length; ++i)
            {
                sb.AppendLine(sf[i].ToString());
            }
            return sb.ToString();
        }
        private static string GetStackInfo()
        {
            StackTrace st = new StackTrace(true);
            if (st.FrameCount > 2)
            {
                StackFrame sf = st.GetFrame(2);
                var method = sf.GetMethod();
                return string.Format("{0}.{1}():", method.ReflectedType.Name, method.Name);
            }
            return "StackInfo";
        }

        private static void Log(string message, LogLevel level, bool needUEDebugOutputLog = true)
        {
            if (_logMsgCallback != null)
                _logMsgCallback(message, level);
            if (_filterMsgCallback != null)
            {
                if (_filterMsgCallback(message))
                    return;
            }
            if (_logWriter != null)
            {
                string msg = string.Concat(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,fff"), message);
                if (_isWrite)
                    _logWriter.WriteLog(msg, level, needUEDebugOutputLog);
                else
                    _logWriter.SafeUnityEnginDebugOutput(message, level);
            }
        }
        private static void ProcessExceptionReport(string message, string stackTrace, LogType type)
        {
            if (message.IndexOf(LOGGER_PREFIX) == 0) return;
            LogLevel logLevel = LogLevel.DEBUG;
            switch (type)
            {
                case LogType.Error:
                    logLevel = LogLevel.ERROR;
                    break;
                case LogType.Assert:
                    logLevel = LogLevel.DEBUG;
                    break;
                case LogType.Warning:
                    logLevel = LogLevel.WARNING;
                    break;
                case LogType.Log:
                    logLevel = LogLevel.INFO;
                    break;
                case LogType.Exception:
                    logLevel = LogLevel.EXCEPT;
                    break;
                default:
                    break;
            }
            if (logLevel == (currentLogLevel & logLevel))
                Log(string.Format(" [SYS_", logLevel, "]", message, '\n', stackTrace), logLevel, false);
        }
    }
    //日志写入文件管理类
    class LogWriter
    {
        private string _logPath = Application.persistentDataPath + "/log/";
        private string _logFileNamePattern = "log_game*.txt";
        private static string _logFileFormatStr = "yyMMddHHmmssff";
        private string _logFileName = "log_game{0}.txt";
        //启动清理log文件只留最近指定数目个log文件
#if UNITY_STANDALONE
        private int _levelLogFileNum = 10;
#else
         private int _levelLogFileNum = 3;
#endif
        private string _logFilePath;
        private FileStream _fs;
        private StreamWriter _sw;
        private Action<string, LogLevel, bool> _logWriter;
        private readonly static object _locker = new object();

        public LogWriter()
        {
            if (!Directory.Exists(_logPath))
            {
                Directory.CreateDirectory(_logPath);
            }
            ClearLogFile(_levelLogFileNum - 1);
            _logFilePath = string.Concat(_logPath, string.Format(_logFileName, DateTime.Now.ToString(_logFileFormatStr)));
            try
            {
                _logWriter = Write;
                _fs = new FileStream(_logFilePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                _sw = new StreamWriter(_fs, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError(ex.Message);
            }
        }
        public void Release()
        {
            lock (_locker)
            {
                if (_sw != null)
                {
                    _sw.Close();
                    _sw.Dispose();
                }
                if (_fs != null)
                {
                    _fs.Close();
                    _fs.Dispose();
                }
            }
        }
        public string GetCurLogName()
        {
            return _logFileName;
        }
        public void WriteLog(string msg, LogLevel level, bool needUEDebugOutputLog)
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                _logWriter(msg, level, needUEDebugOutputLog);
            }
            else
            {
                _logWriter.Invoke(msg, level, needUEDebugOutputLog);
            }
        }
        public void SafeUnityEnginDebugOutput(string msg, LogLevel level)
        {
            try
            {
                UnityEngineDebugOutput(msg, level);
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError(ex.Message);
            }
        }
        private void UnityEngineDebugOutput(string msg, LogLevel level)
        {
            switch (level)
            {
                case LogLevel.DEBUG:
                case LogLevel.INFO:
                    UnityEngine.Debug.Log(string.Concat(BaseLogger.LOGGER_PREFIX, msg));
                    break;
                case LogLevel.WARNING:
                    UnityEngine.Debug.LogWarning(string.Concat(BaseLogger.LOGGER_PREFIX, msg));
                    break;
                case LogLevel.ERROR:
                case LogLevel.EXCEPT:
                case LogLevel.CRITICAL:
                    UnityEngine.Debug.LogError(string.Concat(BaseLogger.LOGGER_PREFIX, msg));
                    break;
                default:
                    break;
            }
        }
        private void Write(string msg, LogLevel level, bool needUEDebugOutputLog)
        {
            lock (_locker)
            {
                try
                {
                    if (needUEDebugOutputLog)
                    {
                        UnityEngineDebugOutput(msg, level);
                    }
                    if (_sw != null)
                    {
                        _sw.WriteLine(msg);
                        _sw.Flush();
                    }
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.LogError(ex.Message);
                }
            }
        }
        //清理当前log，只保留指定个数得文件
        private void ClearLogFile(int levelFileNum)
        {
            string[] files = null;
            try
            {
                files = Directory.GetFiles(_logPath, _logFileNamePattern);
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError(string.Format("can't  get log files ({0})", ex.Message));
                return;
            }
            if (files != null)
            {
                int fileNum = files.Length;
                if (fileNum > levelFileNum)
                {
                    Array.Sort(files);

                    int delFileNum = fileNum - levelFileNum;
                    for (int i = 0; i < delFileNum; ++i)
                    {
                        try
                        {
                            File.Delete(files[i]);
                        }
                        catch (Exception ex)
                        {
                            UnityEngine.Debug.LogError(string.Format("delete {0} failed ({1})", files[i], ex.Message));
                        }
                    }
                }
            }
        }
    }
}
