using System;
using System.Collections.Generic;
using System.Globalization;

namespace DeviceEmulator
{
    public class CsvDataTable
    {
        private Windows.Storage.StorageFile _csvFile;
        
        private int _readRawIndex;
        private IList<string> _data;

        public bool ContainsHeader { get; set; }

        public bool IsLoaded { get; protected set; } = false;

        public async void LoadFromCsvFile(Windows.Storage.StorageFile file)
        {
            _csvFile = file;
            _data = await Windows.Storage.FileIO.ReadLinesAsync(_csvFile);
            if (ContainsHeader) _readRawIndex = 1;
            else _readRawIndex = 0;
            IsLoaded = true;
        }

        public CsvDataRaw ReadNext()
        {
            var raw = _data[_readRawIndex];
            _readRawIndex++;

            var values = raw.Split(';');
            var dt = DateTime.Parse(values[0] + " " + values[1]);

            var height = float.Parse(values[2].Replace(',', '.'), NumberStyles.Float);
            var x = float.Parse(values[3].Replace(',', '.'), NumberStyles.Float);
            var y = float.Parse(values[4].Replace(',', '.'), NumberStyles.Float);
            var temp = float.Parse(values[5].Replace(',', '.'), NumberStyles.Float);

            var data = new CsvDataRaw
            {
                DateTime = dt,
                Height = height,
                X = x,
                Y = y,
                Temp = temp,
            };
            return data;
        }
    }

    public class CsvDataRaw
    {
        public DateTime DateTime { get; set; }
        public float Height { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Temp { get; set; }
    }
}
