using System;
using System.Threading;
using System.Threading.Tasks;
using Polly;
using ServiceStack;

namespace Iconlook.Client
{
    public class JsonHttpClient : ServiceStack.JsonHttpClient
    {
        public JsonHttpClient()
        {
            HttpClient = HttpClientPool.Instance;
            RequestCompressionType = CompressionTypes.GZip;
        }

        public JsonHttpClient(string uri) : base(uri)
        {
            HttpClient = HttpClientPool.Instance;
            RequestCompressionType = CompressionTypes.GZip;
        }

        private bool IsHttpGet(object request)
        {
            return ServiceClientBase.GetExplicitMethod(request) == HttpMethods.Get;
        }

        public new async Task<T> SendAsync<T>(string method, string url, object request, CancellationToken token = default)
        {
            var policy = await Policy.Handle<Exception>(x => !(x is TaskCanceledException))
                .WaitAndRetryAsync(IsHttpGet(request) ? 2 : 0, x => TimeSpan.FromSeconds(Math.Pow(2, x)))
                .ExecuteAndCaptureAsync(() => base.SendAsync<T>(method, url, request, token));
            return policy.Result;
        }

        public new Task<T> GetAsync<T>(string url)
        {
            return SendAsync<T>(HttpMethods.Get, ResolveUrl(HttpMethods.Get, url), null);
        }

        public new Task<T> GetAsync<T>(object request)
        {
            return SendAsync<T>(HttpMethods.Get, ResolveTypedUrl(HttpMethods.Get, request), null);
        }

        public new Task<T> GetAsync<T>(IReturn<T> request)
        {
            return SendAsync<T>(HttpMethods.Get, ResolveTypedUrl(HttpMethods.Get, request), null);
        }

        public new Task GetAsync(IReturnVoid request)
        {
            return SendAsync<byte[]>(HttpMethods.Get, ResolveTypedUrl(HttpMethods.Get, request), null);
        }

        public new Task<T> PostAsync<T>(object request)
        {
            return SendAsync<T>(HttpMethods.Post, ResolveTypedUrl(HttpMethods.Post, request), request);
        }

        public new Task<T> PostAsync<T>(string url, object request)
        {
            return SendAsync<T>(HttpMethods.Post, ResolveUrl(HttpMethods.Post, url), request);
        }

        public new Task<T> PostAsync<T>(IReturn<T> request)
        {
            return SendAsync<T>(HttpMethods.Post, ResolveTypedUrl(HttpMethods.Post, request), request);
        }

        public new Task<T> GetAsync<T>(string url, CancellationToken token)
        {
            return SendAsync<T>(HttpMethods.Get, ResolveUrl(HttpMethods.Get, url), null, token);
        }

        public new Task<T> GetAsync<T>(object request, CancellationToken token)
        {
            return SendAsync<T>(HttpMethods.Get, ResolveTypedUrl(HttpMethods.Get, request), null, token);
        }

        public new Task<T> GetAsync<T>(IReturn<T> request, CancellationToken token)
        {
            return SendAsync<T>(HttpMethods.Get, ResolveTypedUrl(HttpMethods.Get, request), null, token);
        }

        public new Task GetAsync(IReturnVoid request, CancellationToken token)
        {
            return SendAsync<byte[]>(HttpMethods.Get, ResolveTypedUrl(HttpMethods.Get, request), null, token);
        }

        public new Task<T> PostAsync<T>(object request, CancellationToken token)
        {
            return SendAsync<T>(HttpMethods.Post, ResolveTypedUrl(HttpMethods.Post, request), request, token);
        }

        public new Task<T> PostAsync<T>(string url, object request, CancellationToken token)
        {
            return SendAsync<T>(HttpMethods.Post, ResolveUrl(HttpMethods.Post, url), request, token);
        }

        public new Task<T> PostAsync<T>(IReturn<T> request, CancellationToken token)
        {
            return SendAsync<T>(HttpMethods.Post, ResolveTypedUrl(HttpMethods.Post, request), request, token);
        }
    }
}