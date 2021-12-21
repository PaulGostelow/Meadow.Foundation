﻿using System.Net;

namespace Meadow.Foundation.Web.Maple.Server
{
    public class NotFoundResult : StatusCodeResult
    {
        public NotFoundResult()
            : base(HttpStatusCode.NotFound)
        {

        }
    }
}