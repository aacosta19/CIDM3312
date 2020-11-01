using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VatsimLibrary.VatsimData;
using VatsimLibrary.VatsimClient;
using VatsimLibrary.VatsimDb;

namespace hw
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine($"{VatsimDbHepler.DATA_DIR}");

            using (var db = new VatsimDbContext())
            {
                //Query1
                Console.WriteLine("Query 1:");
                //public DbSet<VatsimClientPilot> Pilots { get; set; }
                var _pilotLogon = db.Pilots;
                List<VatsimClientPilot> pilotTimeList = new List<VatsimClientPilot>();

                foreach (var h in _pilotLogon)
                {
                    pilotTimeList.Add(h);
                }

                // List<DateTime> dates = new List<DateTime>();
                // foreach (var p in pilotTimeList) {
                //     DateTime x = VatsimDataState.GetDateTimeFromVatsimTimeStamp(p.TimeLogon);
                //     dates.Add(x);                
                //     }

                var query1 =
                    from p in pilotTimeList
                    select new
                    {
                        Name = p.Realname,
                        Hour = Convert.ToInt32(p.TimeLogon.Substring(8, 2)),
                        Minute = Convert.ToInt32(p.TimeLogon.Substring(10, 2)),
                        Second = Convert.ToInt32(p.TimeLogon.Substring(12, 2))
                    };

                var pilotTime = query1.OrderByDescending(p => p.Hour).ThenByDescending(p => p.Minute).ThenByDescending(p => p.Second).First();

                /* Could not find TimeLogoff to subtract from TimeLogon for the exact time logged on, 
                so I used the hours, minutes, seconds of TimeLogon to simulate outputting the time logged on */
                Console.WriteLine($"{pilotTime.Name} (Pilot) has been logged on the longest at {pilotTime.Hour} hours, {pilotTime.Minute} minutes, {pilotTime.Second} seconds");

                //Query 2
                Console.WriteLine("Query 2:");
                var _controllerLogon = db.Controllers;
                List<VatsimClientATC> controllerTimeList = new List<VatsimClientATC>();

                foreach (var h in _controllerLogon)
                {
                    controllerTimeList.Add(h);
                }

                var query2 =
                    from p in controllerTimeList
                    select new
                    {
                        Name = p.Realname,
                        Hour = Convert.ToInt32(p.TimeLogon.Substring(8, 2)),
                        Minute = Convert.ToInt32(p.TimeLogon.Substring(10, 2)),
                        Second = Convert.ToInt32(p.TimeLogon.Substring(12, 2))
                    };

                var controllerTime = query2.OrderByDescending(p => p.Hour).ThenByDescending(p => p.Minute).ThenByDescending(p => p.Second).First();

                /* Could not find TimeLogoff to subtract from TimeLogon for the exact time logged on, 
                so I used the hours, minutes, seconds of TimeLogon to simulate outputting the time logged on */
                Console.WriteLine($"{controllerTime.Name} (Controller) has been logged on the longest at {controllerTime.Hour} hours, {controllerTime.Minute} minutes, {controllerTime.Second} seconds");

                //Query 3
                Console.WriteLine("Query 3:");
                //public DbSet<VatsimClientPlannedFlight> Flights { get; set; }
                var _departure = db.Flights.GroupBy(d => d.PlannedDepairport);

                var query3 =
                    (from d in _departure
                     orderby d.Count() descending
                     select new
                     {
                         Departure = d.Key,
                         Number = d.Count()
                     }).Take(1);

                foreach (var q in query3)
                {
                    Console.WriteLine($"The airport with the most departures is {q.Departure} with {q.Number}");
                }

                //Query 4
                Console.WriteLine("Query 4:");
                //public DbSet<VatsimClientPlannedFlight> Flights { get; set; }
                var _arrival = db.Flights.GroupBy(d => d.PlannedDestairport);

                var query4 =
                    (from a in _arrival
                     orderby a.Count() descending
                     select new
                     {
                         Arrival = a.Key,
                         Number = a.Count()
                     }).Take(1);

                foreach (var q in query4)
                {
                    Console.WriteLine($"The airport with the most arrivals is {q.Arrival} with {q.Number}");
                }

                //Query 5
                Console.WriteLine("Query 5:");
                //public DbSet<VatsimClientPilotSnapshot> Positions { get; set; }
                var _altitude = db.Positions;
                List<VatsimClientPilotSnapshot> altitudeList = new List<VatsimClientPilotSnapshot>();

                foreach (var s in _altitude)
                {
                    altitudeList.Add(s);
                }

                var query5 =
                   from a in altitudeList
                   select new
                   {
                       Altitude = Convert.ToInt32(a.Altitude),
                       Name = a.Realname,
                       ID = a.Cid,
                       Callsign = a.Callsign,
                       Timelogon = a.TimeLogon
                   };

                List<int> queryList = new List<int>();

                foreach (var q in query5)
                {
                    queryList.Add(q.Altitude);
                }

                var maxAltitude = queryList.Max();

                var pilotAltitude =
                   from p in query5
                   where p.Altitude == maxAltitude
                   select p;

                var highestFlyer = pilotAltitude.ToList()[0];

                var _aircraft = db.Flights;
                List<VatsimClientPlannedFlight> flightList = new List<VatsimClientPlannedFlight>();

                foreach (var a in _aircraft)
                {
                    flightList.Add(a);
                }

                var aircraftQuery =
                   from a in flightList
                   select new
                   {
                       AircraftType = a.PlannedAircraft,
                       Name = a.Realname,
                       ID = a.Cid,
                       Callsign = a.Callsign,
                       Timelogon = a.TimeLogon
                   };

                var finalAnswer =
                    from x in aircraftQuery
                    where x.ID == highestFlyer.ID && x.Callsign == highestFlyer.Callsign && x.Timelogon == highestFlyer.Timelogon
                    select x;

                var person = finalAnswer.ToList()[0];
                Console.WriteLine($"{person.Name} is flying the highest at {highestFlyer.Altitude} AGL in a {person.AircraftType} aircraft");

                //Query 6
                Console.WriteLine("Query 6:");
                //public DbSet<VatsimClientPilotSnapshot> Positions { get; set; }
                var _slowSpeed = db.Positions;
                List<VatsimClientPilotSnapshot> slowSpeedList = new List<VatsimClientPilotSnapshot>();

                foreach (var s in _slowSpeed)
                {
                    slowSpeedList.Add(s);
                }

                var query6 =
                   from s in slowSpeedList
                   select new
                   {
                       Speed = Convert.ToInt32(s.Groundspeed),
                       Name = s.Realname,
                       Altitude = Convert.ToInt32(s.Altitude)
                   };

                List<int> _slowSpeedList = new List<int>();

                foreach (var q in query6)
                {
                    if (q.Speed > 0)
                    {
                        _slowSpeedList.Add(q.Speed);
                    }
                }

                var minSpeed = _slowSpeedList.Min();

                var answer =
                    from a in query6
                    where a.Speed == minSpeed && a.Altitude > 0
                    select a;


                var slowestSpeed = answer.ToList()[0];

                Console.WriteLine($"{slowestSpeed.Name} is flying the slowest at a ground speed of {slowestSpeed.Speed} mph");

                //Query 7
                Console.WriteLine("Query 7:");
                //public DbSet<VatsimClientPlannedFlight> Flights { get; set; }
                var _aircraftType = db.Flights.GroupBy(d => d.PlannedAircraft);

                var query7 =
                    (from a in _aircraftType
                     orderby a.Count() descending
                     select new
                     {
                         Aircraft = a.Key,
                         Number = a.Count()
                     }).Take(1);

                foreach (var q in query7)
                {
                    Console.WriteLine($"The aircraft type that is being used the most is the {q.Aircraft} with {q.Number} uses");
                }

                //Query 8
                Console.WriteLine("Query 8:");
                //public DbSet<VatsimClientPilotSnapshot> Positions { get; set; }

                var _speed = db.Positions;
                List<VatsimClientPilotSnapshot> speedList = new List<VatsimClientPilotSnapshot>();

                foreach (var s in _speed)
                {
                    speedList.Add(s);
                }

                var query8 =
                   from s in speedList
                   select new
                   {
                       Speed = Convert.ToInt32(s.Groundspeed),
                       Name = s.Realname
                   };

                List<int> _speedList = new List<int>();

                foreach (var q in query8)
                {
                    _speedList.Add(q.Speed);
                }

                var maxSpeed = _speedList.Max();

                var pilotSpeed =
                   from p in query8
                   where p.Speed == maxSpeed
                   select new
                   {
                       Speed = p.Speed,
                       Pilot = p.Name
                   };

                var fastestSpeed = pilotSpeed.ToList()[0];

                Console.WriteLine($"{fastestSpeed.Pilot} is flying the fastest at a ground speed of {fastestSpeed.Speed} mph");


                //Query 9
                Console.WriteLine("Query 9:");
                //public DbSet<VatsimClientPilotSnapshot> Positions { get; set; }
                var _heading = db.Positions;
                List<VatsimClientPilotSnapshot> headingList = new List<VatsimClientPilotSnapshot>();

                foreach (var h in _heading)
                {
                    headingList.Add(h);
                }

                var query9 =
                    from h in headingList
                    select Convert.ToInt32(h.Heading);

                List<int> pilotCount = new List<int>();
                foreach (var q in query9)
                {

                    if (q >= 90 && q <= 270)
                    {
                        pilotCount.Add(q);
                    }
                    else
                    {
                        continue;
                    }
                }

                var pilotTotal = pilotCount.Count();

                Console.WriteLine($"{pilotTotal} Pilots are flying North at the moment");

                //Query 10
                Console.WriteLine("Query 10:");
                //public DbSet<VatsimClientPlannedFlight> Flights { get; set; }
                var _pilotRemark = db.Flights;
                List<VatsimClientPlannedFlight> pilotRemarksList = new List<VatsimClientPlannedFlight>();

                foreach (var p in _pilotRemark)
                {
                    pilotRemarksList.Add(p);
                }

                var query10 =
                    from p in pilotRemarksList
                    select new
                    {
                        Name = p.Realname,
                        Remarks = p.PlannedRemarks,
                        ID = p.Cid,
                        TimeLogon = p.TimeLogon,
                        Departure = p.PlannedDepairport,
                        Arrival = p.PlannedDestairport,
                        Callsign = p.Callsign
                    };

                var masterList = query10.ToList();

                List<string> nameList = new List<string>();

                var remark = "";

                List<int> testList = new List<int>();

                List<string> remarkList = new List<string>()
;
                foreach (var m in masterList) {
                    remark = m.Remarks;
                    var testRemark = remark.Length;
                    testList.Add(testRemark);
                    nameList.Add(m.Name);
                    remarkList.Add(m.Remarks);
                }
  
                var maxCharacters = testList.Max();

                var nameQuery = 
                    from r in remarkList 
                    where r.Length == maxCharacters
                    select r;
                
                var finalRemark = nameQuery.ToList()[0];

                var finalName = "";
                
                var finalID = "";

                var callsign = "";
                
                var departure = "";

                var arrival = "";

                var timeLogon = "";

                foreach (var m in masterList) {
                    if (m.Remarks == finalRemark) {
                        finalName = m.Name;
                        finalID = m.ID;
                        callsign = m.Callsign;
                        departure = m.Departure;
                        arrival = m.Arrival;
                        timeLogon = m.TimeLogon;
                    } 
                }

                var remarkAnswer = db.Flights.Find(finalID, callsign, timeLogon, departure, arrival);
                if (remarkAnswer != null)
                {
                    Console.WriteLine($"{finalName} has the longest remarks at {maxCharacters} characters."); 
                }
                else
                {
                    Console.WriteLine("Pilot not found");
                } 


            }
        }//End Main
    }//End Class
}//End Namespace 