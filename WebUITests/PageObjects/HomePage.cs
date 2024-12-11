using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using WebUITests.PageObjects;

namespace PageObjects
{
    public class HomePage : BasePage
    {
        public HomePage(IWebDriver driver) : base(driver) { }

        private IWebElement AboutLink =>  Driver.FindElement(By.LinkText("About"));

        private IWebElement SearchButton => Driver.FindElement(By.XPath("//*[@id=\"masthead\"]/div[1]/div/div[4]/div"));

        private IWebElement SearchBar => Driver.FindElement(By.XPath("//*[@id=\"masthead\"]/div[1]/div/div[4]/div/form/div/input"));

        private IWebElement LanguageSwitchButton => Driver.FindElement(By.XPath("//*[@id=\"masthead\"]/div[1]/div/div[4]/ul"));
        private IWebElement LithuanianLanguageOption => Driver.FindElement(By.XPath("//*[@id=\"masthead\"]/div[1]/div/div[4]/ul/li/ul/li[3]/a"));

        public void NavigateToAboutPage()
        {
            AboutLink.Click();
        }

        public void PerformSearch(string searchTerm)
        {
            SearchButton.Click();
            SearchBar.SendKeys(searchTerm);
            SearchBar.SendKeys(Keys.Enter);
        }

        public void SwitchLanguageToLithuanian()
        {
            LanguageSwitchButton.Click();

            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementToBeClickable(LithuanianLanguageOption));
            LithuanianLanguageOption.Click();
        }
    }
}
