using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BeyondLauncherV2.Fortnite
{
    internal class Anticheat
    {
        public static string GetFileHash(string FilePath)
        {
            if (File.Exists(FilePath))
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    using (FileStream stream = File.OpenRead(FilePath))
                    {
                        byte[] hash = sha256.ComputeHash(stream);

                        string hashStr = BitConverter.ToString(hash).Replace("-", "").ToLower();

                        return hashStr;
                    }
                }
            }

            return "";
        }
    }
}
