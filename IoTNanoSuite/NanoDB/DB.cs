using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;


namespace NanoDB
{
    public class DB
    {
        CloudStorageAccount CloudStore;
        CloudTableClient CloudTable;
        CloudTable Raw;

        public DB(string conn)
        {
            CloudStore = CloudStorageAccount.Parse(conn);
            CloudTable = CloudStore.CreateCloudTableClient();
            Raw = CloudTable.GetTableReference("rawdata");
            Raw.CreateIfNotExists();
        }

        public IEnumerable<SensorData> GetRawData(string DeviceId = "")
        {
            var res = from x in Raw.CreateQuery<SensorData>()
                      select x;
            return res.ToArray();
        }

    }
}
