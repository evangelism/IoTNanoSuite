using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpolator
{
    public class Generator
    {
        protected Random Rnd = new Random();
        public double Dropout { get; set; } = 0;
        public SensorData Current = new SensorData() { DateTime = DateTime.Now };
        
        protected float Adj => (float)(Rnd.NextDouble() - 0.5);

        public SensorData Next()
        {
            Current.DateTime += TimeSpan.FromSeconds(1);
            Current.X += Adj; Current.Y += Adj;
            Current.Temp+=Adj;
            Current.WindX+=Adj; Current.WindY+=Adj;
            Current.Power += Adj;
            var z = new SensorData(Current);
            if (Rnd.NextDouble()<Dropout)
            {
                switch(Rnd.Next(7))
                {
                    case 0: z.X = float.NaN; break;
                    case 1: z.Y = float.NaN; break;
                    case 2: z.WindX = float.NaN; break;
                    case 3: z.WindY = float.NaN; break;
                    case 4: z.Power = float.NaN; break;
                    case 5: z.Height = float.NaN; break;
                }
            }
            return z;
        }

    }
}
