﻿using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Displays.Ssd130x;
using Meadow.Foundation.Displays.TextDisplayMenu;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Hardware;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Sensors.Buttons;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace MeadowApp
{
    public class MeadowApp : App<F7MicroV2, MeadowApp>
    {
        RgbPwmLed onboardLed;

        Menu menu;

        MicroGraphics graphics;
        Ssd1309 ssd1309;

        IButton up = null;
        IButton down = null;
        IButton left = null;
        IButton right = null;

        public MeadowApp()
        {
            Initialize();
        }

        void Initialize()
        {
            Console.WriteLine("Initialize hardware...");

            onboardLed = new RgbPwmLed(device: Device,
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue,
                3.3f, 3.3f, 3.3f,
                Meadow.Peripherals.Leds.IRgbLed.CommonType.CommonAnode);

            Console.WriteLine("Create Display with SPI...");

            var config = new SpiClockConfiguration(Ssd1309.DefaultSpiBusSpeed, Ssd1309.DefaultSpiClockMode);

            var bus = Device.CreateSpiBus(Device.Pins.SCK, Device.Pins.MOSI, Device.Pins.MISO, config);

            ssd1309 = new Ssd1309
            (
                device: Device,
                spiBus: bus,
                chipSelectPin: Device.Pins.D02,
                dcPin: Device.Pins.D01,
                resetPin: Device.Pins.D00
            );

            Console.WriteLine("Create MicroGraphics...");

            graphics = new MicroGraphics(ssd1309)
            {
                CurrentFont = new Font8x12(),
            };

            graphics.Clear();
            graphics.DrawText(0, 0, "Loading Menu");
            graphics.Show();

            CreateMenu(graphics);

            Console.WriteLine("Create buttons...");

            up = new PushButton(Device, Device.Pins.D09, ResistorMode.InternalPullDown);
            up.Clicked += Up_Clicked;

            left = new PushButton(Device, Device.Pins.D11, ResistorMode.InternalPullDown);
            left.Clicked += Left_Clicked;

            right = new PushButton(Device, Device.Pins.D10, ResistorMode.InternalPullDown);
            right.Clicked += Right_Clicked;

            down = new PushButton(Device, Device.Pins.D12, ResistorMode.InternalPullDown);
            down.Clicked += Down_Clicked;

            menu.Enable();
        }

        private void Down_Clicked(object sender, EventArgs e)
        {
            if (menu.IsEnabled) { menu.Next(); }
        }

        private void Right_Clicked(object sender, EventArgs e)
        {
            Console.WriteLine("Right_Clicked");
            if (menu.IsEnabled)
            {
                menu.Select();
            }
        }

        private void Left_Clicked(object sender, EventArgs e)
        {
            Console.WriteLine("Left_Clicked");
            if (menu.IsEnabled == false)
            {
                playGame = false;
            }
        }

        private void Up_Clicked(object sender, EventArgs e)
        {
            if (menu.IsEnabled)
            {
                menu.Previous();
            }
        }

        bool playGame = false;
        async Task StartGame(string command)
        {
            Console.WriteLine($"******** {command}");

            playGame = true;
            int count = 0;
            int x = 0, y = 0;
            int xD = 1, yD = 1;

            await Task.Run(() =>
            {
                while (count < 150 && playGame == true)
                {
                    graphics.Clear();
                    graphics.DrawText(0, 0, $"{command}:");
                    graphics.DrawText(0, 20, $"{count++}");
                    graphics.DrawPixel(x += xD, y += yD);
                    if (x == graphics.Width || x == 0) { xD *= -1; };
                    if (y == graphics.Height || y == 0) { yD *= -1; };
                    graphics.Show();
                }
            }).ConfigureAwait(false);

            EnableMenu();
        }

        void EnableMenu()
        {
            Console.WriteLine("Enable menu...");

            menu?.Enable();
        }

        void DisableMenu()
        {
            menu?.Disable();
        }

        void CreateMenu(ITextDisplay display)
        {
            Console.WriteLine("Load menu data...");

            var menuData = LoadResource("menu.json");

            Console.WriteLine($"Data length: {menuData.Length}...");

            Console.WriteLine("Create menu...");

            menu = new Menu(display, menuData, false);

            menu.Selected += Menu_Selected;
        }

        private void Menu_Selected(object sender, MenuSelectedEventArgs e)
        {
            Console.WriteLine($"******** Selected: {e.Command}");

            DisableMenu();

            var t = StartGame(e.Command);
        }

        byte[] LoadResource(string filename)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"Graphics.TextDisplayMenu_GameMenu_Sample.{filename}";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (var ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    return ms.ToArray();
                }
            }
        }
    }
}