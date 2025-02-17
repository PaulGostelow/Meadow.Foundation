﻿using System;
using System.Threading.Tasks;
using Meadow;
using Meadow.Units;
using AU = Meadow.Units.Acceleration.UnitType;
using Meadow.Devices;
using Meadow.Foundation.Sensors.Motion;

namespace MeadowApp
{
    public class MeadowApp : App<F7MicroV2, MeadowApp>
    {
        //<!—SNIP—>

        Qmc5883 sensor;

        public MeadowApp()
        {
            Console.WriteLine("Initializing");

            sensor = new Qmc5883(Device.CreateI2cBus());

            // classical .NET events can be used
            sensor.Updated += (sender, result) => {
                Console.WriteLine($"Direction: [X:{result.New.X:N2}," +
                    $"Y:{result.New.Y:N2}," +
                    $"Z:{result.New.Z:N2}]");

                Console.WriteLine($"Heading: [{Hmc5883.DirectionToHeading(result.New).DecimalDegrees:N2}] degrees");
            };

            // Example that uses an IObersvable subscription to only be notified when the filter is satisfied
            var consumer = Qmc5883.CreateObserver(
                handler: result => Console.WriteLine($"Observer: [x] changed by threshold; new [x]: X:{Qmc5883.DirectionToHeading(result.New):N2}," +
                        $" old: X:{((result.Old != null) ? Qmc5883.DirectionToHeading(result.Old.Value) : "n/a"):N2} degrees"),
                // only notify if there's a greater than 5° of heading change
                filter: result => {
                    return result.Old is { } old ? Qmc5883.DirectionToHeading(result.New - old) > new Azimuth(5) : false;
                });
            sensor.Subscribe(consumer);

            //==== one-off read
            ReadConditions().Wait();

            // start updating
            sensor.StartUpdating(TimeSpan.FromMilliseconds(1000));
        }

        protected async Task ReadConditions()
        {
            var result = await sensor.Read();
            Console.WriteLine("Initial Readings:");
            Console.WriteLine($"Direction: [X:{result.X:N2}," +
                $"Y:{result.Y:N2}," +
                $"Z:{result.Z:N2}]");

            Console.WriteLine($"Heading: [{Hmc5883.DirectionToHeading(result).DecimalDegrees:N2}] degrees");
        }

        //<!—SNOP—>
    }
}