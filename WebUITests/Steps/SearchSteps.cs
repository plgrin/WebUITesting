using TechTalk.SpecFlow;
using NUnit.Framework;
using WebUITests.PageObjects;
using WebUITests.Utilities;
using OpenQA.Selenium;

namespace WebUITests.Steps
{
    [Binding]
    public class SearchSteps
    {
        private readonly HomePage _homePage;
        private readonly SearchResultsPage _searchResultsPage;
        private readonly IWebDriver _driver;

        public SearchSteps()
        {
            DriverSingleton.InitializeDriver();
            _driver = DriverSingleton.Driver;
            _homePage = new HomePage(_driver);
            _searchResultsPage = new SearchResultsPage(_driver);
        }

        [Given(@"I am on the homepage")]
        public void GivenIAmOnTheHomepage()
        {
            _homePage.NavigateTo("https://en.ehu.lt/");
        }

        [When(@"I perform a search for ""(.*)""")]
        public void WhenIPerformASearchFor(string searchTerm)
        {
            _homePage.PerformSearch(searchTerm);
        }

        [Then(@"I should see search results containing ""(.*)""")]
        public void ThenIShouldSeeSearchResultsContaining(string expectedTerm)
        {
            bool containsTerm = _searchResultsPage.DoResultsContainTerm(expectedTerm);
            Assert.That(containsTerm, Is.True, $"Search results do not contain the term '{expectedTerm}'.");
        }

        [AfterScenario]
        public void TearDown()
        {
            DriverSingleton.QuitDriver();
        }
    }
}
