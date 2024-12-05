using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Microsoft.Extensions.Configuration;
using System.IO;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using SeleniumExtras.WaitHelpers;
using PageObjects;
using Utilities;
using FluentAssertions; 

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

        /// <summary>
        /// Set up method that initializes the configuration and the Chrome WebDriver.
        /// </summary>
        [SetUp]
        public void Setup()
        {
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
            Logger.Log.Information($"Starting test: VerifyNavigationToAboutEHUPage. Base URL: {baseUrl}, About URL: {aboutUrl}");

            DriverSingleton.Driver.Navigate().GoToUrl(baseUrl);
            Logger.Log.Information($"Navigated to base URL: {baseUrl}");

            _homePage.NavigateToAboutPage();
            Logger.Log.Information("Navigated to About page.");

            Logger.Log.Debug($"Expected URL: {aboutUrl}, Actual URL: {DriverSingleton.Driver.Url}");
            DriverSingleton.Driver.Url.Should().Be(aboutUrl, "the URL should match the About page URL.");

            Logger.Log.Debug($"Expected Title: {expectedTitle}, Actual Title: {DriverSingleton.Driver.Title}");
            DriverSingleton.Driver.Title.Should().Be(expectedTitle, "the page title should match the expected title.");

            Logger.Log.Debug($"Expected Header: {expectedHeader}, Actual Header: {_aboutPage.GetHeaderText()}");
            _aboutPage.GetHeaderText().Should().Be(expectedHeader, "the header text should match the expected header.");

            Logger.Log.Information("Test passed: VerifyNavigationToAboutEHUPage.");
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
            Logger.Log.Information($"Starting test: VerifySearchFunctionality. Base URL: {baseUrl}, Search Term: {searchTerm}");

            DriverSingleton.Driver.Navigate().GoToUrl(baseUrl);
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
        public void VerifyLanguageChangeFunctionality(string baseUrl, string lithuanianUrl)
        {
            Logger.Log.Information($"Starting test: VerifyLanguageChangeFunctionality. Base URL: {baseUrl}, Lithuanian URL: {lithuanianUrl}");

            DriverSingleton.Driver.Navigate().GoToUrl(baseUrl);
            Logger.Log.Information($"Navigated to base URL: {baseUrl}");

            _homePage.SwitchLanguageToLithuanian();
            Logger.Log.Information("Switched language to Lithuanian.");

            Logger.Log.Debug($"Expected URL: {lithuanianUrl}, Actual URL: {DriverSingleton.Driver.Url}");
            DriverSingleton.Driver.Url.Should().Be(lithuanianUrl, "the URL should match the Lithuanian version of the site.");

            Logger.Log.Information("Test passed: VerifyLanguageChangeFunctionality.");
        }

        /// <summary>
        /// Tear down method that closes the browser and cleans up the WebDriver instance.
        /// </summary>
        [TearDown]
        public void Teardown()
        {
            Logger.Log.Information("Tearing down the test environment and quitting WebDriver.");
            DriverSingleton.Driver.Quit();
        }
    }
}
