﻿using Meadow;
using System.Threading;

namespace Displays.Tft.Gc9a01_Sample
{
    class Program
    {
        static IApp app;

        public static void Main(string[] args)
        {
            app = new MeadowApp();

            Thread.Sleep(Timeout.Infinite);
        }
    }
}