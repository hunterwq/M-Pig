using M_Pig.COM;
using M_Pig.SQLite;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("start\r\n");
            
            this.comSelect.ItemsSource = ComX.Ss;
            comSelect.DisplayMemberPath = "Description";
            comSelect.SelectedValuePath = "ComNum";
            comSelect.SelectedIndex = 0;
            SqliteDbContext context = new SqliteDbContext();
            var b = context.Employees.Where(p => p.EmployeeID == 1).FirstOrDefault();
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
    }
}
