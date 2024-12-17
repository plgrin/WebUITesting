using TechTalk.SpecFlow;
using NUnit.Framework;
using WebUITests.PageObjects;
using WebUITests.Utilities;
using OpenQA.Selenium;

namespace WebUITests.Steps
{
    [Binding]
    public class NavigationSteps
    {
        private readonly HomePage _homePage;
        private readonly AboutPage _aboutPage;
        private readonly IWebDriver _driver;

        public NavigationSteps()
        {
            DriverSingleton.InitializeDriver();
            _driver = DriverSingleton.Driver;
            _homePage = new HomePage(_driver);
            _aboutPage = new AboutPage(_driver);
        }

        [Given(@"I am on the homepage")]
        public void GivenIAmOnTheHomepage()
        {
            _homePage.NavigateTo("https://en.ehu.lt/");
        }

        [When(@"I navigate to the ""(.*)"" page")]
        public void WhenINavigateToThePage(string page)
        {
            if (page == "About")
            {
                _homePage.NavigateToAboutPage();
            }
            else
            {
                throw new NotImplementedException($"Page '{page}' is not implemented.");
            }
        }

        [Then(@"I should see the ""(.*)"" page header")]
        public void ThenIShouldSeeThePageHeader(string page)
        {
            if (page == "About")
            {
                string headerText = _aboutPage.GetHeaderText();
                Assert.That(headerText, Is.EqualTo("About"), "The About page header text is incorrect.");
            }
            else
            {
                throw new NotImplementedException($"Header check for page '{page}' is not implemented.");
            }
        }

        [AfterScenario]
        public void TearDown()
        {
            DriverSingleton.QuitDriver();
        }
    }
}
