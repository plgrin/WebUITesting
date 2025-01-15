using API_testing.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_testing.Tests
{
    public abstract class BaseTest
    {
        protected ApiClient ApiClient;
        protected string BaseUrl;

        public BaseTest()
        {
            var baseUrl = ConfigManager.Get("ApiBaseUrl");
            var token = ConfigManager.Get("Auth:Token");

            // Инициализация клиента API
            ApiClient = new ApiClient(baseUrl, token);
        }
    }
}
