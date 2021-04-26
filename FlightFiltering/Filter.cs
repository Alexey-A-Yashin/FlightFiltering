using System;
using System.Collections.Generic;
using System.Linq;

namespace Gridnine.FlightCodingTest
{
    public static class FlightFilter
    {
        public static IList<Flight> GetFlightsWithDepartureUntilNow(this IList<Flight> flights)
        {
            return flights.SelectMany(f => f.Segments, (f, s) => new { Flight = f, Segment = s })
                    .Where(fs => fs.Segment.DepartureDate <= DateTime.Now)
                    .Select(fs => fs.Flight)
                    .Distinct()
                    .ToList();
        }

        public static IList<Flight> GetFlightsWithArrivalEarlierDeparture(this IList<Flight> flights)
        {
            return flights.SelectMany(f => f.Segments, (f, s) => new { Flight = f, Segment = s })
                    .Where(fs => fs.Segment.ArrivalDate < fs.Segment.DepartureDate)
                    .Select(fs => fs.Flight)
                    .Distinct()
                    .ToList();
        }

        public static IList<Flight> GetTimedFlightsOnTheGround(this IList<Flight> flights, double time)
        {
            var flightSegments = flights.SelectMany(f => f.Segments, (f, s) => new { Flight = f, DepartureDate = s.DepartureDate, ArrivalDate = s.ArrivalDate }).ToList();
            return flightSegments.Zip(flightSegments.Skip(1), (currentSegment, nextSegment)
                    => new { currentSegment.Flight,  
                            TimeOnTheGround = (currentSegment.Flight == nextSegment.Flight) ? nextSegment.DepartureDate.Subtract(currentSegment.ArrivalDate) : TimeSpan.Zero})
                    .GroupBy(ft => ft.Flight).Select(ft => new { Flight = ft.Key, TimeOnTheGround = ft.Sum(ft => ft.TimeOnTheGround.TotalHours)})
                    .Where(ft => ft.TimeOnTheGround > time)
                    .Select(fs => fs.Flight)
                    .ToList();
        }
    }
}
