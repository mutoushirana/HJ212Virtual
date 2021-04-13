using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Redflag.Slave.Virtual.Lib.Protocol.HJ212.Win32
{
   
    public class Win32Util
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetSystemTime(ref SYSTEMTIME time);
        public struct SYSTEMTIME
        {
            public short Year;
            public short Month;
            public short DayOfWeek;
            public short Day;
            public short Hour;
            public short Minute;
            public short Second;
            public short Milliseconds;
        }

        public static bool SetSystemTime(DateTime dateTime)
        {
            SYSTEMTIME s = new SYSTEMTIME();
            s.Year = (short)dateTime.Year;
            s.Month = (short)dateTime.Month;
            s.DayOfWeek = (short)dateTime.DayOfWeek;
            s.Day = (short)dateTime.Day;
            s.Hour = (short)dateTime.Hour;
            s.Minute = (short)dateTime.Minute;
            s.Second = (short)dateTime.Second;
            s.Milliseconds = (short)dateTime.Millisecond;
            return SetSystemTime(ref s);
        }

    }
  
    
}
