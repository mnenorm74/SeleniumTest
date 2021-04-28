using System;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace SeleniumTest
{
    public class Task3
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private const string mainPage = "http://localhost/litecart/";
        private const string loginPage = "http://localhost/litecart/admin/login.php";
        private const string dashboardPage = "http://localhost/litecart/admin/";
        
        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }
        
        [Test]
        // Проверка заголовка страницы авторизации
        public void PageTitleIsMyStore()
        {
            driver.Url = loginPage;
            wait.Until(ExpectedConditions.TitleIs("My Store"));
        }
        
        [Test]
        // Проверка url при переходе по клику на изображение
        public void ClickToIconRedirectsToMainPage()
        {
            driver.Url = loginPage;
            driver.FindElement(By.CssSelector("img")).Click();
            wait.Until(ExpectedConditions.UrlToBe(mainPage));
        }
        
        [Test]
        // Проверка заголовка при переходе по клику на изображение
        public void ClickToIconRedirectsToPageWithMainPageTitle()
        {
            driver.Url = loginPage;
            driver.FindElement(By.CssSelector("img")).Click();
            wait.Until(ExpectedConditions.TitleIs("My Store | Online Store"));
        }

        [Test]
        // Проверка состояния чекбокса по умолчанию
        public void CheckBoxNotSelected()
        {
            driver.Url = loginPage;
            Assert.AreEqual(driver.FindElement(By.Name("remember_me")).Selected, false);
        }

        [Test]
        // Проверка url страницы после авторизации
        // Верный логин и пароль, нажатие enter, чекбокс не отмечен
        public void ValidLoginAndEnterRedirectToDashboard()
        {
            driver.Url = loginPage;
            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys(Keys.Enter);
            wait.Until(ExpectedConditions.UrlToBe(dashboardPage));
        }
        
        [Test]
        // Проверка url страницы после авторизации
        // Верный логин и пароль, нажатие enter, чекбокс отмечен
        public void ValidLoginCheckBoxAndEnterRedirectToDashboard()
        {
            driver.Url = loginPage;
            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.Name("remember_me")).Click();
            driver.FindElement(By.Name("password")).SendKeys(Keys.Enter);
            wait.Until(ExpectedConditions.UrlToBe(dashboardPage));
        }
        
        [Test]
        // Проверка заголовка страницы после авторизации
        // Верный логин и пароль, нажатие enter, чекбокс не отмечен
        public void ValidLoginAndEnterRedirectToPageWithDashboardTitle()
        {
            driver.Url = loginPage;
            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys(Keys.Enter);
            wait.Until(ExpectedConditions.TitleIs("Dashboard | My Store"));
        }
        
        [Test]
        // Проверка заголовка страницы после авторизации
        // Верный логин и пароль, нажатие enter, чекбокс отмечен
        public void ValidLoginCheckboxAndEnterRedirectToPageWithDashboardTitle()
        {
            driver.Url = loginPage;
            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.Name("remember_me")).Click();
            driver.FindElement(By.Name("password")).SendKeys(Keys.Enter);
            wait.Until(ExpectedConditions.TitleIs("Dashboard | My Store"));
        }
        
        [Test]
        // Проверка url страницы после авторизации
        // Верный логин и пароль, нажатие на кнопку, чекбокс не отмечен
        public void ValidLoginAndButtonClickRedirectsToDashboard()
        {
            driver.Url = loginPage;
            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElements(By.Name("login")).Last().Click();
            wait.Until(ExpectedConditions.UrlToBe(dashboardPage));
        }
        
        [Test]
        // Проверка url страницы после авторизации
        // Верный логин и пароль, нажатие на кнопку, чекбокс отмечен
        public void ValidLoginCheckBoxAndButtonClickRedirectsToDashboard()
        {
            driver.Url = loginPage;
            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.Name("remember_me")).Click();
            driver.FindElements(By.Name("login")).Last().Click();
            wait.Until(ExpectedConditions.UrlToBe(dashboardPage));
        }
        
        [Test]
        // Проверка заголовка страницы после авторизации
        // Верный логин и пароль, нажатие на кнопку, чекбокс не отмечен
        public void ValidLoginAndButtonClickRedirectToPageWithDashboardTitle()
        {
            driver.Url = loginPage;
            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElements(By.Name("login")).Last().Click();
            wait.Until(ExpectedConditions.TitleIs("Dashboard | My Store"));
        }
        
        [Test]
        // Проверка заголовка страницы после авторизации
        // Верный логин и пароль, нажатие на кнопку, чекбокс отмечен
        public void ValidLoginCheckBoxAndButtonClickRedirectToPageWithDashboardTitle()
        {
            driver.Url = loginPage;
            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.Name("remember_me")).Click();
            driver.FindElements(By.Name("login")).Last().Click();
            wait.Until(ExpectedConditions.TitleIs("Dashboard | My Store"));
        }
        
        [TestCase("", "")]
        [TestCase("admin", "")]
        [TestCase("", "admin")]
        [TestCase("admin", "admin123")]
        // Проверка url страницы после авторизации
        // НЕверный логин и пароль, нажатие enter, чекбокс не отмечен
        public void InvalidLoginAndEnterDoesNotRedirectsToDashboard(string login, string password)
        {
            driver.Url = loginPage;
            driver.FindElement(By.Name("username")).SendKeys(login);
            driver.FindElement(By.Name("password")).SendKeys(password);
            driver.FindElement(By.Name("password")).SendKeys(Keys.Enter);
            wait.Until(ExpectedConditions.UrlToBe(loginPage));
        }
        
        [TestCase("", "")]
        [TestCase("admin", "")]
        [TestCase("", "admin")]
        [TestCase("admin", "admin123")]
        // Проверка url страницы после авторизации
        // НЕверный логин и пароль, нажатие enter, чекбокс отмечен
        public void InvalidLoginCheckBoxAndEnterDoesNotRedirectsToDashboard(string login, string password)
        {
            driver.Url = loginPage;
            driver.FindElement(By.Name("username")).SendKeys(login);
            driver.FindElement(By.Name("password")).SendKeys(password);
            driver.FindElement(By.Name("remember_me")).Click();
            driver.FindElement(By.Name("password")).SendKeys(Keys.Enter);
            wait.Until(ExpectedConditions.UrlToBe(loginPage));
        }

        [TestCase("", "")]
        [TestCase("admin", "")]
        [TestCase("", "admin")]
        [TestCase("admin", "admin123")]
        // Проверка url страницы после авторизации
        // НЕверный логин и пароль, нажатие на кнопку, чекбокс не отмечен
        public void InvalidLoginAndButtonClickDoesNotRedirectsToDashboard(string login, string password)
        {
            driver.Url = loginPage;
            driver.FindElement(By.Name("username")).SendKeys(login);
            driver.FindElement(By.Name("password")).SendKeys(password);
            driver.FindElements(By.Name("login")).Last().Click();
            wait.Until(ExpectedConditions.UrlToBe(loginPage));
        }
        
        [TestCase("", "")]
        [TestCase("admin", "")]
        [TestCase("", "admin")]
        [TestCase("admin", "admin123")]
        // Проверка url страницы после авторизации
        // НЕверный логин и пароль, нажатие на кнопку, чекбокс отмечен
        public void InvalidLoginCheckBoxAndButtonClickDoesNotRedirectsToDashboard(string login, string password)
        {
            driver.Url = loginPage;
            driver.FindElement(By.Name("username")).SendKeys(login);
            driver.FindElement(By.Name("password")).SendKeys(password);
            driver.FindElement(By.Name("remember_me")).Click();
            driver.FindElements(By.Name("login")).Last().Click();
            wait.Until(ExpectedConditions.UrlToBe(loginPage));
        }
        
        // Проверка видимости предупреждений
        [Test]
        public void InvalidInputShowWarnings()
        {
            driver.Url = loginPage;
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElements(By.Name("login")).Last().Click();
            wait.Until(ExpectedConditions.ElementIsVisible(
                By.Id("notices")));
        }
        
        // Проверка предупреждений (не введен логин) 
        // "You must provide a username"
        [Test]
        public void EmptyLoginShowWarnings()
        {
            driver.Url = loginPage;
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElements(By.Name("login")).Last().Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[contains(text(), 'You must provide a username')]")));
        }
        
        // Проверка предупреждения (неверный логин/пароль)
        // "Wrong combination of username and password or the account does not exist."
        [Test]
        public void InvalidLoginPasswordShowWarnings()
        {
            driver.Url = loginPage;
            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("password");
            driver.FindElements(By.Name("login")).Last().Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(
                "//*[contains(text(), 'Wrong combination of username and password or the account does not exist.')]")));
        }
        
        // Проверка предупреждения (неизвестный логин/пароль)
        // "The user could not be found in our database"
        [Test]
        public void UnknownLoginPasswordShowWarnings()
        {
            driver.Url = loginPage;
            driver.FindElement(By.Name("username")).SendKeys("lol");
            driver.FindElement(By.Name("password")).SendKeys("password");
            driver.FindElements(By.Name("login")).Last().Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(
                "//*[contains(text(), 'The user could not be found in our database')]")));
        }
        
        [TearDown]
        public void Teardown()
        {
            driver.Quit();
            driver = null;
        }
    }
}