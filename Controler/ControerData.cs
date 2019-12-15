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
        //public InputRegs inputRegs{ get; set; }

        public ushort DeviceID { get; set; } = new ushort();
        public ushort RadioChannel { get; set; } = new ushort();
        public ushort SubnetAddress { get; set; } = new ushort();
        public ushort ModbusAddress { get; set; } = new ushort();
        public ushort Baundrate { get; set; } = new ushort();
        public ushort RoomCount { get; set; } = new ushort();
        public RoomClass[] Room { get; set; } = new RoomClass[30];
        public ControlerClass()
        {
            for (uint i = 0; i < 30; i++)
                Room[i] = new RoomClass();
        }

    }
    class RoomClass
    {
        public ushort RoomNum { get; set; } = new ushort();
        public ushort BatcherCalibration { get; set; } = new ushort();
        public ushort Threshold { get; set; } = new ushort();
        public ushort PigsCount { get; set; } = new ushort();
        public PigClass[] Pig { get; set; } = new PigClass[30];
        public RoomClass()
        {
            for (uint i = 0; i < 30; i++)
                Pig[i] = new PigClass();
        }
    }
    class PigClass
    {
        public long PigSerial { get; set; } = new long();
        public ushort BatcherSum { get; set; } = new ushort();
        public ushort WaterSum { get; set; } = new ushort();
        public ushort Weight { get; set; } = new ushort();

    }
    
    
    
    
    /*
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
    }*/
}
