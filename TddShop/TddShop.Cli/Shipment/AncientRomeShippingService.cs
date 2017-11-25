using System;
using System.Linq;
using TddShop.Cli.Order.Models;

namespace TddShop.Cli.Shipment
{
    public class AncientRomeShippingService
    {
        private readonly IDeliveryService deliveryService;
        private readonly INumberConverter numberConverter;
        
        public AncientRomeShippingService(IDeliveryService _deliveryService, INumberConverter _numberConverter)
        {
            deliveryService = _deliveryService;
            numberConverter = _numberConverter;
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
            int referencesNumber = deliveryService.GenerateShipmentReferenceNumber(order.Items.Sum(p => p.Quantity));

            if (referencesNumber < 0)
            {
                throw new ArgumentOutOfRangeException("Quantitys value cannot be negative!");
            }

            if (referencesNumber == 0)
            {
                throw new ArgumentOutOfRangeException("Quantitys value cannot equals 0!");
            }

            if (order.Items.Any(x => x == null))
            {
                throw new ArgumentNullException();
            }
            numberConverter.ConvertToRomeSymbols(referencesNumber, out string shipmentReferenceNumber);
            deliveryService.RequestDelivery(shipmentReferenceNumber, order);
        }
    }    
}