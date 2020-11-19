using NUnit.Framework;
using System;
using System.Collections.Generic;
using static TestingLb1.GoodsStore;

namespace TestingLb1
{
    public class Tests
    {

        GoodsStore gs;

        [SetUp]
        public void Setup()
        {
            gs = new GoodsStore();
            Dictionary<string, Product> products = new Dictionary<string, Product>();
            products.Add("Древесный уголь 5кг", new Product { count = 20, price = 300 });
            products.Add("Плоская кисть 50мм", new Product { count = 10, price = 100 });
            products.Add("Плоская кисть 100мм", new Product { count = 10, price = 150 });
            products.Add("Набор отвёрток", new Product { count = 10, price = 1000 });
            products.Add("Стиральный порошок 500г", new Product { count = 20, price = 190 });
            products.Add("Хозяйственные перчатки x10", new Product { count = 40, price = 120 });
            products.Add("Двухсторонний скотч", new Product { count = 40, price = 200 });
            products.Add("Изолента", new Product { count = 50, price = 80 });
            products.Add("Губки для мытья посуды x10", new Product { count = 25, price = 70 });

            foreach(var product in products)
            {
                gs.AddNewProduct(product.Key, product.Value);
            }
        }

        [Test]
        public void BuyingProductTest()
        {
            Assert.IsTrue(gs.Buy("Изолента", 1)); // successful product buying

            Assert.That(Assert.Throws<Exception>(() => gs.Buy("pepega", 1)).Message, Is.EqualTo("Товар не существует")); // unexisting product
            Assert.That(Assert.Throws<Exception>(() => gs.Buy("Изолента", -1)).Message, Is.EqualTo("Не верное количество товара для покупки")); // unexisting product
            Assert.IsFalse(gs.Buy("Набор отвёрток", 20)); // buying a greater amount of products than available

        }

        [Test]
        public void ImportTest()
        {
            Assert.IsTrue(gs.ImportGoods("Изолента", 15)); // successful product import

            Assert.That(Assert.Throws<Exception>(() => gs.ImportGoods("pepega", 1)).Message, Is.EqualTo("Товар не существует")); // unexisting product
            Assert.That(Assert.Throws<Exception>(() => gs.ImportGoods("Изолента", 0)).Message, Is.EqualTo("Не верное количество товара для импорта")); // attempt to import wrong amount of products
            Assert.That(Assert.Throws<ArgumentNullException>(() => gs.ImportGoods(null, 1)).ParamName, Is.EqualTo("name")); // attempt to import "null" product
        }

        [Test]
        public void AdjustingProductPriceTest()
        {
            Assert.IsTrue(gs.ChangeProductPrice("Двухсторонний скотч", 250)); // successful price adjusting

            Assert.That(Assert.Throws<Exception>(() => gs.ChangeProductPrice("pepega", 500)).Message, Is.EqualTo("Товар не существует")); // unexisting product
            Assert.That(Assert.Throws<Exception>(() => gs.ChangeProductPrice("Изолента", -1)).Message, Is.EqualTo("Попытка установить отрицательную цену на товар")); // price is subzero (*_*) 
            Assert.That(Assert.Throws<ArgumentNullException>(() => gs.ChangeProductPrice("", 500)).ParamName, Is.EqualTo("name")); // attempt to change product price with no name
        }

        [Test]
        public void NewProductTest() 
        {
            Assert.AreEqual(true, gs.AddNewProduct("pepega", new Product { count = 20, price = 666 })); // successful product addition

            Assert.That(Assert.Throws<Exception>(() => gs.AddNewProduct("Изолента", new Product { count = 50, price = 20 })).Message, Is.EqualTo("Товар уже числится в списке товара")); // product is already exist
            Assert.That(Assert.Throws<ArgumentNullException>(() => gs.AddNewProduct(null, new Product { count = 50, price = 20 })).ParamName, Is.EqualTo("name")); // attempt to add product with no name
            Assert.That(Assert.Throws<ArgumentNullException>(() => gs.AddNewProduct("kek", null)).ParamName, Is.EqualTo("product")); // attempt to add "null" product

            Assert.IsTrue(gs.ProductExist("pepega")); // product existing test
            Assert.IsFalse(gs.ProductExist("kek")); // product existing test
        }

        [Test]
        public void DeleteProduct()
        {
            Assert.IsTrue(gs.DeleteProduct("Изолента")); // successful product deleting

            Assert.That(Assert.Throws<Exception>(() => gs.DeleteProduct("kek")).Message, Is.EqualTo("Товар не существует")); // unexisting product
            Assert.That(Assert.Throws<ArgumentNullException>(() => gs.DeleteProduct(null)).ParamName, Is.EqualTo("name")); //attempt to delete product with no name

            Assert.IsFalse(gs.ProductExist("Изолента")); // product existing test
        }

        [Test]
        public void TotalProductIncomeTest()
        {
            gs.Buy("Древесный уголь 5кг", 5);

            Assert.AreEqual(1500, gs.totalProductIncome("Древесный уголь 5кг")); // successful product income calculating

           // Assert.That(Assert.Throws<Exception>(() => gs.totalProductIncome("kekega")).Message, Is.EqualTo("Товар не существует")); // unexisting product
            Assert.That(Assert.Throws<ArgumentNullException>(() => gs.totalProductIncome(null)).ParamName, Is.EqualTo("name")); //attempt to calculate total income for product with no name
        }

        [Test]
        public void RefundTest()
        {
            Assert.IsTrue(gs.PerformRefund("Двухсторонний скотч", 1, true)); // successful product refund

            Assert.That(Assert.Throws<Exception>(() => gs.PerformRefund("kek", 1,true)).Message, Is.EqualTo("Товар не существует")); // unexisting product
            Assert.That(Assert.Throws<Exception>(() => gs.PerformRefund("Изолента", -1, true)).Message, Is.EqualTo("Не верное количество товара для возврата")); // attempt to refund wrong amount of products
            Assert.That(Assert.Throws<ArgumentNullException>(() => gs.PerformRefund(null, 1,true)).ParamName, Is.EqualTo("name")); // attempt to refurd product with no name


            Assert.AreEqual(-200, gs.totalProductIncome("Двухсторонний скотч")); // product refund result

        }

    }
}
