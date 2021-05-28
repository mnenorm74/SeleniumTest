using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace SeleniumTest
{
    public class Task10
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
        
        /// <summary>
        /// Проверяет, что при клике на каждый товар открывается правильная страница товара,
        /// названия товара на странице совпадает с названием товара, на который кликнули
        /// </summary>
        [Test]
        public void ProductsNamesAtPagesEquivalentProductsCards()
        {
            var products = driver.FindElements(By.ClassName("product-column"));
            Assert.IsTrue(products.Count > 0, "Список продуктов на главной странице пустой");
            for (var i = 0; i < products.Count; i++)
            {
                CheckProductsNames(i);
                driver.Url = mainPage;
            }
        }

        /// <summary>
        /// Для товара с указанным индексом проверяет совпадение название в карточке продукта на главной странице и
        /// в заголовке на странице продукта
        /// </summary>
        /// <param name="productIndex">Индекс товара - порядковый номер</param>
        private void CheckProductsNames(int productIndex)
        {
            var products = driver.FindElements(By.ClassName("product-column"));
            Assert.IsTrue(products.Count >= productIndex, $"Список продуктов не содержит элемента с порядковым номером {productIndex}");
            var product = products[productIndex];
            
            var productNameAtMainPage = GetElementAttribute(product, By.ClassName("name"), "textContent");
            Assert.IsTrue(productNameAtMainPage is not null, $"Продукт с индексом {productIndex} не содержит название на главной странице");
            
            product.Click();
            product = GetElement(driver, By.Id("box-product"));
            Assert.IsTrue(product is not null, $"Страница продукта не содержит описание продукта на странице {driver.Url}");

            var productNameAtProductPage = GetElementAttribute(product, By.ClassName("title"), "textContent");
            Assert.IsTrue(productNameAtProductPage is not null, $"Продукт с индексом {productIndex} не содержит название на странице {driver.Url}");

            Assert.IsTrue(Equals(productNameAtMainPage, productNameAtProductPage), 
                $"Названия товаров на главной странице и на странице товара не совпадают для товара с индексом {productIndex}, страница {driver.Url}");
        }
        
        /// <summary>
        /// Для первого товара проверяет соответствие на главной странице и на странице продукта параметров:
        /// название товара, цены. 
        /// </summary>
        [Test]
        public void FirstProductContentCorrect()
        {
            var product = GetFirstProduct();
            var productNameAtMainPage = GetProductNameAtMainPage(product);
            var regularPriceAtMainPage = GetRegularPriceAtMainPage(product);
            var campaignPriceAtMainPage = GetCampaignPriceAtMainPage(product);
            
            product.Click();
            product = GetProductAtProductPage();

            var productNameAtProductPage = GetProductNameAtProductPage(product);
            Assert.IsTrue(Equals(productNameAtProductPage, productNameAtMainPage), 
                $"Название товара не соответствует на странице {driver.Url}");

            var regularPriceAtProductPage = GetRegularPriceAtProductPage(product);
            Assert.IsTrue(Equals(regularPriceAtProductPage, regularPriceAtMainPage), 
                $"Обычная цена товара не соответствует на странице {driver.Url}");

            var campaignPriceAtProductPage = GetCampaignPriceAtProductPage(product);
            Assert.IsTrue(Equals(campaignPriceAtProductPage, campaignPriceAtMainPage), 
                $"Акционная цена товара не соответствует на странице {driver.Url}");
        }

        /// <summary>
        /// Возвращает первый товар с главной страницы
        /// </summary>
        /// <returns>Карточка товара первого продукта</returns>
        private IWebElement GetFirstProduct()
        {
            var product = GetElement(driver, By.ClassName("product-column"));
            Assert.IsTrue(product is not null, "Список товаров не содержит элементов");
            return product;
        }

        /// <summary>
        /// Возвращает название товара с главной страницы 
        /// </summary>
        /// <param name="product">Карточка товара</param>
        /// <returns>Название товара</returns>
        private string GetProductNameAtMainPage(IWebElement product)
        {
            var productNameAtMainPage = GetElementAttribute(product, By.ClassName("name"), "textContent");
            Assert.IsTrue(productNameAtMainPage is not null, "Товар не содержит названия на главной странице");
            return productNameAtMainPage;
        }

        /// <summary>
        /// Возвращает обычную цену для товара с главной страницы
        /// </summary>
        /// <param name="product">Карточка товара</param>
        /// <returns>Обычная цена товара</returns>
        private string GetRegularPriceAtMainPage(IWebElement product)
        {
            var regularPriceAtMainPage = GetElementAttribute(product, By.ClassName("regular-price"), "textContent");
            Assert.IsTrue(regularPriceAtMainPage is not null, "Товар не содержит обычную цену на главной странице");
            return regularPriceAtMainPage;
        }

        /// <summary>
        /// Возвращает акционную цену для товара с главной страницы
        /// </summary>
        /// <param name="product"></param>
        /// <returns>Акционная цена товара</returns>
        private string GetCampaignPriceAtMainPage(IWebElement product)
        {
            var campaignPriceAtMainPage = GetElementAttribute(product, By.ClassName("campaign-price"), "textContent");
            Assert.IsTrue(campaignPriceAtMainPage is not null, "Товар не содержит акционную цену на главной странице");
            return campaignPriceAtMainPage;
        }
        
        /// <summary>
        /// Возвращает товар со страницы товара
        /// </summary>
        /// <returns>Товар со страницы товара</returns>
        private IWebElement GetProductAtProductPage()
        {
            var product = GetElement(driver, By.Id("box-product"));
            Assert.IsTrue(product is not null, $"Страница {driver.Url} не содержит товар");
            return product;
        }

        /// <summary>
        /// Возвращает название товара со страницы товара
        /// </summary>
        /// <param name="product">Товар со страницы товара</param>
        /// <returns>Название товара</returns>
        private string GetProductNameAtProductPage(IWebElement product)
        {
            var productNameAtProductPage = GetElementAttribute(product, By.ClassName("title"), "textContent");
            Assert.IsTrue(productNameAtProductPage is not null, $"Название товара не найдено на странице {driver.Url}");
            return productNameAtProductPage;
        }
        
        /// <summary>
        /// Возвращает обычную цену со страницы товара
        /// </summary>
        /// <param name="product">Товар со страницы товара</param>
        /// <returns>Обычная цена товара</returns>
        private string GetRegularPriceAtProductPage(IWebElement product)
        {
            var regularPriceAtProductPage = GetElementAttribute(product, By.ClassName("regular-price"), "textContent");
            Assert.IsTrue(regularPriceAtProductPage is not null, $"Обычная цена не найдена на странице {driver.Url}");
            return regularPriceAtProductPage;
        }

        /// <summary>
        /// Возвращает акционную цену со страницы товара
        /// </summary>
        /// <param name="product">Товар со страницы товара</param>
        /// <returns>Акционная цена товара</returns>
        private string GetCampaignPriceAtProductPage(IWebElement product)
        {
            var campaignPriceAtProductPage =
                GetElementAttribute(product, By.ClassName("campaign-price"), "textContent");
            Assert.IsTrue(campaignPriceAtProductPage is not null, $"Акционная цена не найдена на странице {driver.Url}");
            return campaignPriceAtProductPage;
        }

        /// <summary>
        /// Проверяет, что на каждой странице товара обычная цена серая и зачёркнутая, акционная цена красная и жирная
        /// </summary>
        [Test]
        public void PriceFontStylesCorrect()
        {
            var products = driver.FindElements(By.ClassName("product-column"));
            Assert.IsTrue(products.Count > 0, "Список продуктов на главной странице пустой");
            
            for (var i = 0; i < products.Count; i++)
            {
                CheckProductPricesFontStyle(i);
                driver.Url = mainPage;
            }
        }

        /// <summary>
        /// Для товара с указанным индексом проверяет, что на странице товара обычная цена серая и зачёркнутая,
        /// акционная цена красная и жирная
        /// </summary>
        /// <param name="productIndex">Индекс товара - порядковый номер</param>
        private void CheckProductPricesFontStyle(int productIndex)
        {
            var products = driver.FindElements(By.ClassName("product-column"));
            Assert.IsTrue(products.Count >= productIndex, $"Список продуктов не содержит элемента с порядковым номером {productIndex}");
            var product = products[productIndex];
            
            product.Click();
            product = GetElement(driver, By.Id("box-product"));
            Assert.IsTrue(product is not null, $"Страница продукта не содержит описание продукта на странице {driver.Url}");

            var regularPrice = GetElement(product, By.ClassName("regular-price"));
            if (regularPrice is not null)
            {
                CheckFontColor(regularPrice, "rgba(51, 51, 51");
                CheckFontDecoration(regularPrice, "line-through");
            }
            
            var campaignPrice = GetElement(product, By.ClassName("campaign-price"));
            if (campaignPrice is not null)
            {
                CheckFontColor(campaignPrice, "rgba(204, 0, 0, 1)");
                CheckFontWeight(campaignPrice, "700");
            }
        }
        

        private void CheckFontColor(IWebElement element, string expectedColor)
        {
            var elementFontColor = element.GetCssValue("color");
            Assert.IsTrue(elementFontColor.StartsWith(expectedColor), 
                $"Цвет шрифта на странице {driver.Url} не совпадает: {elementFontColor} вместо {expectedColor}");
        }

        private void CheckFontWeight(IWebElement element, string expectedWeight)
        {
            var elementFontWeight = element.GetCssValue("font-weight");
            Assert.IsTrue(elementFontWeight.Equals(expectedWeight), 
                $"Начертание шрифта на странице {driver.Url} не совпадает: {elementFontWeight} вместо {expectedWeight}");
        }

        private void CheckFontDecoration(IWebElement element, string expectedDecoration)
        {
            var elementFontDecoration = element.GetCssValue("text-decoration").Split(" ")[0];
            Assert.IsTrue(elementFontDecoration.Equals(expectedDecoration), 
                $"Стиль шрифта на странице {driver.Url} не совпадает: {elementFontDecoration} вместо {expectedDecoration}");
        }

        /// <summary>
        /// Проверяет, что а каждой странице товара акционная цена крупнее, чем обычная
        /// </summary>
        [Test]
        public void CampaignPriceLargerRegularPrice()
        {
            var products = driver.FindElements(By.ClassName("product-column"));
            Assert.IsTrue(products.Count > 0, "Список продуктов на главной странице пустой");
            
            for (var i = 0; i < products.Count; i++)
            {
                CheckProductPricesFontSize(i);
                driver.Url = mainPage;
            }
        }

        /// <summary>
        /// Для товара с указанным индексом проверяет, что акционная цена крупнее, чем обычная
        /// </summary>
        /// <param name="productIndex">Индекс товара - порядковый номер</param>
        private void CheckProductPricesFontSize(int productIndex)
        {
            var products = driver.FindElements(By.ClassName("product-column"));
            Assert.IsTrue(products.Count >= productIndex,
                $"Список продуктов не содержит элемента с порядковым номером {productIndex}");
            var product = products[productIndex];

            product.Click();
            product = GetElement(driver, By.Id("box-product"));
            Assert.IsTrue(product is not null,
                $"Страница продукта не содержит описание продукта на странице {driver.Url}");

            var regularPriceSize = GetElementAttribute(product, By.ClassName("regular-price"), "font-size");
            var campaignPriceSize = GetElementAttribute(product, By.ClassName("campaign-price"), "font-size");
            if (regularPriceSize is not null && campaignPriceSize is not null)
            {
                Assert.IsTrue(campaignPriceSize.CompareTo(regularPriceSize) > 0,
                    $"Шрифт акционной цены НЕ крупнее, чем шрифт обычной цены на странице {driver.Url}");
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

        /// <summary>
        /// Получает значение атрибута для элемента
        /// </summary>
        /// <param name="searchContext">Контекст поиска элемента</param>
        /// <param name="elementSelector">Селектор для поиска элемента</param>
        /// <param name="attributeName">Название атрибута</param>
        /// <returns>Значение атрибута или null</returns>
        private string GetElementAttribute(ISearchContext searchContext, By elementSelector, string attributeName)
        {
            try
            {
                return searchContext.FindElement(elementSelector).GetAttribute(attributeName);
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