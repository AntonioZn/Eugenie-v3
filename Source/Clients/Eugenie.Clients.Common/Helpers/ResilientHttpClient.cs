namespace Eugenie.Clients.Common.Helpers
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;

    using Exceptions;

    using Polly;

    public class ResilientHttpClient : IDisposable
    {
        private readonly HttpClient client;

        public ResilientHttpClient()
        {
            this.client = new HttpClient();
        }

        public ResilientHttpClient(HttpClientHandler handler)
        {
            this.client = new HttpClient(handler, true);
        }

        public TimeSpan Timeout
        {
            get => this.client.Timeout;
            set => this.client.Timeout = value;
        }

        public HttpRequestHeaders DefaultRequestHeaders => this.client.DefaultRequestHeaders;

        public Uri BaseAddress
        {
            get => this.client.BaseAddress;
            set => this.client.BaseAddress = value;
        }

        public async Task<HttpResponseMessage> GetAsync(Uri location, CancellationToken token = default(CancellationToken))
        {
            return await this.GetAsync(location.ToString(), token);
        }

        public async Task<HttpResponseMessage> GetAsync(string location, CancellationToken token = default(CancellationToken))
        {
            var response = await this.SendAsync(new HttpRequestMessage(HttpMethod.Get, location), token);
            return response;
        }

        public async Task<HttpResponseMessage> PostAsync(string location, HttpContent content, CancellationToken token = default(CancellationToken))
        {
            var response = await this.SendAsync(new HttpRequestMessage(HttpMethod.Post, location) { Content = content }, token);
            return response;
        }

        public async Task<HttpResponseMessage> PutAsync(string location, HttpContent content, CancellationToken token = default(CancellationToken))
        {
            var response = await this.SendAsync(new HttpRequestMessage(HttpMethod.Put, location) { Content = content }, token);
            return response;
        }

        public async Task<HttpResponseMessage> DeleteAsync(string location, CancellationToken token = default(CancellationToken))
        {
            var response = await this.SendAsync(new HttpRequestMessage(HttpMethod.Delete, location), token);
            return response;
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken token = default(CancellationToken))
        {
            try
            {
                return await this.Retry(async () =>
                                        {
                                            var requestClone = await this.CloneHttpRequestMessageAsync(request);

                                            var response = await this.client.SendAsync(requestClone, token);
                                            return response;
                                        }, token);
            }
            finally
            {
                request.Dispose();
            }
        }

        private async Task<T> Retry<T>(Func<Task<T>> func, CancellationToken token)
        {
            return await Policy
                       .Handle<HttpRequestException>()
                       .Or<WebException>()
                       .Or<HttpClientTimeoutException>()
                       .RetryAsync(2)
                       .ExecuteAsync(
                           async t =>
                           {
                               try
                               {
                                   return await func();
                               }
                               catch (TaskCanceledException) when (!token.IsCancellationRequested)
                               {
                                   throw new HttpClientTimeoutException($"Timeout after {this.Timeout.TotalSeconds} seconds");
                               }
                           }, token);
        }

        public void Dispose()
        {
            this.client.Dispose();
        }

        private async Task<HttpRequestMessage> CloneHttpRequestMessageAsync(HttpRequestMessage req)
        {
            var clone = new HttpRequestMessage(req.Method, req.RequestUri);

            // Copy the request's content (via a MemoryStream) into the cloned object
            var ms = new MemoryStream();
            if (req.Content != null)
            {
                await req.Content.CopyToAsync(ms).ConfigureAwait(false);
                ms.Position = 0;
                clone.Content = new StreamContent(ms);

                // Copy the content headers
                if (req.Content.Headers != null) foreach (var h in req.Content.Headers) clone.Content.Headers.Add(h.Key, h.Value);
            }

            clone.Version = req.Version;

            foreach (var prop in req.Properties) clone.Properties.Add(prop);

            foreach (var header in req.Headers) clone.Headers.TryAddWithoutValidation(header.Key, header.Value);

            return clone;
        }
    }
}