using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using static SeleniumTest.ElementsInteraction;
using static SeleniumTest.Logining;

namespace SeleniumTest
{
    public class Task14
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private string email;
        private string password;
        private const string adminPage = "http://localhost/litecart/admin/";
        private const string countriesPage = "http://localhost/litecart/admin/?app=countries&doc=countries";

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            driver.Url = adminPage;
        }

        /// <summary>
        /// Проверяет, что все внешние ссылки на странице редактирования страны открываются в новом окне
        /// </summary>
        [Test]
        public void ExternalLinksOpenInNewWindow()
        {
            LogInByAdmin(driver, wait, adminPage);
            driver.Url = countriesPage;
            OpenCountryPage(3);
            CheckAllWindowOpens();
        }
        
        /// <summary>
        /// Открывает страницу редактирования для страны с заданным порядковым номером
        /// </summary>
        /// <param name="number">Порядковый номер страны</param>
        private void OpenCountryPage(int number = 1)
        {
            var editIcons = driver.FindElements(By.ClassName("fa-pencil"));
            Assert.IsTrue(editIcons.Count > 0, $"Не найдено ни одного элемента редактирования для стран на {driver.Url}");
            Assert.IsTrue(number <= editIcons.Count || number < 1,
                $"Не удалось найти элемент редактирования для страны с порядковым номером {number}");
            editIcons[number - 1].Click();
        }
        
        /// <summary>
        /// Для всех внешних ссылок осуществляет переходы: дожидается открытия, переключается, закрывает
        /// </summary>
        private void CheckAllWindowOpens()
        {
            var externalLinksElements = driver.FindElements(By.ClassName("fa-external-link"));
            Assert.IsTrue(externalLinksElements.Count > 0, "Ни одного элемента в виде квадратика со стрелкой не найдено");
            
            foreach (var externalLinksElement in externalLinksElements)
            {
                var mainWindow = driver.CurrentWindowHandle;
                var oldWindows = driver.WindowHandles;
                externalLinksElement.Click();
                var newWindow = wait.Until(GetNewOpenWindow(oldWindows));
                driver.SwitchTo().Window(newWindow);
                driver.Close();
                driver.SwitchTo().Window(mainWindow);
            }
        }

        [TearDown]
        public void Teardown()
        {
            driver.Quit();
            driver = null;
        }
    }
}