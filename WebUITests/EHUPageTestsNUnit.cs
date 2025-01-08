using NUnit.Framework;
using Microsoft.Extensions.Configuration;
using WebUITests.PageObjects;
using WebUITests.Utilities;
using FluentAssertions;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework.Interfaces;

namespace WebUITests
{
    /// <summary>
    /// This class contains tests for verifying various functionalities on the EHU website.
    /// </summary>
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class EHUPageTestsNUnit
    {
        private HomePage _homePage;
        private AboutPage _aboutPage;
        private SearchResultsPage _searchResultsPage;
        private static ExtentReports _extent;
        private ExtentTest _test;

        /// <summary>
        /// Initialize ExtentReports before any tests are run.
        /// </summary>
        [OneTimeSetUp]
        public void SetupReporting()
        {
            var reporter = new ExtentHtmlReporter(Path.Combine(Directory.GetCurrentDirectory(), "TestExecutionReport.html"));

            var config = reporter.Configuration();
            config.DocumentTitle = "EHU Website Test Execution Report";
            config.ReportName = "Test Suite Execution";
            config.Theme = AventStack.ExtentReports.Reporter.Configuration.Theme.Standard;

            _extent = new ExtentReports();
            _extent.AttachReporter(reporter);

            _extent.AddSystemInfo("Environment", "QA");
            _extent.AddSystemInfo("Tester", "Polina");
        }

        /// <summary>
        /// Set up method that initializes the configuration and the Chrome WebDriver.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _test = _extent.CreateTest(TestContext.CurrentContext.Test.Name);
            Logger.Log.Information("Initializing WebDriver and setting up the test environment.");
            DriverSingleton.InitializeDriver();

            _homePage = new HomePage(DriverSingleton.Driver);
            _aboutPage = new AboutPage(DriverSingleton.Driver);
            _searchResultsPage = new SearchResultsPage(DriverSingleton.Driver);
        }

        // Data provider for "VerifyNavigationToAboutEHUPage"
        private static IEnumerable<object[]> NavigationTestCases()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            Logger.Log.Information("Loaded configuration for navigation test cases.");


            yield return new object[]
            {
                config["TestSettings:EHUBaseUrl"],
                config["TestSettings:AboutPageUrl"],
                "About",
                "About"
            };
        }

        /// <summary>
        /// Test to verify navigation to the "About EHU" page.
        /// </summary>
        [Test, TestCaseSource(nameof(NavigationTestCases))]
        public void VerifyNavigationToAboutEHUPage(string baseUrl, string aboutUrl, string expectedTitle, string expectedHeader)
        {
            try
            {
                Logger.Log.Information($"Starting test: VerifyNavigationToAboutEHUPage. Base URL: {baseUrl}, About URL: {aboutUrl}");

                _homePage.NavigateTo(baseUrl);
                Logger.Log.Information($"Navigated to base URL: {baseUrl}"); Logger.Log.Information($"Navigated to base URL: {baseUrl}");

                _homePage.NavigateToAboutPage();
                Logger.Log.Information("Navigated to About page.");

                Logger.Log.Debug($"Expected URL: {aboutUrl}, Actual URL: {DriverSingleton.Driver.Url}");
                DriverSingleton.Driver.Url.Should().Be(aboutUrl, "the URL should match the About page URL.");

                Logger.Log.Debug($"Expected Title: {expectedTitle}, Actual Title: {DriverSingleton.Driver.Title}");
                DriverSingleton.Driver.Title.Should().Be(expectedTitle, "the page title should match the expected title.");

                Logger.Log.Debug($"Expected Header: {expectedHeader}, Actual Header: {_aboutPage.GetHeaderText()}");
                _aboutPage.GetHeaderText().Should().Be(expectedHeader, "the header text should match the expected header.");

                Logger.Log.Information("Test passed: VerifyNavigationToAboutEHUPage.");
                _test.Pass("VerifyNavigationToAboutEHUPage passed successfully.");
            }
            catch (Exception ex)
            {
                _test.Fail("Test failed: " + ex.Message);
                throw;
            }
        }

        // Data provider for "VerifySearchFunctionality"
        private static IEnumerable<object[]> SearchTestCases()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            Logger.Log.Information("Loaded configuration for search test cases.");

            yield return new object[]
            {
                config["TestSettings:EHUBaseUrl"],
                config["TestSettings:SearchTerm"]
            };
        }

        /// <summary>
        /// Test to verify the search functionality on the EHU website.
        /// </summary>
        [Test, TestCaseSource(nameof(SearchTestCases))]
        public void VerifySearchFunctionality(string baseUrl, string searchTerm)
        {
            try
            {
                Logger.Log.Information($"Starting test: VerifySearchFunctionality. Base URL: {baseUrl}, Search Term: {searchTerm}");

                _homePage.NavigateTo(baseUrl);
                Logger.Log.Information($"Navigated to base URL: {baseUrl}");

                _homePage.PerformSearch(searchTerm);
                Logger.Log.Information($"Performed search with term: {searchTerm}");

                Logger.Log.Debug($"Expected part of URL: /?s={searchTerm.Replace(" ", "+")}, Actual URL: {DriverSingleton.Driver.Url}");
                DriverSingleton.Driver.Url.Should().Contain("/?s=" + searchTerm.Replace(" ", "+"), "the search query should be part of the URL.");

                Logger.Log.Debug("Verifying search results presence.");
                _searchResultsPage.AreResultsPresent().Should().BeTrue("search results should be present.");

                Logger.Log.Debug("Verifying search results contain the expected term.");
                _searchResultsPage.DoResultsContainTerm("study program").Should().BeTrue("search results should contain the expected term 'study programs'.");

                Logger.Log.Information("Test passed: VerifySearchFunctionality.");
                _test.Pass("VerifySearchFunctionality passed successfully.");
            }
            catch (Exception ex)
            {
                _test.Fail("Test failed: " + ex.Message);
                throw;
            }
        }

        // Data provider for "VerifyLanguageChangeFunctionality"
        private static IEnumerable<object[]> LanguageChangeTestCases()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            Logger.Log.Information("Loaded configuration for language change test cases.");

            yield return new object[]
            {
                config["TestSettings:EHUBaseUrl"],
                config["TestSettings:LithuanianVersionUrl"]
            };
        }

        /// <summary>
        /// Test to verify the functionality of changing the website language from English to Lithuanian.
        /// </summary>
        [Test, TestCaseSource(nameof(LanguageChangeTestCases))]
        [Ignore("Skipping due to known issue with language switching functionality. Pending fix.")]
        public void VerifyLanguageChangeFunctionality(string baseUrl, string lithuanianUrl)
        {
            try
            {
                Logger.Log.Information($"Starting test: VerifyLanguageChangeFunctionality. Base URL: {baseUrl}, Lithuanian URL: {lithuanianUrl}");

                _homePage.NavigateTo(baseUrl);
                Logger.Log.Information($"Navigated to base URL: {baseUrl}");

                _homePage.SwitchLanguageToLithuanian();
                Logger.Log.Information("Switched language to Lithuanian.");

                Logger.Log.Debug($"Expected URL: {lithuanianUrl}, Actual URL: {DriverSingleton.Driver.Url}");
                DriverSingleton.Driver.Url.Should().Be(lithuanianUrl, "the URL should match the Lithuanian version of the site.");

                Logger.Log.Information("Test passed: VerifyLanguageChangeFunctionality.");
                _test.Pass("VerifyLanguageChangeFunctionality passed successfully.");
            }
            catch (Exception ex)
            {
                _test.Fail("Test failed: " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Tear down method that closes the browser and cleans up the WebDriver instance.
        /// </summary>
        [TearDown]
        public void Teardown()
        {
            var testStatus = TestContext.CurrentContext.Result.Outcome.Status;
            var stackTrace = TestContext.CurrentContext.Result.StackTrace;

            switch (testStatus)
            {
                case TestStatus.Passed:
                    _test.Pass("Test Passed");
                    break;
                case TestStatus.Failed:
                    _test.Fail($"Test Failed. {TestContext.CurrentContext.Result.Message}");
                    if (!string.IsNullOrEmpty(stackTrace))
                    {
                        _test.Fail(stackTrace);
                    }
                    break;
                case TestStatus.Skipped:
                    _test.Skip("Test Skipped.");
                    break;
                default:
                    _test.Warning("Test status unclear.");
                    break;
            }

            Logger.Log.Information("Tearing down the test environment and quitting WebDriver.");
            DriverSingleton.Driver.Quit();
        }

        /// <summary>
        /// Generate the ExtentReport after all tests are executed.
        /// </summary>
        [OneTimeTearDown]
        public void TearDownReporting()
        {
            try
            {
                Console.WriteLine("Flushing ExtentReports...");
                _extent.Flush();
                Console.WriteLine("ExtentReports flushed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during report generation: {ex.Message}");
            }
        }

    }
}
