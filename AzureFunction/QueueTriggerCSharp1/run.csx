#r "Microsoft.WindowsAzure.Storage"
#r "Newtonsoft.Json"
#load "data.csx"
#load "utils.csx"
#load "Interpolator.csx"

using System;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

static float Threshold_A = 25.0f;
static float Threshold_W = 125.0f;

public static void Run(string msg, CloudTable devices, CloudTable interpol, ICollector<SensorData> rawdata, ICollector<DataRecord> data, TraceWriter log)
{
    log.Info($"C# Queue trigger function triggered: {msg}");

    // Store raw data
    var rec = JsonConvert.DeserializeObject<SensorData>(msg);
    rec.PartitionKey = rec.DeviceId;
    rec.RowKey = rec.DateTime.ToString("u");
    rawdata.Add(rec);
    
    // Update device status
    log.Info($"Updating device status for deviceID=: {rec.DeviceId}");
    TableOperation operation = TableOperation.Retrieve<DeviceRecord>("Devices", rec.DeviceId);
    TableResult result = devices.Execute(operation);
    // log.Info($"Result is {result}");
    DeviceRecord dev = (DeviceRecord)result.Result;
    if (dev==null)
    {
        log.Info($"Adding new device: {rec.DeviceId}");
        dev = new DeviceRecord()
        {
            PartitionKey = "Devices",
            RowKey =rec.DeviceId,
            Health = "New",
            Status = "New"
        };
        process(dev,rec,interpol,data);
        operation = TableOperation.Insert(dev);
        devices.Execute(operation);        
    }
    else
    {
        log.Info($"Updating device: {rec.DeviceId}");
        process(dev,rec,interpol,data);
        operation = TableOperation.Replace(dev);
        devices.Execute(operation);
    }
}


public static void process(DeviceRecord dev, SensorData d, CloudTable interpol, ICollector<DataRecord> data)
{
    dev.DateTime = d.DateTime;
    if (d.HasMissingValues)
    {
        dev.Status = "Interpolating";
        Utils.InsertDeviceData(interpol, d);
    }
    else
    {
        if (dev.Status == "Interpolating")
        {
            var L = Utils.GetDevicePartition(interpol, d.DeviceId);
            L.Add(d);
            Interpolator.Interpolate(L);
            L.RemoveAt(0);
            foreach (var x in L)
            {
                var xx = new DataRecord(x);
                data.Add(xx);
            }
            dev.Status = "Normal";
        }
        Utils.DeleteDevicePartition(interpol, d.DeviceId);
        Utils.InsertDeviceData(interpol, d);
    }
    dev.X = d.X; dev.Y = d.Y; dev.Temp = d.Temp;
    dev.Height = d.Height;
    if (!d.HasMissingValues)
    {
        var dd = new DataRecord(d);
        if (Math.Abs(dd.A) > Threshold_A || Math.Abs(dd.WindA) > Threshold_W) dev.Health = "BAD";
        else dev.Health = "GOOD";
    }
}
