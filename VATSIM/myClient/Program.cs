using System;
using System.Collections.Generic;
using System.Linq;
using VatsimLibrary.VatsimClient;
using VatsimLibrary.VatsimData;
using VatsimLibrary.VatsimDb;


namespace myClient
{
    class Program
    {
        static void Main(string[] args)
        {
            using(var db = new VatsimDbContext())
            {
                var _controller = db.Controllers;
                List<VatsimClientATC> controllerList = new List<VatsimClientATC>();
    
                foreach(var i in _controller) {
                        Console.WriteLine(i);
                        controllerList.Add(i);
                    }

                var _pilot = db.Pilots;
                 List<VatsimClientPilot> pilotList = new List<VatsimClientPilot>();
                foreach(var pilot in _pilot) {
                    pilotList.Add(pilot);
                }

                var pilotSign = from p in pilotList select p.Callsign;
                foreach(var sign in pilotSign) {
                    Console.WriteLine(sign);
                }


                var cQuery = from con in controllerList select con;



            }
        }
    }
}
