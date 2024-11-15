using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace WebUITests
{
    /// <summary>
    /// This class contains tests for verifying various functionalities on the EHU website.
    /// </summary>
    [TestFixture]
    public class EHUPageTests
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
            var aboutLink = driver.FindElement(By.LinkText("About EHU"));
            aboutLink.Click();
            Assert.That(driver.Url, Is.EqualTo(aboutPageUrl), "The URL does not match the expected value.");
            Assert.That(driver.Title, Is.EqualTo("About EHU"), "The page title does not match the expected value.");
            var header = driver.FindElement(By.TagName("h1")).Text;
            Assert.That(header, Is.EqualTo("About European Humanities University"), "The content header does not match the expected value.");
        }

        /// <summary>
        /// Test to verify the search functionality on the EHU website.
        /// </summary>
        [Test]
        public void VerifySearchFunctionality()
        {
            driver.Navigate().GoToUrl(ehuBaseUrl);
            var searchBox = driver.FindElement(By.Name("s"));
            searchBox.SendKeys(searchTerm);
            searchBox.SendKeys(Keys.Enter);
            Assert.That(driver.Url, Does.Contain("/?s=study+programs"), "The URL does not contain the expected search query.");
            var searchResults = driver.FindElements(By.CssSelector(".search-result-class")); // Replace with the correct selector
            Assert.That(searchResults.Count, Is.GreaterThan(0), "No search results were found.");
        }

        /// <summary>
        /// Test to verify the functionality of changing the website language from English to Lithuanian.
        /// </summary>
        [Test]
        public void VerifyLanguageChangeFunctionality()
        {
            driver.Navigate().GoToUrl(ehuBaseUrl);
            var languageSwitcher = driver.FindElement(By.Id("language-switcher-id")); // Replace with the correct selector
            languageSwitcher.Click();
            var lithuanianOption = driver.FindElement(By.LinkText("Lietuvių")); // Replace with the correct text or selector
            lithuanianOption.Click();
            Assert.That(driver.Url, Is.EqualTo(lithuanianVersionUrl), "The URL does not match the Lithuanian version of the site.");
            var content = driver.FindElement(By.TagName("body")).Text;
            Assert.That(content, Does.Contain("lietuvų"), "The page content is not displayed in Lithuanian.");
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
