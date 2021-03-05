using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace SeleniumTest
{
    public class Task1
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        
        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [Test]
        public void TitleContainsSearchWord()
        {
            driver.Url = "https://yandex.ru/";
            driver.FindElement(By.Name("text")).SendKeys("webdriver");
            driver.FindElement(By.Name("text")).SendKeys(Keys.Enter);
            wait.Until(ExpectedConditions.TitleContains("webdriver — Яндекс: нашлось"));
        }

        [TearDown]
        public void Teardown()
        {
            driver.Quit();
            driver = null;
        }
    }
}