using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using Xunit;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using WebUITests_Xunit.PageObjects;
using WebUITests_Xunit.Utilities;
using FluentAssertions;

namespace WebUITestsXUnit
{

    /// <summary>
    /// This class contains tests for verifying various functionalities on the EHU website using xUnit.
    /// </summary>
    /// 

    public class EHUPageTestsXUnit : IDisposable
    {
        private readonly HomePage _homePage;
        private readonly AboutPage _aboutPage;
        private readonly SearchResultsPage _searchResultsPage;

        /// <summary>
        /// Constructor for initializing the test environment, including loading configuration and setting up the WebDriver.
        /// </summary>
        public EHUPageTestsXUnit()
        {
            Logger.Log.Information("Initializing WebDriver and setting up the test environment.");
            DriverSingleton.InitializeDriver();

            _homePage = new HomePage(DriverSingleton.Driver);
            _aboutPage = new AboutPage(DriverSingleton.Driver);
            _searchResultsPage = new SearchResultsPage(DriverSingleton.Driver);
        }

        /// <summary>
        /// Data provider for "VerifyNavigationToAboutEHUPage"
        /// </summary>
        public static IEnumerable<object[]> NavigationTestData =>
            new[]
            {
                new object[] { "https://en.ehu.lt", "https://en.ehu.lt/about/", "About", "About" }
            };

        /// <summary>
        /// Test to verify navigation to the "About EHU" page.
        /// </summary>
        [Theory]
        [MemberData(nameof(NavigationTestData))]
        [Trait("Category", "Navigation")]
        public void VerifyNavigationToAboutEHUPage(string baseUrl, string aboutUrl, string expectedTitle, string expectedHeader)
        {
            Logger.Log.Information($"Starting test: VerifyNavigationToAboutEHUPage. Base URL: {baseUrl}, About URL: {aboutUrl}");

            _homePage.NavigateTo(baseUrl);
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

        /// <summary>
        /// Data provider for "VerifySearchFunctionality"
        /// </summary>
        public static IEnumerable<object[]> SearchTestData =>
            new[]
            {
                new object[] { "https://en.ehu.lt", "study programs" }
            };

        /// <summary>
        /// Test to verify the search functionality on the EHU website.
        /// </summary>
        [Theory]
        [MemberData(nameof(SearchTestData))]
        [Trait("Category", "Search")]
        public void VerifySearchFunctionality(string baseUrl, string searchTerm)
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
        }

        /// <summary>
        /// Data provider for "VerifyLanguageChangeFunctionality"
        /// </summary>
        public static IEnumerable<object[]> LanguageChangeTestData =>
            new[]
            {
                new object[] { "https://en.ehu.lt", "https://lt.ehu.lt/" }
            };

        /// <summary>
        /// Test to verify the functionality of changing the website language from English to Lithuanian.
        /// </summary>
        [Theory]
        [MemberData(nameof(LanguageChangeTestData))]
        [Trait("Category", "Language")]
        public void VerifyLanguageChangeFunctionality(string baseUrl, string lithuanianUrl)
        {
            Logger.Log.Information($"Starting test: VerifyLanguageChangeFunctionality. Base URL: {baseUrl}, Lithuanian URL: {lithuanianUrl}");

            _homePage.NavigateTo(baseUrl);
            Logger.Log.Information($"Navigated to base URL: {baseUrl}");

            _homePage.SwitchLanguageToLithuanian();
            Logger.Log.Information("Switched language to Lithuanian.");

            Logger.Log.Debug($"Expected URL: {lithuanianUrl}, Actual URL: {DriverSingleton.Driver.Url}");
            DriverSingleton.Driver.Url.Should().Be(lithuanianUrl, "the URL should match the Lithuanian version of the site.");

            Logger.Log.Information("Test passed: VerifyLanguageChangeFunctionality.");
        }

        /// <summary>
        /// Cleanup method to dispose of the WebDriver instance after tests are completed.
        /// </summary>
        public void Dispose()
        {
            Logger.Log.Information("Tearing down the test environment and quitting WebDriver.");
            DriverSingleton.Driver?.Quit();
        }
    }
}
