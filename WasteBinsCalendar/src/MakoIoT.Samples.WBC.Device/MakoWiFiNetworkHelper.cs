// Decompiled with JetBrains decompiler
// Type: nanoFramework.Networking.MakoWifiNetworkHelper
// Assembly: System.Device.Wifi, Version=1.5.71.0, Culture=neutral, PublicKeyToken=c07d481e9758c731
// MVID: F83C7E3E-F765-47C2-9934-6FFC408D4C3E
// Assembly location: C:\CSHARK\Code\MAKO-IoT\Mako-IoT.Samples\WasteBinsCalendar\packages\nanoFramework.System.Device.Wifi.1.5.71\lib\System.Device.Wifi.dll
// XML documentation location: C:\CSHARK\Code\MAKO-IoT\Mako-IoT.Samples\WasteBinsCalendar\packages\nanoFramework.System.Device.Wifi.1.5.71\lib\System.Device.Wifi.xml

using System;
using System.Device.Wifi;
using System.Net.NetworkInformation;
using System.Threading;
using nanoFramework.Networking;

namespace MakoIoT.Samples.WBC.Device
{
    /// <summary>
    /// Network helper class providing helper methods to assist on connecting to a Wifi network.
    /// </summary>
    public static class MakoWifiNetworkHelper
    {
        private static ManualResetEvent _ipAddressAvailable;
        private static ManualResetEvent _networkReady = new ManualResetEvent(false);
        private static bool _requiresDateTime;
        private static string _ssid;
        private static string _password;
        private static WifiAdapter _wifi = (WifiAdapter)null;
        private static WifiReconnectionKind _reconnectionKind;
        private static IPConfiguration _ipConfiguration;
        private static NetworkHelperStatus _networkHelperStatus = NetworkHelperStatus.None;
        private static Exception _helperException;
        private static NetworkInterfaceType _workingNetworkInterface = NetworkInterfaceType.Wireless80211;
        /// <summary>
        /// This flag will make sure there is only one and only call to any of the helper methods.
        /// </summary>
        private static bool _helperInstanciated = false;

        /// <summary>Event signaling that networking it's ready.</summary>
        /// <remarks>
        /// The conditions for this are setup in the call to <see cref="M:MakoIoT.Samples.WBC.Device.MakoWifiNetworkHelper.SetupNetworkHelper(System.Boolean)" />.
        /// It will be a composition of network connected, IpAddress available and valid system <see cref="T:System.DateTime" />.</remarks>
        public static ManualResetEvent NetworkReady => MakoWifiNetworkHelper._networkReady;

        /// <summary>Status of MakoNetworkHelper.</summary>
        public static NetworkHelperStatus Status => MakoWifiNetworkHelper._networkHelperStatus;

        /// <summary>
        /// Exception that occurred when waiting for the network to become ready.
        /// </summary>
        public static Exception HelperException
        {
            get => MakoWifiNetworkHelper._helperException;
            internal set => MakoWifiNetworkHelper._helperException = value;
        }

        /// <summary>
        /// This method will setup the network configurations so that the <see cref="P:MakoIoT.Samples.WBC.Device.MakoWifiNetworkHelper.NetworkReady" /> event will fire when the required conditions are met.
        /// That will be the network connection to be up, having a valid IpAddress and optionally for a valid date and time to become available.
        /// </summary>
        /// <param name="requiresDateTime">Set to <see langword="true" /> if valid date and time are required.</param>
        /// <exception cref="T:System.InvalidOperationException">If any of the <see cref="T:nanoFramework.Networking.MakoNetworkHelper" /> methods is called more than once.</exception>
        /// <exception cref="T:System.NotSupportedException">There is no network interface configured. Open the 'Edit Network Configuration' in Device Explorer and configure one.</exception>
        public static void SetupNetworkHelper(bool requiresDateTime = false)
        {
            MakoWifiNetworkHelper._requiresDateTime = requiresDateTime;
            MakoWifiNetworkHelper.SetupHelper(true);
            new Thread(new ThreadStart(MakoWifiNetworkHelper.WorkingThread)).Start();
        }

        /// <summary>
        /// This method will setup the network configurations so that the <see cref="P:MakoIoT.Samples.WBC.Device.MakoWifiNetworkHelper.NetworkReady" /> event will fire when the required conditions are met.
        /// That will be the network connection to be up, having a valid IpAddress and optionally for a valid date and time to become available.
        /// </summary>
        /// <param name="ipConfiguration">The static IP configuration you want to apply.</param>
        /// <param name="requiresDateTime">Set to <see langword="true" /> if valid date and time are required.</param>
        /// <exception cref="T:System.NotSupportedException">There is no network interface configured. Open the 'Edit Network Configuration' in Device Explorer and configure one.</exception>
        public static void SetupNetworkHelper(IPConfiguration ipConfiguration, bool requiresDateTime = false)
        {
            MakoWifiNetworkHelper._requiresDateTime = requiresDateTime;
            MakoWifiNetworkHelper._ipConfiguration = ipConfiguration;
            MakoWifiNetworkHelper.SetupHelper(true);
            new Thread(new ThreadStart(MakoWifiNetworkHelper.WorkingThread)).Start();
        }

        /// <summary>
        /// This method will setup the network configurations so that the <see cref="P:MakoIoT.Samples.WBC.Device.MakoWifiNetworkHelper.NetworkReady" /> event will fire when the required conditions are met.
        /// That will be the network connection to be up, having a valid IpAddress and optionally for a valid date and time to become available.
        /// </summary>
        /// <param name="requiresDateTime">Set to <see langword="true" /> if valid date and time are required.</param>
        /// <param name="ssid">The SSID of the network you are trying to connect to.</param>
        /// <param name="password">The password associated to the SSID of the network you are trying to connect to.</param>
        /// <param name="reconnectionKind">The <see cref="T:System.Device.Wifi.WifiReconnectionKind" /> to setup for the connection.</param>
        /// <exception cref="T:System.InvalidOperationException">If any of the <see cref="T:nanoFramework.Networking.MakoNetworkHelper" /> methods is called more than once.</exception>
        /// <exception cref="T:System.NotSupportedException">There is no network interface configured. Open the 'Edit Network Configuration' in Device Explorer and configure one.</exception>
        public static void SetupNetworkHelper(
          string ssid,
          string password,
          WifiReconnectionKind reconnectionKind = WifiReconnectionKind.Automatic,
          bool requiresDateTime = false)
        {
            MakoWifiNetworkHelper._requiresDateTime = requiresDateTime;
            MakoWifiNetworkHelper._ssid = ssid;
            MakoWifiNetworkHelper._password = password;
            MakoWifiNetworkHelper._reconnectionKind = reconnectionKind;
            MakoWifiNetworkHelper.SetupHelper(true);
            new Thread(new ThreadStart(MakoWifiNetworkHelper.WorkingThread)).Start();
        }

        /// <summary>Disconnect from the network.</summary>
        public static void Disconnect()
        {
            MakoWifiNetworkHelper._wifi?.Disconnect();
            MakoWifiNetworkHelper._wifi?.Dispose();
            MakoWifiNetworkHelper._wifi = (WifiAdapter)null;
        }

        /// <summary>Gets the Wifi Adapter.</summary>
        public static WifiAdapter WifiAdapter => MakoWifiNetworkHelper._wifi;

        /// <summary>
        /// This method will connect the network with DHCP enabled, for your SSID and try to connect to it with the credentials you've passed. This will save as well
        /// the configuration of your network.
        /// </summary>
        /// <param name="ssid">The SSID of the network you are trying to connect to.</param>
        /// <param name="password">The password associated to the SSID of the network you are trying to connect to.</param>
        /// <param name="reconnectionKind">The <see cref="T:System.Device.Wifi.WifiReconnectionKind" /> to setup for the connection.</param>
        /// <param name="requiresDateTime">Set to <see langword="true" /> if valid date and time are required.</param>
        /// <param name="wifiAdapterId">The index of the Wifi adapter to be used. Relevant only if there are multiple Wifi adapters.</param>
        /// <param name="token">A <see cref="T:System.Threading.CancellationToken" /> used for timing out the operation.</param>
        /// <returns><see langword="true" /> on success. On failure returns <see langword="false" /> and details with the cause of the failure are made available in the <see cref="P:MakoIoT.Samples.WBC.Device.MakoWifiNetworkHelper.Status" /> property</returns>
        public static bool ConnectDhcp(
          string ssid,
          string password,
          WifiReconnectionKind reconnectionKind = WifiReconnectionKind.Automatic,
          bool requiresDateTime = false,
          int wifiAdapterId = 0,
          CancellationToken token = default(CancellationToken))
        {
            return MakoWifiNetworkHelper.ScanAndConnect(ssid, password, (IPConfiguration)null, false, reconnectionKind, requiresDateTime, wifiAdapterId, token);
        }

        /// <summary>
        /// This method will connect the network with the static IP address you are providing, for your SSID and try to connect to it with the credentials you've passed. This will save as well
        /// the configuration of your network.
        /// </summary>
        /// <param name="ssid">The SSID you are trying to connect to.</param>
        /// <param name="password">The password associated to the SSID you are trying to connect to.</param>
        /// <param name="ipConfiguration">The static IPv4 configuration to apply to the Wifi network interface.</param>
        /// <param name="reconnectionKind">The <see cref="T:System.Device.Wifi.WifiReconnectionKind" /> to setup for the connection.</param>
        /// <param name="requiresDateTime">Set to <see langword="true" /> if valid date and time are required.</param>
        /// <param name="WifiAdapter">The index of the Wifi adapter to be used. Relevant only if there are multiple Wifi adapters.</param>
        /// <param name="token">A <see cref="T:System.Threading.CancellationToken" /> used for timing out the operation.</param>
        /// <returns><see langword="true" /> on success. On failure returns <see langword="false" /> and details with the cause of the failure are made available in the <see cref="P:MakoIoT.Samples.WBC.Device.MakoWifiNetworkHelper.Status" /> property</returns>
        public static bool ConnectFixAddress(
          string ssid,
          string password,
          IPConfiguration ipConfiguration,
          WifiReconnectionKind reconnectionKind = WifiReconnectionKind.Automatic,
          bool requiresDateTime = false,
          int WifiAdapter = 0,
          CancellationToken token = default(CancellationToken))
        {
            return MakoWifiNetworkHelper.ScanAndConnect(ssid, password, ipConfiguration, false, reconnectionKind, requiresDateTime, WifiAdapter, token);
        }

        /// <summary>
        /// This method will scan and connect the network with DHCP enabled, for your SSID and try to connect to it with the credentials you've passed. This will save as well
        /// the configuration of your network.
        /// </summary>
        /// <param name="ssid">The SSID you are trying to connect to.</param>
        /// <param name="password">The password associated to the SSID you are trying to connect to.</param>
        /// <param name="reconnectionKind">The reconnection type to the network.</param>
        /// <param name="requiresDateTime">Set to <see langword="true" /> if valid date and time are required.</param>
        /// <param name="WifiAdapter">The index of the Wifi adapter to be used. Relevant only if there are multiple Wifi adapters.</param>
        /// <param name="token">A <see cref="T:System.Threading.CancellationToken" /> used for timing out the operation.</param>
        /// <returns><see langword="true" /> on success. On failure returns <see langword="false" /> and details with the cause of the failure are made available in the <see cref="P:MakoIoT.Samples.WBC.Device.MakoWifiNetworkHelper.Status" /> property</returns>
        public static bool ScanAndConnectDhcp(
          string ssid,
          string password,
          WifiReconnectionKind reconnectionKind = WifiReconnectionKind.Automatic,
          bool requiresDateTime = false,
          int WifiAdapter = 0,
          CancellationToken token = default(CancellationToken))
        {
            return MakoWifiNetworkHelper.ScanAndConnect(ssid, password, (IPConfiguration)null, true, reconnectionKind, requiresDateTime, WifiAdapter, token);
        }

        /// <summary>
        /// This method will connect the network, the information used to connect is the one already stored on the device.
        /// </summary>
        /// <param name="requiresDateTime">Set to <see langword="true" /> if valid date and time are required.</param>
        /// <param name="wifiAdapterId">The index of the Wifi adapter to be used. Relevant only if there are multiple Wifi adapters.</param>
        /// <param name="token">A <see cref="T:System.Threading.CancellationToken" /> used for timing out the operation.</param>
        /// <returns><see langword="true" /> on success. On failure returns <see langword="false" /> and details with the cause of the failure are made available in the <see cref="P:MakoIoT.Samples.WBC.Device.MakoWifiNetworkHelper.Status" /> property</returns>
        /// <remarks>
        /// This function can be called only if an existing network has been setup previously and if the credentials are valid.
        /// </remarks>
        public static bool Reconnect(bool requiresDateTime = false, int wifiAdapterId = 0, CancellationToken token = default(CancellationToken))
        {
            try
            {
                MakoWifiNetworkHelper._wifi = WifiAdapter.FindAllAdapters()[wifiAdapterId];
                return MakoNetworkHelper.InternalWaitNetworkAvailable(MakoWifiNetworkHelper._workingNetworkInterface, ref MakoWifiNetworkHelper._networkHelperStatus, false, token, requiresDateTime);
            }
            catch (Exception ex)
            {
                MakoWifiNetworkHelper._networkHelperStatus = NetworkHelperStatus.ExceptionOccurred;
                MakoWifiNetworkHelper.HelperException = ex;
                return false;
            }
        }

        private static bool ScanAndConnect(
          string ssid,
          string password,
          IPConfiguration ipConfiguration,
          bool scan,
          WifiReconnectionKind reconnectionKind,
          bool setDateTime,
          int wifiAdapterId,
          CancellationToken token)
        {
            try
            {
                MakoWifiNetworkHelper._ssid = ssid;
                MakoWifiNetworkHelper._password = password;
                MakoWifiNetworkHelper._wifi = WifiAdapter.FindAllAdapters()[wifiAdapterId];
                MakoWifiNetworkHelper._reconnectionKind = reconnectionKind;
                MakoWifiNetworkHelper._ipConfiguration = ipConfiguration;
                bool flag = false;
                MakoWifiNetworkHelper._workingNetworkInterface = NetworkInterfaceType.Wireless80211;
                if (MakoNetworkHelperInternal.CheckIP(MakoWifiNetworkHelper._workingNetworkInterface, MakoWifiNetworkHelper._ipConfiguration))
                {
                    foreach (NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces())
                    {
                        if (networkInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 && Wireless80211Configuration.GetAllWireless80211Configurations()[(int)networkInterface.SpecificConfigId].Ssid == MakoWifiNetworkHelper._ssid)
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (!flag)
                    {
                        MakoWifiNetworkHelper._wifi.Disconnect();
                        flag = false;
                    }
                }
                if (!flag)
                {
                    if (scan)
                    {
                        MakoWifiNetworkHelper._wifi.AvailableNetworksChanged += new AvailableNetworksChangedEventHandler(MakoWifiNetworkHelper.WifiAvailableNetworksChanged);
                        MakoWifiNetworkHelper._wifi.ScanAsync();
                    }
                    else
                    {
                        foreach (NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces())
                        {
                            if (networkInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                            {
                                MakoWifiNetworkHelper._wifi.Disconnect();
                                MakoWifiNetworkHelper.StoreWifiConfiguration(networkInterface);
                                if (MakoWifiNetworkHelper._ipConfiguration != null)
                                {
                                    networkInterface.EnableStaticIPv4(MakoWifiNetworkHelper._ipConfiguration.IPAddress, MakoWifiNetworkHelper._ipConfiguration.IPSubnetMask, MakoWifiNetworkHelper._ipConfiguration.IPGatewayAddress);
                                    if (MakoWifiNetworkHelper._ipConfiguration.IPDns != null && MakoWifiNetworkHelper._ipConfiguration.IPDns.Length != 0)
                                        networkInterface.EnableStaticIPv4Dns(MakoWifiNetworkHelper._ipConfiguration.IPDns);
                                }
                                else if (networkInterface.IsDhcpEnabled)
                                {
                                    networkInterface.EnableDhcp();
                                }
                                else
                                {
                                    networkInterface.EnableStaticIPv4(networkInterface.IPv4Address, networkInterface.IPv4SubnetMask, networkInterface.IPv4GatewayAddress);
                                    if (networkInterface.IPv4DnsAddresses.Length != 0)
                                        networkInterface.EnableStaticIPv4Dns(networkInterface.IPv4DnsAddresses);
                                }
                                MakoWifiNetworkHelper._wifi.Connect(MakoWifiNetworkHelper._ssid, MakoWifiNetworkHelper._reconnectionKind, MakoWifiNetworkHelper._password);
                                break;
                            }
                        }
                    }
                }
                return MakoNetworkHelper.InternalWaitNetworkAvailable(MakoWifiNetworkHelper._workingNetworkInterface, ref MakoWifiNetworkHelper._networkHelperStatus, false, token, setDateTime);
            }
            catch (Exception ex)
            {
                MakoWifiNetworkHelper._networkHelperStatus = NetworkHelperStatus.ExceptionOccurred;
                MakoWifiNetworkHelper.HelperException = ex;
                return false;
            }
        }

        private static void StoreWifiConfiguration(NetworkInterface ni)
        {
            Wireless80211Configuration wireless80211Configuration = Wireless80211Configuration.GetAllWireless80211Configurations()[(int)ni.SpecificConfigId];
            wireless80211Configuration.Ssid = MakoWifiNetworkHelper._ssid;
            wireless80211Configuration.Password = MakoWifiNetworkHelper._password;
            wireless80211Configuration.SaveConfiguration();
        }

        private static void WifiAvailableNetworksChanged(WifiAdapter sender, object e)
        {
            foreach (WifiAvailableNetwork availableNetwork in sender.NetworkReport.AvailableNetworks)
            {
                if (availableNetwork.Ssid == MakoWifiNetworkHelper._ssid)
                {
                    sender.Disconnect();
                    NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
                    NetworkInterface ni = (NetworkInterface)null;
                    foreach (NetworkInterface networkInterface in networkInterfaces)
                    {
                        if (networkInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                        {
                            ni = networkInterface;
                            break;
                        }
                    }
                    if (ni == null)
                        break;
                    ni.EnableAutomaticDns();
                    if (MakoWifiNetworkHelper._ipConfiguration == null)
                        ni.EnableDhcp();
                    else
                        ni.EnableStaticIPv4(MakoWifiNetworkHelper._ipConfiguration.IPAddress, MakoWifiNetworkHelper._ipConfiguration.IPSubnetMask, MakoWifiNetworkHelper._ipConfiguration.IPGatewayAddress);
                    if (sender.Connect(availableNetwork, MakoWifiNetworkHelper._reconnectionKind, MakoWifiNetworkHelper._password).ConnectionStatus == WifiConnectionStatus.Success)
                    {
                        sender.AvailableNetworksChanged -= new AvailableNetworksChangedEventHandler(MakoWifiNetworkHelper.WifiAvailableNetworksChanged);
                        MakoWifiNetworkHelper.StoreWifiConfiguration(ni);
                        break;
                    }
                    MakoWifiNetworkHelper._networkHelperStatus = NetworkHelperStatus.ErrorConnetingToWiFiWhileScanning;
                }
            }
        }

        private static void WorkingThread()
        {
            if (!MakoNetworkHelperInternal.CheckIP(MakoWifiNetworkHelper._workingNetworkInterface, MakoWifiNetworkHelper._ipConfiguration))
                MakoWifiNetworkHelper._ipAddressAvailable.WaitOne();
            if (MakoWifiNetworkHelper._requiresDateTime)
                MakoNetworkHelperInternal.WaitForValidDateTime();
            MakoWifiNetworkHelper._networkReady.Set();
            MakoWifiNetworkHelper._networkHelperStatus = NetworkHelperStatus.NetworkIsReady;
        }

        /// <summary>
        /// Perform setup of the various fields and events, along with any of the required event handlers.
        /// </summary>
        /// <param name="setupEvents">Set true to setup the events. Required for the thread approach. Not required for the CancelationToken implementation.</param>
        private static void SetupHelper(bool setupEvents)
        {
            MakoWifiNetworkHelper._helperInstanciated = !MakoWifiNetworkHelper._helperInstanciated ? true : throw new InvalidOperationException();
            bool flag1 = false;
            MakoWifiNetworkHelper._ipAddressAvailable = new ManualResetEvent(false);
            MakoWifiNetworkHelper._wifi = WifiAdapter.FindAllAdapters()[0];
            NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            if (setupEvents)
            {
                if (networkInterfaces.Length == 0)
                {
                    MakoWifiNetworkHelper._networkHelperStatus = NetworkHelperStatus.FailedNoNetworkInterface;
                    throw new NotSupportedException();
                }
                NetworkChange.NetworkAddressChanged += new NetworkAddressChangedEventHandler(MakoWifiNetworkHelper.AddressChangedCallback);
            }
            if (!string.IsNullOrEmpty(MakoWifiNetworkHelper._ssid) && !string.IsNullOrEmpty(MakoWifiNetworkHelper._password))
            {
                bool flag2 = false;
                foreach (NetworkInterface networkInterface in networkInterfaces)
                {
                    if (networkInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 && Wireless80211Configuration.GetAllWireless80211Configurations()[(int)networkInterface.SpecificConfigId].Ssid == MakoWifiNetworkHelper._ssid)
                    {
                        flag2 = true;
                        break;
                    }
                }
                if (!flag2)
                {
                    MakoWifiNetworkHelper._wifi.Disconnect();
                    flag2 = false;
                }
                if (!flag2)
                {
                    networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
                    foreach (NetworkInterface ni in networkInterfaces)
                    {
                        if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                        {
                            MakoWifiNetworkHelper._wifi.Disconnect();
                            MakoWifiNetworkHelper.StoreWifiConfiguration(ni);
                            flag1 = true;
                            break;
                        }
                    }
                }
            }
            MakoNetworkHelperInternal.InternalSetupHelper(networkInterfaces, MakoWifiNetworkHelper._workingNetworkInterface, MakoWifiNetworkHelper._ipConfiguration);
            if (flag1)
                MakoWifiNetworkHelper._wifi.Connect(MakoWifiNetworkHelper._ssid, MakoWifiNetworkHelper._reconnectionKind, MakoWifiNetworkHelper._password);
            MakoWifiNetworkHelper._networkHelperStatus = NetworkHelperStatus.Started;
        }

        private static void AddressChangedCallback(object sender, EventArgs e)
        {
            if (!MakoNetworkHelperInternal.CheckIP(MakoWifiNetworkHelper._workingNetworkInterface, MakoWifiNetworkHelper._ipConfiguration))
                return;
            MakoWifiNetworkHelper._ipAddressAvailable.Set();
        }

        /// <summary>
        /// Method to reset internal fields to it's defaults
        /// ONLY TO BE USED BY UNIT TESTS
        /// </summary>
        internal static void ResetInstance()
        {
            MakoWifiNetworkHelper._ipAddressAvailable = (ManualResetEvent)null;
            MakoWifiNetworkHelper._networkReady = new ManualResetEvent(false);
            MakoWifiNetworkHelper._requiresDateTime = false;
            MakoWifiNetworkHelper._networkHelperStatus = NetworkHelperStatus.None;
            MakoWifiNetworkHelper._helperException = (Exception)null;
            MakoWifiNetworkHelper._ipConfiguration = (IPConfiguration)null;
            MakoWifiNetworkHelper._helperInstanciated = false;
            MakoWifiNetworkHelper._ssid = (string)null;
            MakoWifiNetworkHelper._password = (string)null;
            MakoNetworkHelper.ResetInstance();
        }
    }
}
