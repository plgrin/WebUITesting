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
        }

        public void AddDefaultHeader(string name, string value)
        {
            _client.AddDefaultHeader(name, value);
        }

        public RestResponse Get(string resource)
        {
            var request = new RestRequest(resource, Method.Get);
            return _client.Execute(request);
        }

        public RestResponse Post(string resource, object body)
        {
            var request = new RestRequest(resource, Method.Post);
            request.AddJsonBody(body);
            return _client.Execute(request);
        }

        public RestResponse Put(string resource, object body)
        {
            var request = new RestRequest(resource, Method.Put);
            request.AddJsonBody(body);
            return _client.Execute(request);
        }

        public RestResponse Delete(string resource)
        {
            var request = new RestRequest(resource, Method.Delete);
            return _client.Execute(request);
        }
    }


}
