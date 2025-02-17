﻿using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Sensors.LoadCell;
using Meadow.Units;
using System;
using System.Threading;

namespace Sensors.LoadCell.Nau7802_Sample
{
    public class MeadowApp : App<F7MicroV2, MeadowApp>
    {
        //<!—SNIP—>

        private Nau7802 loadSensor;

        public int CalibrationFactor { get; set; } = 16526649; // TODO: change this based on your scale (using the method provided below)
        public Mass CalibrationWeight { get; set; } = new Mass(1600, Mass.UnitType.Grams); // TODO: enter the known-weight you used in calibration

        public MeadowApp()
        {
            Console.WriteLine("Initializing ...");

            loadSensor = new Nau7802(Device.CreateI2cBus());
            if (CalibrationFactor == 0)
            {
                GetAndDisplayCalibrationUnits(loadSensor);
            }
            else
            {   // wait for the ADC to settle
                Thread.Sleep(500);

                // Set the current load to be zero
                loadSensor.SetCalibrationFactor(CalibrationFactor, CalibrationWeight);
                loadSensor.Tare();

                // start reading
                loadSensor.MassUpdated += (sender, values) => Console.WriteLine($"Mass is now returned {values.New.Grams:N2}g");
                loadSensor.StartUpdating(TimeSpan.FromSeconds(2));
            }
        }

        public void GetAndDisplayCalibrationUnits(Nau7802 sensor)
        {
            // first notify the user we're starting
            Console.WriteLine($"Beginning Calibration. First we'll tare (set a zero).");
            Console.WriteLine($"Make sure scale bed is clear. Next step in 5 seconds...");
            Thread.Sleep(5000);
            sensor.Tare();
            Console.WriteLine($"Place a known weight on the scale. Next step in 5 seconds...");
            Thread.Sleep(5000);
            var factor = sensor.CalculateCalibrationFactor();
            Console.WriteLine($"Your scale's Calibration Factor is: {factor}. Enter this into the code for future use.");
        }

        //<!—SNOP—>
    }
}