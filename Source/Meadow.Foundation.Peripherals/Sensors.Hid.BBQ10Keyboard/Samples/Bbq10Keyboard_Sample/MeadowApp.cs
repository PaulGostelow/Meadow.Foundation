﻿using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Sensors.Hid;
using System;

namespace Bbq10Keyboard_Sample
{
    public class MeadowApp : App<F7MicroV2, MeadowApp>
    {
        BBQ10Keyboard keyboard;

        public MeadowApp()
        {
            Console.WriteLine("Initialize hardware...");
            var i2cBus = Device.CreateI2cBus(0);
            keyboard = new BBQ10Keyboard(Device, i2cBus, Device.Pins.D10);
            keyboard.OnKeyEvent += Keyboard_OnKeyEvent;
        }

        private void Keyboard_OnKeyEvent(object sender, BBQ10Keyboard.KeyEvent e)
        {
            if (e.KeyState == BBQ10Keyboard.KeyState.StatePress)
            {
                Console.WriteLine($"{e.AsciiValue}, {(byte)e.AsciiValue}, pressed");
            }
        }
    }
}
