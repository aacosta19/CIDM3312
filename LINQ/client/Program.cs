using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MedianExtensions;
using FindMode;

using LINQDataSources;

namespace client
{
    class Program
    {
        static void Main(string[] args)
        {

            IEnumerable<IrisRecord> records = LoadIrisData();
            //IEnumerable<IrisRecord> filtered;

            if(records != null)
            {
                //print all
                // PrintRecords("unfiltered", records);

                // filter: all petal widths are at or above 4.0
                // float boundary = 4.0f;
                // filtered = records.Where(n => n.PetalWidth >= boundary);
                // PrintRecords($"Petal Width at or above {boundary}", filtered);

                /*
                from num in numbers
                    where num % 2 == 0
                    orderby num
                    select num;
                */

                // var otherfilter = 
                //     from record in records
                //     where record.PetalLength < boundary
                //     select record;

                // PrintRecords($"Petal Length at or below {boundary}", otherfilter);                    

                // complete all of the queries here

                
            // Query 1: Create and show a LINQ Query that lists all Sepal Widths that are above the average Sepal Width
                Console.WriteLine("\nQuery 1:\n");
                //Get average Sepal Width of every record in records
                var sepalWidthAverage = records.Average(record => record.SepalWidth);
                Console.WriteLine("The average Sepal Width is: {0} ", sepalWidthAverage);
                    var query1 = 
                        from record in records
                        where record.SepalWidth > sepalWidthAverage
                        orderby record.SepalWidth
                        select record.SepalWidth;

                    Console.WriteLine("\nThe following are the Sepal Widths above the average:");

                    foreach(var q in query1) {
                    Console.WriteLine(q);
                    }


            //Query 2: Create and show a LINQ Query that lists all Sepal Lengths that are below the average Sepal Length
                Console.WriteLine("\nQuery 2:\n");
                  //Get average Sepal Length of every record in records
                var sepalLengthAverage = records.Average(record => record.SepalLength);
                Console.WriteLine("The average Sepal Length is: {0} ", sepalLengthAverage);
                    var query2 =
                        from record in records
                        where record.SepalLength < sepalLengthAverage
                        orderby record.SepalLength descending
                        select record.SepalLength;

                
                    Console.WriteLine("\nThe following are the Sepal Lengths below the average:");    

                    foreach(var q in query2) {
                    Console.WriteLine(q);
                    }


            //Query 3:Create and show a LINQ Query that indicates which class of iris has the lowest average Petal Width
                Console.WriteLine("\nQuery 3:\n");
                    var query3 =
                    from record in records
                    group record by record.IrisClassificationName into lowestAvgWidth
                    select new {
                        Classification = lowestAvgWidth.Key,
                        AverageWidth = lowestAvgWidth.Average(record => record.PetalWidth),
                    };

                    //Make a new list and select the average petal width of each record to be added 
                    List<float> AvgWidthList = new List<float>();
                    
                    foreach(var q in query3) {
                        AvgWidthList.Add(q.AverageWidth);
                    }

                    //Find out which is the minimum average Petal Width in the new list made
                    var minimumWidthAverage = AvgWidthList.Min();

                    var lowestWidthClass = 
                        from q in query3
                        where q.AverageWidth == minimumWidthAverage
                        select q.Classification;

                    //Make a new variable and select the classificaiton of the class that has an average Petal Width equal to the minimum width average
                    var lowestIrisClass = lowestWidthClass.ToList()[0];

                    Console.WriteLine(lowestIrisClass + " has the lowest average Petal Width, which equals: " + minimumWidthAverage);


            //Query 4: Create and show a LINQ Query that indicates which class of iris has the highest average Petal Length
                 Console.WriteLine("\nQuery 4:\n");
                    var query4 =
                    from record in records
                    group record by record.IrisClassificationName into highestAvgLength
                    select new {
                        Classification = highestAvgLength.Key,
                        AverageLength = highestAvgLength.Average(record => record.PetalLength),
                    };
                    
                    //Make a new list and select the average petal length of each record to be added 
                    List<float> AvgLengthList = new List<float>();
                    
                    foreach(var q in query4) {
                        AvgLengthList.Add(q.AverageLength);
                    }
                    
                    //Find out which is the maximum average Petal Length in the new list made
                    var maximumLengthAverage = AvgLengthList.Max();

                    var highestLengthClass = 
                        from q in query4
                        where q.AverageLength == maximumLengthAverage
                        select q.Classification;

                    //Make a new variable and select the classificaiton of the class that has an average Petal Length equal to the maximum length average
                    var highestIrisClass = highestLengthClass.ToList()[0];

                    Console.WriteLine(highestIrisClass + " has the highest average Petal Length, which equals: " + maximumLengthAverage);


            //Query 5: Create and show a LINQ Query that indicates the widest Sepal Width for each class of iris
                Console.WriteLine("\nQuery 5:\n");
                var query5 =
                    from record in records
                    group record by record.IrisClassificationName into widestSepalGroup
                    select new {
                        Classification = widestSepalGroup.Key,
                        MaxWidth = widestSepalGroup.Max(record => record.SepalWidth),
                    };

                    //Make a new list and select the widest Sepal Width of each class to be added
                    List<float> widestSepalWidth = new List<float>();

                    foreach (var q in query5) {
                        widestSepalWidth.Add(q.MaxWidth);
                        Console.WriteLine(q.Classification + "'s widest Sepal Width is: " + q.MaxWidth);
                    }


            //Query 6: Create and show a LINQ Query that indicates the shortest Sepal Length for each class of iris
                Console.WriteLine("\nQuery 6:\n");
                var query6 =
                    from record in records 
                    orderby record.IrisClassificationName
                    group record by record.IrisClassificationName into shortestSepalGroup
                    select new
                    {
                        Classification = shortestSepalGroup.Key,
                        ShortestLength = shortestSepalGroup.Min(record => record.SepalLength),
                    };

                //Make a new list and select the shortest Sepal Length of each class to be added
                List<float> shortestSepalList = new List<float>();

                foreach (var q in query6) {
                    shortestSepalList.Add(q.ShortestLength);
                    Console.WriteLine(q.Classification + "'s shortest Sepal Length is: " + q.ShortestLength);
                }

            //Query 7: Create and show a LINQ Query that indicates the ranks the top 5 widest Petal Widths
                Console.WriteLine("\nQuery 7:\n");
                var query7 =
                    (from record in records
                    orderby record.PetalWidth descending
                    select record).Take(5);

                //Make a new list and select the top 5 widest Petal Widths to be added
                List<float> top5List = new List<float>(); 
                
                foreach(var q in query7) {
                            top5List.Add(q.PetalWidth);    
                    }

                Console.WriteLine("The Top 5 widest Petal Widths are:");

                //Output the top 5 widest Petal Widths in numeric order 
                var counter = 1;
                foreach (var q in top5List) {
                    Console.WriteLine(counter + ". " + q);
                    counter ++;
                }

            //Query 8: Create and show a LINQ Query that indicates the ranks the bottom 5 shortest Petal Lengths
                Console.WriteLine("\nQuery 8:\n");
                var query8 =
                    (from record in records
                    orderby record.PetalLength 
                    select record).Take(5);

                //Make a new list and select the bottom 5 shortest Petal Lengths to be added
                List<float> bottom5List = new List<float>(); 
                
                foreach(var q in query8) {
                            bottom5List.Add(q.PetalLength);    
                    }

                Console.WriteLine("The Bottom 5 shortest Petal Lengths are:");

                //Output the bottom 5 shortest Petal Lengths in numeric order 
                var counter1 = 1;
                foreach (var q in bottom5List) {
                    Console.WriteLine(counter1 + ". " + q);
                    counter1 ++;
                }

            //Query 9: Create and show a LINQ Query that indicates the median Sepal Width for each class of iris
                Console.WriteLine("\nQuery 9:\n");
                var query9 =
                    from record in records
                    group record by record.IrisClassificationName into medianGroup
                    select new {
                        Classification = medianGroup.Key,
                        //Using a Median Extension from: https://gist.github.com/axelheer/b1cb9d7c267d6762b244
                        MedianSepalWidth = medianGroup.Median(record => record.SepalWidth)
                    };

                foreach (var q in query9)
                {
                    Console.WriteLine(q.Classification + " has a median Sepal Width of " + q.MedianSepalWidth);
                }


                //The following are my attempts at solving for the Median without the extension 
                
                //Attempt: 1

                // int count = records.Count();
                // var sepalWidth = records.Select(record => record.SepalWidth);

                // if (count % 2 == 0) 
                //     var median = (count/2)
                //     var median = records.Select(record = records.SepalWidth).OrderBy(record => record.SepalWidth).Skip((count / 2) - 1).Take(2);
                // else
                //     var median = records.Select(record => record.SepalWidth).OrderBy(record => record.SepalWidth).ElementAt(count / 2);


                //Attempt: 2

                //     if(q.Count % 2 == 0) {
                //         IEnumerable<int> middleItem1 = value.Select(q => q.SepalWidth);
                //         // var middleItem1 = ((q.Count/2).Take(1)).ToList();
                //         var middleItem2 = ((q.Count/2) -1);
                //         // var middleItem2 = orderedGroup.Skip((count/2) - 1).First();

                //         medianValue = (middleItem1 + middleItem2) /2;
                //     } else {
                //         medianValue = q.Count/2;
                //     }

                //     Console.WriteLine("Class: " + q.Classification + "\n Sepal Width median: " + medianValue);
                // }

            //Query 10: Create and show a LINQ Query that indicates the mode Petal Length for each class of iris
            
            //The following are my attempts at solving for Petal Lenght Mode with the FindMode Class 
            // Console.WriteLine("\nQuery 10:\n");
            //     var query10 =
            //         from record in records
            //         group record by record.IrisClassificationName into modeGroup
            //         select new {
            //             Classification = modeGroup.Key,
            //             ModePetalLength = modeGroup.Mode(record => record.PetalLength)
            //         };

                // foreach (var q in query10)
                // {
                //     Console.WriteLine(q.Classification + " has a mode Petal Length of " + q.ModePetalLength);
                // }                   


            //My second attmempt without the FindMode Class 
            
        //         List<float> modeList = new List<float>(); 
                
        //         foreach(var q in query10) {
        //                     modeList.Add(q.ModePetalLength);    
        //             }

        //         int? modeValue =
        //             modeList
        //                 .GroupBy(x => x)
        //                 .OrderByDescending(x => x.Count()).ThenBy(x => x.Key)
        //                 .Select(x => (int?)x.Key)
        //                 .FirstOrDefault();

        //          foreach (var m in modeList)
        //         {
        //             Console.WriteLine(modeValue);
        //         }    
                

            } //End If
        } // End Main

        static void PrintRecords(string message, IEnumerable<IrisRecord> records)
        {
            // simplest query shows all records
            Console.WriteLine(message);
            foreach(IrisRecord record in records)
            {
                Console.WriteLine(record);
            }
        }

        static IEnumerable<IrisRecord> LoadIrisData()
        {
            // this is somewhat "brittle" code as it only works when the project is
            // run within the client folder.
            Console.WriteLine($@"{Directory.GetCurrentDirectory()}\data\iris.data");
            FileInfo file = new FileInfo($@"{Directory.GetCurrentDirectory()}\data\iris.data");
            Console.WriteLine(file.FullName);
            
            IEnumerable<IrisRecord> records = null;

            try
            {
                records = IrisDataSourceHelper.GetIrisRecordsFromFlatFile(file.FullName);
            }catch (Exception exp)
            {
                Console.Error.WriteLine(exp.StackTrace);
            }
            return records;
        }
    }
}
