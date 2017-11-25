using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TddShop.Cli.Shipment
{
    public class ConverterNumberToRomeSymbols : INumberConverter
    {
        private List<int> symbolsList = new List<int>();
        private StringBuilder romeSymbolsNumberBuilder;

        private enum BasicRomeUnitSymbols
        {
            I = 1,
            V = 5,
            X = 10,
            L = 50,
            C = 100,
            D = 500,
            M = 1000
        }

        private Dictionary<string, int> PairOfBasicRomeSymbolsAndDigitalNumbers = new Dictionary<string, int>();
        
        private int maxNumber = 3999;

        public ConverterNumberToRomeSymbols()
        {
            foreach (var pair in Enum.GetValues(typeof(BasicRomeUnitSymbols)))
            {
                PairOfBasicRomeSymbolsAndDigitalNumbers.Add(pair.ToString(), (int)pair);
            }
        }

        public void ConvertToRomeSymbols(int numberToConvert, out string ancientRomeNumbers)
        {
            romeSymbolsNumberBuilder = new StringBuilder();

            if (numberToConvert == 0)
            {
                throw new ArgumentOutOfRangeException("Quantitys value cannot equals zero!");
            }

            if (numberToConvert > maxNumber)
            {
                throw new ArgumentOutOfRangeException("Too high number (max 3999)");
            }

            if (numberToConvert < 0)
            {
                throw new ArgumentOutOfRangeException("Ancient Rome symbols has not negative value!");
            }

            if (PairOfBasicRomeSymbolsAndDigitalNumbers.ContainsValue(numberToConvert))
                DesignateKeyByValue(numberToConvert);
            else
            {
                int[] tabOfNumbers = ConvertNumberToArray(numberToConvert);
                foreach (var item in MainNumbersToSymbolsConverter(tabOfNumbers))
                {
                    DesignateKeyByValue(item);
                }
            }

            ancientRomeNumbers = romeSymbolsNumberBuilder.ToString();

            symbolsList.Clear();
            romeSymbolsNumberBuilder.Clear();
        }
        
        private void DesignateKeyByValue(int enumValue)
        {
            romeSymbolsNumberBuilder
                        .Append(PairOfBasicRomeSymbolsAndDigitalNumbers.FirstOrDefault(x => x.Value == enumValue).Key);
        }

        private int[] ConvertNumberToArray(int number)
        {
            return Array.ConvertAll(number.ToString().ToArray(), x => int.Parse(x.ToString()));
        }

        private List<int> NumberOfOnesEntry(int reps, int loopValue)
        {
            while (reps > 0)
            {
                symbolsList.Add(1 * loopValue);
                reps--;
            }
            return symbolsList;
        }

        private List<int> MainNumbersToSymbolsConverter(int[] tabDecimalNumbers)
        {
            int loop = 1;

            for (int i = tabDecimalNumbers.Length; i > 0; i--)
            {
                int multiplesOfFive = 5 * loop;
                int multiplesOfTen = 10 * loop;

                int currentDigit = tabDecimalNumbers[i - 1];
                if (PairOfBasicRomeSymbolsAndDigitalNumbers
                    .Any(p => currentDigit == p.Value))
                {
                    symbolsList.Add(currentDigit * loop);
                }

                else
                {
                    switch (currentDigit)
                    {
                        case 2:
                            NumberOfOnesEntry(2, loop);
                            break;
                        case 3:
                            NumberOfOnesEntry(3, loop);
                            break;
                        case 4:
                            symbolsList.Add(multiplesOfFive);
                            NumberOfOnesEntry(1, loop);
                            break;
                        case 6:
                            NumberOfOnesEntry(1, loop);
                            symbolsList.Add(multiplesOfFive);
                            break;
                        case 7:
                            NumberOfOnesEntry(2, loop);
                            symbolsList.Add(multiplesOfFive);
                            break;
                        case 8:
                            NumberOfOnesEntry(3, loop);
                            symbolsList.Add(multiplesOfFive);
                            break;
                        case 9:
                            symbolsList.Add(multiplesOfTen);
                            NumberOfOnesEntry(1, loop);
                            break;

                        default:
                            throw new IndexOutOfRangeException("Number doesn't match");
                    }

                }
                loop = loop * 10;
            }

            symbolsList.Reverse();
            return symbolsList;
        }
    }
}
