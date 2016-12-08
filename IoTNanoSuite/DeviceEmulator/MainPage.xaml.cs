using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DeviceEmulator
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Windows.Storage.StorageFile _csvFile;
        readonly DispatcherTimer _dispatcherTimer = new DispatcherTimer();
        private int _readRawIndex = 0;
        private IList<string> _data;
        private string _connectionString = "HostName=NanoHub.azure-devices.net;DeviceId=Emulator;SharedAccessKey=rq48ZKTXYshDdqmqZqhBhdE5aZTKhgulSO8ZIyLPf1U=";
        private DeviceClient _deviceClient;
        private string _deviceId = "Emulator";

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            RunBtn.IsEnabled = false;
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".csv");

            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                // Application now has read/write access to the picked file
                _csvFile = file;
                CsvPathTb.Text = file.Path;
                RunBtn.IsEnabled = true;
            }
        }

        private async void RunBtn_Click(object sender, RoutedEventArgs e)
        {

            _data = await Windows.Storage.FileIO.ReadLinesAsync(_csvFile);
            if (ContainsHeaderCb.IsChecked == true) _readRawIndex = 1;
            else _readRawIndex = 0;

            _dispatcherTimer.Tick += _dispatcherTimer_Tick;
            var deltaTimeS = int.Parse(SendDeltaTb.Text);
            _dispatcherTimer.Interval = new TimeSpan(0, 0, deltaTimeS);
            _dispatcherTimer.Start();


            _deviceClient = DeviceClient.CreateFromConnectionString(_connectionString);
        }

        private async void _dispatcherTimer_Tick(object sender, object e)
        {
            var raw = _data[_readRawIndex];
            var values = raw.Split(';');
            var dt = DateTime.Parse(values[0] + " " + values[1]);

            var height = float.Parse(values[2].Replace(',', '.'), NumberStyles.Float);
            var x = float.Parse(values[3].Replace(',', '.'), NumberStyles.Float);
            var y = float.Parse(values[4].Replace(',', '.'), NumberStyles.Float);
            var temp = float.Parse(values[5].Replace(',', '.'), NumberStyles.Float);

            var info = new SensorData
            {
                DateTime = dt,
                Height = height,
                X = x,
                Y = y,
                Temp = temp,
                DeviceId = _deviceId
            };
            var serializedString = JsonConvert.SerializeObject(info);
            var bytes = Encoding.UTF8.GetBytes(serializedString);
            var message = new Message(bytes);
            await _deviceClient.SendEventAsync(message);
            _readRawIndex++;
        }
    }
}
