using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DeviceQueueDemo.WPF
{
    public class AddressInfo : INotifyPropertyChanged
    {
        private string name;

        public string _Name
        {
            get { return name; }
            set { name = value; OnpropertyChanged(); }
        }

        private string address;

        public string _address
        {
            get { return address; }
            set { address = value; OnpropertyChanged(); }
        }

        private object Readvalue;

        public object _Value
        {
            get { return Readvalue; }
            set { Readvalue = value; OnpropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 实现通知更新
        /// </summary>
        /// <param name="propertyName"></param>
        public void OnpropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}