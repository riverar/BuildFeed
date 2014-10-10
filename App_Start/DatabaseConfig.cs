using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace BuildFeed
{
    public static class DatabaseConfig
    {
        public static string Host { get; private set; }
        public static int Port { get; private set; }
        public static long Database { get; private set; }

        static DatabaseConfig()
        {
            Host = ConfigurationManager.AppSettings["data:ServerHost"];

            int _port;
            bool success = int.TryParse(ConfigurationManager.AppSettings["data:ServerPort"], out _port);
            if(!success)
            {
                _port = 6379; // redis default port
            }
            Port = _port;

            long _db;
            success = long.TryParse(ConfigurationManager.AppSettings["data:ServerDB"], out _db);
            if (!success)
            {
                _db = 0; // redis default db
            }
            Database = _db;
        }
    }
}