﻿using RealEstates.Models;

namespace RealEstates.Services
{
    public interface IDistrictService
    {
        IEnumerable<DistrictInfoDto> GetMostExpensiveDistricts(int count);
    }
}
