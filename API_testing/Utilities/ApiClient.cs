using AventStack.ExtentReports;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace API_testing.Utilities
{
    public class ApiClient
    {
        private readonly RestClient _client;

        public ApiClient(string baseUrl, string token)
        {
            _client = new RestClient(baseUrl);
            _client.AddDefaultHeader("Authorization", $"Bearer {token}");
            ExtentReportManager.CurrentTest?.Log(Status.Info, "client API authorized.");
        }

        public void AddDefaultHeader(string name, string value)
        {
            _client.AddDefaultHeader(name, value);
        }

        public RestResponse Get(string resource)
        {
            var request = new RestRequest(resource, Method.Get);
            ExtentReportManager.CurrentTest?.Log(Status.Info, $"GET request: {resource}");
            var response = _client.Execute(request);
            ExtentReportManager.CurrentTest?.Log(Status.Info, $"response: {response.Content}");
            return response;
        }

        public RestResponse Post(string resource, object body)
        {
            var request = new RestRequest(resource, Method.Post);
            request.AddJsonBody(body);
            ExtentReportManager.CurrentTest?.Log(Status.Info, $"POST request: {resource} with body {JsonConvert.SerializeObject(body)}");
            var response = _client.Execute(request);
            ExtentReportManager.CurrentTest?.Log(Status.Info, $"response: {response.Content}");
            return response;
        }

        public RestResponse Put(string resource, object body)
        {
            var request = new RestRequest(resource, Method.Put);
            request.AddJsonBody(body);
            ExtentReportManager.CurrentTest?.Log(Status.Info, $"PUT request: {resource} with body {JsonConvert.SerializeObject(body)}");
            var response = _client.Execute(request);
            ExtentReportManager.CurrentTest?.Log(Status.Info, $"response: {response.Content}");
            return response;
        }

        public RestResponse Delete(string resource)
        {
            var request = new RestRequest(resource, Method.Delete);
            ExtentReportManager.CurrentTest?.Log(Status.Info, $"DELETE запрос: {resource}");
            var response = _client.Execute(request);
            ExtentReportManager.CurrentTest?.Log(Status.Info, $"Ответ: {response.Content}");
            return response;
        }
    }


}
