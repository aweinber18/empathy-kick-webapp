using System.ComponentModel.DataAnnotations;

namespace EmpathyKick.Models
{
    public class Addresses
    {
        [Key]
        public int AddressId { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }

        public List<string> GetSpecifiedData(List<string> specifiedColumns)
        {
            List<string> returnedData = new List<string>();
            for (int i = 0; i < specifiedColumns.Count; i++)
            {
                switch (specifiedColumns[i])
                {
                    case "AddressId":
                        returnedData.Add(AddressId.ToString());
                        break;
                    case "Address":
                        returnedData.Add(Address);
                        break;
                    case "City":
                        returnedData.Add(City);
                        break;
                    case "Region":
                        returnedData.Add(Region);
                        break;
                    case "Country":
                        returnedData.Add(Country);
                        break;
                }
            }
            return returnedData;
        }
    }
}