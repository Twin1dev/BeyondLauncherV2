using System.Diagnostics;
using System.Management;
using System.Net.NetworkInformation;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;

namespace HardwareBanningTest
{
    internal class Program
    {


        static string GetDeviceId()
        {

            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystemProduct");
                ManagementObjectCollection collection = searcher.Get();

                foreach (ManagementObject obj in collection)
                {
                    return obj["UUID"].ToString(); // Device ID is often stored in the UUID property
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return null;
        }

        static string GetWindowsProductId()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
                ManagementObjectCollection collection = searcher.Get();

                foreach (ManagementObject obj in collection)
                {
                    return obj["SerialNumber"].ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return null;
        }

        static byte[] EncryptStringToBytes(byte[] plaintextBytes, byte[] key)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.Mode = CipherMode.ECB; // ECB mode for simplicity, not recommended for actual use
                //aesAlg.Padding = PaddingMode.PKCS7; // Padding mode

                // Create an encryptor to perform the stream transform
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        // Write all data to the crypto stream and flush it
                        csEncrypt.Write(plaintextBytes, 0, plaintextBytes.Length);
                        csEncrypt.FlushFinalBlock();
                    }
                    // Return the encrypted bytes from the memory stream
                    return msEncrypt.ToArray();
                }
            }
        }

        static void Main(string[] args)
        {
            WindowsIdentity winId = WindowsIdentity.GetCurrent();

            SecurityIdentifier sid = winId.User;

            Console.WriteLine(sid.Value);

          /*  Console.WriteLine(GetDeviceId());
            Console.WriteLine(GetWindowsProductId());

            string PNPDeviceID = "";

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_DesktopMonitor");
            foreach (ManagementObject obj in searcher.Get())
            {
                PNPDeviceID = obj["PNPDeviceID"].ToString();
            }
            string DeviceId = GetDeviceId();
            // Your plaintext string
            string plaintext = $"{PNPDeviceID.Split("\\").Last()}___{DeviceId}";

            // Your secret key (must be 16, 24, or 32 bytes long for AES)
            string key = "J9eLm3QxR7sWn2Yp";

            // Convert the plaintext and key to byte arrays
            byte[] plaintextBytes = Encoding.UTF8.GetBytes(plaintext);
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);

            byte[] encryptedBytes = EncryptStringToBytes(plaintextBytes, keyBytes);

            string base64EncryptedString = Convert.ToBase64String(encryptedBytes);

            Console.WriteLine($"\n\n{base64EncryptedString}");
*/
            Console.ReadLine();
        }
    }
}
