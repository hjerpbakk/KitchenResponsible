using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using LightBench;

namespace LoadTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            PerformanceTest();
        }

        static void PerformanceTest() {
            var httpClient = new HttpClient();
            Console.WriteLine(LightBench.Benchmark.Run(() => {
                httpClient.GetStringAsync("http://localhost:5000/api/Employees/1").Wait();
            }, 5, "Get by index", false).ToString());
        }

        static void LoadTest() {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
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
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds);
        }
    }
}
