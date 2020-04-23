﻿using Dapper.Contrib.Extensions;

namespace Specter.Data.Models
{
    public class Setting
    {
        [ExplicitKey]
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
