﻿using System;
using System.Net.Http;
using System.Threading.Tasks;
using Flurl.Http;

namespace DiabloII.Application.Tests
{
    public class MyHttpClient : IDisposable
    {
        private readonly FlurlClient _flurlClient;

        public int StatusCode { get; private set; }

        public MyHttpClient(HttpClient httpClient) => _flurlClient = new FlurlClient(httpClient);

        public async Task<TResponse> PostAsync<TResponse>(string endpoint, object dto)
        {
            var flurlResponse = await _flurlClient
                .Request(endpoint)
                .PostJsonAsync(dto);

            StatusCode = flurlResponse.StatusCode;

            return await flurlResponse.GetJsonAsync<TResponse>();
        }

        public void Dispose() => _flurlClient.Dispose();
    }
}