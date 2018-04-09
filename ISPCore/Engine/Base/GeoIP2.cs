namespace ISPCore.Engine.Base
{
    public class GeoIP2
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IP">IP пользователя</param>
        /// <returns>Страна/Город/Регион</returns>
        public static (string Country, string City, string Region) City(string IP)
        {
            string Country = "Unknown", City = "Unknown", Region = "Unknown";

            try
            {
                using (var reader = new MaxMind.GeoIP2.DatabaseReader(Folders.Databases + "/GeoLite2-City.mmdb"))
                {
                    var response = reader.City(IP);
                    City = string.IsNullOrEmpty(response.City.Name) ? "Unknown" : response.City.Name;
                    Country = string.IsNullOrEmpty(response.Country.Name) ? "Unknown" : response.Country.Name;
                    Region = string.IsNullOrEmpty(response.MostSpecificSubdivision.Name) ? "Unknown" : response.MostSpecificSubdivision.Name;
                }
            }
            catch { }

            return (Country, City, Region);
        }
    }
}
