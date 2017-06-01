using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.DataServices;
using System;
using Microsoft.WindowsAzure.Storage;
using System.Collections.Generic;

namespace NanoDB
{
    public class SensorData : TableEntity
    {
        public SensorData() { }

        public override void ReadEntity(IDictionary<string, EntityProperty> properties, OperationContext operationContext)
        {
            base.ReadEntity(properties, operationContext);
            X = (float) properties["X"].DoubleValue;
            Y = (float)properties["Y"].DoubleValue;
            Height = (float)properties["Height"].DoubleValue;
            Temp = (float)properties["Temp"].DoubleValue;
            WindX = (float)properties["WindX"].DoubleValue;
            WindY = (float)properties["WindY"].DoubleValue;
            Power = (float)properties["Power"].DoubleValue;
        }

        public DateTime DateTime { get; set; }
        public float Height { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float WindY { get; set; }
        public float WindX { get; set; }
        public float Power { get; set; }
        public float Temp { get; set; }
        public string DeviceId { get; set; }
    }
}

