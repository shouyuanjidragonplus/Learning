using System.IO;
using System.Text;
using System;

namespace Zero.Engine.Base
{
    static public class FileHelper
    {
        private static readonly Encoding DefaultEnconding = new UTF8Encoding(false);
        public static bool IsDirExists(string dir)
        {
            return Directory.Exists(dir);
        }
        public static bool IsFileExists(string file)
        {
            return File.Exists(file);
        }
        public static bool CreateDir(string dir)
        {
            try
            {
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
            }
            catch (Exception ex)
            {
                Zlogger.Error("CreateDir failed, dir: {0} ({1})", dir, ex.Message);
                return false;
            }
            return true;
        }
        //isSync 是否等待到确保目录被删除再返回，目前部分系统会执行异步操作就返回
        //等待检测时间100ms，总共检测次数100次
        private static int DELETE_WAITIME = 100;
        private static int DELETE_WAITIMES = 100;
        public static bool DeleteDir(string dir, bool isSync = true)
        {
            if (Directory.Exists(dir))
            {
                try
                {
                    Directory.Delete(dir, true);
                    bool isDeleted = !isSync;
                    if (isSync)
                    {
                        for (int i = 0; i < DELETE_WAITIMES; ++i)
                        {
                            isDeleted = !Directory.Exists(dir);
                            if (isDeleted)
                                break;
                            Zlogger.Info("DeleteDir Deleting, dir: {0}", dir);
                            System.Threading.Thread.Sleep(DELETE_WAITIME);
                        }
                    }
                    return isDeleted;
                }
                catch (Exception ex)
                {
                    Zlogger.Error("DeleteDir failed, dir: {0} ({1})", dir, ex.Message);
                    return false;
                }
            }
            else
            {
                Zlogger.Error("DeleteDir not exist dir, dir: {0}", dir);
                return true;
            }
        }
        public static bool ClearDir(string dir)
        {
            if (!Directory.Exists(dir))
            {
                Zlogger.Warning("ClearDir not exist dir, dir: {0}", dir);
                return true;
            }
            try
            {
                DirectoryInfo di = new DirectoryInfo(dir);

                FileInfo[] fs = di.GetFiles();
                int fsNum = fs.Length;
                for (int i = 0; i < fsNum; ++i)
                {
                    fs[i].Delete();
                }

                DirectoryInfo[] dis = di.GetDirectories();
                int disNum = dis.Length;
                for (int i = 0; i < disNum; ++i)
                {
                    dis[i].Delete(true);
                }
                return true;
            }
            catch (Exception ex)
            {
                Zlogger.Error("ClearDir failed, dir: {0} ({1})", dir, ex.Message);
                return false;
            }
        }
        public static bool DeleteFile(string file)
        {
            if (File.Exists(file))
            {
                try
                {
                    File.Delete(file);
                }
                catch (Exception ex)
                {
                    Zlogger.Error("DeleteFile failed, file: {0} ({1})", file, ex.Message);
                    return false;
                }
            }
            return true;
        }
        public static byte[] ReadBinFile(string file)
        {
            try
            {
                byte[] content = File.ReadAllBytes(file);
                return content;
            }
            catch (Exception ex)
            {
                Zlogger.Error("ReadBinFile failed, file: {0} ({1})", file, ex.Message);
                return null;
            }
        }
        public static bool WriteBinFile(string file, byte[] content)
        {
            try
            {
                using (FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write, FileShare.Read))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        bw.Write(content);
                        bw.Flush();
                        bw.Close();
                    }
                    fs.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Zlogger.Error("WriteBinFile failed, file: {0} ({1})", file, ex.Message);
                return false;
            }
        }
        public static string ReadTxtFile(string file)
        {
            try
            {
                using (StreamReader sr = new StreamReader(file, DefaultEnconding))
                {
                    string content = sr.ReadToEnd();
                    sr.Close();

                    return content;
                }
            }
            catch (Exception ex)
            {
                Zlogger.Error("ReadTxtFile failed, file: {0} ({1})", file, ex.Message);
                return null;
            }
        }
        public static bool WriteTxtFile(string file, string content)
        {
            try
            {
                using (FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write, FileShare.Read))
                {
                    using (StreamWriter sw = new StreamWriter(fs, DefaultEnconding))
                    {
                        sw.Write(content);
                        sw.Flush();
                        fs.Close();
                    }
                    fs.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Zlogger.Error("WriteTxtFile failed, file: {0} ({1})", file, ex.Message);
                return false;
            }
        }
        public static string GetFileDir(string file)
        {
            try
            {
                FileInfo fi = new FileInfo(file);
                return fi.DirectoryName;
            }
            catch (Exception ex)
            {
                Zlogger.Error("GetFileDir failed, file: {0} ({1})", file, ex.Message);
                return null;
            }
        }
        public static bool CreateBinFile(string file, byte[] content)
        {
            CreateDir(GetFileDir(file));
            return WriteBinFile(file, content);
        }
        public static bool CreateTxtFile(string file, string content)
        {
            CreateDir(GetFileDir(file));
            return WriteTxtFile(file, content);
        }
    }
}
