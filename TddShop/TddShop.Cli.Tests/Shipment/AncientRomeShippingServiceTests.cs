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
        private AncientRomeShippingService _target;

        [SetUp]
        public void Initialize()
        {
            _deliveryService = new Mock<IDeliveryService>();
            _target = new AncientRomeShippingService(_deliveryService.Object);
        }

        [Test, Sequential]
        public void ShipOrder_BasicOfRomeSymbols_WereWellConverted(
            [Values(1, 5, 10, 50, 100, 500, 1000)]int digitalNumber, 
            [Values("I", "V", "X", "L", "C", "D", "M")]string symbol)
        {
            //Arrange
            var dummyOrderModel = new OrderModel{};

            _deliveryService.Setup(x => x.GenerateShipmentReferenceNumber(It.IsAny<int>())).Returns<int>(x=>digitalNumber);

            //Act
            _target.ShipOrder(dummyOrderModel);

            //Assert
            _deliveryService.Verify(x=>x.RequestDelivery(symbol, dummyOrderModel), Times.Once);
        }

        [Test, Sequential]
        public void ShipOrder_NumbersFrom1To10_WereWellConverted(
            [Values(1, 2, 3, 4, 5, 6, 7, 8, 9, 10)]int digitalNumber,
            [Values("I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X")]string symbol)
        {
            //Arrange
            var dummyOrderModel = new OrderModel{};

            _deliveryService.Setup(x => x.GenerateShipmentReferenceNumber(It.IsAny<int>())).Returns<int>(x => digitalNumber);

            //Act
            _target.ShipOrder(dummyOrderModel);

            //Assert
            _deliveryService.Verify(x => x.RequestDelivery(symbol, dummyOrderModel), Times.Once);
        }

        [Test, Sequential]
        public void ShipOrder_FewDifferentNumbers_WereWellConverted(
            [Values(111, 222, 333, 444, 555, 666, 777, 888, 999)]int digitalNumber,
            [Values("CXI", "CCXXII", "CCCXXXIII", "CDXLIV", "DLV", "DCLXVI", "DCCLXXVII", "DCCCLXXXVIII", "CMXCIX")]string symbol)
        {
            //Arrange
            var dummyOrderModel = new OrderModel { };

            _deliveryService.Setup(x => x.GenerateShipmentReferenceNumber(It.IsAny<int>())).Returns<int>(x => digitalNumber);

            //Act
            _target.ShipOrder(dummyOrderModel);

            //Assert
            _deliveryService.Verify(x => x.RequestDelivery(symbol, dummyOrderModel), Times.Once);
        }

        [Test]
        public void ShipOrder_ReferenceNumberEqualZero_ThrowException([Values(0)]int digitalNumber)
        {
            //Arrange
            var dummyOrderModel = new OrderModel { };

            _deliveryService.Setup(x => x.GenerateShipmentReferenceNumber(It.IsAny<int>())).Returns<int>(x => digitalNumber);
            
            //Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => _target.ShipOrder(dummyOrderModel), "Unlucky! Romas did not expect a zero!");
        }

        [Test]
        public void ShipOrder_ReferencesValueUnder1000_ThrowException([Random(1001, 10000, 10)]int digitalNumber)
        {
            //Arrange
            var dummyOrderModel = new OrderModel { };

            //Act
            _deliveryService.Setup(x => x.GenerateShipmentReferenceNumber(It.IsAny<int>())).Returns<int>(x => digitalNumber);

            //Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => _target.ShipOrder(dummyOrderModel), "Too high number (max 1000, no idea why :D)");
        }
    }
}
