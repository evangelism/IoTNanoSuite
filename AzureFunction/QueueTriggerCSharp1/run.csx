#r "Microsoft.WindowsAzure.Storage"
#r "Newtonsoft.Json"

using System;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

static float Threshold_A = 25.0f;

public static void Run(string msg, CloudTable devices, CloudTable interpol, ICollector<RawDataRecord> rawdata, ICollector<DataRecord> data, TraceWriter log)
{
    log.Info($"C# Queue trigger function triggered: {msg}");

    // Store raw data
    var rec = JsonConvert.DeserializeObject<RawDataRecord>(msg);
    rec.PartitionKey = rec.DeviceId;
    rec.RowKey = rec.DateTime.ToString("u");
    rawdata.Add(rec);
    
    // Calculate Alpha parameter
    log.Info($"Adding calculated data: {msg}");
    var d = new DataRecord(); 
    d.PartitionKey = rec.PartitionKey; d.RowKey = rec.RowKey;
    d.DeviceId = rec.DeviceId;
    d.X = rec.X; d.Y = rec.Y; d.Temp = rec.Temp;
    d.Height = rec.Height;
    d.DateTime = rec.DateTime;

    // Update device status
    log.Info($"Updating device status for deviceID=: {d.DeviceId}");
    TableOperation operation = TableOperation.Retrieve<DeviceRecord>("Devices", rec.DeviceId);
    TableResult result = devices.Execute(operation);
    log.Info($"Result is {result}");
    DeviceRecord dev = (DeviceRecord)result.Result;
    log.Info($"DevRecord is {dev}");
    if (dev==null)
    {
        log.Info($"Adding new device: {d.DeviceId}");
        dev = new DeviceRecord() { PartitionKey = "Devices", RowKey=d.DeviceId };
        update(dev,d);
        operation = TableOperation.Insert(dev);
        devices.Execute(operation);        
        log.Info($"Executed");
    }
    else
    {
        log.Info($"Updating device: {d.DeviceId}");
        update(dev,d);
        operation = TableOperation.Replace(dev);
        devices.Execute(operation);
    }

    // Store filtered Data
    data.Add(d);

}


public static void update(DeviceRecord dev, DataRecord d)
{
    dev.DateTime = d.DateTime;
    if (d.HasMissingValues)
    {
        dev.Status = "Interpolating";
    }
    else
    {
        dev.Status = ""
        dev.X = d.X; dev.Y = d.Y; dev.Temp = d.Temp;
        dev.Height = d.Height; dev.A = d.A;
        if (Math.Abs(dev.A) > Threshold_A) dev.Health = "BAD";
        else dev.Health = "GOOD";
    }
}

public class DeviceRecord : TableEntity
{
    public DateTime DateTime {get;set;}
    public float X { get;set; }
    public float Y { get;set; }
    public float A { get;set; }
    public float WindX { get; set; }
    public float WindY { get; set; }
    public float WindA { get; set; }
    public float Power { get; set; }
    public float Temp { get;set; }
    public float Height { get; set; }
    public string DeviceId { get; set; }
    public string Health {get;set;}
    public string Status { get; set; }
}

// The same class as SensorData in Generator, but with PartitionKey/RowKey added
public class SensorData
{
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
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
            return X == float.NaN || Y == float.NaN || Temp == float.NaN || Height == float.NaN
                                  || WindX == float.NaN || WindY == float.NaN || Power == float.NaN;
        }
    }
}

// Class of the data that contains additional computed values after interpolation
public class DataRecord : SensorData
{
    public float A {get; set; }
    public float WindA { get; set; }    
    public DataRecord(SensorData d)
    {
        this.Height = d.Height;
        this.Temp = d.Temp;
        this.X = d.X; this.Y = d.Y;
        this.WindX = d.WindX; this.WindY = d.WindY;
        this.Power = d.Power;
        this.DateTime = d.DateTime;
        this.PartitionKey = d.PartitionKey; this.RowKey = d.RowKey;

        // Computed Properties
        this.A = (float)Math.Sqrt(d.X * d.X + d.Y * d.Y);
        this.WindA = (float)Math.Sqrt(d.WindX * d.WindX + d.WindY * d.WindY);
    }
}