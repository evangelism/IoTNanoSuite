using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace NanoDB
{
    public class SensorData : TableEntity
    {
        public SensorData(string DeviceId, DateTime moment)
        {
            PartitionKey = DeviceId;
            RowKey = moment.ToString("u");
        }

        public DateTime DateTime { get; set; }
        public float Height { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Temp { get; set; }
        public string DeviceId { get; set; }
    }
}

