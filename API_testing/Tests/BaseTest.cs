using API_testing.Utilities;
using AventStack.ExtentReports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_testing.Tests
{
    public abstract class BaseTest
    {
        protected ExtentTest Test;

        protected ApiClient ApiClient;
        protected string BaseUrl;

        public BaseTest()
        {
            var baseUrl = ConfigManager.Get("ApiBaseUrl");
            var token = ConfigManager.Get("Auth:Token");

            // Инициализация клиента API
            ApiClient = new ApiClient(baseUrl, token);

            ExtentReportManager.InitializeReport();
        }

        public void Dispose()
        {
            Test.Log(Status.Info, "test end.");
            ExtentReportManager.FinalizeReport();
        }

        protected void StartTest(string testName)
        {
            Test = ExtentReportManager.CreateTest(testName);
            Test.Log(Status.Info, $"test start: {testName}");
        }
    }
}
