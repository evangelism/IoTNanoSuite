#r "Microsoft.WindowsAzure.Storage"
#r "Newtonsoft.Json"

using System;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

static float Threshold_A = 25.0f;

public static void Run(string msg, CloudTable devices, ICollector<RawDataRecord> rawdata, ICollector<DataRecord> data, TraceWriter log)
{
    log.Info($"C# Queue trigger function triggered: {msg}");

    // Store raw data
    var rec = JsonConvert.DeserializeObject<RawDataRecord>(msg);
    rec.PartitionKey = rec.DeviceId;
    rec.RowKey = rec.DateTime.ToString("u");
    rawdata.Add(rec);
    
    // Calculate Alpha parameter and store filtered Data
    log.Info($"Adding calculated data: {msg}");
    var d = new DataRecord(); 
    d.PartitionKey = rec.PartitionKey; d.RowKey = rec.RowKey;
    d.DeviceId = rec.DeviceId;
    d.X = rec.X; d.Y = rec.Y; d.Temp = rec.Temp;
    d.Height = rec.Height;
    d.DateTime = rec.DateTime;
    d.A = (float)Math.Sqrt(rec.X*rec.X+rec.Y*rec.Y);
    data.Add(d);

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
}

public static void update(DeviceRecord dev, DataRecord d)
{
    dev.DateTime = d.DateTime;
    dev.X = d.X; dev.Y = d.Y; dev.Temp = d.Temp;
    dev.Height = d.Height; dev.A = d.A;
    if (Math.Abs(dev.A)>Threshold_A) dev.Status = "BAD";
    else dev.Status = "GOOD";
}

public class DeviceRecord : TableEntity
{
    public DateTime DateTime {get;set;}
    public float X { get;set; }
    public float Y { get;set; }
    public float A { get;set; }
    public float Temp { get;set; }
    public float Height { get; set; }
    public string DeviceId { get; set; }
    public string Status {get;set;}
}

public class RawDataRecord
{
    public string PartitionKey {get;set;}
    public string RowKey {get;set;}
    public DateTime DateTime {get;set;}
    public float X { get;set; }
    public float Y { get;set; }
    public float Temp { get;set; }
    public float Height { get; set; }
    public string DeviceId { get; set; }
}

public class DataRecord
{
    public string PartitionKey {get;set;}
    public string RowKey {get;set;}
    public DateTime DateTime {get;set;}
    public float X { get;set; }
    public float Y { get;set; }
    public float Temp { get;set; }
    public float Height { get; set; }
    public string DeviceId { get; set; }
    public float A {get;set;}
}