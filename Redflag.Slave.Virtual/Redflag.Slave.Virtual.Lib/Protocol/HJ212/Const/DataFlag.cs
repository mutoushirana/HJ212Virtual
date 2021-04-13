using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redflag.Slave.Virtual.Lib.Protocol.HJ212.Const
{
    public class DataFlag
    {
        //在线监控（监测）仪器仪表工作正常
        public static string Normal = "N";

        //在线监控（监测）仪器仪表停运
        public static string Stop = "F";

        //在线监控（监测）仪器仪表处于维护期间产生的数据
        public static string Maintain = "M";

        //手工输入的设定值
        public static string Hand = "S";

        //在线监控（监测）仪器仪表故障
        public static string Fault = "D";

        //在线监控（监测）仪器仪表处于校准状态
        public static string Calibration = "C";

        //在线监控（监测）仪器仪表采样数值超过测量上限
        public static string Exceed = "T";

        //在线监控（监测）仪器仪表与数采仪通讯异常
        public static string CommuncationException = "B";
    }
}
