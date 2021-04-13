using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redflag.Slave.Virtual.Lib.Protocol.HJ212.Config
{
    public class ParamCodeConfig
    {
        /// <summary>
        /// 最大值
        /// </summary>
        public double Max { get; set; }

        /// <summary>
        /// 最小值
        /// </summary>
        public double Min { get; set; }

        /// <summary>
        /// 基础值
        /// </summary>
        public double DefaultValue { get; set; }

        /// <summary>
        /// 参数编码
        /// </summary>
        public string ParamCode { get; set; }

        /// <summary>
        /// 是否有累计值
        /// </summary>
        public bool CouValue { get; set; }

        /// <summary>
        /// 数据标识
        /// </summary>
        public string DataFlag { get; set; }

        /// <summary>
        /// 开始采样时间 00000
        /// </summary>
        public string CstartTime { get; set; }

        /// <summary>
        /// 采样频率（小时）
        /// </summary>
        public int CTime { get; set; }

        /// <summary>
        /// 出样时间（分钟）
        /// </summary>
        public int Stime { get; set; }

        /// <summary>
        /// 设备序列号
        /// </summary>
        public string SN { get; set; }

        /// <summary>
        /// 参数信息
        /// </summary>
        public List<InfoConfig> Infos { get; set; }
    }
}
