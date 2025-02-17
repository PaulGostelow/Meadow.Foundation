﻿using System;
using System.Threading.Tasks;
using Meadow;
using Meadow.Units;
using MU = Meadow.Units.MagneticField.UnitType;
using Meadow.Devices;
using Meadow.Foundation.Sensors.Motion;

namespace MeadowApp
{
    public class MeadowApp : App<F7MicroV2, MeadowApp>
    {
        //<!—SNIP—>

        Mag3110 sensor;

        public MeadowApp()
        {
            Console.WriteLine("Initializing");

            sensor = new Mag3110(Device.CreateI2cBus());

            // classical .NET events can  be used
            sensor.Updated += (sender, result) => {
                Console.WriteLine($"Magnetic Field: [X:{result.New.MagneticField3D?.X.MicroTesla:N2}," +
                    $"Y:{result.New.MagneticField3D?.Y.MicroTesla:N2}," +
                    $"Z:{result.New.MagneticField3D?.Z.MicroTesla:N2} (MicroTeslas)]");

                Console.WriteLine($"Temp: {result.New.Temperature?.Celsius:N2}C");
            };

            // Example that uses an IObersvable subscription to only be notified when the filter is satisfied
            var consumer = Mag3110.CreateObserver(
                handler: result => Console.WriteLine($"Observer: [x] changed by threshold; new [x]: X:{result.New.MagneticField3D?.X.MicroTesla:N2}, old: X:{result.Old?.MagneticField3D?.X.MicroTesla:N2}"),
                // only notify if there's a greater than 1 micro tesla on the Y axis
                filter: result => {
                    if (result.Old is { } old) { //c# 8 pattern match syntax. checks for !null and assigns var.
                        return ((result.New.MagneticField3D - old.MagneticField3D)?.Y > new MagneticField(1, MU.MicroTesla));
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
            Console.WriteLine($"Mangetic field: [X:{result.MagneticField3D?.X.MicroTesla:N2}," +
                $"Y:{result.MagneticField3D?.Y.MicroTesla:N2}," +
                $"Z:{result.MagneticField3D?.Z.MicroTesla:N2} (microteslas)]");

            Console.WriteLine($"Temp: {result.Temperature?.Celsius:N2}C");
        }

        //<!—SNOP—>
    }
}