// Decompiled with JetBrains decompiler
// Type: nanoFramework.Networking.MakoNetworkHelper
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
    public static class MakoNetworkHelper
    {
        private static ManualResetEvent _ipAddressAvailable;
        private static ManualResetEvent _networkReady = new ManualResetEvent(false);
        private static bool _requiresDateTime;
        private static NetworkHelperStatus _networkHelperStatus = NetworkHelperStatus.None;
        private static Exception _helperException;
        private static NetworkInterfaceType _workingNetworkInterface = NetworkInterfaceType.Ethernet;
        private static IPConfiguration _ipConfiguration;
        /// <summary>
        /// This flag will make sure there is only one and only call to any of the helper methods.
        /// </summary>
        private static bool _helperInstanciated = false;

        /// <summary>Event signaling that networking it's ready.</summary>
        /// <remarks>
        /// The conditions for this are setup in the call to <see cref="M:MakoIoT.Samples.WBC.Device.MakoNetworkHelper.SetupNetworkHelper(System.Boolean)" />.
        /// It will be a composition of network connected, IpAddress available and valid system <see cref="T:System.DateTime" />.</remarks>
        public static ManualResetEvent NetworkReady => MakoNetworkHelper._networkReady;

        /// <summary>Status of MakoNetworkHelper.</summary>
        public static NetworkHelperStatus Status => MakoNetworkHelper._networkHelperStatus;

        /// <summary>
        /// Exception that occurred when waiting for the network to become ready.
        /// </summary>
        public static Exception HelperException
        {
            get => MakoNetworkHelper._helperException;
            internal set => MakoNetworkHelper._helperException = value;
        }

        /// <summary>
        /// This method will setup the network configurations so that the <see cref="P:MakoIoT.Samples.WBC.Device.MakoNetworkHelper.NetworkReady" /> event will fire when the required conditions are met.
        /// That will be the network connection to be up, having a valid IpAddress and optionally for a valid date and time to become available.
        /// </summary>
        /// <param name="requiresDateTime">Set to <see langword="true" /> if valid date and time are required.</param>
        /// <exception cref="T:System.InvalidOperationException">If any of the <see cref="T:MakoIoT.Samples.WBC.Device.MakoNetworkHelper" /> methods is called more than once.</exception>
        /// <exception cref="T:System.NotSupportedException">There is no network interface configured. Open the 'Edit Network Configuration' in Device Explorer and configure one.</exception>
        public static void SetupNetworkHelper(bool requiresDateTime = false)
        {
            MakoNetworkHelper._requiresDateTime = requiresDateTime;
            MakoNetworkHelper.SetupHelper(true);
            new Thread(new ThreadStart(MakoNetworkHelper.WorkingThread)).Start();
        }

        /// <summary>
        /// This method will setup the network configurations so that the <see cref="P:MakoIoT.Samples.WBC.Device.MakoNetworkHelper.NetworkReady" /> event will fire when the required conditions are met.
        /// That will be the network connection to be up, having a valid IpAddress and optionally for a valid date and time to become available.
        /// </summary>
        /// <param name="ipConfiguration">The static IP configuration you want to apply.</param>
        /// <param name="requiresDateTime">Set to <see langword="true" /> if valid date and time are required.</param>
        /// <exception cref="T:System.NotSupportedException">There is no network interface configured. Open the 'Edit Network Configuration' in Device Explorer and configure one.</exception>
        public static void SetupNetworkHelper(IPConfiguration ipConfiguration, bool requiresDateTime = false)
        {
            MakoNetworkHelper._requiresDateTime = requiresDateTime;
            MakoNetworkHelper._ipConfiguration = ipConfiguration;
            MakoNetworkHelper.SetupHelper(true);
            new Thread(new ThreadStart(MakoNetworkHelper.WorkingThread)).Start();
        }

        /// <summary>
        /// This will wait for the network connection to be up and optionally for a valid date and time to become available.
        /// </summary>
        /// <param name="token">A <see cref="T:System.Threading.CancellationToken" /> used for timing out the operation.</param>
        /// <param name="requiresDateTime">Set to <see langword="true" /> if valid date and time are required.</param>
        /// <returns><see langword="true" /> on success. On failure returns <see langword="false" /> and details with the cause of the failure are made available in the <see cref="P:MakoIoT.Samples.WBC.Device.MakoNetworkHelper.Status" /> property</returns>
        public static bool SetupAndConnectNetwork(CancellationToken token = default(CancellationToken), bool requiresDateTime = false) => MakoNetworkHelper.SetupAndConnectNetwork((IPConfiguration)null, token, requiresDateTime);

        /// <summary>
        /// This will wait for the network connection to be up and optionally for a valid date and time to become available.
        /// </summary>
        /// <param name="ipConfiguration">The static IPv4 configuration to apply to the Ethernet network interface.</param>
        /// <param name="token">A <see cref="T:System.Threading.CancellationToken" /> used for timing out the operation.</param>
        /// <param name="requiresDateTime">Set to <see langword="true" /> if valid date and time are required.</param>
        /// <returns><see langword="true" /> on success. On failure returns <see langword="false" /> and details with the cause of the failure are made available in the <see cref="P:MakoIoT.Samples.WBC.Device.MakoNetworkHelper.Status" /> property</returns>
        public static bool SetupAndConnectNetwork(
          IPConfiguration ipConfiguration,
          CancellationToken token = default(CancellationToken),
          bool requiresDateTime = false)
        {
            MakoNetworkHelper._ipConfiguration = ipConfiguration;
            return MakoNetworkHelper.InternalWaitNetworkAvailable(MakoNetworkHelper._workingNetworkInterface, ref MakoNetworkHelper._networkHelperStatus, false, token, requiresDateTime);
        }

        internal static bool InternalWaitNetworkAvailable(
          NetworkInterfaceType networkInterface,
          ref NetworkHelperStatus helperStatus,
          bool setupEvents,
          CancellationToken token = default(CancellationToken),
          bool requiresDateTime = false)
        {
            try
            {
                MakoNetworkHelper.SetupHelper(setupEvents);
                while (!token.IsCancellationRequested && !MakoNetworkHelperInternal.CheckIP(networkInterface, MakoNetworkHelper._ipConfiguration))
                    Thread.Sleep(200);
                if (token.IsCancellationRequested)
                {
                    helperStatus = NetworkHelperStatus.TokenExpiredWaitingIPAddress;
                    return false;
                }
                if (requiresDateTime)
                {
                    while (!token.IsCancellationRequested && DateTime.UtcNow.Year < 2021)
                        Thread.Sleep(200);
                }
                if (token.IsCancellationRequested)
                {
                    helperStatus = NetworkHelperStatus.TokenExpiredWaitingDateTime;
                    return false;
                }
                helperStatus = NetworkHelperStatus.NetworkIsReady;
                return true;
            }
            catch (Exception ex)
            {
                MakoNetworkHelper._networkHelperStatus = NetworkHelperStatus.ExceptionOccurred;
                MakoNetworkHelper.HelperException = ex;
                return false;
            }
        }

        private static void WorkingThread()
        {
            if (!MakoNetworkHelperInternal.CheckIP(MakoNetworkHelper._workingNetworkInterface, MakoNetworkHelper._ipConfiguration))
                MakoNetworkHelper._ipAddressAvailable.WaitOne();
            if (MakoNetworkHelper._requiresDateTime)
                MakoNetworkHelperInternal.WaitForValidDateTime();
            MakoNetworkHelper._networkReady.Set();
            MakoNetworkHelper._networkHelperStatus = NetworkHelperStatus.NetworkIsReady;
        }

        private static void AddressChangedCallback(object sender, EventArgs e)
        {
            if (!MakoNetworkHelperInternal.CheckIP(MakoNetworkHelper._workingNetworkInterface, MakoNetworkHelper._ipConfiguration))
                return;
            MakoNetworkHelper._ipAddressAvailable.Set();
        }

        /// <summary>
        /// Perform setup of the various fields and events, along with any of the required event handlers.
        /// </summary>
        /// <param name="setupEvents">Set true to setup the events. Required for the thread approach. Not required for the CancelationToken implementation.</param>
        private static void SetupHelper(bool setupEvents)
        {
            MakoNetworkHelper._helperInstanciated = !MakoNetworkHelper._helperInstanciated ? true : throw new InvalidOperationException();
            MakoNetworkHelper._ipAddressAvailable = new ManualResetEvent(false);
            NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            if (setupEvents)
            {
                if (networkInterfaces.Length == 0)
                {
                    MakoNetworkHelper._networkHelperStatus = NetworkHelperStatus.FailedNoNetworkInterface;
                    throw new NotSupportedException();
                }
                NetworkChange.NetworkAddressChanged += new NetworkAddressChangedEventHandler(MakoNetworkHelper.AddressChangedCallback);
            }
            MakoNetworkHelperInternal.InternalSetupHelper(networkInterfaces, MakoNetworkHelper._workingNetworkInterface, MakoNetworkHelper._ipConfiguration);
            MakoNetworkHelper._networkHelperStatus = NetworkHelperStatus.Started;
        }

        /// <summary>
        /// Method to reset internal fields to it's defaults
        /// ONLY TO BE USED BY UNIT TESTS
        /// </summary>
        internal static void ResetInstance()
        {
            MakoNetworkHelper._ipAddressAvailable = (ManualResetEvent)null;
            MakoNetworkHelper._networkReady = new ManualResetEvent(false);
            MakoNetworkHelper._requiresDateTime = false;
            MakoNetworkHelper._networkHelperStatus = NetworkHelperStatus.None;
            MakoNetworkHelper._helperException = (Exception)null;
            MakoNetworkHelper._ipConfiguration = (IPConfiguration)null;
            MakoNetworkHelper._helperInstanciated = false;
        }
    }
}
