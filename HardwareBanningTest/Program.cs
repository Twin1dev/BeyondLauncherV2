using System.Management;
using System.Net.NetworkInformation;

namespace HardwareBanningTest
{
    internal class Program
    {

        static NetworkInterface GetPrimaryNetworkInterface()
        {
            NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

            foreach (NetworkInterface networkInterface in networkInterfaces)
            {
                // Check if the network interface is up and not a loopback interface
                if (networkInterface.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
                    networkInterface.OperationalStatus == OperationalStatus.Up &&
                    networkInterface.GetIPProperties().GatewayAddresses.Count > 0)
                {
                    // Return the first found primary network interface
                    return networkInterface;
                }
            }

            return null;
        }

        static void SKibidiSIgma()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystemProduct");

                foreach (ManagementObject obj in searcher.Get())
                {
                    string uuid = obj["UUID"].ToString();


                    Console.WriteLine("SMBIOS UUID: " + uuid);
                }
            }
            catch (ManagementException e)
            {
                Console.WriteLine("Error: " + e.Message);
            }

            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_BIOS");

                foreach (ManagementObject obj in searcher.Get())
                {
                    string biosSerialNumber = obj["SerialNumber"].ToString();

                    Console.WriteLine("BIOS Serial Number: " + biosSerialNumber);
                }
            }
            catch (ManagementException e)
            {
                Console.WriteLine("Error: " + e.Message);
            }

            try
            {
                // Get the network interfaces
                NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();

                // Find the active network interfaces
                NetworkInterface activeInterface = interfaces.FirstOrDefault(
                    x => x.OperationalStatus == OperationalStatus.Up &&
                         x.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
                         x.GetIPProperties().GatewayAddresses.Count > 0);

                if (activeInterface != null)
                {
                    // Get the MAC address of the active interface
                    PhysicalAddress macAddress = activeInterface.GetPhysicalAddress();
                    string macAddressString = BitConverter.ToString(macAddress.GetAddressBytes());

                    Console.WriteLine("MAC Address of the Active Interface: " + macAddressString);
                }
                else
                {
                    Console.WriteLine("No active network interface found.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }

        static void Main(string[] args)
        {
            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard");

                foreach (ManagementObject obj in searcher.Get())
                {
                    string motherboardSerial = obj["SerialNumber"].ToString();

                    Console.WriteLine("Motherboard Serial Number: " + motherboardSerial);
                }
                SKibidiSIgma();
            }
            catch (ManagementException e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }
    }
}
