using System;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using static SeleniumTest.ElementsInteraction;
using static SeleniumTest.RandomGenerator;

namespace SeleniumTest
{
    public class Task11
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private string email;
        private string password;
        private const string mainPage = "http://localhost/litecart/";
        private const string adminPage = "http://localhost/litecart/admin/";
        private const string securityPage = "http://localhost/litecart/admin/?app=settings&doc=security";

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            DisableCaptcha();
            driver.Url = mainPage;
        }

        /// <summary>
        /// Проверка регистрации нового пользователя:
        /// 1. Регистрация новой учётной записи с достаточно уникальным адресом электронной почты
        /// 2. Выход из учетной записи
        /// 3. Повторный вход в учётную запись
        /// 4. Выход из учетной записи
        /// </summary>
        [Test]
        public void NewUserRegistration()
        {
            email = GetRandomEmail();
            password = GetRandomPassword();
            OpenRegistrationPage();
            AcceptCookies();
            RegistrateUser("Test", "User", "United States", email, password);
            wait.Until(ExpectedConditions.UrlToBe(mainPage));
            LogOut();
            LogInByUser(email, password);
            LogOut();
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
        /// Отключение капчи
        /// </summary>
        private void DisableCaptcha()
        {
            driver.Url = adminPage;
            LogInByAdmin();
            driver.Url = securityPage;
            var pencilElement = GetElement(driver, By.ClassName("fa-pencil"));
            Assert.IsTrue(pencilElement is not null, "Иконка редактирования не найдена");
            pencilElement.Click();
            var buttonGroups = driver.FindElements(By.ClassName("btn-group"));
            Assert.IsTrue(buttonGroups.Count > 0, "Группы кнопок не найдены");
            var valueButtons = buttonGroups[0].FindElements(By.ClassName("btn"));
            Assert.IsTrue(valueButtons.Count == 2, "Кнопки true/false не найдены");
            valueButtons.Last().Click();
            var actionButtons = buttonGroups[1].FindElements(By.ClassName("btn"));
            Assert.IsTrue(actionButtons.Count > 0, "Кнопки действия не найдены");
            actionButtons.First().Click();
        }

        /// <summary>
        /// Вход под учетной записью админа
        /// </summary>
        private void LogInByAdmin()
        {
            SetField(driver, By.Name("username"), "admin");
            SetField(driver, By.Name("password"), "admin");
            var loginButton = driver.FindElements(By.Name("login"));
            Assert.IsTrue(loginButton.Count > 0, $"Кнопка Login не найдена на {driver.Url}");
            loginButton.Last().Click();
            wait.Until(ExpectedConditions.UrlToBe(adminPage));
        }

        /// <summary>
        /// Вход под учетной записью пользователя
        /// </summary>
        /// <param name="email">e-mail для входа</param>
        /// <param name="password">пароль для входа</param>
        private void LogInByUser(string email, string password)
        {
            var signInDropdown = GetElement(driver, By.ClassName("account"));
            Assert.IsTrue(signInDropdown is not null, $"Dropdown Sign In не найден на странице {driver.Url}");
            signInDropdown.Click();
            SetField(driver, By.Name("email"), email);
            SetField(driver, By.Name("password"), password);
            var signInButton = GetElement(driver, By.Name("login"));
            signInButton.Click();
        }

        /// <summary>
        /// Выход из аккаунта
        /// </summary>
        private void LogOut()
        {
            var userOptionsButton = GetElement(driver, By.ClassName("account"));
            Assert.IsTrue(userOptionsButton is not null, $"Dropdown не найден на странице {driver.Url}");
            userOptionsButton.Click();
            var logoutLink = GetElement(driver, By.XPath("//*[contains(text(), 'Logout')]"));
            Assert.IsTrue(logoutLink is not null, $"Ссылка для выхода из аккаунта не найдена на странице {driver.Url}");
            logoutLink.Click();
        }

        /// <summary>
        /// Переход на страницу регистрации
        /// </summary>
        private void OpenRegistrationPage()
        {
            var signInButton = GetElement(driver, By.ClassName("account"));
            Assert.IsTrue(signInButton is not null, $"Dropdown Sign In не найден на странице {driver.Url}");
            signInButton.Click();
            var registrationLink = GetElement(driver, By.XPath("//*[contains(text(), 'New customers click here')]"));
            Assert.IsTrue(registrationLink is not null, $"Ссылка для регистрации не найдена на странице {driver.Url}");
            registrationLink.Click();
        }

        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        /// <param name="firstName">Имя</param>
        /// <param name="lastName">Фамилия</param>
        /// <param name="country">Страна</param>
        /// <param name="email">E-mail</param>
        /// <param name="password">Пароль</param>
        private void RegistrateUser(string firstName, string lastName, string country, string email, string password)
        {
            var form = GetElement(driver, By.Name("customer_form"));
            Assert.IsTrue(form is not null, "Форма не найдена");
            SelectValueByText(form, By.Name("country_code"), country);
            SetField(form, By.Name("firstname"), firstName);
            SetField(form, By.Name("lastname"), lastName);
            SetField(form, By.Name("email"), email);
            SetField(form, By.Name("password"), password);
            SetField(form, By.Name("confirmed_password"), password);
            var privacyCheckBox = GetElement(form, By.Name("terms_agreed"));
            Assert.IsTrue(privacyCheckBox is not null, "Чек-бокс Privacy Policy не найден");
            privacyCheckBox.Click();
            var submitButton = GetElement(form, By.ClassName("btn"));
            Assert.IsTrue(submitButton is not null, "Кнопка отправки формы не найдена");
            submitButton.Click();
        }
        
        [TearDown]
        public void Teardown()
        {
            driver.Quit();
            driver = null;
        }
    }
}