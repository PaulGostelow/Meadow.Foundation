﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Web.Maple.Server;
using Meadow.Gateway.WiFi;

namespace Maple.ServerSimpleMeadow_Sample
{
    public class MeadowApp : App<F7MicroV2, MeadowApp>
    {
        MapleServer server;

        public MeadowApp()
        {
            //Thread.Sleep(5000);

            Initialize().Wait();
            server.Start();
        }

        async Task Initialize()
        {
            Console.WriteLine("Initialize hardware...");

            // connnect to the wifi network.
            Console.WriteLine($"Connecting to WiFi Network {Secrets.WIFI_NAME}");
            var connectionResult = await Device.WiFiAdapter.Connect(Secrets.WIFI_NAME, Secrets.WIFI_PASSWORD);
            if (connectionResult.ConnectionStatus != ConnectionStatus.Success) {
                throw new Exception($"Cannot connect to network: {connectionResult.ConnectionStatus}");
            }
            Console.WriteLine($"Connected. IP: {Device.WiFiAdapter.IpAddress}");

            // create our maple web server
            server = new MapleServer(
                Device.WiFiAdapter.IpAddress,
                advertise: true,
                processMode: RequestProcessMode.Parallel
                );

            Console.WriteLine("Finished initialization.");
        }
    }
}