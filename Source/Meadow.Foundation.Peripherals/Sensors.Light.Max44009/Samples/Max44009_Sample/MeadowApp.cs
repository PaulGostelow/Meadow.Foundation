﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Sensors.Light;

namespace Sensors.Light.Max44009_Sample
{
    public class MeadowApp : App<F7MicroV2, MeadowApp>
    {
        //<!—SNIP—>

        Max44009 sensor;

        public MeadowApp()
        {
            Console.WriteLine("Initializing...");

            sensor = new Max44009(Device.CreateI2cBus());

            // Example that uses an IObersvable subscription to only be notified when the filter is satisfied
            var consumer = Max44009.CreateObserver(
                handler: result => Console.WriteLine($"Observer: filter satisifed: {result.New.Lux:N2}Lux, old: {result.Old?.Lux:N2}Lux"),

                // only notify if the visible light changes by 100 lux (put your hand over the sensor to trigger)
                filter: result => {
                    if (result.Old is { } old) { //c# 8 pattern match syntax. checks for !null and assigns var.
                        // returns true if > 100lux change
                        return ((result.New - old).Abs().Lux > 100);
                    }
                    return false;
                });

            sensor.Subscribe(consumer);

            // classical .NET events can also be used:
            sensor.Updated += (sender, result) => {
                Console.WriteLine($"Light: {result.New.Lux:N2}Lux");
            };

            //==== one-off read
            ReadConditions().Wait();

            // start updating continuously
            sensor.StartUpdating(TimeSpan.FromSeconds(1));
        }

        protected async Task ReadConditions()
        {
            var result = await sensor.Read();
            Console.WriteLine("Initial Readings:");
            Console.WriteLine($"   Light: {result.Lux:N2}Lux");
        }

        //<!—SNOP—>
    }
}