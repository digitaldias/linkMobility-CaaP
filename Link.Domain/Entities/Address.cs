using System;

namespace Link.Domain.Entities
{
    [Serializable]
    public class Address
    {
        public string StreetAddress { get; set; }


        public string ZipCode { get; set; }


        public string City { get; set; }


        public string Country { get; set; }
    }
}
