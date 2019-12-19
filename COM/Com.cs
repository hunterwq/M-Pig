using M_Pig.Controler;
using M_Pig.SQLite;
using Modbus.Device;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Threading;

namespace M_Pig.COM
{
    class ComList
    {
        public string ComNum { get; set; }
        public string Description { get; set; }
    }
    class Com
    {

        /// <summary>
        /// 枚举win32 api
        /// </summary>
        public enum HardwareEnum
        {
            // 硬件
            Win32_Processor, // CPU 处理器
            Win32_PhysicalMemory, // 物理内存条
            Win32_Keyboard, // 键盘
            Win32_PointingDevice, // 点输入设备，包括鼠标。
            Win32_FloppyDrive, // 软盘驱动器
            Win32_DiskDrive, // 硬盘驱动器
            Win32_CDROMDrive, // 光盘驱动器
            Win32_BaseBoard, // 主板
            Win32_BIOS, // BIOS 芯片
            Win32_ParallelPort, // 并口
            Win32_SerialPort, // 串口
            Win32_SerialPortConfiguration, // 串口配置
            Win32_SoundDevice, // 多媒体设置，一般指声卡。
            Win32_SystemSlot, // 主板插槽 (ISA & PCI & AGP)
            Win32_USBController, // USB 控制器
            Win32_NetworkAdapter, // 网络适配器
            Win32_NetworkAdapterConfiguration, // 网络适配器设置
            Win32_Printer, // 打印机
            Win32_PrinterConfiguration, // 打印机设置
            Win32_PrintJob, // 打印机任务
            Win32_TCPIPPrinterPort, // 打印机端口
            Win32_POTSModem, // MODEM
            Win32_POTSModemToSerialPort, // MODEM 端口
            Win32_DesktopMonitor, // 显示器
            Win32_DisplayConfiguration, // 显卡
            Win32_DisplayControllerConfiguration, // 显卡设置
            Win32_VideoController, // 显卡细节。
            Win32_VideoSettings, // 显卡支持的显示模式。

            // 操作系统
            Win32_TimeZone, // 时区
            Win32_SystemDriver, // 驱动程序
            Win32_DiskPartition, // 磁盘分区
            Win32_LogicalDisk, // 逻辑磁盘
            Win32_LogicalDiskToPartition, // 逻辑磁盘所在分区及始末位置。
            Win32_LogicalMemoryConfiguration, // 逻辑内存配置
            Win32_PageFile, // 系统页文件信息
            Win32_PageFileSetting, // 页文件设置
            Win32_BootConfiguration, // 系统启动配置
            Win32_ComputerSystem, // 计算机信息简要
            Win32_OperatingSystem, // 操作系统信息
            Win32_StartupCommand, // 系统自动启动程序
            Win32_Service, // 系统安装的服务
            Win32_Group, // 系统管理组
            Win32_GroupUser, // 系统组帐号
            Win32_UserAccount, // 用户帐号
            Win32_Process, // 系统进程
            Win32_Thread, // 系统线程
            Win32_Share, // 共享
            Win32_NetworkClient, // 已安装的网络客户端
            Win32_NetworkProtocol, // 已安装的网络协议
            Win32_PnPEntity,//all device
        }
        /// <summary>
        /// WMI取硬件信息
        /// </summary>
        /// <param name="hardType"></param>
        /// <param name="propKey"></param>
        /// <returns></returns>
        public static List<ComList> MulGetHardwareInfo(HardwareEnum hardType, string propKey)
        {

            List<ComList> strs = new List<ComList>();
            
            //string temp,va,vb;
            try
            {
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from " + hardType))
                {
                    var hardInfos = searcher.Get();
                    foreach (var hardInfo in hardInfos)
                    {
                        if (hardInfo.Properties[propKey].Value != null)
                        {
                            if (hardInfo.Properties[propKey].Value.ToString().Contains("COM"))
                            {
                                ComList comList = new ComList();
                                string temp = hardInfo.Properties[propKey].Value.ToString();
                                string va = temp.Substring(temp.IndexOf("(") + 1, temp.IndexOf(")") - (temp.IndexOf("(") + 1));
                                string vb = temp.Substring(0, temp.IndexOf("("));
                                comList.ComNum = va;
                                comList.Description = va + ":" + vb;
                                strs.Add(comList);
                            }
                        }                        
                    }
                    searcher.Dispose();
                }
                return strs;
            }
            catch
            {
                return null;
            }
            finally
            { strs = null; }
        }
        //通过WMI获取COM端口
        //string[] ss = (MulGetHardwareInfo(HardwareEnum.Win32_PnPEntity, "Name"));

        //public string[] Ss { get; set; } = (MulGetHardwareInfo(HardwareEnum.Win32_PnPEntity, "Name"));
        public List<ComList> Ss { get; set; } = (MulGetHardwareInfo(HardwareEnum.Win32_PnPEntity, "Name"));

        public void ComUpdate()
        {
            Ss = (MulGetHardwareInfo(HardwareEnum.Win32_PnPEntity, "Name"));
        }
    }

    class MyModbus
    {
        public delegate void DeviceScanProgress(int d);
        public event DeviceScanProgress DeviceScanProgressEvent;

        public delegate void PigDataUpdate(PigData d);
        public event PigDataUpdate PigDataUpdateEvent;

        public List<ControlerClass> Controler { get; set; } = new List<ControlerClass>();
        public IModbusSerialMaster Master { get; set; }

        public SerialPort Port { get; set; }
        public string ComNum { get; set; } = "COM1";

        public int Delay { get; set; } = 5;
        public void DataScan()
        {
            while (true)
            {
                for (int i = 0; i < Controler.Count; i++)
                {
                    Console.WriteLine("DataRead");
                    Console.WriteLine(i);
                    Console.WriteLine(Controler[i].RoomCount);
                    DataRead(i);
                }
                Thread.Sleep(new TimeSpan(0, 0, Delay));
            } 
        }
        public bool ModbusInit(string num)
        {
            ComNum = num;
            Port = new SerialPort(num, 9600, Parity.None, 8, StopBits.One);
            try
            {
                Port.Open();
                // create modbus master 
                Master = ModbusSerialMaster.CreateRtu(Port);
                Master.Transport.ReadTimeout = 500;
                Master.Transport.Retries = 1;
                return true;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
           
        }
        public void ModbusDeinit()
        {
            Port.Close();
        }


        private void DataRead(int controlerIndex)
        {
            for (ushort i = 0; i < Controler[controlerIndex].RoomCount; i++)
            {
                //读取栏位基本信息
                ushort[] a = Master.ReadInputRegisters(
                   BitConverter.GetBytes(Controler[controlerIndex].ModbusAddress)[1], 14, 4);
                Controler[controlerIndex].Room[i].RoomNum = a[0];
                Controler[controlerIndex].Room[i].BatcherCalibration = a[1];
                Controler[controlerIndex].Room[i].Threshold = a[2];
                Controler[controlerIndex].Room[i].PigsCount = a[3];
                //controlerClass.

                for (ushort d=0;d< Controler[controlerIndex].Room[i].PigsCount; d++)
                {
                    ushort[] temp= Master.ReadInputRegisters(
                        BitConverter.GetBytes(Controler[controlerIndex].ModbusAddress)[1], (ushort)(18 + (10 * d)), 10);
                    byte[] p = {
                        0,
                        BitConverter.GetBytes(temp[0])[0],
                        BitConverter.GetBytes(temp[1])[0],
                        BitConverter.GetBytes(temp[2])[0],
                        BitConverter.GetBytes(temp[3])[0],
                        BitConverter.GetBytes(temp[4])[0],
                        BitConverter.GetBytes(temp[5])[0],
                        BitConverter.GetBytes(temp[6])[0]
                    };
                    p.Reverse();
                    Controler[controlerIndex].Room[i].Pig[d].PigSerial = BitConverter.ToInt64(p, 0);
                    Controler[controlerIndex].Room[i].Pig[d].BatcherSum = temp[7];
                    Controler[controlerIndex].Room[i].Pig[d].WaterSum = temp[8];
                    Controler[controlerIndex].Room[i].Pig[d].Weight = temp[9];

                    PigData pigData = new PigData
                    {
                        PigSerial = Controler[controlerIndex].Room[i].Pig[d].PigSerial,
                        Date = DateTime.Now,
                        BatcherSum = Controler[controlerIndex].Room[i].Pig[d].BatcherSum,
                        WaterSum = Controler[controlerIndex].Room[i].Pig[d].WaterSum,
                        Weight = Controler[controlerIndex].Room[i].Pig[d].Weight,
                        DeviceAddress = Controler[controlerIndex].ModbusAddress,
                        RoomNum = i,
                        BatcherCalibration = Controler[controlerIndex].Room[i].BatcherCalibration,
                        Threshold = Controler[controlerIndex].Room[i].Threshold,
                    };

                    PigDataUpdateEvent(pigData);


                    Console.Write(i.ToString() + "," + d.ToString() + "," + Controler[controlerIndex].Room[i].Pig[d].PigSerial.ToString() + "," + Controler[controlerIndex].Room[i].Pig[d].Weight.ToString() + "\r\n");
                }
            }

        }
        private void AddControler(ushort[] vs)
        {
            ushort[] temp = vs;
            ControlerClass a = new ControlerClass
            {
                DeviceID = temp[0],
                RadioChannel = temp[8],
                SubnetAddress = temp[9],
                ModbusAddress = temp[10],
                Baundrate = temp[11],
                RoomCount = temp[12]
            };
            Controler.Add(a);
            Console.Write(Controler.Count);
            //Console.Write("Controler[0].RoomCount  "+ Controler[0].RoomCount.ToString()+"\r\n");
        }
        public void ControlerScan()
        {
            Master.Transport.ReadTimeout = 50;
            Controler.Clear();
            for (byte i=0;i<10;i++)
            {
                try
                {
                    ushort[] temp = Master.ReadInputRegisters(i, 1, 13);
                    AddControler(temp);
                }
                catch
                {
                    //Console.WriteLine("nothing!!");
                }
                finally
                {
                    DeviceScanProgressEvent(i);
                }
            }
            Master.Transport.ReadTimeout = 500;
        }
        public MyModbus()
        {
            
        }
        public MyModbus(string num)
        {
            ModbusInit(num);
        } 
    }


}
