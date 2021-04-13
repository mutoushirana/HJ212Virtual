using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redflag.Slave.Virtual.Lib.Protocol.HJ212.Config
{
    public class DeviceConfig
    {
        /// <summary>
        /// 设备编号
        /// </summary>
        public int DeviceNumber { get; set; }

        /// <summary>
        /// 设备状态
        /// </summary>
        public int DeviceRunState { get; set; }
    }
}
