using System;
using System.Collections.Generic;

namespace challenge_api.Database.Models
{
    public partial class StateProvince
    {
        public StateProvince()
        {
            Address = new HashSet<Address>();
        }

        public int StateProvinceId { get; set; }
        public string StateProvinceCode { get; set; }
        public string CountryRegionCode { get; set; }
        public bool? IsOnlyStateProvinceFlag { get; set; }
        public string Name { get; set; }
        public int TerritoryId { get; set; }
        public Guid Rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }

        public CountryRegion CountryRegionCodeNavigation { get; set; }
        public ICollection<Address> Address { get; set; }
    }
}
