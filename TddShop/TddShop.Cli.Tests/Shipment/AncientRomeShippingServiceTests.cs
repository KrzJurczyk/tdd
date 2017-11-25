using Moq;
using NUnit.Framework;
using System;
using TddShop.Cli.Order.Models;
using TddShop.Cli.Shipment;

namespace TddShop.Cli.Tests.Shipment
{
    [TestFixture]
    public class AncientRomeShippingServiceTests
    {
        private Mock<IDeliveryService> _deliveryService;
        private Mock<INumberConverter> _numberConverter;
        private AncientRomeShippingService _target;

        [SetUp]
        public void Initialize()
        {
            _deliveryService = new Mock<IDeliveryService>();
            _numberConverter = new Mock<INumberConverter>();
            _target = new AncientRomeShippingService(_deliveryService.Object, _numberConverter.Object);
        }

        [Test]
        public void ReferencesNumber_WhenAllArgumentsAreGood_PassedCorrect([Values("someString")]string dummyString, [Values(1, 2, 3)]int dummyNumber)
        {
            _numberConverter.Setup(x => x.ConvertToRomeSymbols(dummyNumber, out dummyString));
            var dummyOrderModel = new OrderModel
            {
                Items = new[]
                {
                    new ItemModel
                    {
                        Category = dummyString,
                        Name = dummyString,
                        Price = dummyNumber,
                        Quantity = dummyNumber
                    }
                }
            };

            _deliveryService.Setup(x => x.GenerateShipmentReferenceNumber(It.IsAny<int>())).Returns<int>(x => dummyNumber);

            //Act
            _target.ShipOrder(dummyOrderModel);
            
            //Assert
            _deliveryService.Verify(x => x.RequestDelivery(dummyString, dummyOrderModel), Times.Once);
        }

        [Test]
        public void ReferencesNumber_WithNegativeValue_ThrowedException([Values(-10, -7, -2, -1)]int number)
        {
            //Arrange
            var dummyOrderModel = new OrderModel{};

            _deliveryService.Setup(x => x.GenerateShipmentReferenceNumber(It.IsAny<int>())).Returns<int>(x => number);
            
            //Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => _target.ShipOrder(dummyOrderModel), "Quantitys value cannot be negative!");
        }

        [Test]
        public void ShipOrder_ReferenceNumberEqualZero_ThrowException([Values(0)]int number)
        {
            //Arrange
            var dummyOrderModel = new OrderModel { };

            _deliveryService.Setup(x => x.GenerateShipmentReferenceNumber(It.IsAny<int>())).Returns<int>(x => number);
            
            //Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => _target.ShipOrder(dummyOrderModel), "Quantitys value cannot equals zero!!");
        }

        [Test]
        public void ShipOrder_NoItemInOrder_ThrowNullException()
        {
            //Arrange
            var dummyOrderModel = new OrderModel { Items = null };

            //Assert
            Assert.Throws<ArgumentNullException>(() => _target.ShipOrder(dummyOrderModel));
        }
    }
}
