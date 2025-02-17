﻿using Meadow.Devices;
using Meadow.Foundation.Graphics;
using Meadow.Hardware;
using Meadow.Units;
using System;
using System.Threading;

namespace Meadow.Foundation.Displays.TftSpi
{
    public class St7789 : TftSpiBase
    {
        private byte xOffset;
        private byte yOffset;

        public static Frequency DefaultSpiBusSpeed = new Frequency(48000, Frequency.UnitType.Kilohertz);
        public override ColorType DefautColorMode => ColorType.Format12bppRgb444;

        public St7789(IMeadowDevice device, ISpiBus spiBus, IPin chipSelectPin, IPin dcPin, IPin resetPin,
            int width, int height, ColorType displayColorMode = ColorType.Format12bppRgb444) 
            : base(device, spiBus, chipSelectPin, dcPin, resetPin, width, height, displayColorMode)
        {
            Initialize();
        }

        protected override void Initialize()
        {
            if (resetPort != null)
            {
                resetPort.State = true;
                Thread.Sleep(50);
                resetPort.State = false;
                Thread.Sleep(50);
                resetPort.State = true;
                Thread.Sleep(50);
            }
            else
            {
                Thread.Sleep(150); //Not sure if this is needed but can't hurt
            }

            if (Width == 135)
            {   //unknown if this is consistant across all displays with this res
                xOffset = 52;
                yOffset = 40;
            }
            else
            {
                xOffset = yOffset = 0;
            }
            
            SendCommand(SWRESET);
            DelayMs(150);
            SendCommand(SLPOUT);
            DelayMs(500);

            SendCommand(Register.COLOR_MODE);  // set color mode - 16 bit color (x55), 12 bit color (x53), 18 bit color (x56)
            if (ColorMode == ColorType.Format16bppRgb565)
                SendData(0x55);  // 16-bit color RGB565
            else
                SendData(0x53); //12-bit color RGB444
           
            DelayMs(10);

            SendCommand(Register.MADCTL);
            SendData(0x00); //some variants use 0x08

            SendCommand((byte)LcdCommand.CASET);

            SendData(new byte[] { 0, 0, 0, (byte)Width });

            SendCommand((byte)LcdCommand.RASET);
            SendData(new byte[] { 0, 0, (byte)(Height >> 8), (byte)(Height & 0xFF) });

            SendCommand(INVON); //inversion on
            DelayMs(10);
            SendCommand(NORON); //normal display
            DelayMs(10);
            SendCommand(DISPON); //display on
            DelayMs(500);

            SetAddressWindow(0, 0, (Width - 1), (Height - 1));

            dataCommandPort.State = Data;
        }

        protected override void SetAddressWindow(int x0, int y0, int x1, int y1)
        {
            x0 += xOffset;
            y0 += yOffset;

            x1 += xOffset;
            y1 += yOffset;

            SendCommand((byte)LcdCommand.CASET);  // column addr set
            dataCommandPort.State = Data;
            Write((byte)(x0 >> 8));
            Write((byte)(x0 & 0xff));   // XSTART 
            Write((byte)(x1 >> 8));
            Write((byte)(x1 & 0xff));   // XEND

            SendCommand((byte)LcdCommand.RASET);  // row addr set
            dataCommandPort.State = Data;
            Write((byte)(y0 >> 8));
            Write((byte)(y0 & 0xff));    // YSTART
            Write((byte)(y1 >> 8));
            Write((byte)(y1 & 0xff));    // YEND

            SendCommand((byte)LcdCommand.RAMWR);  // write to RAM
        }

        public void SetRotation(Rotation rotation)
        {
            SendCommand(Register.MADCTL);

            switch (rotation)
            {
                case Rotation.Normal:
                    SendData((byte)Register.MADCTL_MX | (byte)Register.MADCTL_MY | (byte)Register.MADCTL_RGB);
                    break;
                case Rotation.Rotate_90:
                    SendData((byte)Register.MADCTL_MY | (byte)Register.MADCTL_MV | (byte)Register.MADCTL_RGB);
                    break;
                case Rotation.Rotate_180:
                    SendData((byte)Register.MADCTL_RGB);
                    break;
                case Rotation.Rotate_270:
                    SendData((byte)Register.MADCTL_MX | (byte)Register.MADCTL_MV | (byte)Register.MADCTL_RGB);
                    break;
            }
        }

        static byte SWRESET = 0x01;
        static byte SLPOUT = 0x11;
        static byte NORON = 0x13;
        static byte INVON = 0x21;
        static byte DISPON = 0x29;
    }
}