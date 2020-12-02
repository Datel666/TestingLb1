using System;
using System.Collections.Generic;
using System.Text;

namespace TestingLb1
{
    class GoodsStore
    {

        private Dictionary<string, Product> Products { get; set; }

        private Dictionary<string, Product> Statistics { get; set; }

        public GoodsStore()
        {
            Products = new Dictionary<string, Product>();
            Statistics = new Dictionary<string, Product>();
        }


        public struct Product
        {
            public int count;
            public int price;
        }

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

        public int productcount(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            if (!Products.ContainsKey(name))
                throw new Exception("Товар не существует");

            return Products[name].count;

        }


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

         

        public bool DeleteProduct(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if (!Products.ContainsKey(name))
                throw new Exception("Товар не существует");

            Products.Remove(name);

            return true;
        }

        public bool ProductExist(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            return Products.ContainsKey(name);
        }



    }
}
