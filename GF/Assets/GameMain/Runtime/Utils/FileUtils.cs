using System;
using System.IO;
using System.Text;
using UnityEngine;
using GameMain.Base;

namespace GameMain.Utils
{
    public class FileUtils
    {
        private static readonly Encoding Default_Encoding = new UTF8Encoding(false);
        private static string _root = null;

        /// <summary>
        /// 本地的游戏持久化路劲
        /// </summary>
        public static string root
        {
            get
            {
                if (null == _root)
                    _root = Application.persistentDataPath;
                return _root;
            }
        }
        private static string _resRoot = null;
        /// <summary>
        /// 游戏资源的存放目录
        ///      1，游戏更新时，新资源也会存放到这个目录下
        ///      2，读取游戏资源时，如无特殊，也优先从这个目录中读取
        /// </summary>
        public static string resRoot
        {
            get
            {
                if (null == _resRoot)
                    _resRoot = string.Concat(root, "/res");
                return _resRoot;
            }
        }
        public static string GetFileDir(string filePath)
        {
            try
            {
                FileInfo fi = new FileInfo(filePath);
                return fi.DirectoryName;
            }
            catch (Exception ex)
            {
                BaseLogger.Error("GetFileDir failed,file: {0} ({1})", filePath, ex.Message);
                return null;
            }
        }
        public static void MakeSureDirExists(string dir)
        {
            try
            {
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
            }
            catch (Exception ex)
            {
                BaseLogger.Error("MakeSureDirExists failed,dir :{0} ({1})", dir, ex.Message);
            }
        }
        public static bool IsFileExists(string filePath)
        {
            return File.Exists(filePath);
        }
        /// <summary>
        /// 删除文件
        /// 若文件不再存在，或 文件存在并且最终删除成功，返回true，否则返回false
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool DeleteFile(string filePath)
        {
            if (!IsFileExists(filePath))
                return true;
            try
            {
                File.Delete(filePath);
                return true;
            }
            catch (Exception ex)
            {
                BaseLogger.Error("DeleteFile {0} failed,error is ({1})", filePath, ex.Message);
                return false;
            }
        }
        //删除等待次数，目前会sleep 1ms，即可粗略计算为对应毫秒数
        private static int Delete_WaitTimes = 10 * 1000;
        private static bool DeleteDirWithoutLock(string dirPath, bool isSync = true)
        {
            if (!Directory.Exists(dirPath))
            {
                BaseLogger.Warning("DeleteDir not exist dir,dir: {0}", dirPath);
                return true;
            }
            try
            {
                Directory.Delete(dirPath, true);
                bool isDeleted = !isSync;
                if (isSync)
                {
                    for (int i = 0; i < Delete_WaitTimes; ++i)
                    {
                        isDeleted = !Directory.Exists(dirPath);
                        if (isDeleted) break;

                        BaseLogger.Info("DeleteDir deleting, dir: {1} ", dirPath);
                        System.Threading.Thread.Sleep(1);
                    }
                }
                return isDeleted;
            }
            catch (Exception ex)
            {
                BaseLogger.Error("DeleteDir failed, dir: {0}({1})", dirPath, ex.Message);
                return false;
            }
        }
        public static bool DeleteDir(string dirPath, bool isSync = true)
        {
            return DeleteDirWithoutLock(dirPath, isSync);
        }
        private static bool ClearnDirWithoutLock(string dirPath)
        {
            if (!Directory.Exists(dirPath))
            {
                BaseLogger.Warning("ClearDir not exist dir,dir: {0}", dirPath);
                return true;
            }
            try
            {
                DirectoryInfo di = new DirectoryInfo(dirPath);

                FileInfo[] fList = di.GetFiles();
                for (int i = 0; i < fList.Length; ++i)
                {
                    fList[i].Delete();
                }
                DirectoryInfo[] diList = di.GetDirectories();
                for (int i = 0; i < diList.Length; ++i)
                {
                    diList[i].Delete(true);
                }
                return true;
            }
            catch (Exception ex)
            {
                BaseLogger.Error("ClearDir failed,dir: {0} ({1})", dirPath, ex.Message);
                return false;
            }
        }
        public static bool ClearDir(string dirPath)
        {
            return ClearnDirWithoutLock(dirPath);
        }

        private static bool CreateTxtFileWithoutLock(string filePath, string content)
        {
            MakeSureDirExists(GetFileDir(filePath));
            if (File.Exists(filePath))
                File.Delete(filePath);

            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
                {
                    using (StreamWriter sw = new StreamWriter(fs, Default_Encoding))
                    {
                        sw.Write(content);
                        sw.Flush();
                        sw.Close();
                    }
                    fs.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                BaseLogger.Error("CreateTxtFile failed,file: {0} ({1})", filePath, ex.Message);
                return false;
            }
        }
        public static bool CreateTxtFile(string filePath, string content)
        {
            return CreateTxtFileWithoutLock(filePath, content);
        }
        public static string RendTxtFromFile(string filePath)
        {
            try
            {
                using (StreamReader sr = new StreamReader(filePath, Default_Encoding))
                {
                    string content = sr.ReadToEnd();
                    sr.Close();
                    return content;
                }
            }
            catch (Exception ex)
            {
                BaseLogger.Error("ReadTxtFromFile failed,file:{0}({1})", filePath, ex.Message);
                return null;
            }
        }
        private static bool CreateBinFileWithoutLock(string filePath, byte[] content)
        {
            MakeSureDirExists(GetFileDir(filePath));
            if (IsFileExists(filePath))
            {
                if (!DeleteFile(filePath))
                    return false;
            }
            try
            {
                FileInfo fi = new FileInfo(filePath);
                FileStream fs = fi.Create();
                fs.Write(content, 0, content.Length);
                fs.Close();
                fs = null;
                return true;
            }
            catch (Exception ex)
            {
                BaseLogger.Error("CreateBinFile failed,file: {0}({1})", filePath, ex.Message);
                return false;
            }
        }
        public static bool CreateBinFile(string filePath, byte[] content)
        {
            return CreateBinFileWithoutLock(filePath, content);
        }
        public static byte[] ReadBinFromFile(string filePath)
        {
            try
            {
                byte[] content = File.ReadAllBytes(filePath);
                return content;
            }
            catch (Exception ex)
            {
                BaseLogger.Error("ReadBinFromFile failed,file:{0}({1})", filePath, ex.Message);
                return null;
            }
        }
        private static string GenFileMD5WithoutLock(string filePath)
        {
            if (!IsFileExists(filePath))
                return null;
            FileStream fs = new FileStream(filePath, FileMode.Open);
            string md5 = MD5Hash.Get(fs);
            fs.Close();
            fs = null;
            return md5;
        }
        public static string GenFileMD5(string filePath)
        {
            return GenFileMD5WithoutLock(filePath);
        }
        public static float GetFileSize(string filePath)
        {
            float size = 0;
            if (IsFileExists(filePath))
                size = new FileInfo(filePath).Length;
            return size;
        }
    }
}
