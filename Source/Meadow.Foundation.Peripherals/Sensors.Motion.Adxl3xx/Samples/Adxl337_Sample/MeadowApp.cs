﻿using System;
using System.Threading.Tasks;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Sensors.Motion;
using Meadow.Units;
using AU = Meadow.Units.Acceleration.UnitType;

namespace Sensors.Motion.Adxl337_Sample
{
    public class MeadowApp : App<F7MicroV2, MeadowApp>
    {
        //<!—SNIP—>

        Adxl337 sensor;

        public MeadowApp()
        {
            Console.WriteLine("Initializing");

            // create the sensor driver
            sensor = new Adxl337(Device, Device.Pins.A00, Device.Pins.A01, Device.Pins.A02, null);

            // classical .NET events can also be used:
            sensor.Updated += (sender, result) => {
                Console.WriteLine($"Accel: [X:{result.New.X.MetersPerSecondSquared:N2}," +
                    $"Y:{result.New.Y.MetersPerSecondSquared:N2}," +
                    $"Z:{result.New.Z.MetersPerSecondSquared:N2} (m/s^2)]");
            };

            // Example that uses an IObersvable subscription to only be notified when the filter is satisfied
            var consumer = Adxl337.CreateObserver(
                handler: result => Console.WriteLine($"Observer: [x] changed by threshold; new [x]: X:{result.New.X:N2}, old: X:{result.Old?.X:N2}"),
                // only notify if there's a greater than 1G change in the Z direction
                filter: result => {
                    if (result.Old is { } old) { //c# 8 pattern match syntax. checks for !null and assigns var.
                        return ((result.New - old).Z > new Acceleration(1, AU.Gravity));
                    }
                    return false;
                });
            sensor.Subscribe(consumer);

            //==== one-off read
            ReadConditions().Wait();

            // start updating
            sensor.StartUpdating(TimeSpan.FromMilliseconds(500));
        }

        protected async Task ReadConditions()
        {
            var result = await sensor.Read();
            Console.WriteLine("Initial Readings:");
            Console.WriteLine($"Accel: [X:{result.X.MetersPerSecondSquared:N2}," +
                $"Y:{result.Y.MetersPerSecondSquared:N2}," +
                $"Z:{result.Z.MetersPerSecondSquared:N2} (m/s^2)]");
        }

        //<!—SNOP—>
    }
}