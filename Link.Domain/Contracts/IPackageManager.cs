﻿using Link.Domain.Entities;
using System.Threading.Tasks;

namespace Link.Domain.Contracts
{
    public interface IPackageManager
    {
        Task<Package> RetrievePackageInfoAsync(string packageId);


        Task<Package> SetDeliveryAddressAsync(string packageId, Address deliveryAddress);
    }
}
