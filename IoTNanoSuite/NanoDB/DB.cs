using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Globalization;
using System.Threading;

namespace NanoDB
{
    public class DB
    {
        CloudStorageAccount CloudStore;
        CloudTableClient CloudTable;
        CloudTable Raw;

        public DB() : this(Config.DBConnectioString)
        {
        }
        public DB(string conn)
        {
            CloudStore = CloudStorageAccount.Parse(conn);
            CloudTable = CloudStore.CreateCloudTableClient();
            Raw = CloudTable.GetTableReference("sensordata");
            Raw.CreateIfNotExists();
        }

        public SensorData[] GetRawData(string DeviceId = "")
        {
            var q = new TableQuery<SensorData>();
            return Raw.ExecuteQuery<SensorData>(q).ToArray();
        }

    }
}
