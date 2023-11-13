using ContactsManager.Core.Domain.Entities;

namespace ContactsManager.Core.DTO.CountryDTO
{
    public class CountryAddResponse
    {
        public Guid CountryID { get; set; }

        public string? CountryName { get; set; }


        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj.GetType() != typeof(CountryAddResponse))
            {
                return false;
            }

            CountryAddResponse obj_to_compare=(CountryAddResponse)obj;

            return CountryID==obj_to_compare.CountryID && CountryName==obj_to_compare.CountryName;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }  

    public static class CountryExtensions
    {
        public static CountryAddResponse ToCountryResponse(this Country country)
        {
            return new CountryAddResponse { CountryID = country.CountryID, CountryName = country.CountryName };
        }
    }
}
