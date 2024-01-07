using NModbus;
using NModbus.Device;
using Prism.Mvvm;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace DeviceQueueDemo.WPF.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private BlockingCollection<Func<Task>> BlockQueue = new BlockingCollection<Func<Task>>();

        private static Dictionary<string, (byte, ushort, ushort)> Addressd = new Dictionary<string, (byte, ushort, ushort)>()
        {
            {"40100",(1,0,1)}
        };

        public static ConcurrentDictionary<string, string> keyValuePairs = new ConcurrentDictionary<string, string>();

        private ObservableCollection<AddressInfo> addressinfo;

        public ObservableCollection<AddressInfo> _AddressInfo
        {
            get { return addressinfo; }
            set { SetProperty(ref addressinfo, value); }
        }

        private TcpClient _Client;
        private IModbusMaster _Master;
        private ModbusFactory _ModbusFactory;
        private ModbusReader _ModbusMster;
        private CancellationTokenSource cancellationTokenSource;

        public MainWindowViewModel()
        {
            _Client = new TcpClient("127.0.0.1", 502);
            _ModbusFactory = new ModbusFactory();

            _Master = _ModbusFactory.CreateMaster(_Client);
            _Master.Transport.ReadTimeout = 1000;
            _Master.Transport.WriteTimeout = 1000;
            _Master.Transport.Retries = 10;
            _ModbusMster = new ModbusReader(_Master);

            var data = _ModbusMster.ReadModbusData(1, 40100, 1);

            cancellationTokenSource = new CancellationTokenSource();
            Test();
        }

        private async Task Test()
        {
            var data2 = await _ModbusMster.ReadModbusDataAsync(1, 40100, 1);
        }

        private async Task EnQueue()
        {
            var EnQueueTask = Task.Factory.StartNew(() =>
            {
                while (cancellationTokenSource.IsCancellationRequested)
                {
                    if (BlockQueue.Count == 0)
                    {
                    }
                }
            }, TaskCreationOptions.LongRunning);
        }

        private async Task DeQueue()
        {
            var DeQueueTask = Task.Factory.StartNew(async () =>
            {
                while (!BlockQueue.IsCompleted)
                {
                    var action = BlockQueue.Take();
                    await action();
                }
            }, TaskCreationOptions.LongRunning);
        }
    }
}