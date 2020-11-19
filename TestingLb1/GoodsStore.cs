using System;
using System.Collections.Generic;

namespace TestingLb1
{
    class GoodsStore
    {
        //Коллекция товаров
        private Dictionary<string, Product> Products { get; set; }

        //Коллекция для хранения информации о продажах
        private Dictionary<string, Product> Statistics { get; set; }


        //Инициализация
        public GoodsStore()
        {
            Products = new Dictionary<string, Product>();
            Statistics = new Dictionary<string, Product>();
        }


        //Структура, хранящая информацию о товарах
        public struct Product
        {
            public int count;
            public int price;
        }

        /// <summary>
        /// Покупка товара
        /// </summary>
        /// <param name="name">Название товара</param>
        /// <param name="count">Количество товара</param>
        /// <returns>Результат выполнения операции</returns>
        public bool Buy(string name, int count)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            if (!Products.ContainsKey(name))
                throw new Exception("Товар не существует");

            if (count < 1)
                throw new Exception("Не верное количество товара для покупки");

            if (Products[name].count < count)
            {
                return false;
            }
            else
            {
                var product = Products[name];
                product.count -= count;
                Products[name] = product;

                var productIncome = Statistics[name];
                productIncome.count += count;
                productIncome.price += product.price * count;
                Statistics[name] = productIncome;

                return true;
            }
        }

        /// <summary>
        /// Подсчёт прибыли
        /// </summary>
        /// <param name="name">Название товара</param>
        /// <returns>Общую прибыль с продажи указанного вида товара</returns>
        public double totalProductIncome(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            if (!Statistics.ContainsKey(name))
                throw new Exception("Товар не существует");

            double income = 0;
            foreach(var product in Statistics)
            {
                if(product.Key == name)
                income += product.Value.price;
            }
            return income;
        }


        /// <summary>
        /// Оформление возврата товара
        /// </summary>
        /// <param name="name">Название товара</param>
        /// <param name="count">Количество товаров</param>
        /// <param name="quality">Качество возвращаемого товара</param>
        /// <returns>Результат выполнения операции</returns>
        public bool PerformRefund(string name,int count, bool quality)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            if (!Products.ContainsKey(name))
                throw new Exception("Товар не существует");

            if (count < 1)
                throw new Exception("Не верное количество товара для возврата");

            var product = Products[name];
            if (quality)
            {
                product.count += count;
                Products[name] = product;
            }

            var productIncome = Statistics[name];
            productIncome.price -= product.price * count;
            Statistics[name] = productIncome;

            return true;         
        }


        /// <summary>
        /// Завоз товаров в магазин
        /// </summary>
        /// <param name="name">Название товара</param>
        /// <param name="count">Количество товара</param>
        /// <returns>Результат выполнения операции</returns>
        public bool ImportGoods(string name, int count)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            if (!Products.ContainsKey(name))
                throw new Exception("Товар не существует");

            if (count < 1)
                throw new Exception("Не верное количество товара для импорта");

            var product = Products[name];
            product.count += count;
            Products[name] = product;
            
            return true;

        }

        /// <summary>
        /// Изменение цены на товар
        /// </summary>
        /// <param name="name">Название товара</param>
        /// <param name="newValue">Новое значение цены</param>
        /// <returns></returns>
        public bool ChangeProductPrice(string name, int newValue)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            if (!Products.ContainsKey(name))
                throw new Exception("Товар не существует");

            if (newValue < 0)
                throw new Exception("Попытка установить отрицательную цену на товар");

            var product = Products[name];
            product.price = newValue;
            Products[name] = product;

            return true;
        }


        /// <summary>
        /// Добавление нового товара в коллекцию товаров
        /// </summary>
        /// <param name="name">Название товара</param>
        /// <param name="product">Информация о товаре</param>
        /// <returns>Результат выполнения операции</returns>
        public bool AddNewProduct(string name, Product? product)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));
            if (!product.HasValue)
                throw new ArgumentNullException(nameof(product));

            if (Products.ContainsKey(name))
                throw new Exception("Товар уже числится в списке товара");

            Products.Add(name, (Product)product);
            Statistics.Add(name, new Product { count = 0, price = 0 });

            return true;
        }   

        /// <summary>
        /// Удаление товара из коллекции товаров
        /// </summary>
        /// <param name="name">Название товара</param>
        /// <returns>Результат выполнения операции</returns>
        public bool DeleteProduct(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if (!Products.ContainsKey(name))
                throw new Exception("Товар не существует");

            Products.Remove(name);

            return true;
        }


        /// <summary>
        /// Проверка существования товара в коллекции товаров
        /// </summary>
        /// <param name="name">Название товара</param>
        /// <returns>Результат выполнения операции</returns>
        public bool ProductExist(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            return Products.ContainsKey(name);
        }

    }
}
