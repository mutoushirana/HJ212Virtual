using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redflag.Slave.Virtual.Lib.Protocol
{
    /// <summary>
    /// 协议接口
    /// </summary>
    public abstract class IProtocol
    {
        /// <summary>
        /// 协议名称
        /// </summary>
        public string ProtocolName;

        /// <summary>
        /// 开始任务
        /// </summary>
        public abstract void BeginTask();

        /// <summary>
        /// 数据接收
        /// </summary>
        public abstract void DataReceive(string data);

        /// <summary>
        /// 连接中断
        /// </summary>
        public abstract void ConnectStop();

        /// <summary>
        /// 数据发送结果
        /// </summary>
        public abstract void OnSendResult(SendDataInfo data);

        /// <summary>
        /// </summary>
        public Action<SendDataInfo> OnDataSend;

    }
}
