using System;
using System.Linq;
using System.IO;
using GeoCoordinatePortable;
using System.Net;
using System.Collections.Generic;

namespace LoggingKata
{
    class Program
    {
        static readonly ILog logger = new TacoLogger();

        static void Main(string[] args)
        {
            string csvPath = "";
            var testIfFileExists = new List<string>();

            logger.LogInfo("Log initialized");
            logger.LogInfo("Ready to perform distance calculations.");
            logger.LogInfo("Please enter the filepath of the CSV file.");
            logger.LogInfo("Each row should contain a latitude, longitude, and name.");
            logger.LogInfo("Type 'default' if you wish to use the demo file.\n");

            while (testIfFileExists.Count == 0) // loops until valid csv filepath is entered
            {
                csvPath = Console.ReadLine();
                if (csvPath == "default") { csvPath = "TacoBell-US-AL.csv"; } // demo file contains all Alabama Taco Bell locations
                try
                {
                    testIfFileExists = File.ReadAllLines(csvPath).ToList(); // attempts to read file
                }
                catch (Exception)
                {
                    logger.LogError("File not found. Be sure you have the complete, exact filepath and the file is closed. Please try again.");
                }
            }
            
            var lines = File.ReadAllLines(csvPath); // converts file into an array with one string per location
            Console.WriteLine();
            logger.LogInfo($"Imported CSV file. Contained {lines.Length} entries.");
            
            var parser = new TacoParser(); // class capable of parsing the latitude, longitude, and name from a string and creating a new location object with those properties 
            var allRestaurants = lines.Select(parser.Parse).ToArray(); // sends each string in the array through the Parse method, returning an IEnum of locations 
            logger.LogInfo("Parsed each entry to identify each location's latitude, longitude, and name.");

            ITrackable finalRestaurant1 = null; // for storing the locations with the current distance record
            ITrackable finalRestaurant2 = null;
            double finalDistance = 0;
            
            ITrackable? locA = null; // for comparing each location
            ITrackable? locB = null;
            var corA = new GeoCoordinate(0,0);
            var corB = new GeoCoordinate(0,0);
            double testDistance = 0; // for comparing each distance to the record distance

            for (int index = 0; index < allRestaurants.Length; index++) // loops through each location
            {
                locA = allRestaurants[index];
                corA.Latitude = locA.Location.Latitude;
                corA.Longitude = locA.Location.Longitude;

                for (int index2 = 0; index2 < allRestaurants.Length; index2++) // loops through each location
                {
                    locB = allRestaurants[index2];
                    corB.Latitude = locB.Location.Latitude;
                    corB.Longitude = locB.Location.Longitude;
                    testDistance = corA.GetDistanceTo(corB); // measures distance from locationA to locationB         
                    
                    if (testDistance > finalDistance) // if the distance is greater than the record, the record is updated
                        {
                        finalDistance = testDistance;
                        finalRestaurant1 = locA;
                        finalRestaurant2 = locB;
                        }
                }
            }
            logger.LogInfo("Measured distance of each location to all other locations.\n");
            
            finalDistance = Math.Round(finalDistance * 0.000621371,2);
            logger.LogInfo($"{finalRestaurant1.Name} and {finalRestaurant2.Name} are {finalDistance} miles apart.");
            logger.LogInfo("Of all the locations in the file, they are the furthest apart.");            
        }
    }
}
