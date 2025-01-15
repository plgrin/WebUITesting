using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using System;

namespace API_testing.Utilities
{
    public static class ExtentReportManager
    {
        private static ExtentReports _extent;
        private static ExtentTest _test;

        public static ExtentTest CurrentTest => _test;


        public static void InitializeReport()
        {
            var htmlReporter = new ExtentSparkReporter($"Reports/TestReport_{DateTime.Now:yyyyMMddHHmmss}.html");
            _extent = new ExtentReports();
            _extent.AttachReporter(htmlReporter);
        }

        public static ExtentTest CreateTest(string testName)
        {
            _test = _extent.CreateTest(testName);
            return _test;
        }

        public static void FinalizeReport()
        {
            _extent.Flush();
        }
    }
}
