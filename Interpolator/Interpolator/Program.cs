using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpolator
{
    class Program
    {
        static void Main(string[] args)
        {
            var G = new Generator();
            G.Dropout = 0.8;
            var last = G.Current;
            var Q = new List<SensorData>();
            var interpol = false;
            for(int i=0;i<1000;i++)
            {
                var sd = G.Next();
                if (sd.HasMissingValues)
                {
                    if (!interpol)
                    {
                        Q.Add(last);
                        interpol = true;
                    }
                    Q.Add(sd);
                    Console.WriteLine($" > {sd}");
                }
                else
                {
                    if (interpol)
                    {
                        Q.Add(sd);
                        Interpolator.Interpolate(Q);
                        Q.RemoveAt(0);
                        foreach (var x in Q) Console.WriteLine($" < {x}");
                        Q.Clear();
                        interpol = false;
                    }
                    else Console.WriteLine($" + {sd}");
                }
                last = sd;
            }
            Console.ReadKey();
        }
    }
}
