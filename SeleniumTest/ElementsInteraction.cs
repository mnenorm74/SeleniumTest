using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SeleniumTest
{
    public static class ElementsInteraction
    {
        /// <summary>
        /// Получает элемент страницы
        /// </summary>
        /// <param name="searchContext">Контекст поиска</param>
        /// <param name="selector">Селектор для поиска</param>
        /// <returns>Элемент или null</returns>
        public static IWebElement GetElement(ISearchContext searchContext, By selector)
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
        /// Заполняет поле заданным значением
        /// </summary>
        /// <param name="searchContext">Контекст поиска</param>
        /// <param name="selector">Селектор для поиска поля</param>
        /// <param name="value">Значение для заполнения</param>
        public static void SetField(ISearchContext searchContext, By selector, string value)
        {
            var field = GetElement(searchContext, selector);
            Assert.IsTrue(field is not null, $"Поле {selector} не найдено");
            field.Clear();
            field.SendKeys(value);
        }

        /// <summary>
        /// Выбирает значение из дропдауна по заданному тексту
        /// </summary>
        /// <param name="searchContext">Контекст поиска</param>
        /// <param name="selector">Селектор для поиска дропдауна</param>
        /// <param name="text">Текст для выбора значения</param>
        public static void SelectValueByText(ISearchContext searchContext, By selector, string text)
        {
            var dropdown = GetElement(searchContext, selector);
            Assert.IsTrue(dropdown is not null, $"Поле {selector} не найдено");
            var dropdownSelectElement = new SelectElement(dropdown);
            dropdownSelectElement.SelectByText(text);
        }
    }
}