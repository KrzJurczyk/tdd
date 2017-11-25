using NUnit.Framework;
using System;
using TddShop.Cli.Shipment;

namespace TddShop.Cli.Tests.Shipment
{
    [TestFixture]
    public class ConverterNumberToRomeSymbolsTests
    {
        private ConverterNumberToRomeSymbols _target;

        [SetUp]
        public void Initialize()
        {
            _target = new ConverterNumberToRomeSymbols();
        }
        [Test, Sequential]
        public void ConvertToRomeSymbols_BasicOfRomeSymbols_WereWellConverted(
            [Values(1, 5, 10, 50, 100, 500, 1000)]int digitalNumber,
            [Values("I", "V", "X", "L", "C", "D", "M")]string symbol)
        {
            //Act
            _target.ConvertToRomeSymbols(digitalNumber, out string result);
            //Assert
            Assert.That(result, Is.EqualTo(symbol));
        }

        [Test, Sequential]
        public void ConvertToRomeSymbols_NumbersFrom1To10_WereWellConverted(
            [Values(1, 2, 3, 4, 5, 6, 7, 8, 9, 10)]int digitalNumber,
            [Values("I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X")]string symbol)
        {
            //Act
            _target.ConvertToRomeSymbols(digitalNumber, out string result);
            //Assert
            Assert.That(result, Is.EqualTo(symbol));
        }

        [Test, Sequential]
        public void ConvertToRomeSymbols_FewDifferentNumbers_WereWellConverted(
            [Values(111, 222, 333, 444, 555, 666, 777, 888, 999, 3999)]int digitalNumber,
            [Values("CXI", "CCXXII", "CCCXXXIII", "CDXLIV", "DLV", "DCLXVI", "DCCLXXVII", "DCCCLXXXVIII", "CMXCIX", "MMMCMXCIX")]string symbol)
        {
            //Act
            _target.ConvertToRomeSymbols(digitalNumber, out string result);
            //Assert
            Assert.That(result, Is.EqualTo(symbol));
        }

        [Test]
        public void ConvertToRomeSymbols_ReferenceNumberEqualZero_ThrowException([Values(0)]int digitalNumber)
        {
            //Assert + Act
            Assert.Throws<ArgumentOutOfRangeException>(() => _target.ConvertToRomeSymbols(digitalNumber, out string result),
                "Quantitys value cannot equals zero!!");
        }


        [Test]
        public void ConvertToRomeSymbols_ReferencesValueUnder1000_ThrowException([Values(4000, 5000)]int digitalNumber)
        {
            //Assert + Act
            Assert.Throws<ArgumentOutOfRangeException>(() => _target.ConvertToRomeSymbols(digitalNumber, out string result),
                "Too high number (max 3999)");
        }

        [Test]
        public void ConvertToRomeSymbols_NegativeNumbers_ThrowException([Values(4000, 5000)]int digitalNumber)
        {
            //Assert + Act
            Assert.Throws<ArgumentOutOfRangeException>(() => _target.ConvertToRomeSymbols(digitalNumber, out string result)
            , "Ancient Rome symbols has not negative value!");
        }
    }
}
