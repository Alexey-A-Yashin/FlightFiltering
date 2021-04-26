using System;
using System.Collections.Generic;
using System.Linq;

namespace Gridnine.FlightCodingTest
{
    class Program
    {
        static void Main(string[] args)
        {
            FlightBuilder flightBuilder = new FlightBuilder();
            IList<Flight> flights = flightBuilder.GetFlights();

            ShowMessege("All flights:", ConsoleColor.Red);
            foreach (Flight flight in flights)
                ShowMessege(flight.ToString(), ConsoleColor.Yellow);

            ShowMessege("Flights with departure date up to the current time are excluded:", ConsoleColor.Red);
            foreach (Flight flight in flights.Except(flights.GetFlightsWithDepartureUntilNow()))
                ShowMessege(flight.ToString(), ConsoleColor.Yellow);

            ShowMessege("Flights with an arrival date earlier than the departure date are excluded:", ConsoleColor.Red);
            foreach (Flight flight in flights.Except(flights.GetFlightsWithArrivalEarlierDeparture()))
                ShowMessege(flight.ToString(), ConsoleColor.Yellow);

            ShowMessege("flights with time on the ground exceeding 2 hours:", ConsoleColor.Red);
            foreach (Flight flight in flights.GetTimedFlightsOnTheGround(2))
                ShowMessege(flight.ToString(), ConsoleColor.Yellow);
            
            Console.ReadLine();
        }

        public static void ShowMessege(string message, ConsoleColor color)
        {
            ConsoleColor currentColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = currentColor;
        }
    }
}
