using Xunit;
using WebUITests_Xunit.PageObjects;
using WebUITests_Xunit.Utilities;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace WebUITests_Xunit.StepDefinitions
{
    public class SearchSteps
    {
        private readonly HomePage _homePage;
        private readonly SearchResultsPage _searchResultsPage;

        public SearchSteps()
        {
            DriverSingleton.InitializeDriver();
            _homePage = new HomePage(DriverSingleton.Driver);
            _searchResultsPage = new SearchResultsPage(DriverSingleton.Driver);
        }

        [Given(@"I open the EHU homepage at ""(.*)""")]
        public void GivenIOpenTheEHUHomepageAt(string url)
        {
            _homePage.NavigateTo(url);
        }

        [When(@"I perform a search with the term ""(.*)""")]
        public void WhenIPerformASearchWithTheTerm(string term)
        {
            _homePage.PerformSearch(term);
        }

        [Then(@"the search results page URL should contain ""(.*)""")]
        public void ThenTheSearchResultsPageURLShouldContain(string expectedPart)
        {
            DriverSingleton.Driver.Url.Should().Contain(expectedPart);
        }

        [Then(@"search results should be present")]
        public void ThenSearchResultsShouldBePresent()
        {
            _searchResultsPage.AreResultsPresent().Should().BeTrue();
        }

        [Then(@"search results should contain the term ""(.*)""")]
        public void ThenSearchResultsShouldContainTheTerm(string term)
        {
            _searchResultsPage.DoResultsContainTerm(term).Should().BeTrue();
        }
    }
}
