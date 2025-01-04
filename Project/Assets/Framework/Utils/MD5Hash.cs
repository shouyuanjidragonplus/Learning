using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Framework.Utils
{
    public static class MD5Hash
    {
        public static string Get(string input, Encoding e = null)
        {
            if (e == null) e = Encoding.Default;
            return Get(e.GetBytes(input));
        }
        public static string Get(byte[] input)
        {
            MD5 md5Hash = MD5.Create();
            byte[] data = md5Hash.ComputeHash(input);

            StringBuilder sb = new StringBuilder();
            int dataLen = data.Length;
            for (int i = 0; i < dataLen; ++i)
            {
                sb.Append(data[i].ToString("x2"));
            }
            return sb.ToString();
        }
        public static string Get(Stream stream)
        {
            MD5 md5 = MD5.Create();
            byte[] data = md5.ComputeHash(stream);

            StringBuilder sb = new StringBuilder();
            int dataLen = data.Length;
            for (int i = 0; i < dataLen; ++i)
            {
                sb.Append(data[i].ToString("x2"));
            }
            return sb.ToString();
        }
        public static bool Verify(string input, string hash, Encoding e = null)
        {
            string hashOfInput = Get(input, e);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            return (0 == comparer.Compare(hashOfInput, hash));
        }
        public static bool Verify(byte[] input, string hash)
        {
            string hashOfInput = Get(input);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            return (0 == comparer.Compare(hashOfInput, hash));
        }
        public static bool Verify(Stream input, string hash)
        {
            string hashOfInput = Get(input);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            return (0 == comparer.Compare(hashOfInput, hash));
        }
    }
}
