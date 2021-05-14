using System;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace SeleniumTest
{
    public class Task8
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private const string mainPage = "http://localhost/litecart/";
        
        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            driver.Url = mainPage;
        }


        // Проверка наличия единственно стикера у каждого товара
        [Test]
        public void ProductsHaveOneSticker()
        {
            var products = driver.FindElements(By.ClassName("product-column"));
            foreach (var product in products)
            {
                var stickers = product.FindElements(By.ClassName("sticker"));
                Assert.IsTrue(stickers.Count==1);
            }
        }
        
        // Проверка наличия не более одного стикера (0, 1) - текущая реализация
        [Test]
        public void ProductsHaveNoMoreOneSticker()
        {
            var products = driver.FindElements(By.ClassName("product-column"));
            foreach (var product in products)
            {
                var stickers = product.FindElements(By.ClassName("sticker"));
                Assert.IsTrue(stickers.Count<=1);
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