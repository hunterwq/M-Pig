using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace M_Pig.Controler
{
    class ControlerClass
    {
        public InputRegs inputRegs{ get; set; }

        public ushort DeviceID { get; set; }
        public ushort RadioChannel { get; set; }
        public ushort SubnetAddress { get; set; }
        public ushort ModbusAddress { get; set; }
        public ushort Baundrate { get; set; }
        public ushort RoomCount { get; set; }
        public RoomClass[] Room { get; set; } = new RoomClass[30];

    }
    class RoomClass
    {
        public ushort RoomNum { get; set; }
        public ushort BatcherCalibration { get; set; }
        public ushort Threshold { get; set; }
        public ushort PigsCount { get; set; }
        public PigClass[] Pig { get; set; } = new PigClass[30];
    }
    class PigClass
    {
        public string PigSerial { get; set; }
        public ushort BatcherSum { get; set; }
        public ushort WaterSum { get; set; }
        public ushort Weight { get; set; }

    }
    [StructLayoutAttribute(LayoutKind.Explicit, Pack = 1)]
    public struct InputRegs
    {
        [FieldOffset(0 * 2)]
        public ushort DeviceID;
        [FieldOffset(8 * 2)]
        public ushort RadioChannel;
        [FieldOffset(9 * 2)]
        public ushort SubnetAddress;
        [FieldOffset(10 * 2)]
        public ushort ModbusAddress;
        [FieldOffset(11 * 2)]
        public ushort Baundrate;
        [FieldOffset(12 * 2)]
        public ushort RoomCount;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 30, ArraySubType = UnmanagedType.Struct)]
        [FieldOffset(13 * 2)]
        public Room[] rooms;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 9131, ArraySubType = UnmanagedType.U2)]
        [FieldOffset(0)]
        public ushort[] data;

    }

    [StructLayoutAttribute(LayoutKind.Explicit, Pack = 1)]
    public struct Room
    {
        [FieldOffset(0 * 2)]
        public ushort RoomNum;
        [FieldOffset(1 * 2)]
        public ushort BatcherCalibration;
        [FieldOffset(2 * 2)]
        public ushort Threshold;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 30, ArraySubType = UnmanagedType.Struct)]
        [FieldOffset(3 * 2)]
        public Pig[] pigs;
    }
    [StructLayoutAttribute(LayoutKind.Explicit, Pack = 1)]
    public struct Pig
    {
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 7, ArraySubType = UnmanagedType.U2)]
        [FieldOffset(0 * 2)]
        public ushort[] PigSerial;
        [FieldOffset(7 * 2)]
        public ushort BatcherSum;
        [FieldOffset(8 * 2)]
        public ushort WaterSum;
        [FieldOffset(9 * 2)]
        public ushort Weight;
    }
}
