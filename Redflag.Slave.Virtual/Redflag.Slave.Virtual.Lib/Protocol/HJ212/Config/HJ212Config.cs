using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redflag.Slave.Virtual.Lib.Protocol.HJ212.Config
{
    public class HJ212Config
    {
        /// <summary>
        /// 系统编码
        /// </summary>
        public string ST { get; set; }

        /// <summary>
        /// 访问密码
        /// </summary>
        public string PW { get; set; }

        /// <summary>
        /// 设备唯一标识
        /// </summary>
        public string MN { get; set; }

        /// <summary>
        /// 拆分包及应答标志
        /// </summary>
        public string Flag { get; set; }

        /// <summary>
        /// 参数列表
        /// </summary>
        public IEnumerable<ParamCodeConfig> ParamCodes { get; set; }

        /// <summary>
        /// 设备列表
        /// </summary>
        public IEnumerable<DeviceConfig> Devices { get; set; }

        /// <summary>
        /// 是否发送心跳包
        /// </summary>
        public bool HeartBeat { get; set; }

        /// <summary>
        /// 心跳包发送间隔（秒）
        /// </summary>
        public int HeartBeatInterval { get; set; }

        /// <summary>
        /// 是否发送实时数据
        /// </summary>
        public bool RtdData { get; set; }

        /// <summary>
        /// 实时数据发送间隔（秒）
        /// </summary>
        public int RtdInterval { get; set; }

        /// <summary>
        /// 是否发送分钟数据
        /// </summary>
        public bool MinuteData { get; set; }

        /// <summary>
        /// 分钟数据发送间隔（分钟）
        /// </summary>
        public int MinInterval { get; set; }

        /// <summary>
        /// 是否发送小时数据
        /// </summary>
        public bool HourData { get; set; }

        /// <summary>
        /// 在每个小时第几分钟发送分钟数据（分钟）
        /// </summary>
        public int HourDataDelay { get; set; }

        /// <summary>
        /// 是否发送周期数据
        /// </summary>
        public bool CycleData { get; set; }

        /// <summary>
        /// 周期数据发送间隔（小时）
        /// </summary>
        public int CycleInterval { get; set; }

        /// <summary>
        /// 在每个周期的第几分钟发送周期数据（分钟）
        /// </summary>
        public int CycleDataDelay { get; set; }

        /// <summary>
        /// 是否发送日数据
        /// </summary>
        public bool DayData { get; set; }

        /// <summary>
        /// 在每天的第几分钟发送周期数据（分钟）
        /// </summary>
        public int DayDataDelay { get; set; }

        /// <summary>
        /// 是否发送设备状态
        /// </summary>
        public bool DeviceRunState { get; set; }

        /// <summary>
        /// 设备状态发送间隔
        /// </summary>
        public int DeviceRunStateInterval { get; set; }

        /// <summary>
        /// 消息超时时间
        /// </summary>
        public int OverTime { get; set; }

        /// <summary>
        /// 重发次数
        /// </summary>
        public int ReCount { get; set; }

        /// <summary>
        /// 是否校验CRC
        /// </summary>
        public bool CheckCRC { get; set; }

        /// <summary>
        /// 留样瓶编号
        /// </summary>
        public int VaseNo { get; set; }

        /// <summary>
        /// 消息发送间隔（毫秒）
        /// </summary>
        public int MessageInterval { get; set; }

        /// <summary>
        /// 是否上传开机时间
        /// </summary>
        public bool RestartTime { get; set; }

        /// <summary>
        /// 是否校时
        /// </summary>
        public bool SceneDeviceTimeCalibration { get; set; }
        

    }
}
