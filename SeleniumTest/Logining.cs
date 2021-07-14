using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using static SeleniumTest.ElementsInteraction;

namespace SeleniumTest
{
    public static class Logining
    {
        /// <summary>
        /// Вход под учетной записью админа
        /// </summary>
        public static void LogInByAdmin(IWebDriver driver, WebDriverWait wait, string adminPage)
        {
            driver.Url = adminPage;
            SetField(driver, By.Name("username"), "admin");
            SetField(driver, By.Name("password"), "admin");
            var loginButton = driver.FindElements(By.Name("login"));
            Assert.IsTrue(loginButton.Count > 0, $"Кнопка Login не найдена на {driver.Url}");
            loginButton.Last().Click();
            wait.Until(ExpectedConditions.UrlToBe(adminPage));
        }
    }
}