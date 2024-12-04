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
            DriverSingleton.Driver.Navigate().GoToUrl(baseUrl);

            _homePage.NavigateToAboutPage();

            Assert.That(DriverSingleton.Driver.Url, Is.EqualTo(aboutUrl), "The URL does not match the expected value.");
            Assert.That(DriverSingleton.Driver.Title, Is.EqualTo(expectedTitle), "The page title does not match the expected value.");
            Assert.That(_aboutPage.GetHeaderText(), Is.EqualTo(expectedHeader), "The content header does not match the expected value.");
        }

        // Data provider for "VerifySearchFunctionality"
        private static IEnumerable<object[]> SearchTestCases()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

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
            DriverSingleton.Driver.Navigate().GoToUrl(baseUrl);

            _homePage.PerformSearch(searchTerm);

            Assert.That(DriverSingleton.Driver.Url, Does.Contain("/?s=" + searchTerm.Replace(" ", "+")), "The URL does not contain the expected search query.");
            Assert.That(_searchResultsPage.AreResultsPresent(), Is.True, "No search results were found.");
            Assert.That(_searchResultsPage.DoResultsContainTerm("study program"), Is.True, "Search results do not contain expected term 'study programs'.");
        }

        // Data provider for "VerifyLanguageChangeFunctionality"
        private static IEnumerable<object[]> LanguageChangeTestCases()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

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
            DriverSingleton.Driver.Navigate().GoToUrl(baseUrl);

            _homePage.SwitchLanguageToLithuanian();

            Assert.That(DriverSingleton.Driver.Url, Is.EqualTo(lithuanianUrl), "The URL does not match the expected value.");
        }

        /// <summary>
        /// Tear down method that closes the browser and cleans up the WebDriver instance.
        /// </summary>
        [TearDown]
        public void Teardown()
        {
            DriverSingleton.Driver.Quit();
        }
    }
}
