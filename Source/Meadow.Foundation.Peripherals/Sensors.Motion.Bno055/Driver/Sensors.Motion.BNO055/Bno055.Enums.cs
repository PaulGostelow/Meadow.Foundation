﻿namespace Meadow.Foundation.Sensors.Motion
{
    public partial class Bno055
    {
        /// <summary>
		///     Valid addresses for the sensor.
		/// </summary>
		public enum Addresses : byte
        {
            /// <summary>
            /// Bus address 0x28
            /// </summary>
            Address_0x28 = 0x28,
            /// <summary>
            /// Bus address 0x29
            /// </summary>
            Address_0x29 = 0x29,
            /// <summary>
            /// Bus address 0x28
            /// </summary>
            Default = Address_0x28
        }

        /// <summary>
        ///     Error codes that may be generated by the sensor.
        /// </summary>
        public enum ErrorCodes : byte
        {
            /// <summary>
            ///     No error.
            /// </summary>
            NoError = 0x00,

            /// <summary>
            ///     Peripheral initialization error.
            /// </summary>
            PeripheralInitializationError = 0x01,

            /// <summary>
            ///     System initialization error.
            /// </summary>
            SystemInitializationError = 0x02,

            /// <summary>
            ///     Self test failed.
            /// </summary>
            SelfTestFailed = 0x04,

            /// <summary>
            ///     Register map value out of range.
            /// </summary>
            RegisterMapValueError = 0x05,

            /// <summary>
            ///     Register map address out of range.
            /// </summary>
            RegisterMapAddressError = 0x06,

            /// <summary>
            ///     Low power mode not available for the selected mode.
            /// </summary>
            LowPowerModeNotAvailable = 0x07,

            /// <summary>
            ///     Accelerometer power mode not available.
            /// </summary>
            AccelerationModeNotAvailable = 0x08,

            /// <summary>
            ///     Fusion algorithm configuration error.
            /// </summary>
            FusionConfigurationError = 0x09,

            /// <summary>
            ///     Sensor configuration error.
            /// </summary>
            SensorConfigurationError = 0x0a
        }

        /// <summary>
        ///     System status codes.
        /// </summary>
        public enum SystemStatusCodes : byte
        {
            /// <summary>
            ///     System is idle.
            /// </summary>
            Idle = 0x00,

            /// <summary>
            ///     System error.
            /// </summary>
            SystemError = 0x01,

            /// <summary>
            ///     Peripheral initialization in progess.
            /// </summary>
            PeripheralInitializationInProgess = 0x02,

            /// <summary>
            ///     System is initializing.
            /// </summary>
            SystemInitializationInProgress = 0x03,

            /// <summary>
            ///     Self test is running.
            /// </summary>
            SelfTestInProgress = 0x04,

            /// <summary>
            ///     Fusion algorithm is running.
            /// </summary>
            FusionAlgorithmRunning = 0x05,

            /// <summary>
            ///     System is running without using the fusion algorithm.
            /// </summary>
            FusionAlgorithmNotUsed = 0x06
        }

        /// <summary>
        ///     Sensor type.
        /// </summary>
        public enum Sensor
        {
            /// <summary>
            ///     Accelerometer sensor.
            /// </summary>
            Accelerometer = 0x00,

            /// <summary>
            ///     Gyroscope sensor.
            /// </summary>
            Gyroscope = 0x01,

            /// <summary>
            ///     Magnetometer sensor.
            /// </summary>
            Magnetometer = 0x02
        };
    }
}
