using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using VatsimLibrary.VatsimDb;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace api
{
    public class PilotsEndpoint
    {
        public static async Task CallsignEndpoint(HttpContext context)
        {
            string responseText = null;
            string callsign = context.Request.RouteValues["callsign"] as string;
            switch ((callsign ?? "").ToLower())
            {
                case "aal1":
                    responseText = "Callsign: AAL1";
                    break;
                default:
                    responseText = "Callsign: INVALID";
                    break;
            }

            if (callsign != null)
            {
                await context.Response.WriteAsync($"{responseText} is the callsign");
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
            }
        }


        /* NOTE: All of these require that you first obtain a pilot and then search in Positions */

        public static async Task AltitudeEndpoint(HttpContext context)
        {
            //TO DO
            //Test: SUR4400
            //I'm using callsign to grab a record and returning the altitude for that record. If there's more than one record, I'm returning the first one. 
            string responseText = null;
            string callsign = context.Request.RouteValues["callsign"] as string;
          
            using (var db = new VatsimDbContext())
            {
                if (callsign != null)
                {
                    Console.WriteLine($"{callsign}");
                    var _altitude = await db.Positions.Where(f => f.Callsign == callsign).ToListAsync();

                    responseText = $"{_altitude[0].Realname} (Callsign: {_altitude[0].Callsign}) is flying at an altitude of {_altitude[0].Altitude} AGL";
                    await context.Response.WriteAsync($"{responseText}");
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                }

            }
        }

        public static async Task GroundspeedEndpoint(HttpContext context)
        {
            //TO DO
            //Test: MLA1799 & GLD879
            //I'm using callsign to grab a record and returning the groundspeed for that record. If there's more than one record, I'm returning the first one. 
            string responseText = null;
            string callsign = context.Request.RouteValues["callsign"] as string;
           
            using (var db = new VatsimDbContext())
            {
                if (callsign != null)
                {
                    Console.WriteLine($"{callsign}");
                    var _groundspeed = await db.Positions.Where(f => f.Callsign == callsign).ToListAsync();

                    //Checking if the Pilot is not moving or on the ground 
                    if (Convert.ToInt32(_groundspeed[0].Groundspeed) == 0)
                    {
                        responseText = $"{_groundspeed[0].Realname} (Callsign: {_groundspeed[0].Callsign}) is on the ground at the moment.";
                    }
                    else
                    {
                        responseText = $"{_groundspeed[0].Realname} (Callsign: {_groundspeed[0].Callsign}) is travelling at {_groundspeed[0].Groundspeed} KTAS";
                    }
                    
                    await context.Response.WriteAsync($"{responseText}");
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                }

            }
        }

        public static async Task LatitudeEndpoint(HttpContext context)
        {
            //TO DO
            //Test: VPCAL & GLO9204
            //I'm using callsign to grab a record and returning the latitude for that record. If there's more than one record, I'm returning the first one. 
            string responseText = null;
            string callsign = context.Request.RouteValues["callsign"] as string;
            
            using (var db = new VatsimDbContext())
            {
                if (callsign != null)
                {
                    Console.WriteLine($"{callsign}");
                    var _latitude = await db.Positions.Where(f => f.Callsign == callsign).ToListAsync();
        
                    var check = Convert.ToDecimal(_latitude[0].Latitude);
                    var degrees = "";

                    if (check > 0 && check <= 90)
                    {
                        degrees = "N";
                    }
                    else
                    {
                        degrees = "S";
                    }

                    responseText = $"{_latitude[0].Realname} (Callsign: {_latitude[0].Callsign}) is travelling at a latitude of {_latitude[0].Latitude} {degrees}";
                    await context.Response.WriteAsync($"{responseText}");

                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                }
            }
        }

        public static async Task LongitudeEndpoint(HttpContext context)
        {
            //TO DO
            //Test: PPWMA & CXD079
            //I'm using callsign to grab a record and returning the longitude for that record. If there's more than one record, I'm returning the first one. 
            string responseText = null;
            string callsign = context.Request.RouteValues["callsign"] as string;
           
            using (var db = new VatsimDbContext())
            {
                if (callsign != null)
                {
                    Console.WriteLine($"{callsign}");
                    var _longitude = await db.Positions.Where(f => f.Callsign == callsign).ToListAsync();
            
                    var check = Convert.ToDecimal(_longitude[0].Longitude);
                    var degrees = "";

                    if (check > 0 && check <= 180)
                    {
                        degrees = "E";
                    }
                    else
                    {
                        degrees = "W";
                    }

                    responseText = $"{_longitude[0].Realname} (Callsign: {_longitude[0].Callsign}) is travelling at a longitude of {_longitude[0].Longitude} {degrees}";
                    await context.Response.WriteAsync($"{responseText}");

                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                }
            }
        }
    }
}