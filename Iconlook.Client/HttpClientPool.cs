using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Iconlook.Client
{
    public static class HttpClientPool
    {
        private static readonly Dictionary<TimeSpan, HttpClient> Pool = new Dictionary<TimeSpan, HttpClient>();

        public static HttpClient Instance { get; } = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };

        static HttpClientPool()
        {
            Pool.Add(Instance.Timeout, Instance);
        }

        public static HttpClient Get(TimeSpan timeout)
        {
            if (Pool.ContainsKey(timeout))
            {
                return Pool[timeout];
            }
            var instance = new HttpClient { Timeout = timeout };
            Pool.Add(timeout, instance);
            return instance;
        }
    }
}