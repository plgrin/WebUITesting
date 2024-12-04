using OpenQA.Selenium;

namespace WebUITests_Xunit.PageObjects
{
    public class SearchResultsPage
    {
        private readonly IWebDriver _driver;

        public SearchResultsPage(IWebDriver driver)
        {
            _driver = driver;
        }

        // Локатор для поиска контейнера с результатами
        private IReadOnlyCollection<IWebElement> SearchResults => _driver.FindElements(By.XPath("//*[@id=\"page\"]/div[3]"));

        public bool AreResultsPresent()
        {
            return SearchResults.Count > 0;
        }

        public bool DoResultsContainTerm(string term)
        {
            return SearchResults.Any(result => result.Text.Contains(term, StringComparison.OrdinalIgnoreCase));
        }
    }
}
