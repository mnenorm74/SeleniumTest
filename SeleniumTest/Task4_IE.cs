using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace SeleniumTest
{
    public class Task4
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private const string loginPage = "http://localhost/litecart/admin/login.php";
        
        [SetUp]
        public void Setup()
        {
            driver = new InternetExplorerDriver();
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