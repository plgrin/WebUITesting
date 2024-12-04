using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace PageObjects
{
    public class HomePage
    {
        private readonly IWebDriver _driver;

        public HomePage(IWebDriver driver)
        {
            _driver = driver;
        }

        private IWebElement AboutLink => _driver.FindElement(By.LinkText("About"));

        private IWebElement SearchButton => _driver.FindElement(By.XPath("//*[@id=\"masthead\"]/div[1]/div/div[4]/div"));

        private IWebElement SearchBar => _driver.FindElement(By.XPath("//*[@id=\"masthead\"]/div[1]/div/div[4]/div/form/div/input"));

        private IWebElement LanguageSwitchButton => _driver.FindElement(By.XPath("//*[@id=\"masthead\"]/div[1]/div/div[4]/ul"));
        private IWebElement LithuanianLanguageOption => _driver.FindElement(By.XPath("//*[@id=\"masthead\"]/div[1]/div/div[4]/ul/li/ul/li[3]/a"));

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

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementToBeClickable(LithuanianLanguageOption));
            LithuanianLanguageOption.Click();
        }
    }
}
