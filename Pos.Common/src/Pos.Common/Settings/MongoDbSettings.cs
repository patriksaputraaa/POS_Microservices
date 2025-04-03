using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Play.Common.Service.Settings
{
    public class MongoDbSettings
    {
        public string Host { get; set; } = null!;
        public string Port { get; set; } = null!;
        public string ConnectionString => $"mongodb://{Host}:{Port}";
    }
}