using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace SeleniumTest
{
    public class Task6
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private const string loginPage = "http://localhost/litecart/admin/login.php";
        private const string firefoxNightlyPath = @"C:\Program Files\Firefox Nightly\firefox.exe";
        
        [SetUp]
        public void Setup()
        {
            var options = new FirefoxOptions {BrowserExecutableLocation = firefoxNightlyPath};
            driver = new FirefoxDriver(options);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }
        
        [Test]
        // Проверка заголовка страницы авторизации
        public void PageTitleIsMyStore()
        {
            driver.Url = loginPage;
            wait.Until(ExpectedConditions.TitleIs("My Store"));
        }

        [TearDown]
        public void Teardown()
        {
            driver.Quit();
            driver = null;
        }
    }
}