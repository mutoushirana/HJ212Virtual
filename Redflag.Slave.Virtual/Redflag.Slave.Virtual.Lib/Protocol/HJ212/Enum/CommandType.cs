using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redflag.Slave.Virtual.Lib.Protocol.HJ212.Enum
{
    public enum CommandType
    {
        /// <summary>
        /// 请求命令
        /// </summary>
        Request = 1,

        /// <summary>
        /// 上传命令
        /// </summary>
        Upload = 2,

        /// <summary>
        /// 通知命令
        /// </summary>
        Notice = 3,

        /// <summary>
        /// 其他
        /// </summary>
        Other = 4,

        /// <summary>
        /// 不支持
        /// </summary>
        None = 5
    }
}
