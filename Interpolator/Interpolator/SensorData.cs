using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpolator
{
    public class SensorData
    {
        public SensorData() { }
        public SensorData(SensorData x)
        {
            this.DateTime = x.DateTime;
            this.X = x.X; this.Y = x.Y;
            this.Power = x.Power; this.Height = x.Height;
            this.WindX = x.WindX; this.WindY = x.WindY;
            this.Height = x.Height;
        }
        public string DeviceId { get; set; }
        public DateTime DateTime { get; set; }
        public float Height { get; set; }
        public float WindX { get; set; }
        public float WindY { get; set; }
        public float Power { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Temp { get; set; }
        // A property to show if the data has missing values
        public bool HasMissingValues
        {
            get
            {
                return float.IsNaN(X)
                    || float.IsNaN(Y)
                    || float.IsNaN(Temp)
                    || float.IsNaN(Height)
                    || float.IsNaN(WindX)
                    || float.IsNaN(WindY)
                    || float.IsNaN(Power);
            }
        }
        public override string ToString()
        {
            return $"{DateTime}: X={X},Y={Y},Temp={Temp},WindX={WindX},WindY={WindY},Power={Power}";
        }
    }
}
