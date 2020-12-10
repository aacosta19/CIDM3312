using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VatsimLibrary.VatsimClientV1;
using VatsimLibrary.VatsimDb;
using System;

namespace VATSIMData.WebApp.Pages {
    public class PilotDetailModel : PageModel {
        private VatsimDbContext db;

        public VatsimClientPilotV1 Pilot { get; set; }

        public List <VatsimClientPilotSnapshotV1> Position { get; set; }
        public List <VatsimClientPlannedFlightV1> Flight { get; set; }

        public decimal latitude { get; set; }
        public int fastestSpeed { get; set; }
        public int highestAlt { get; set; }
        public string depAirport { get; set; }
        public int depCount { get; set; }
        public decimal longitude { get; set; }
        public PilotDetailModel(VatsimDbContext db) {
            this.db = db;
        }

        public async Task<IActionResult> OnGetAsync(string cid, string callsign, string timelogon) {
            Pilot = await db.Pilots.FindAsync(cid, callsign, timelogon);
            
            if(Pilot == null) {
                return RedirectToPage("NotFound");
            }  

            Position = db.Positions.Where(p=>p.Cid==cid && p.Callsign==callsign && p.TimeLogon==timelogon).ToList();

            Flight = db.Flights.Where(p=>p.Cid==cid && p.Callsign==callsign).ToList();

            var longDeg = Position.OrderByDescending(p=>Convert.ToDecimal(p.Longitude)).ToList();
            longitude = Convert.ToDecimal(longDeg[0].Longitude);

            var latDeg = Position.OrderBy(p=>Convert.ToDecimal(p.Latitude)).ToList();
            latitude = Convert.ToDecimal(latDeg[0].Latitude);

            var _fastestSpeed = Position.OrderByDescending(p=>Convert.ToInt32(p.Groundspeed)).ToList();
            fastestSpeed = Convert.ToInt32(_fastestSpeed[0].Groundspeed);

            var _highestAlt = Position.OrderByDescending(p=>Convert.ToInt32(p.Altitude)).ToList();
            highestAlt = Convert.ToInt32(_highestAlt[0].Altitude);

            //Lists the airport most departed from by the pilot and the count for that airport. Try CID: 1366545 & 1339151 as an example.  
            var dep = Flight.GroupBy(p => p.PlannedDepairport).OrderByDescending(p=>p.Count()).ToList();
            depAirport = dep[0].Key;
            depCount = dep[0].Count();

            return Page();
        }
    }
}
