using System;
using System.IO;
using System.Security.Cryptography;

namespace FortniteLauncher
{
    public class Hash
    {
        public static string GetSha1(string path)
        {
            var sha1 = SHA1.Create().ComputeHash(new FileStream(path, FileMode.Open, FileAccess.Read));
            return BitConverter.ToString(sha1);
        }
        public static string GetSha256(string path)
        {
            var sha256 = SHA256.Create().ComputeHash(new FileStream(path, FileMode.Open, FileAccess.Read));
            return BitConverter.ToString(sha256);
        }
    }
}
