using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace M_Pig.Controler
{
    class ControlerData
    {
        public InputRegs inputRegs{ get; set; }

}
    [StructLayoutAttribute(LayoutKind.Explicit, Pack = 1)]
    public struct InputRegs
    {
        [FieldOffset(0)]
        ushort DeviceID;
        [FieldOffset(8)]
        ushort RadioChannel;
        [FieldOffset(9)]
        ushort SubnetAddress;
        [FieldOffset(10)]
        ushort ModbusAddress;
        [FieldOffset(11)]
        ushort
    }
}
