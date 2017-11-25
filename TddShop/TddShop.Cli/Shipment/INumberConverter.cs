namespace TddShop.Cli.Shipment
{
    public interface INumberConverter
    {
        void ConvertToRomeSymbols(int numberToConvert, out string ancientRomeNumbers);
    }
}