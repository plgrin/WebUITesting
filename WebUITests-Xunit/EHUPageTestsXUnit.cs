using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using Xunit;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace WebUITestsXUnit
{

    /// <summary>
    /// This class contains tests for verifying various functionalities on the EHU website using xUnit.
    /// </summary>
    /// 

    public class EHUPageTestsXUnit : IDisposable
    {
        private IWebDriver driver;

        private readonly string ehuBaseUrl;
        private readonly string aboutPageUrl;
        private readonly string searchTerm;
        private readonly string lithuanianVersionUrl;

        /// <summary>
        /// Constructor for initializing the test environment, including loading configuration and setting up the WebDriver.
        /// </summary>
        public EHUPageTestsXUnit()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--incognito");
            chromeOptions.AddArgument("--disable-extensions");
            driver = new ChromeDriver(chromeOptions);
            driver.Manage().Window.Maximize();

            // Load configuration from appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // Retrieve data from the configuration file
            ehuBaseUrl = configuration["TestSettings:EHUBaseUrl"];
            aboutPageUrl = configuration["TestSettings:AboutPageUrl"];
            searchTerm = configuration["TestSettings:SearchTerm"];
            lithuanianVersionUrl = configuration["TestSettings:LithuanianVersionUrl"];
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
            driver.Navigate().GoToUrl(baseUrl);
            var aboutLink = driver.FindElement(By.LinkText("About"));
            aboutLink.Click();
            Assert.Equal(aboutUrl, driver.Url);
            Assert.Equal(expectedTitle, driver.Title);
            var header = driver.FindElement(By.TagName("h1")).Text;
            Assert.Equal(expectedHeader, header);
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
            driver.Navigate().GoToUrl(baseUrl);

            // Locate and click the search button to open the search bar
            var searchButton = driver.FindElement(By.XPath("//*[@id=\"masthead\"]/div[1]/div/div[4]/div"));
            searchButton.Click();

            // Locate the search bar and input the search term
            var searchBar = driver.FindElement(By.XPath("//*[@id=\"masthead\"]/div[1]/div/div[4]/div/form/div/input"));
            searchBar.SendKeys(searchTerm);

            // Submit the search query
            searchBar.SendKeys(Keys.Enter);

            // Verify the URL contains the search term
            Assert.Contains("/?s=study+programs", driver.Url);

            // Verify search results are displayed
            var searchResults = driver.FindElements(By.XPath("//*[@id=\"page\"]/div[3]"));
            Assert.True(searchResults.Count > 0, "No search results were found.");
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
            driver.Navigate().GoToUrl(baseUrl);

            // Locate and click the language switcher to open the menu
            var languageSwitchButton = driver.FindElement(By.XPath("//*[@id=\"masthead\"]/div[1]/div/div[4]/ul"));
            languageSwitchButton.Click();

            // Locate and click the Lithuanian language option (Lietuvių)
            var ltButton = driver.FindElement(By.XPath("//*[@id=\"masthead\"]/div[1]/div/div[4]/ul/li/ul/li[3]/a"));
            ltButton.Click();

            // Wait for the Lithuanian version to load and verify the URL
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.Url.Equals(lithuanianUrl));

            // Assert the URL has changed to the Lithuanian version
            Assert.Equal(lithuanianUrl, driver.Url);
        }

        /// <summary>
        /// Cleanup method to dispose of the WebDriver instance after tests are completed.
        /// </summary>
        public void Dispose()
        {
            driver?.Quit();
        }
    }
}
