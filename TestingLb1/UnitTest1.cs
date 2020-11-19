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
            products.Add("Äðåâåñíûé óãîëü 5êã", new Product { count = 20, price = 300 });
            products.Add("Ïëîñêàÿ êèñòü 50ìì", new Product { count = 10, price = 100 });
            products.Add("Ïëîñêàÿ êèñòü 100ìì", new Product { count = 10, price = 150 });
            products.Add("Íàáîð îòâ¸ðòîê", new Product { count = 10, price = 1000 });
            products.Add("Ñòèðàëüíûé ïîðîøîê 500ã", new Product { count = 20, price = 190 });
            products.Add("Õîçÿéñòâåííûå ïåð÷àòêè x10", new Product { count = 40, price = 120 });
            products.Add("Äâóõñòîðîííèé ñêîò÷", new Product { count = 40, price = 200 });
            products.Add("Èçîëåíòà", new Product { count = 50, price = 80 });
            products.Add("Ãóáêè äëÿ ìûòüÿ ïîñóäû x10", new Product { count = 25, price = 70 });

            foreach(var product in products)
            {
                gs.AddNewProduct(product.Key, product.Value);
            }
        }

        [Test]
        public void BuyingProductTest()
        {
            Assert.IsTrue(gs.Buy("Èçîëåíòà", 1)); // successful product buying

            Assert.That(Assert.Throws<Exception>(() => gs.Buy("pepega", 1)).Message, Is.EqualTo("Òîâàð íå ñóùåñòâóåò")); // unexisting product
            Assert.That(Assert.Throws<Exception>(() => gs.Buy("Èçîëåíòà", -1)).Message, Is.EqualTo("Íå âåðíîå êîëè÷åñòâî òîâàðà äëÿ ïîêóïêè")); // unexisting product
            Assert.IsFalse(gs.Buy("Íàáîð îòâ¸ðòîê", 20)); // buying a greater amount of products than available

        }

        [Test]
        public void ImportTest()
        {
            Assert.IsTrue(gs.ImportGoods("Èçîëåíòà", 15)); // successful product import

            Assert.That(Assert.Throws<Exception>(() => gs.ImportGoods("pepega", 1)).Message, Is.EqualTo("Òîâàð íå ñóùåñòâóåò")); // unexisting product
            Assert.That(Assert.Throws<Exception>(() => gs.ImportGoods("Èçîëåíòà", 0)).Message, Is.EqualTo("Íå âåðíîå êîëè÷åñòâî òîâàðà äëÿ èìïîðòà")); // attempt to import wrong amount of products
            Assert.That(Assert.Throws<ArgumentNullException>(() => gs.ImportGoods(null, 1)).ParamName, Is.EqualTo("name")); // attempt to import "null" product
        }

        [Test]
        public void AdjustingProductPriceTest()
        {
            Assert.IsTrue(gs.ChangeProductPrice("Äâóõñòîðîííèé ñêîò÷", 250)); // successful price adjusting

            Assert.That(Assert.Throws<Exception>(() => gs.ChangeProductPrice("pepega", 500)).Message, Is.EqualTo("Òîâàð íå ñóùåñòâóåò")); // unexisting product
            Assert.That(Assert.Throws<Exception>(() => gs.ChangeProductPrice("Èçîëåíòà", -1)).Message, Is.EqualTo("Ïîïûòêà óñòàíîâèòü îòðèöàòåëüíóþ öåíó íà òîâàð")); // price is subzero (*_*) 
            Assert.That(Assert.Throws<ArgumentNullException>(() => gs.ChangeProductPrice("", 500)).ParamName, Is.EqualTo("name")); // attempt to change product price with no name
        }

        [Test]
        public void NewProductTest() 
        {
            Assert.AreEqual(true, gs.AddNewProduct("pepega", new Product { count = 20, price = 666 })); // successful product addition

            Assert.That(Assert.Throws<Exception>(() => gs.AddNewProduct("Èçîëåíòà", new Product { count = 50, price = 20 })).Message, Is.EqualTo("Òîâàð óæå ÷èñëèòñÿ â ñïèñêå òîâàðà")); // product is already exist
            Assert.That(Assert.Throws<ArgumentNullException>(() => gs.AddNewProduct(null, new Product { count = 50, price = 20 })).ParamName, Is.EqualTo("name")); // attempt to add product with no name
            Assert.That(Assert.Throws<ArgumentNullException>(() => gs.AddNewProduct("kek", null)).ParamName, Is.EqualTo("product")); // attempt to add "null" product

            Assert.IsTrue(gs.ProductExist("pepega")); // product existing test
            Assert.IsFalse(gs.ProductExist("kek")); // product existing test
        }

        [Test]
        public void DeleteProduct()
        {
            Assert.IsTrue(gs.DeleteProduct("Èçîëåíòà")); // successful product deleting

            Assert.That(Assert.Throws<Exception>(() => gs.DeleteProduct("kek")).Message, Is.EqualTo("Òîâàð íå ñóùåñòâóåò")); // unexisting product
            Assert.That(Assert.Throws<ArgumentNullException>(() => gs.DeleteProduct(null)).ParamName, Is.EqualTo("name")); //attempt to delete product with no name

            Assert.IsFalse(gs.ProductExist("Èçîëåíòà")); // product existing test
        }

        [Test]
        public void TotalProductIncomeTest()
        {
            gs.Buy("Äðåâåñíûé óãîëü 5êã", 5);

            Assert.AreEqual(1500, gs.totalProductIncome("Äðåâåñíûé óãîëü 5êã")); // successful product income calculating

           // Assert.That(Assert.Throws<Exception>(() => gs.totalProductIncome("kekega")).Message, Is.EqualTo("Òîâàð íå ñóùåñòâóåò")); // unexisting product
            Assert.That(Assert.Throws<ArgumentNullException>(() => gs.totalProductIncome(null)).ParamName, Is.EqualTo("name")); //attempt to calculate total income for product with no name
        }

        [Test]
        public void RefundTest()
        {
            Assert.IsTrue(gs.PerformRefund("Äâóõñòîðîííèé ñêîò÷", 1, true)); // successful product refund

            Assert.That(Assert.Throws<Exception>(() => gs.PerformRefund("kek", 1,true)).Message, Is.EqualTo("Òîâàð íå ñóùåñòâóåò")); // unexisting product
            Assert.That(Assert.Throws<Exception>(() => gs.PerformRefund("Èçîëåíòà", -1, true)).Message, Is.EqualTo("Íå âåðíîå êîëè÷åñòâî òîâàðà äëÿ âîçâðàòà")); // attempt to refund wrong amount of products
            Assert.That(Assert.Throws<ArgumentNullException>(() => gs.PerformRefund(null, 1,true)).ParamName, Is.EqualTo("name")); // attempt to refurd product with no name


            Assert.AreEqual(-200, gs.totalProductIncome("Äâóõñòîðîííèé ñêîò÷")); // product refund result

        }

    }
}