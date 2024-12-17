using WebUITests_Xunit.PageObjects;
using WebUITests_Xunit.Utilities;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace WebUITests_Xunit.Steps
{
    public class NavigationSteps
    {
        private readonly HomePage _homePage;
        private readonly AboutPage _aboutPage;

        public NavigationSteps()
        {
            DriverSingleton.InitializeDriver();
            _homePage = new HomePage(DriverSingleton.Driver);
            _aboutPage = new AboutPage(DriverSingleton.Driver);
        }

        [Given(@"I open the EHU homepage at ""(.*)""")]
        public void GivenIOpenTheEHUHomepageAt(string url)
        {
            _homePage.NavigateTo(url);
        }

        [When(@"I navigate to the About page")]
        public void WhenINavigateToTheAboutPage()
        {
            _homePage.NavigateToAboutPage();
        }

        [Then(@"the current URL should be ""(.*)""")]
        public void ThenTheCurrentURLShouldBe(string expectedUrl)
        {
            DriverSingleton.Driver.Url.Should().Be(expectedUrl);
        }

        [Then(@"the page title should be ""(.*)""")]
        public void ThenThePageTitleShouldBe(string expectedTitle)
        {
            DriverSingleton.Driver.Title.Should().Be(expectedTitle);
        }

        [Then(@"the page header should be ""(.*)""")]
        public void ThenThePageHeaderShouldBe(string expectedHeader)
        {
            _aboutPage.GetHeaderText().Should().Be(expectedHeader);
        }
    }
}
