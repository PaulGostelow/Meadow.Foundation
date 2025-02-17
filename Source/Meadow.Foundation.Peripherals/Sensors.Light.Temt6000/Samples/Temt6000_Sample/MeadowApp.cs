﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Sensors.Light;
using Meadow.Units;

namespace MeadowApp
{
    public class MeadowApp : App<F7MicroV2, MeadowApp>
    {
        //<!—SNIP—>

        Temt6000 sensor;

        public MeadowApp()
        {
            Console.WriteLine("Initializing...");

            // configure our sensor
            sensor = new Temt6000(Device, Device.Pins.A03);

            // Example that uses an IObersvable subscription to only be notified when the voltage changes by at least 0.5V
            var consumer = Temt6000.CreateObserver(
                handler: result => Console.WriteLine($"Observer filter satisfied: {result.New.Volts:N2}V, old: {result.Old?.Volts:N2}V"),
                // only notify if the change is greater than 0.5V
                filter: result => {
                    if (result.Old is { } old) { //c# 8 pattern match syntax. checks for !null and assigns var.
                        return (result.New - old).Abs().Volts > 0.5; // returns true if > 0.5V change.
                    }
                    return false;
                });

            sensor.Subscribe(consumer);

            // classical .NET events can also be used:
            sensor.Updated += (sender, result) => {
                Console.WriteLine($"Voltage Changed, new: {result.New.Volts:N2}V, old: {result.Old?.Volts:N2}V");
            };

            //==== One-off reading use case/pattern
            ReadTemp().Wait();

            sensor.StartUpdating(TimeSpan.FromMilliseconds(1000));
        }

        protected async Task ReadTemp()
        {
            var result = await sensor.Read();
            Console.WriteLine($"Initial temp: {result.Volts:N2}V");
        }

        //<!—SNOP—>
    }
}