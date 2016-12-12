using System;

namespace DeviceEmulator
{
    public class SensorData
    {
        public string DeviceId { get; set; }
        public DateTime DateTime { get; set; }
        public float Height { get; set; }
        public float WindX { get; set; }
        public float WindY { get; set; }
        public float Power { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Temp { get; set; }
    }
}

