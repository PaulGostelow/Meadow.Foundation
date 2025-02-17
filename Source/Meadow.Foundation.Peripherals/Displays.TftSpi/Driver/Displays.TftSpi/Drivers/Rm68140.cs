﻿using System.Threading;
using Meadow.Devices;
using Meadow.Foundation.Graphics;
using Meadow.Hardware;

namespace Meadow.Foundation.Displays.TftSpi
{
    public class Rm68140 : TftSpiBase
    {
        public override ColorType DefautColorMode => ColorType.Format12bppRgb444;

        public Rm68140(IMeadowDevice device, ISpiBus spiBus, IPin chipSelectPin, IPin dcPin, IPin resetPin,
            int width = 320, int height = 480, ColorType displayColorMode = ColorType.Format12bppRgb444) 
            : base(device, spiBus, chipSelectPin, dcPin, resetPin, width, height, displayColorMode)
        {
            Initialize();

            SetRotation(Rotation.Normal);
        }

        protected override void Initialize()
        {
            SendCommand(TFT_SLPOUT);
            Thread.Sleep(20);

            SendCommand(0xD0);
            SendData(0x07);
            SendData(0x42);
            SendData(0x18);

            SendCommand(0xD1);
            SendData(0x00);
            SendData(0x07);
            SendData(0x10);

            SendCommand(0xD2);
            SendData(0x01);
            SendData(0x02);

            SendCommand(0xC0);
            SendData(0x10);
            SendData(0x3B);
            SendData(0x00);
            SendData(0x02);
            SendData(0x11);

            SendCommand(0xC5);
            SendData(0x03);

            SendCommand(0xC8);
            SendData(0x00);
            SendData(0x32);
            SendData(0x36);
            SendData(0x45);
            SendData(0x06);
            SendData(0x16);
            SendData(0x37);
            SendData(0x75);
            SendData(0x77);
            SendData(0x54);
            SendData(0x0C);
            SendData(0x00);

            SendCommand((byte)Register.MADCTL);
            SendData(0x0A);

            SendCommand((byte)Register.COLOR_MODE);
            if (ColorMode == ColorType.Format16bppRgb565)
                SendData(0x55); //16 bit RGB565
            else
                SendData(0x53); //12 bit RGB444

            SendCommand((byte)LcdCommand.CASET);
            SendData(0x00);
            SendData(0x00);
            SendData(0x01);
            SendData(0x3F);

            SendCommand((byte)LcdCommand.RASET);
            SendData(0x00);
            SendData(0x00);
            SendData(0x01);
            SendData(0xDF);

            Thread.Sleep(120);
            SendCommand(TFT_DISPON);
            Thread.Sleep(25);
        }

        protected override void SetAddressWindow(int x0, int y0, int x1, int y1)
        {
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
            SendCommand((byte)Register.MADCTL);

            switch (rotation)
            {
                case Rotation.Normal:
                    SendData((byte)Register.MADCTL_BGR);
                    SendCommand(0xB6);
                    SendData(0);
                    SendData(0x22);
                    SendData(0x3B);
                    break;
                case Rotation.Rotate_90:
                    SendData((byte)Register.MADCTL_MV | (byte)Register.MADCTL_BGR);
                    SendCommand(0xB6);
                    SendData(0);
                    SendData(0x02);
                    SendData(0x3B);
                    break;
                case Rotation.Rotate_180:
                    SendData((byte)Register.MADCTL_BGR);
                    SendCommand(0xB6);
                    SendData(0);
                    SendData(0x42);
                    SendData(0x3B);
                    break;
                case Rotation.Rotate_270:
                    SendData((byte)Register.MADCTL_MV | (byte)Register.MADCTL_BGR);
                    SendCommand(0xB6);
                    SendData(0);
                    SendData(0x62);
                    SendData(0x3B);
                    break;
            }
        }

        const byte TFT_NOP = 0x00;
        const byte TFT_SWRST = 0x01;
        const byte TFT_SLPIN = 0x10;
        const byte TFT_SLPOUT = 0x11;
        const byte TFT_INVOFF = 0x20;
        const byte TFT_INVON = 0x21;
        const byte TFT_DISPOFF = 0x28;
        const byte TFT_DISPON = 0x29;
    }
}