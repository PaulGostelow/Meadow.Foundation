﻿using System;
using Xunit;

namespace Meadow.Modbus.Unit.Tests
{
    public class ModbusSerialTStatTests
    {
        // this class assumes a connected serial Temco Controls TSTAT7 or TSTAT8
        /*
        [Fact]
        public async void ReadHoldingRegisterTest()
        {
            using (var port = new SerialPortShim("COM4", 19200, Hardware.Parity.None, 8, Hardware.StopBits.One))
            {
                port.ReadTimeout = 15000;
                port.Open();

                byte address = 201;
                ushort startRegister = 1;
                var readCount = 1;

                var client = new ModbusRtuClient(port);
                var r1 = await client.ReadHoldingRegisters(address, startRegister, readCount);
                Assert.Equal(readCount, r1.Length);

                readCount = 2;
                var r2 = await client.ReadHoldingRegisters(address, startRegister, readCount);
                Assert.Equal(readCount, r2.Length);

                Assert.Equal(r1[0], r2[0]);
            }
        }

        [Fact]
        public async void ReadWriteHoldingRegisterTest()
        {
            using (var port = new SerialPortShim("COM4", 19200, Hardware.Parity.None, 8, Hardware.StopBits.One))
            {
                port.ReadTimeout = 15000;
                port.Open();

                byte address = 201;
                ushort startRegister = 345; // occupied setpoint, in tenths of a degree
                var readCount = 1;

                var client = new ModbusRtuClient(port);
                var setpoint = await client.ReadHoldingRegisters(address, startRegister, readCount);

                // TODO: verify it's reasonable?

                // add or subtract some random amount
                var r = new Random();
                var delta = r.Next(-20, 20);
                var newSetpoint = (ushort)(setpoint[0] + delta);

                await client.WriteHoldingRegister(address, startRegister, newSetpoint);
                var verifySetpoint = await client.ReadHoldingRegisters(address, startRegister, readCount);

                Assert.Equal(newSetpoint, verifySetpoint[0]);
            }
        }
        */
    }
}