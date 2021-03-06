// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace System.Net.NetworkInformation.Tests
{
    internal static class TestSettings
    {
        public static readonly string LocalHost = "localhost";
        public const int PingTimeout = 10 * 1000;

        public const string PayloadAsString = "'Post hoc ergo propter hoc'. 'After it, therefore because of it'. It means one thing follows the other, therefore it was caused by the other. But it's not always true. In fact it's hardly ever true.";
        public static readonly byte[] PayloadAsBytes = Encoding.UTF8.GetBytes(TestSettings.PayloadAsString);

        public static IPAddress GetLocalIPAddress(AddressFamily addressFamily = AddressFamily.Unspecified)
        {
            IPHostEntry hostEntry = Dns.GetHostEntry(LocalHost);
            return GetIPAddressForHost(hostEntry, addressFamily);
        }

        public static async Task<IPAddress> GetLocalIPAddressAsync(AddressFamily addressFamily = AddressFamily.Unspecified)
        {
            IPHostEntry hostEntry = await Dns.GetHostEntryAsync(LocalHost);
            return GetIPAddressForHost(hostEntry, addressFamily);
        }

        private static IPAddress GetIPAddressForHost(IPHostEntry hostEntry, AddressFamily addressFamily = AddressFamily.Unspecified)
        {
            foreach (IPAddress address in hostEntry.AddressList)
            {
                if (address.AddressFamily == addressFamily || (addressFamily == AddressFamily.Unspecified && address.AddressFamily == AddressFamily.InterNetworkV6))
                {
                    return address;
                }
            }

            // If there's no IPv6 addresses, just take the first (IPv4) address.
            if (addressFamily == AddressFamily.Unspecified)
            {
                if (hostEntry.AddressList.Length > 0)
                {
                    return hostEntry.AddressList[0];
                }

                throw new InvalidOperationException("Unable to discover any addresses for the local host.");
            }

            return addressFamily == AddressFamily.InterNetwork ? IPAddress.Loopback : IPAddress.IPv6Loopback;
        }
    }
}
