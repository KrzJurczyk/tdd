using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TddShop.Cli.Order.Models;

namespace TddShop.Cli.Shipment
{
    public class AncientRomeShippingService
    {
        private enum RomaUnitSymbols
        {
            I = 1,
            V = 5,
            X = 10,
            L = 50,
            C = 100,
            D = 500,
            M = 1000
        }

        private List<int> reversedSymbolsList = new List<int>();

        private Dictionary<string, int> PairOfBasicRomanSymbolsAndDigitalNumbers = new Dictionary<string, int>();

        private readonly IDeliveryService _deliveryService;

        private StringBuilder _romanianNumber = new StringBuilder();

        private int maxReferenceNumberValue = 1000;

        public AncientRomeShippingService(IDeliveryService deliveryService)
        {
            _deliveryService = deliveryService;

            foreach (var pair in Enum.GetValues(typeof(RomaUnitSymbols)))
            {
                PairOfBasicRomanSymbolsAndDigitalNumbers.Add(pair.ToString(), (int)pair);
            }
        }

        /// <summary>
        /// To ship an order you need to generate a shipment reference number (see IDeliveryService).
        /// Ancient Rome works with romanian numbers so you will need to convert shipment reference number to a valid romanian number (string).
        /// Use IDeliveryService to ship an order.
        ///                
        /// </summary>
        /// <param name="order"></param>
        public void ShipOrder(OrderModel order)
        {
            try
            {
                int referencesNumber = _deliveryService.GenerateShipmentReferenceNumber(order.Items.Sum(p => p.Quantity));

                if (referencesNumber == 0)
                {
                    throw new ArgumentOutOfRangeException("Unlucky! Romas did not expect a zero!");
                }

                if (referencesNumber > maxReferenceNumberValue)
                {
                    throw new ArgumentOutOfRangeException("Too high number (max 1000, no idea why :D)");
                }

                if (PairOfBasicRomanSymbolsAndDigitalNumbers.ContainsValue(referencesNumber))
                    _romanianNumber
                        .Append(PairOfBasicRomanSymbolsAndDigitalNumbers
                        .FirstOrDefault(r => r.Value == referencesNumber)
                        .Key);
                else
                {
                    int[] tabOfNumbers = Array.ConvertAll(referencesNumber.ToString().ToArray(), x => (int)x - 48);
                    foreach (var item in RomanNumbersConverter(tabOfNumbers))
                    {
                        _romanianNumber.Append(PairOfBasicRomanSymbolsAndDigitalNumbers
                        .FirstOrDefault(r => r.Value == item)
                        .Key);
                    }
                }

                _deliveryService.RequestDelivery(_romanianNumber.ToString(), order);
            }

            catch(ArgumentNullException e)
            {
                throw e;
            }

            catch (ArgumentOutOfRangeException e)
            {
                throw e;
            }

            catch (Exception e)
            {
                throw e;
            }
        }

        private List<int> NumberOfOnesEntry(int reps, int loopValue)
        {
            while (reps > 0)
            {
                reversedSymbolsList.Add(1 * loopValue);
                reps--;
            }
            return reversedSymbolsList;
        }

        private List<int> RomanNumbersConverter(int[] digitalNumberTab)
        {
            int loop = 1;

            for (int i = digitalNumberTab.Length; i > 0; i--)
            {
                int f = 5 * loop;
                int t = 10 * loop;

                int digit = digitalNumberTab[i - 1];
                if (PairOfBasicRomanSymbolsAndDigitalNumbers
                    .Any(p => digit == p.Value))
                {
                    reversedSymbolsList.Add(digit * loop);
                }

                else
                {
                    switch (digit)
                    {
                        case 2:
                            NumberOfOnesEntry(2, loop);
                            break;
                        case 3:
                            NumberOfOnesEntry(3, loop);
                            break;
                        case 4:
                            reversedSymbolsList.Add(f);
                            NumberOfOnesEntry(1, loop);
                            break;
                        case 6:
                            NumberOfOnesEntry(1, loop);
                            reversedSymbolsList.Add(f);
                            break;
                        case 7:
                            NumberOfOnesEntry(2, loop);
                            reversedSymbolsList.Add(f);
                            break;
                        case 8:
                            NumberOfOnesEntry(3, loop);
                            reversedSymbolsList.Add(f);
                            break;
                        case 9:
                            reversedSymbolsList.Add(t);
                            NumberOfOnesEntry(1, loop);
                            break;

                        default:
                            throw new IndexOutOfRangeException("Number doesn't match");
                    }

                }
                loop = loop * 10;
            }

            reversedSymbolsList.Reverse();
            return reversedSymbolsList;
        }
    }    
}