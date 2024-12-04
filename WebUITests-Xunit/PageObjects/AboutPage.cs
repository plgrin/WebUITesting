using OpenQA.Selenium;

namespace WebUITests_Xunit.PageObjects
{
    public class AboutPage
    {
        private readonly IWebDriver _driver;

        public AboutPage(IWebDriver driver)
        {
            _driver = driver;
        }

        // Локатор заголовка H1
        private IWebElement Header => _driver.FindElement(By.TagName("h1"));

        public string GetHeaderText()
        {
            return Header.Text;
        }
    }
}
