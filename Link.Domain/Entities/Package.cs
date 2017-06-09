using System;

namespace Link.Domain.Entities
{
    public class Package
    {
        public string Id { get; set; }

        public DateTime ShipmentDate { get; set; }

        public DateTime ExpectedDeliveryDate { get; set; }

        public double Weight { get; set; }

        public string Status { get; set; }

        public string WeightUnit { get; set; }

        public Dimensions Dimensions { get; set; }

        public Address DeliveryAddress { get; set; }

        public string CurrentLocation { get; set; }
    }
}
