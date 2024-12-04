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
            DriverSingleton.Driver.Navigate().GoToUrl(baseUrl);
            _homePage.NavigateToAboutPage();

            Assert.Equal(aboutUrl, DriverSingleton.Driver.Url);
            Assert.Equal(expectedTitle, DriverSingleton.Driver.Title);
            Assert.Equal(expectedHeader, _aboutPage.GetHeaderText());
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
            DriverSingleton.Driver.Navigate().GoToUrl(baseUrl);

            _homePage.PerformSearch(searchTerm);

            Assert.Contains("/?s=" + searchTerm.Replace(" ", "+"), DriverSingleton.Driver.Url);
            Assert.True(_searchResultsPage.AreResultsPresent(), "No search results were found.");
            Assert.True(_searchResultsPage.DoResultsContainTerm("study program"), "Search results do not contain expected term 'study programs'.");
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
            DriverSingleton.Driver.Navigate().GoToUrl(baseUrl);

            _homePage.SwitchLanguageToLithuanian();

            Assert.Equal(lithuanianUrl, DriverSingleton.Driver.Url);
        }

        /// <summary>
        /// Cleanup method to dispose of the WebDriver instance after tests are completed.
        /// </summary>
        public void Dispose()
        {
            DriverSingleton.Driver?.Quit();
        }
    }
}
