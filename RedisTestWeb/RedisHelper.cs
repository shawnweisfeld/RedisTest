using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using StackExchange.Redis;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace RedisTestWeb
{
    public static class RedisHelper
    {
        private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            string cacheConnection = ConfigurationManager.AppSettings["CacheConnection"].ToString();
            return ConnectionMultiplexer.Connect(cacheConnection);
        });

        private static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }

        private static Lazy<TelemetryClient> lazyTelemetry = new Lazy<TelemetryClient>(() =>
        {
            return new TelemetryClient();
        });

        private static TelemetryClient Telemetry
        {
            get
            {
                return lazyTelemetry.Value;
            }
        }

        public static async Task<bool> StringSetAsync(string key, string value)
        {
            var dependency = new DependencyTelemetry()
            {
                Type = "Redis",
                Name = "StringSetAsync"
            };
            dependency.Properties["key"] = key;

            using (Telemetry.StartOperation(dependency))
            {
                var result = await Connection.GetDatabase().StringSetAsync(key, value);
                dependency.Success = result;
                return result;
            }
        }

        public static async Task<RedisValue> StringGetAsync(string key)
        {
            var dependency = new DependencyTelemetry()
            {
                Type = "Redis",
                Name = "StringGetAsync"
            };
            dependency.Properties["key"] = key;

            using (Telemetry.StartOperation(dependency))
            {
                var result = await Connection.GetDatabase().StringGetAsync(key);
                dependency.Success = true;
                return result;
            }
        }
    }
}