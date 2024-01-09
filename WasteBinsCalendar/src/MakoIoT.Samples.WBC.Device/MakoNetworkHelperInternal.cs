// Decompiled with JetBrains decompiler
// Type: nanoFramework.Networking.MakoNetworkHelperInternal
// Assembly: System.Net, Version=1.10.64.0, Culture=neutral, PublicKeyToken=c07d481e9758c731
// MVID: CF031AFF-5ECD-4C34-86E4-D2AB3ADD44C5
// Assembly location: C:\CSHARK\Code\MAKO-IoT\Mako-IoT.Samples\WasteBinsCalendar\packages\nanoFramework.System.Net.1.10.64\lib\System.Net.dll
// XML documentation location: C:\CSHARK\Code\MAKO-IoT\Mako-IoT.Samples\WasteBinsCalendar\packages\nanoFramework.System.Net.1.10.64\lib\System.Net.xml

using System;
using System.Net.NetworkInformation;
using System.Threading;
using nanoFramework.Networking;

namespace MakoIoT.Samples.WBC.Device
{
    /// <summary>
    /// Network helper class providing helper methods to assist on connecting to a network.
    /// </summary>
    internal static class MakoNetworkHelperInternal
    {
        /// <summary>
        /// Checks if there is an IPAddress assigned to the specified network interface.
        /// </summary>
        public static bool CheckIP(NetworkInterfaceType interfaceType, IPConfiguration _ipConfiguration)
        {
            foreach (NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (networkInterface.NetworkInterfaceType == interfaceType && networkInterface.IPv4Address[0] != '0')
                    return _ipConfiguration == null || !(_ipConfiguration.IPAddress != networkInterface.IPv4Address);
            }
            return false;
        }

        /// <summary>
        /// Checks and waits until a valid DateTime is set on the system.
        /// </summary>
        public static void WaitForValidDateTime()
        {
            while (DateTime.UtcNow.Year < 2021)
                Thread.Sleep(1000);
        }

        public static void InternalSetupHelper(
          NetworkInterface[] nis,
          NetworkInterfaceType targetNetworkInterface,
          IPConfiguration ipConfiguration)
        {
            foreach (NetworkInterface ni in nis)
            {
                if (ni.NetworkInterfaceType == targetNetworkInterface)
                {
                    if (ipConfiguration != null)
                    {
                        ni.EnableStaticIPv4(ipConfiguration.IPAddress, ipConfiguration.IPSubnetMask, ipConfiguration.IPGatewayAddress);
                        if (ipConfiguration.IPDns != null && ipConfiguration.IPDns.Length != 0)
                            ni.EnableStaticIPv4Dns(ipConfiguration.IPDns);
                    }
                    else if (ni.IsDhcpEnabled)
                    {
                        ni.EnableDhcp();
                    }
                    else
                    {
                        ni.EnableStaticIPv4(ni.IPv4Address, ni.IPv4SubnetMask, ni.IPv4GatewayAddress);
                        if (ni.IPv4DnsAddresses.Length != 0)
                            ni.EnableStaticIPv4Dns(ni.IPv4DnsAddresses);
                    }
                }
            }
        }
    }
}
