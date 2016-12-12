using System;
using System.Numerics;
using System.Text;
using Windows.UI.Xaml;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DeviceEmulator
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage
    {
        readonly DispatcherTimer _dispatcherTimer = new DispatcherTimer();
        private string _connectionString = "HostName=NanoHub.azure-devices.net;DeviceId=Emulator;SharedAccessKey=rq48ZKTXYshDdqmqZqhBhdE5aZTKhgulSO8ZIyLPf1U=";
        private readonly DeviceClient _deviceClient;

        private Windows.Storage.StorageFile _csvFile;
        private readonly CsvDataTable _csvData = new CsvDataTable();

        public MainPage()
        {
            InitializeComponent();
            _dispatcherTimer.Tick += _dispatcherTimer_Tick;
            _deviceClient = DeviceClient.CreateFromConnectionString(_connectionString);
            StopBtn.IsEnabled = false;
            ErrorTextBlock.Text = string.Empty;
        }

        private async void FindCsvFileBtn_Click(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker
            {
                ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail,
                SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary
            };

            picker.FileTypeFilter.Add(".csv");

            _csvFile = await picker.PickSingleFileAsync();
            CsvFilePathTb.Text = _csvFile.Path;
        }

        private void LoadCsvFileBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_csvFile != null)
            {
                _csvData.LoadFromCsvFile(_csvFile);
            }
        }

        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            var deltaTimeS = int.Parse(SendDeltaTb.Text);
            _dispatcherTimer.Interval = new TimeSpan(0, 0, deltaTimeS);

            if (UseCsvRb.IsChecked == true)
            {
                if (!_csvData.IsLoaded)
                {
                    ErrorTextBlock.Text = "Csv file is not loaded.";
                    return;
                }
            }

            _dispatcherTimer.Start();
            StopBtn.IsEnabled = true;
            StartBtn.IsEnabled = false;
            UseCsvRb.IsEnabled = false;
            UseGeneratorRb.IsEnabled = false;
            DeviceNameTb.IsEnabled = false;
            SendDeltaTb.IsEnabled = false;
            ErrorTextBlock.Text = string.Empty;
        }

        private async void _dispatcherTimer_Tick(object sender, object e)
        {
            SensorData data = new SensorData
            {
                DeviceId = DeviceNameTb.Text
            };

            var powerBase = float.Parse(PowerValueBaseTb.Text.Replace(',', '.'));
            data.Power = DataGenerator.GeneratePowerValue(powerBase);

            var windBase = float.Parse(WindValueBaseTb.Text.Replace(',', '.'));
            var windDelta = float.Parse(WindDeltaTb.Text.Replace(',', '.'));

            var wind = DataGenerator.GenerateWindValue(windBase, windDelta);
            data.WindX = wind.X;
            data.WindY = wind.Y;

            if (UseCsvRb.IsChecked == true)
            {
                var csvData = _csvData.ReadNext();
                data.DateTime = csvData.DateTime;
                data.X = csvData.X;
                data.Y = csvData.Y;
                data.Height = csvData.Height;
                data.Temp = csvData.Temp;
            }
            else if (UseGeneratorRb.IsChecked == true)
            {
                data.DateTime = DateTime.Now;

                var height = float.Parse(SensorHeightTb.Text.Replace(',', '.'));
                data.Height = height;

                var temperatureBase = float.Parse(PowerValueBaseTb.Text.Replace(',', '.'));
                data.Temp = DataGenerator.GenerateTemperatureValue(temperatureBase);

                var mainBaseX = float.Parse(MainValueBaseXTb.Text.Replace(',', '.'));
                var mainBaseY = float.Parse(MainValueBaseYTb.Text.Replace(',', '.'));
                var mainDelta = float.Parse(WindDeltaTb.Text.Replace(',', '.'));

                var mainVal = DataGenerator.GenerateMainValue(new Vector2(mainBaseX, mainBaseY), mainDelta);

                data.X = mainVal.X;
                data.Y = mainVal.Y;
            }

            var serializedString = JsonConvert.SerializeObject(data);
            var bytes = Encoding.UTF8.GetBytes(serializedString);
            var message = new Message(bytes);
            await _deviceClient.SendEventAsync(message);
        }

        private void StopBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_dispatcherTimer.IsEnabled)
            {
                _dispatcherTimer.Stop();

                StopBtn.IsEnabled = false;
                StartBtn.IsEnabled = true;

                UseCsvRb.IsEnabled = true;
                UseGeneratorRb.IsEnabled = true;
                DeviceNameTb.IsEnabled = true;
                SendDeltaTb.IsEnabled = true;
            }
        }

        private void ContainsHeaderCb_Checked(object sender, RoutedEventArgs e)
        {
            _csvData.ContainsHeader = ContainsHeaderCb.IsChecked == true;
        }
    }
}
