using System;
using System.Collections.Generic;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace SeleniumTest
{
    public class Task9
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private const string adminPage = "http://localhost/litecart/admin/";
        private const string countriesPage = "http://localhost/litecart/admin/?app=countries&doc=countries";
        private const string zonesPage = "http://localhost/litecart/admin/?app=geo_zones&doc=geo_zones";

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Login();
        }

        private void Login()
        {
            driver.Url = $"{adminPage}login.php";
            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.ClassName("btn")).Click();
            wait.Until(ExpectedConditions.UrlToBe(adminPage));
        }

        /// <summary>
        /// Проверяет, страны расположены в алфавитном порядке на странице Countries
        /// </summary>
        [Test]
        public void CountriesArrangedAlphabetically()
        {
            driver.Url = countriesPage;
            var rows = driver.FindElements(By.CssSelector(".table tbody tr"));
            Assert.IsTrue(rows.Count > 0, "Таблица стран не содержит строк");
            CheckSorting(rows, By.CssSelector("a"), "textContent");
        }
        
        /// <summary>
        /// На странице Countries для стран, у которых количество зон отлично от нуля открывает страницу этой страны
        /// и проверяет, что зоны расположены в алфавитном порядке
        /// </summary>
        [Test]
        public void ZonesArrangedAlphabetically()
        {
            driver.Url = countriesPage;
            var rows = driver.FindElements(By.CssSelector(".table tbody tr"));
            Assert.IsTrue(rows.Count > 0, "Таблица стран не содержит строк");
            
            for (var i = 1; i < rows.Count + 1; i++)
            {
                var line = GetElement(driver, By.CssSelector($".table tbody tr:nth-child({i})"));
                Assert.IsTrue(line is not null, $"Строка {i} таблицы не найдена");
                var zonesCount = line.FindElements(By.CssSelector("td"))[5].Text;
                
                if (!zonesCount.Equals("0"))
                {
                    var linkToCountrySettings = GetElement(line, By.CssSelector("a"));
                    Assert.IsTrue(linkToCountrySettings is not null, $"Ссылка в строке таблицы {i} не найдена");
                    linkToCountrySettings.Click();
                    
                    var zonesLines = driver.FindElements(By.CssSelector(".table tbody tr"));
                    Assert.IsTrue(zonesLines.Count>0, $"Таблица зон для страны {i} пустая");
                    
                    CheckSorting(zonesLines, By.CssSelector("[name$='[name]']"), "value");
                    driver.Url = countriesPage;
                }
            }
        }

        /// <summary>
        /// Для каждой страны со страницы Geo Zones проверяет, что зоны расположены в алфавитном порядке
        /// </summary>
        [Test]
        public void ZonesArrangedAlphabetically2()
        {
            driver.Url = zonesPage;
            var rows = driver.FindElements(By.CssSelector(".table tbody tr"));
            Assert.IsTrue(rows.Count > 0, "Таблица стран не содержит строк");
            
            for (var i = 1; i < rows.Count + 1; i++)
            {
                var countryLink = GetElement(driver, By.CssSelector($".table tbody tr:nth-child({i}) a"));
                Assert.IsTrue(countryLink is not null, $"Ссылка в строке таблицы {i} не найдена");
                countryLink.Click();
                
                var zones = driver.FindElements(By.CssSelector(".table tbody tr"));
                Assert.IsTrue(zones.Count>0, $"Таблица зон для страны {i} пустая");
                    
                CheckSorting(zones, By.CssSelector(":nth-child(2)"), "textContent");
                driver.Url = zonesPage;
            }
        }
        
        /// <summary>
        /// Проверяет сортировку коллекции в алфавитном порядке
        /// </summary>
        /// <param name="collection">Коллекция веб-элементов</param>
        /// <param name="elementSelector">Селектор элемента, содержащего текст</param>
        /// <param name="textAttribute">Атрибут для получения текста из элемента</param>
        private void CheckSorting(IEnumerable<IWebElement> collection, By elementSelector, string textAttribute)
        {
            var previousElementText = string.Empty;
            
            foreach (var line in collection)
            {
                var element = GetElement(line, elementSelector);
                Assert.IsTrue(element is not null, $"Элемент {elementSelector} не найден");
                
                var elementText = element.GetAttribute(textAttribute);
                Assert.IsTrue(previousElementText.CompareTo(elementText) <= 0,
                    $"Элементы  расположены не в алфавитном порядке {previousElementText} {elementText}");
                previousElementText = elementText;
            }
        }

        /// <summary>
        /// Получает элемент страницы
        /// </summary>
        /// <param name="searchContext">Контекст поиска</param>
        /// <param name="selector">Селектор для поиска</param>
        /// <returns>Элемент или null</returns>
        private IWebElement GetElement(ISearchContext searchContext, By selector)
        {
            try
            {
                return searchContext.FindElement(selector);
            }
            catch (NoSuchElementException)
            {
                return null;
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