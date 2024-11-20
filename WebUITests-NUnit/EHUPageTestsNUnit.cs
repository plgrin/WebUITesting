using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Microsoft.Extensions.Configuration;
using System.IO;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using SeleniumExtras.WaitHelpers;


namespace WebUITests
{
    /// <summary>
    /// This class contains tests for verifying various functionalities on the EHU website.
    /// </summary>
    [TestFixture]
    public class EHUPageTestsNUnit
    {
        private IWebDriver? driver;
        private string? ehuBaseUrl;
        private string? aboutPageUrl;
        private string? searchTerm;
        private string? lithuanianVersionUrl;

        /// <summary>
        /// Set up method that initializes the configuration and the Chrome WebDriver.
        /// </summary>
        [SetUp]
        public void Setup()
        {
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

            // Initialize ChromeDriver
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
        }

        /// <summary>
        /// Gets the initialized WebDriver instance.
        /// </summary>
        /// <returns>The WebDriver instance.</returns>
        public IWebDriver? GetDriver()
        {
            return driver;
        }

        /// <summary>
        /// Test to verify navigation to the "About EHU" page.
        /// </summary>
        [Test]
        public void VerifyNavigationToAboutEHUPage()
        {
            driver.Navigate().GoToUrl(ehuBaseUrl);
            var aboutLink = driver.FindElement(By.LinkText("About"));
            aboutLink.Click();
            Assert.That(driver.Url, Is.EqualTo(aboutPageUrl), "The URL does not match the expected value.");
            Assert.That(driver.Title, Is.EqualTo("About"), "The page title does not match the expected value.");
            var header = driver.FindElement(By.TagName("h1")).Text;
            Assert.That(header, Is.EqualTo("About"), "The content header does not match the expected value.");
        }

        /// <summary>
        /// Test to verify the search functionality on the EHU website.
        /// </summary>
        [Test]
        public void VerifySearchFunctionality()
        {
            // Step 1: Navigate to the base URL
            driver.Navigate().GoToUrl(ehuBaseUrl);

            // Step 2: Locate and click the search button to open the search bar
            var searchButton = driver.FindElement(By.XPath("//*[@id=\"masthead\"]/div[1]/div/div[4]/div"));
            searchButton.Click();

            // Step 3: Locate the search bar and input the search term
            var searchBar = driver.FindElement(By.XPath("//*[@id=\"masthead\"]/div[1]/div/div[4]/div/form/div/input"));
            searchBar.SendKeys(searchTerm);

            // Step 4: Submit the search query
            searchBar.SendKeys(Keys.Enter);

            // Step 5: Verify the URL contains the search term
            Assert.That(driver.Url, Does.Contain("/?s=study+programs"), "The URL does not contain the expected search query.");

            // Step 6: Verify search results are displayed
            var searchResults = driver.FindElements(By.XPath("//*[@id=\"page\"]/div[3]")); 
            Assert.That(searchResults.Count, Is.GreaterThan(0), "No search results were found.");

            // Step 7 (Optional): Check if search results contain relevant content
            bool resultsContainSearchTerm = searchResults.Any(result => result.Text.Contains("study program", StringComparison.OrdinalIgnoreCase));
            Assert.That(resultsContainSearchTerm, Is.True, "Search results do not contain expected term 'study programs'.");
        }

        /// <summary>
        /// Test to verify the functionality of changing the website language from English to Lithuanian.
        /// </summary>
        [Test]
        public void VerifyLanguageChangeFunctionality()
        {
            driver.Navigate().GoToUrl(ehuBaseUrl);

            // Step 2: Locate and click the language switcher to open the menu
            var languageSwitchButton = driver.FindElement(By.XPath("//*[@id=\"masthead\"]/div[1]/div/div[4]/ul"));
            languageSwitchButton.Click();

            // Step 3: Locate and click the Lithuanian language option (Lietuvių)
            var ltButton = driver.FindElement(By.XPath("//*[@id=\"masthead\"]/div[1]/div/div[4]/ul/li/ul/li[3]/a"));
            ltButton.Click();

            // Step 4: Wait for the Lithuanian version to load and verify the URL
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.Url.Equals(lithuanianVersionUrl));

            // Step 5: Assert the URL has changed to the Lithuanian version
            Assert.That(driver.Url, Is.EqualTo(lithuanianVersionUrl), "The URL does not match the expected value.");
        }

        /// <summary>
        /// Tear down method that closes the browser and cleans up the WebDriver instance.
        /// </summary>
        [TearDown]
        public void Teardown()
        {
            driver?.Quit();
        }
    }
}
