using System;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using static SeleniumTest.ElementsInteraction;

namespace SeleniumTest
{
    public class Task13
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private string email;
        private string password;
        private const string mainPage = "http://localhost/litecart/";

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            driver.Url = mainPage;
        }
        
        /// <summary>
        /// Сценарий последовательного добавления товаров в корзину и их последовательного удаления из неё
        /// </summary>
        /// <param name="count">Количество добавляемых товаров</param>
        [TestCase(3)]
        [TestCase(12)]
        [TestCase(25)]
        public void AddAndDeleteProducts(int count)
        {
            AcceptCookies();
            AddProductsToCart(count);
            OpenCart();
            ClearCart();
        }
        
        /// <summary>
        /// Принятие куки
        /// </summary>
        private void AcceptCookies()
        {
            var acceptButton = GetElement(driver, By.Name("accept_cookies"));
            acceptButton?.Click();
        }

        /// <summary>
        /// Добавляет заданное число товаров в корзину
        /// </summary>
        /// <param name="count">Число товаров</param>
        private void AddProductsToCart(int count)
        {
            for (var i = 0; i < count; i++)
            {
                var products = driver.FindElements(By.ClassName("product-column"));
                Assert.IsTrue(products.Count > 0, "Главная страница не содержит товары");
                products[i < products.Count ? i : i % products.Count].Click();
                var addingButton = GetElement(driver, By.Name("add_cart_product"));
                Assert.IsTrue(addingButton is not null, $"Страница {driver.Url} не содержит кнопку добавления товара в корзину");
                addingButton.Click();
                var productsCounter = GetElement(driver, By.ClassName("quantity"));
                Assert.IsTrue(productsCounter is not null, "Счетчик товаров в корзине не найден");
                wait.Until(ExpectedConditions.TextToBePresentInElement(productsCounter, (i+1).ToString()));
                driver.Url = mainPage;
            }
        }

        /// <summary>
        /// Открывает корзину, дожидается загрузку итоговой таблицы
        /// </summary>
        private void OpenCart()
        {
            var cart = GetElement(driver, By.Id("cart"));
            Assert.IsTrue(cart is not null, "Корзина не найдена");
            cart.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("box-checkout-summary")));
        }

        
        /// <summary>
        /// Последовательно удаляет товары из корзины
        /// </summary>
        private void ClearCart()
        {
            var deletingButtons = driver.FindElements(By.Name("remove_cart_item"));
            Assert.IsTrue(deletingButtons.Count > 0, "Товары в корзине не найдены");
            for (var i = 0; i < deletingButtons.Count; i++)
            {
                DeleteFirstProduct();
            }
        }
        
        /// <summary>
        /// Удаляет первый товар из корзины, дожидается обновления итоговой таблицы (если элемент не единственный)
        /// </summary>
        private void DeleteFirstProduct()
        {
            var deletingButtons = driver.FindElements(By.Name("remove_cart_item"));
            Assert.IsTrue(deletingButtons.Count > 0, "Кнопки удаления в корзине не найдены");
            var summaryBox = GetElement(driver, By.Id("box-checkout-summary"));
            Assert.IsTrue(summaryBox is not null, "Блок Order Summary не найден");
            var orderSummaryTable = GetElement(summaryBox, By.ClassName("table"));
            Assert.IsTrue(orderSummaryTable is not null, "Таблица Order Summary не найдена");
            var amountElement = GetElement(orderSummaryTable, By.ClassName("currency-amount"));
            Assert.IsTrue(amountElement is not null, "Итоговая сумма не найдена");
            deletingButtons.First().Click();
            wait.Until(ExpectedConditions.StalenessOf(amountElement));
            if (deletingButtons.Count != 1)
            {
                wait.Until(ExpectedConditions.ElementExists(By.ClassName("currency-amount")));
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