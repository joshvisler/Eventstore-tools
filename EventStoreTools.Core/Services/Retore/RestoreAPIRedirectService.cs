﻿using System.Collections.Generic;
using System.Threading.Tasks;
using EventStoreTools.Core.Entities.Enums;
using EventStoreTools.Core.Interfaces.APIRedirect;
using EventStoreTools.DTO.Entities.Restore;
using RestSharp;

namespace EventStoreTools.Core.Services.Retore
{
    public class RestoreAPIRedirectService : IRestoreAPIRedirectService
    {
        private readonly RestClient _webClient;
        private const string _restoreApiAddress = "restore";

        public RestoreAPIRedirectService(string baseAddress)
        {
            _webClient = new RestClient(baseAddress);
        }

        public Task DeleteAsync(int restoreId)
        {
            return Task.Run(() =>
            {
                var request = new RestRequest(_restoreApiAddress, Method.DELETE);
                request.AddBody(restoreId);
                _webClient.Execute(request);
            });
        }

        public async Task<IEnumerable<RestoreResultDTO>> GetAllRestorsAsync()
        {
            var request = new RestRequest(_restoreApiAddress, Method.GET);
            var result = await _webClient.ExecuteGetTaskAsync<IEnumerable<RestoreResultDTO>>(request);

            return await Task.FromResult(result.Data);
        }

        public async Task<RestoreStatus> RestoreAsync(RestoreParamsDTO restore)
        {
            return await Task.Run(async () =>
            {
                var request = new RestRequest(_restoreApiAddress, Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddBody(restore);
                var result = await _webClient.ExecutePostTaskAsync<RestoreStatus>(request);
                return await Task.FromResult(result.Data);
            });
        }

        public void Dispose()
        {
        }
    }
}
