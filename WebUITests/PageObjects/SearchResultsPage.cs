﻿using OpenQA.Selenium;

namespace WebUITests.PageObjects
{
    public class SearchResultsPage : BasePage
    {
        public SearchResultsPage(IWebDriver driver) : base(driver) { }


        // Локатор для поиска контейнера с результатами
        private IReadOnlyCollection<IWebElement> SearchResults => Driver.FindElements(By.XPath("//*[@id=\"page\"]/div[3]"));

        public bool AreResultsPresent()
        {
            return SearchResults.Count > 0;
        }

        public bool DoResultsContainTerm(string term)
        {
            return SearchResults.Any(result => result.Text.Contains(term, StringComparison.OrdinalIgnoreCase));
        }

        public List<string> GetSearchResultsText()
        {
            return SearchResults
                .Select(result => result.Text.Trim()) // Извлекаем текст и убираем лишние пробелы
                .Where(text => !string.IsNullOrEmpty(text)) // Исключаем пустые строки
                .ToList();
        }
    }
}
