using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redflag.Slave.Virtual.Lib.Protocol.HJ212.Enum
{
    public enum ExecuteResult
    {
        //执行成功
        Success = 1,

        //执行失败，但不知道原因
        Fail = 2,

        //命令请求条件错误
        CommandError = 3,

        //通讯超时
        TimeOut = 4,

        //系统故障
        SystemBusy = 5,

        //系统故障
        SystemError = 6,

        //未知错误
        NoData = 100
    }
}
