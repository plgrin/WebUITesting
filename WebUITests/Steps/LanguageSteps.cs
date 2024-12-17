using NUnit.Framework;
using OpenQA.Selenium;
using TechTalk.SpecFlow;
using WebUITests.PageObjects;
using WebUITests.Utilities;

namespace WebUITests.Steps
{
    [Binding]
    public class LanguageSteps
    {
        private readonly IWebDriver _driver;
        private readonly HomePage _homePage;

        public LanguageSteps()
        {
            DriverSingleton.InitializeDriver();
            _driver = DriverSingleton.Driver;
            _homePage = new HomePage(_driver);
        }

        [Given(@"I am on the homepage")]
        public void GivenIAmOnTheHomepage()
        {
            string baseUrl = "https://en.ehu.lt";
            _homePage.NavigateTo(baseUrl);
        }

        [When(@"I change the language to ""(.*)""")]
        public void WhenIChangeTheLanguageTo(string language)
        {
            if (language == "Lithuanian")
            {
                _homePage.SwitchLanguageToLithuanian();
            }
        }

        [Then(@"I should be redirected to the Lithuanian version of the site")]
        public void ThenIShouldBeRedirectedToTheLithuanianVersionOfTheSite()
        {
            string lithuanianUrl = "https://lt.ehu.lt/";
            Assert.That(_driver.Url, Is.EqualTo("https://lt.ehu.lt/"), "The user was not redirected to the Lithuanian version of the site.");
        }

        [AfterScenario]
        public void TearDown()
        {
            DriverSingleton.QuitDriver();
        }
    }
}
