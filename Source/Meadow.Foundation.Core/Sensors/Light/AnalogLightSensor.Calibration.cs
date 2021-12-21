﻿using Meadow.Units;
using VU = Meadow.Units.Voltage.UnitType;

namespace Meadow.Foundation.Sensors.Light
{
    public partial class AnalogLightSensor
    {
        /// <summary>
        ///     Calibration class for AnalogLightSensor. 
        /// </summary>
        public class Calibration
        {
            public Voltage VoltsAtZero { get; protected set; } = new Voltage(1, VU.Volts);

            /// <summary>
            ///     Linear change in the sensor output
            ///     change in temperature.
            /// </summary>
            public Voltage VoltsPerLuminance{ get; protected set; } = new Voltage(0.25, VU.Volts);

            /// <summary>
            ///     Default constructor. Create a new Calibration object with default values
            ///     for the properties.
            /// </summary>
            public Calibration()
            {
            }

            /// <summary>
            ///     Create a new Calibration object using the specified values.
            /// </summary>
            /// <param name="voltsPerCentimeter">Voltage change per cenimeter.</param>
            /// <param name="voltsAtZero">Voltage at a zero water level reading.</param>
            public Calibration(Voltage voltsPerLuminance, Voltage voltsAtZero)
            {
                VoltsPerLuminance = voltsPerLuminance;
                VoltsAtZero = voltsAtZero;
            }
        }
    }
}