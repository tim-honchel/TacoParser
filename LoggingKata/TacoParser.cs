using System;

namespace LoggingKata
{
    // Parses a POI file to locate all the Taco Bells
    public class TacoParser
    {
        readonly ILog logger = new TacoLogger();
        
        public ITrackable Parse(string line)
        {
            var cells = line.Split(',');
            
            if (cells.Length < 3) // checks if the row is missing latitude, longitude, or name
            {
                logger.LogWarning($"This line contained fewer than 3 elements: {cells}");
                return null;
            }
            
            double latitude = Convert.ToDouble(cells[0]);
            double longitude = Convert.ToDouble(cells[1]);
            string name = cells[2];

            var restaurant = new TacoBell(latitude, longitude, name);         
            return restaurant; // returns the info as an object with 3 properties
        }
    }
}