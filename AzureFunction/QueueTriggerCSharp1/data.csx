using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

public class DeviceRecord : TableEntity
{
    public DateTime DateTime { get; set; }
    public float X { get; set; }
    public float Y { get; set; }
    public float A { get; set; }
    public float WindX { get; set; }
    public float WindY { get; set; }
    public float WindA { get; set; }
    public float Power { get; set; }
    public float Temp { get; set; }
    public float Height { get; set; }
    public string DeviceId { get; set; }
    public string Health { get; set; }
    public string Status { get; set; }
}

// The same class as SensorData in Generator, but with PartitionKey/RowKey added
public class SensorData : TableEntity
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
            return float.IsNaN(X)
                || float.IsNaN(Y)
                || float.IsNaN(Temp)
                || float.IsNaN(Height)
                || float.IsNaN(WindX)
                || float.IsNaN(WindY)
                || float.IsNaN(Power);
        }
    }

    public override string ToString()
    {
        return $"{DateTime}: X={X},Y={Y},Temp={Temp},WindX={WindX},WindY={WindY},Power={Power}";
    }
}

// Class of the data that contains additional computed values after interpolation
public class DataRecord : SensorData
{
    public float A { get; set; }
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