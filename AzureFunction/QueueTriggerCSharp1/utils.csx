using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

public class Utils
{
    public static void InsertDeviceData(CloudTable tab, SensorData d)
    {
        var t = TableOperation.Insert(d);
        tab.Execute(t);
    }

    public static List<SensorData> GetDevicePartition(CloudTable tab, string PartitionKey)
    {
        var q = tab.CreateQuery<SensorData>();
        var rec = (from x in q where x.DeviceId == PartitionKey select x).AsTableQuery();
        return rec.Execute().ToList();
    }

    public static void DeleteDevicePartition(CloudTable tab, string PartitionKey)
    {
        var res = GetDevicePartition(tab, PartitionKey);
        var bt = new TableBatchOperation();
        foreach(var x in res)
        {
            bt.Delete(x);
        }
        tab.ExecuteBatch(bt);
    }
}