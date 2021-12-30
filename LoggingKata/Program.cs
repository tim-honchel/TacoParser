using System;
using System.Linq;
using System.IO;
using GeoCoordinatePortable;

namespace LoggingKata
{
    class Program
    {
        static readonly ILog logger = new TacoLogger();
        const string csvPath = "TacoBell-US-AL.csv";

        static void Main(string[] args)
        {
            // TODO:  Find the two Taco Bells that are the furthest from one another.
            // HINT:  You'll need two nested forloops ---------------------------

            logger.LogInfo("Log initialized");

            // use File.ReadAllLines(path) to grab all the lines from your csv file
            // Log and error if you get 0 lines and a warning if you get 1 line
            var lines = File.ReadAllLines(csvPath);

            //for (int index = 0; index < lines.Length; index ++)
            //{
                //logger.LogInfo($"Imported data point: {lines[index]}");
                   logger.LogInfo($"Imported CSV file. Contained {lines.Length} entries.");
            //}

            // Create a new instance of your TacoParser class
            var parser = new TacoParser();

            // Grab an IEnumerable of locations using the Select command: var locations = lines.Select(parser.Parse);
            var allRestaurants = lines.Select(parser.Parse).ToArray();

            // DON'T FORGET TO LOG YOUR STEPS

            // Now that your Parse method is completed, START BELOW ----------

            // TODO: Create two `ITrackable` variables with initial values of `null`. These will be used to store your two taco bells that are the farthest from each other.
            // Create a `double` variable to store the distance

            ITrackable finalRestaurant1 = null;
            ITrackable finalRestaurant2 = null;
            double finalDistance = 0;
            
            // Include the Geolocation toolbox, so you can compare restaurants: `using GeoCoordinatePortable;`

            //HINT NESTED LOOPS SECTION---------------------
            // Do a loop for your restaurants to grab each restaurant as the origin (perhaps: `locA`)

            ITrackable? locA = null;
            ITrackable? locB = null;
            //Point? corA = null;
            //Point? corB = null;
            var corA = new GeoCoordinate(0,0);
            var corB = new GeoCoordinate(0,0);
            
            double testDistance = 0;

            for (int index = 0; index < allRestaurants.Length; index++)
            {
                locA = allRestaurants[index];
                corA.Latitude = locA.Location.Latitude;
                corA.Longitude = locA.Location.Longitude;
                logger.LogInfo($"Measuring distance of {locA.Name} to all other restaurants");

                for (int index2 = 0; index2 < allRestaurants.Length; index2++)
                {
                    locB = allRestaurants[index2];
                    corB.Latitude = locB.Location.Latitude;
                    corB.Longitude = locB.Location.Longitude;

                    testDistance = corA.GetDistanceTo(corB);
                    
                    if (testDistance > finalDistance)
                        {
                        finalDistance = testDistance;
                        finalRestaurant1 = locA;
                        finalRestaurant2 = locB;
                        }
                }
            }

            finalDistance = Math.Round(finalDistance * 0.000621371,2);

            Console.WriteLine();
            Console.WriteLine($"{finalRestaurant1.Name} and {finalRestaurant2.Name} are {finalDistance} miles apart.");
            Console.WriteLine("They are the 2 Taco Bells furthest apart in Alabama.");

            // Create a new corA Coordinate with your locA's lat and long

            // Now, do another loop on the locations with the scope of your first loop, so you can grab the "destination" location (perhaps: `locB`)

            // Create a new Coordinate with your locB's lat and long

            // Now, compare the two using `.GetDistanceTo()`, which returns a double
            // If the distance is greater than the currently saved distance, update the distance and the two `ITrackable` variables you set above

            // Once you've looped through everything, you've found the two Taco Bells farthest away from each other.


            
        }
    }
}
