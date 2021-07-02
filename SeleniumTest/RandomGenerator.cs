using System;
using System.Text;

namespace SeleniumTest
{
    public static class RandomGenerator
    {
        /// <summary>
        /// Генерация случайного символа
        /// </summary>
        /// <param name="startIndex">Стартовый индекс кода ascii символа (по умолчанию 32 = пробел)</param>
        /// <param name="endIndex">Конечный индекс кода ascii символа (по умолчанию 126 = ~)</param>
        /// <returns></returns>
        public static char GetRandomChar(int startIndex = 32, int endIndex = 126)
        {
            var random = new Random();
            return (char) random.Next(startIndex, endIndex);
        }
        
        /// <summary>
        /// Генерация случайной буквы от 'a' до 'z'
        /// </summary>
        /// <returns></returns>
        public static char GetRandomLetter()
        {
            var random = new Random();
            return (char) random.Next('a', 'z');
        }
        
        /// <summary>
        /// Генерация случайной строки
        /// </summary>
        /// <param name="length">Длина строки (по умолчанию 10)</param>
        /// <returns></returns>
        public static string GetRandomString(int length = 10)
        {
            var line = new StringBuilder();
            for (var i = 0; i < length; i++)
            {
                line.Append(GetRandomLetter());
            }

            return line.ToString();
        }
        
        /// <summary>
        /// Генерация случайного e-mail
        /// </summary>
        /// <param name="firstPartLength">Длина первой части (по умолчанию 15)</param>
        /// <param name="secondPartLength">Длина второй части - доменной (по умолчанию 10)</param>
        /// <returns></returns>
        public static string GetRandomEmail(int firstPartLength = 15, int secondPartLength = 10)
        {
            var firstPart = GetRandomString(firstPartLength);
            var secondPart = GetRandomString(secondPartLength);
            
            return new StringBuilder()
                .Append(firstPart)
                .Append('@')
                .Append(secondPart)
                .Append(".ru")
                .ToString();
        }
        
        /// <summary>
        /// Генерация случайного пароля
        /// </summary>
        /// <param name="length">Длина пароля (по умолчанию 12)</param>
        /// <returns></returns>
        public static string GetRandomPassword(int length = 12)
        {
            var password = new StringBuilder();
            for (var i = 0; i < length; i++)
            {
                password.Append(GetRandomChar());
            }

            return password.ToString();
        }
    }
}