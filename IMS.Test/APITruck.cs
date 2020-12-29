using System;
using Xunit;
using IMS.Domain;
using IMS.Domain.Entity;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using IMS.Api.Information;
using System.Net;
using System.IO;
using Microsoft.Extensions.PlatformAbstractions;
using AutoMapper.Configuration;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace IMS.Test
{
    public class APITruck
    {
        private readonly HttpClient _client;

        public APITruck()
        {
            string projectDir = Path.GetFullPath(Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "..", "..", "..", "..", "IMS.Api.Information"));
            var server = new TestServer(
                new WebHostBuilder().
                UseStartup<IMS.Api.Information.Startup>().
                UseEnvironment("Development").
                UseContentRoot(projectDir).
                UseUrls("https://locahost:4200").
                UseConfiguration(new ConfigurationBuilder()
                    .SetBasePath(projectDir)
                    .AddJsonFile("appsettings.json")
                    .Build()
                )
                );
            _client = server.CreateClient();
        }

        [Theory]
        [InlineData("GET")]
        public async Task GetAllTrucks(string method)
        {
            var request = new HttpRequestMessage(new HttpMethod(method), "/api/v1/Truck");
            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        }

        [Theory]
        [InlineData("GET")]
        public async Task GetTruck(string method)
        {
            var request = new HttpRequestMessage(new HttpMethod(method), "/api/v1/Truck/1");
            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        }

        [Theory]
        [InlineData("DELETE")]
        public async Task DeteleTruck(string method)
        {
            var response = await _client.DeleteAsync("/api/v1/Truck/1");
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData("POST")]
        public async Task PostTruck(string method)
        {
            Truck truck = new Truck() { Model = Domain.Enum.TruckModel.FH, ManufactureYear = 2000, ModelYear = 2000 };
            var json = JsonConvert.SerializeObject(truck, Formatting.Indented);
            var stringContent = new StringContent(json);
            
            var responser = await _client.PostAsync("/api/v1/Truck", stringContent);
            responser.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, responser.StatusCode);

        }

        [Theory]
        [InlineData("PUT")]
        public async Task PUTTruck(string method)
        {
            Truck truck = new Truck() { Model = Domain.Enum.TruckModel.FH, ManufactureYear = 2000, ModelYear = 2000 };
            var json = JsonConvert.SerializeObject(truck, Formatting.Indented);
            var stringContent = new StringContent(json);

            var responser = await _client.PutAsync("/api/v1/Truck/2", stringContent);
            responser.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, responser.StatusCode);

        }

    }
}
