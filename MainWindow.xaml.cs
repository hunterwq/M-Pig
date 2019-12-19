using M_Pig.COM;
using M_Pig.SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace M_Pig
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent(); 
        }
        private Com ComX { get; set; } = new Com();
        private MyModbus modbus { get; set; } = new MyModbus();
        private Thread t_scanDevice { get; set; }
        private Thread t_scanData { get; set; }
        private Thread t_sqlite { get; set; }
        private ObservableCollection<PigData> pigDatas{ get; set; }=new ObservableCollection<PigData>();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("start\r\n");
            
            this.comSelect.ItemsSource = ComX.Ss;
            comSelect.DisplayMemberPath = "Description";
            comSelect.SelectedValuePath = "ComNum";
            comSelect.SelectedIndex = 0;

            DeveiceList.DataContext = modbus.Controler;
            DataList.DataContext = pigDatas;

            modbus.PigDataUpdateEvent += Modbus_PigDataUpdateEvent;

            t_sqlite = new Thread(SqliteThread);
            t_sqlite.Start();

        //SqliteDbContext context = new SqliteDbContext();
        //var b = context.Pigs.Where(p => p.PigID == 1).FirstOrDefault();
        }

        private void SqliteThread()
        {
            //throw new NotImplementedException();
            SqliteOperate sqliteOperate = new SqliteOperate();
            modbus.PigDataUpdateEvent += sqliteOperate.SqliteAddPig;
        }

        private void Modbus_PigDataUpdateEvent(PigData d)
        {
            //throw new NotImplementedException();
            _ = Dispatcher.BeginInvoke((Action)delegate ()
            {
                PigData pig = new PigData();
                pig = d;
                int v = pigDatas.ToList().FindIndex(i => i.PigSerial == d.PigSerial);
                if(v>=0)
                    pigDatas.RemoveAt(v);
                pigDatas.Insert(0, pig);
                //DeveiceList.Items.Refresh();
            });
        }

        private void comButton_Checked(object sender, RoutedEventArgs e)
        {
            if (modbus.ModbusInit(ComX.Ss[comSelect.SelectedIndex].ComNum))
                comButton.Content = "关闭串口";
            else
            {
                comButton.IsChecked = false;
            }
        }

        private void comButton_Unchecked(object sender, RoutedEventArgs e)
        {
            modbus.ModbusDeinit();
            comButton.Content = "打开串口";
        }

        private void DeviceScan_Click(object sender, RoutedEventArgs e)
        {
            t_scanDevice = new Thread(modbus.ControlerScan);
            t_scanDevice.Start();
            modbus.DeviceScanProgressEvent += Modbus_DeviceScanProgressEvent;
        }

        private void Modbus_DeviceScanProgressEvent(int d)
        {
            //throw new NotImplementedException();
            _ = Dispatcher.BeginInvoke((Action)delegate ()
            {
                DeveiceList.Items.Refresh();
            });
        }

        private void dataScan_Checked(object sender, RoutedEventArgs e)
        {
            t_scanData = new Thread(modbus.DataScan);
            t_scanData.Start();
        }

        private void dataScan_Unchecked(object sender, RoutedEventArgs e)
        {
            t_scanData.Abort();
        }
    }
}
