using Xunit;
using WebUITests_Xunit.PageObjects;
using WebUITests_Xunit.Utilities;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace WebUITests_Xunit.StepDefinitions
{
    public class LanguageChangeSteps
    {
        private readonly HomePage _homePage;

        public LanguageChangeSteps()
        {
            DriverSingleton.InitializeDriver();
            _homePage = new HomePage(DriverSingleton.Driver);
        }

        [Given(@"I open the EHU homepage at ""(.*)""")]
        public void GivenIOpenTheEHUHomepageAt(string url)
        {
            _homePage.NavigateTo(url);
        }

        [When(@"I switch the language to Lithuanian")]
        public void WhenISwitchTheLanguageToLithuanian()
        {
            _homePage.SwitchLanguageToLithuanian();
        }

        [Then(@"the current URL should be ""(.*)""")]
        public void ThenTheCurrentURLShouldBe(string expectedUrl)
        {
            DriverSingleton.Driver.Url.Should().Be(expectedUrl);
        }
    }
}
