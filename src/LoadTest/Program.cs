using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace LoadTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            
            LoadTest().Wait();

            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds);
        }

        static async Task LoadTest() {
            Console.WriteLine("Start");
            var httpClient = new HttpClient();

            try {   
                var loop = Parallel.For(0, 1000, (i, b) => {               
                    httpClient.GetStringAsync("http://localhost:5000/api/Employees/1").Wait();    
                });
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            
            Console.WriteLine("End");
        }
    }
}
