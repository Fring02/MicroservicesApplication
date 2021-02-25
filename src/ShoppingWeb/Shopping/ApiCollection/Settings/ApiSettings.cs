﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopping.ApiCollection.Settings
{
    public class ApiSettings : IApiSettings
    {
        public string BaseAddress { get; set; }
        public string CatalogPath { get; set; }
        public string BasketPath { get; set; }
        public string OrderingPath { get; set; }
        public string UsersPath { get; set; }
    }
}
