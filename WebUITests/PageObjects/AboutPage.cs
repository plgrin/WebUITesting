﻿using OpenQA.Selenium;

namespace WebUITests.PageObjects
{
    public class AboutPage : BasePage
    {
        public AboutPage(IWebDriver driver) : base(driver) { }


        // Локатор заголовка H1
        private IWebElement Header => Driver.FindElement(By.TagName("h1"));

        public string GetHeaderText()
        {
            return Header.Text;
        }
    }
}
