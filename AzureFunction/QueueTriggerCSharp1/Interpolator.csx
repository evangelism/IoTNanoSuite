using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Interpolator
{
    public static Func<SensorData, double>[] gets =
    {
        s => s.X, s=>s.Y, s=>s.WindX, s=>s.WindY,
        s => s.Height, s=>s.Power, s=>s.Temp
    };

    public static Action<SensorData, double>[] sets =
    {       
        (s,x) => s.X=x, (s,y) => s.Y=y,
        (s,t) => s.WindX=t, (s,t) => s.WindY=t,
        (s,t) => s.Height=t, (s,t) => s.Power=t,
        (s,t) => s.Temp=t
    };

    public static void Interpolate(List<SensorData> L)
    {
        for (int i = 0; i < sets.Length; i++)
        {
            DateTime da, db;
            double va, vb;
            for(int j=0;j<L.Count;j++)
            {
                if (double.IsNaN(gets[i](L[j])))
                {
                    var k = j;
                    while (double.IsNaN(gets[i](L[k]))) k++;
                    va = gets[i](L[j-1]); da = L[j - 1].DateTime;
                    vb = gets[i](L[k]); db = L[k].DateTime;
                    for (int l = j; l < k;l++)
                    {
                        sets[i](L[l], Interpolate1(va, vb, da, db, L[l].DateTime));
                    }
                    j = k;
                }
            }
        }            
    }

    private static double Interpolate1(double va, double vb, DateTime da, DateTime db, DateTime x)
    {
        double t = ((x - da).TotalMilliseconds/ (db - da).TotalMilliseconds);
        return va + (vb - va) * t;
    }
}
