﻿using System.Text;

namespace Meadow.Foundation.Web.Maple.Server
{
    public class TextOutputFormatter : IOutputFormatter
    {
        public byte[] FormatContent(object content)
        {
            return Encoding.UTF8.GetBytes(content.ToString());
        }
    }
}