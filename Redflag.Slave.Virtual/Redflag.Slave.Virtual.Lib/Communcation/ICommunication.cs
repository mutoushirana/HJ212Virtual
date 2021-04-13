using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redflag.Slave.Virtual.Lib.Communcation
{
    /// <summary>
    /// 通讯接口
    /// </summary>
    public abstract class ICommunication
    {
        /// <summary>
        /// 连接状态改变事件
        /// </summary>
        /// <returns></returns>
        public Action<bool> OnStateChange;

        /// <summary>
        /// 连接异常
        /// </summary>
        /// <returns></returns>
        public Action<Exception> OnError;

        /// <summary>
        /// 接收数据事件
        /// </summary>
        /// <returns></returns>
        public Action<string> OnDataReceive;

        /// <summary>
        /// 建立连接
        /// </summary>
        public abstract void Connect();

        /// <summary>
        /// 重连
        /// </summary>
        public abstract void ReConnect();

        /// <summary>
        /// 断开连接
        /// </summary>
        public abstract void Close();

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public abstract SendDataInfo SendData(SendDataInfo data);

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public abstract void SendData(string data);
    }
}
