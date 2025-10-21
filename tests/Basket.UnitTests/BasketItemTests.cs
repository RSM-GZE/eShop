using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace eShop.Basket.API.Model
{
    [TestClass]
    public class BasketItemTest
    {
        [TestMethod]
        public void Validate_WithValidQuantity_ReturnsNoValidationErrors()
        {
            // Arrange
            var item = new BasketItem
            {
                Id = "1",
                ProductId = 10,
                ProductName = "Test Product",
                UnitPrice = 5.0m,
                OldUnitPrice = 8.0m,
                Quantity = 2,
                PictureUrl = "test.jpg"
            };
            var context = new ValidationContext(item);

            // Act
            var results = new List<ValidationResult>(item.Validate(context));

            // Assert
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void Validate_WithQuantityLessThanOne_ReturnsValidationError()
        {
            var item = new BasketItem
            {
                Id = "2",
                ProductId = 11,
                ProductName = "Test Product",
                UnitPrice = 5.0m,
                OldUnitPrice = 8.0m,
                Quantity = 0,
                PictureUrl = "test.jpg"
            };
            var context = new ValidationContext(item);
            var results = new List<ValidationResult>(item.Validate(context));
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Invalid number of units", results[0].ErrorMessage);
        }

        [TestMethod]
        public void Validate_WithQuantityGreaterThanMax_ReturnsValidationError()
        {
            var item = new BasketItem
            {
                Id = "3",
                ProductId = 12,
                ProductName = "Test Product",
                UnitPrice = 5.0m,
                OldUnitPrice = 8.0m,
                Quantity = 10000,
                PictureUrl = "test.jpg"
            };
            var context = new ValidationContext(item);
            var results = new List<ValidationResult>(item.Validate(context));
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Invalid number of units", results[0].ErrorMessage);
        }

        [TestMethod]
        public void Validate_WithQuantityAtLowerBoundary_ReturnsNoValidationErrors()
        {
            var item = new BasketItem
            {
                Id = "4",
                ProductId = 13,
                ProductName = "Test Product",
                UnitPrice = 5.0m,
                OldUnitPrice = 8.0m,
                Quantity = 1,
                PictureUrl = "test.jpg"
            };
            var context = new ValidationContext(item);
            var results = new List<ValidationResult>(item.Validate(context));
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void Validate_WithQuantityAtUpperBoundary_ReturnsNoValidationErrors()
        {
            var item = new BasketItem
            {
                Id = "5",
                ProductId = 14,
                ProductName = "Test Product",
                UnitPrice = 5.0m,
                OldUnitPrice = 8.0m,
                Quantity = 9999,
                PictureUrl = "test.jpg"
            };
            var context = new ValidationContext(item);
            var results = new List<ValidationResult>(item.Validate(context));
            Assert.AreEqual(0, results.Count);
        }

        // Neues Business-Feature: Unique-Produkte dürfen nur einmal im Warenkorb liegen
        [TestMethod]
        public void Validate_UniqueProductWithQuantityGreaterThanOne_ReturnsValidationError()
        {
            var item = new BasketItem
            {
                Id = "6",
                ProductId = 15,
                ProductName = "Unique Product",
                UnitPrice = 100.0m,
                OldUnitPrice = 120.0m,
                Quantity = 2,
                PictureUrl = "unique.jpg",
                Unique = true
            };
            var context = new ValidationContext(item);
            var results = new List<ValidationResult>(item.Validate(context));
            // Erwartet: Fehler, weil Unique-Produkte nur einmal erlaubt sind
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Unique products can only be added once to the basket", results[0].ErrorMessage);
        }

        [TestMethod]
        public void Validate_UniqueProductWithQuantityOne_ReturnsNoValidationErrors()
        {
            var item = new BasketItem
            {
                Id = "7",
                ProductId = 16,
                ProductName = "Unique Product",
                UnitPrice = 100.0m,
                OldUnitPrice = 120.0m,
                Quantity = 1,
                PictureUrl = "unique.jpg",
                Unique = true
            };
            var context = new ValidationContext(item);
            var results = new List<ValidationResult>(item.Validate(context));
            Assert.AreEqual(0, results.Count);
        }
    }
}
