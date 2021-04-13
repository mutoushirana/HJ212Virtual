using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redflag.Slave.Virtual.Lib.Protocol.HJ212.Enum
{
    public enum CommandResult
    {

        //准备执行请求
        Ready = 1,

        //请求被拒绝
        Forbidden = 2,

        //PW 错误
        PWError = 3,

        //MN 错误
        MNError = 4,

        //ST 错误
        STError = 5,

        //Flag 错误
        FlagError = 6,

        //QN 错误
        QNError = 7,

        //CN 错误
        CNError = 8,

        //CRC 校验错误
        CRCError = 9,

        //未知错误
        UndefinedError = 100
    }
}
