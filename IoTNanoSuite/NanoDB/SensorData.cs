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
            X = float.Parse(properties["X"].StringValue);
            Y = float.Parse(properties["Y"].StringValue);
            Height = float.Parse(properties["Height"].StringValue);
        }

        public DateTime DateTime { get; set; }
        public float Height { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Temp { get; set; }
        public string DeviceId { get; set; }
    }
}

