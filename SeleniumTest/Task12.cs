using System;
using System.IO;
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
    public class Task12
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private string email;
        private string password;
        private const string adminPage = "http://localhost/litecart/admin/";

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            driver.Url = adminPage;
        }

        /// <summary>
        /// Добавление нового товара в админке
        /// </summary>
        [Test]
        public void NewProductAdding()
        {
            var productName = GetRandomString(15);
            LogInByAdmin();
            OpenCatalogSettings();
            OpenProductAddingPage();
            FillGeneralSection("01.12.2020", "05.12.2022", productName, GetRandomString(), GetRandomString(),
                GetRandomString(), GetRandomString(), GetRandomString(),
                GetFileFromImagesDirectoryPath("punkDuck.jpg"));
            OpenAddingProductSection("Information");
            FillInformationSection("short test", "test description", "test technical data", "Punk duck", "meta");
            OpenAddingProductSection("Attributes");
            FillAttributesSection("Black", "Very big");
            SaveNewProduct();
            Assert.IsTrue(ProductContainsInCatalog(productName), $"Продукт {productName} не добавлен в каталог");
        }

        /// <summary>
        /// Входит под учетной записью админа
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
        /// Открывает раздел меню Catalog
        /// </summary>
        private void OpenCatalogSettings()
        {
            var catalogMenuItem = GetElement(driver, By.CssSelector("[data-code='catalog']"));
            Assert.IsTrue(catalogMenuItem is not null, "Пункт меню Catalog не найден");
            catalogMenuItem.Click();
        }

        /// <summary>
        /// Открывает страницу добавления товара
        /// </summary>
        private void OpenProductAddingPage()
        {
            var panelActions = GetElement(driver, By.ClassName("panel-action"));
            Assert.IsTrue(panelActions is not null, "Панель действий добавления не найдена");
            var addingButtons = panelActions.FindElements(By.ClassName("btn"));
            Assert.IsTrue(addingButtons.Count == 2,
                $"Количество кнопок добавления отлично от 2, кнопок: {addingButtons.Count}");
            addingButtons[1].Click();
        }

        /// <summary>
        /// Заполняет информацию на вкладке General
        /// </summary>
        /// <param name="dateValidFrom">Значение поля Date Valid From</param>
        /// <param name="dateValidTo">Значение поля Date Valid To</param>
        /// <param name="name">Значение поля Name</param>
        /// <param name="code">Значение поля Code</param>
        /// <param name="SKU">Значение поля SKU</param>
        /// <param name="MPN">Значеине поля MPN</param>
        /// <param name="GTIN">Значение поля GTIN</param>
        /// <param name="TARIC">Значение поля TARIC</param>
        /// <param name="productImagePath">Путь до изображения товара</param>
        private void FillGeneralSection(string dateValidFrom, string dateValidTo, string name, string code, string SKU,
            string MPN, string GTIN, string TARIC, string productImagePath)
        {
            SetField(driver, By.Name("date_valid_from"), dateValidFrom);
            SetField(driver, By.Name("date_valid_to"), dateValidTo);
            SetField(driver, By.Name("name[en]"), name);
            SetField(driver, By.Name("code"), code);
            SetField(driver, By.Name("sku"), SKU);
            SetField(driver, By.Name("mpn"), MPN);
            SetField(driver, By.Name("gtin"), GTIN);
            SetField(driver, By.Name("taric"), TARIC);
            SetField(driver, By.Name("new_images[]"), productImagePath);
        }

        /// <summary>
        /// Заполняет информацию на вкладке Information
        /// </summary>
        /// <param name="shortDescription">Значение поля Short Description</param>
        /// <param name="description">Значенеи поля Description</param>
        /// <param name="technicalData">Значение поля Technical Data</param>
        /// <param name="headTitle">Значение поля Head Title</param>
        /// <param name="metaDescription">Значение поля Meta Description</param>
        private void FillInformationSection(string shortDescription, string description, string technicalData,
            string headTitle, string metaDescription)
        {
            SetField(driver, By.Name("short_description[en]"), shortDescription);
            SetField(driver, By.ClassName("trumbowyg-editor"), description);
            SetField(driver, By.Name("technical_data[en]"), technicalData);
            SetField(driver, By.Name("head_title[en]"), headTitle);
            SetField(driver, By.Name("meta_description[en]"), metaDescription);
        }

        /// <summary>
        /// Заполняет информацию на вкладке Attributes
        /// </summary>
        /// <param name="color">Цвет товара</param>
        /// <param name="size">Размер товара</param>
        private void FillAttributesSection(string color, string size)
        {
            AddAttribute("Size", size);
            AddAttribute("Color", color);
        }

        /// <summary>
        /// Добавляет атрибут на вкладке Attributes
        /// </summary>
        /// <param name="attributeName">Название атрибута</param>
        /// <param name="value">Значение атрибута</param>
        private void AddAttribute(string attributeName, string value)
        {
            SelectValueByText(driver, By.Name("new_attribute[group_id]"), attributeName);
            SetField(driver, By.Name("new_attribute[custom_value]"), value);
            var addingButton = GetElement(driver, By.Name("add"));
            Assert.IsTrue(addingButton is not null, "Кнопка добавления атрибута не найдена");
            addingButton.Click();
        }

        /// <summary>
        /// Открывает вкладку в разделе Catalog
        /// </summary>
        /// <param name="name">Название вкладки</param>
        private void OpenAddingProductSection(string name)
        {
            var hrefName = $"#tab-{name.ToLower()}";
            var sectionLink = GetElement(driver, By.CssSelector($"[href='{hrefName}']"));
            Assert.IsTrue(sectionLink is not null, $"Вкладка {name} не найдена");
            sectionLink.Click();
        }

        /// <summary>
        /// Получает путь до файла в папке Images 
        /// </summary>
        /// <param name="imageName">Имя файла с раширением</param>
        /// <returns>Относительный путь до до файла</returns>
        private string GetFileFromImagesDirectoryPath(string imageName)
        {
            var currentPath = Directory.GetCurrentDirectory();
            var projectPath = Directory.GetParent(currentPath).Parent.Parent.ToString();
            return Path.Combine(projectPath, $@"Images\{imageName}");
        }

        /// <summary>
        /// Сохраняет введенные данные о новом добавляемом продукте
        /// </summary>
        private void SaveNewProduct()
        {
            var savingButton = GetElement(driver, By.Name("save"));
            Assert.True(savingButton is not null, "Кнопка сохранения не найдена");
            savingButton.Click();
        }

        /// <summary>
        /// Проверяет наличие товара с указанным именем в каталоге админа
        /// </summary>
        /// <param name="productName">Имя товара</param>
        /// <returns>Наличие файла, true - существует, false - не существует</returns>
        private bool ProductContainsInCatalog(string productName)
        {
            var elements = driver.FindElements(By.CssSelector("tbody tr td a"));
            return elements.Any(element => element.GetAttribute("text").Equals(productName));
        }

        [TearDown]
        public void Teardown()
        {
            driver.Quit();
            driver = null;
        }
    }
}