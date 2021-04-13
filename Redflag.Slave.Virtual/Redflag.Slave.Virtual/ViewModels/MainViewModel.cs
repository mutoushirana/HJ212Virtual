using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redflag.Slave.Virtual.Lib;
using Redflag.Slave.Virtual.Lib.Communcation;
using Redflag.Slave.Virtual.Lib.Protocol;
using Redflag.Slave.Virtual.Lib.Protocol.HJ212.Model;

namespace Redflag.Slave.Virtual.ViewModels
{
    public class MainViewModel : Screen, INotifyPropertyChanged
    {
        private ICommunication communication;
        private IProtocol protocol;
        private string connectionColor = "Red";
        private string connectionContext = "连  接";
        private string needSendText;
        private string acceptText;
        private string ip = "127.0.0.1";
        private string port = "8401";
        private bool sendTextEnable = false;
        private log4net.ILog logger = log4net.LogManager.GetLogger(typeof(MainViewModel));
        private IList<SendDataInfo> sendDataInfoList = new List<SendDataInfo>();
        public string Ip
        {
            get
            {
                return ip;
            }
            set
            {
                ip = value;
                NotifyOfPropertyChange();
            }
        }

        public string Port
        {
            get
            {
                return port;
            }
            set
            {
                port = value;
                NotifyOfPropertyChange();
            }
        }

        public string ConnectionColor
        {
            get
            {
                return connectionColor;
            }
            set
            {
                connectionColor = value;
                NotifyOfPropertyChange();
            }
        }

        public string ConnectionContext
        {
            get
            {
                return connectionContext;
            }
            set
            {
                connectionContext = value;
                NotifyOfPropertyChange();
            }
        }

        public string NeedSendText
        {
            get
            {
                return needSendText;
            }
            set
            {
                needSendText = value;
                NotifyOfPropertyChange();
            }
        }

        public string AcceptText
        {
            get
            {
                return acceptText;
            }
            set
            {
                acceptText = value;
                NotifyOfPropertyChange();
            }
        }

        public bool SendTextEnable
        {
            get
            {
                return sendTextEnable;
            }
            set
            {
                sendTextEnable = value;
                NotifyOfPropertyChange();
            }
        }

        public void Connect()
        {
            try
            {

                log4net.Config.XmlConfigurator.Configure();
                string host = Ip;
                int port = Convert.ToInt16(Port);
                communication = new TcpCommunication(host, port);
                protocol = new HJ212Protocol();
                protocol.OnDataSend += SendData;
                communication.OnStateChange += state =>
                {
                    if (state)
                    {
                        ConnectionColor = "Green";
                        ConnectionContext = "断开";
                        SendTextEnable = true;
                        protocol.BeginTask();
                    }
                    else
                    {
                        ConnectionColor = "Red";
                        ConnectionContext = "连接";
                        SendTextEnable = false;
                        protocol.ConnectStop();
                    }
                };

                communication.OnDataReceive += data =>
                {
                    try
                    {
                        AcceptText += "【" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "】收到数据：" + data;
                        logger.Info("【" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "】收到数据：" + data);
                        protocol.DataReceive(data);
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex);
                    }

                };
                communication.OnError += err =>
                 {
                     logger.Error(err);
                     AcceptText += "【" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "】异常：" + err.Message;
                     ConnectionColor = "Red";
                     ConnectionContext = "连接";
                     SendTextEnable = false;
                     protocol.ConnectStop();
                 };
                
                communication.Connect();
                
            }
            catch (Exception ex)
            {
                ConnectionColor = "Red";
                ConnectionContext = "连接";
                SendTextEnable = false;
                logger.Error(ex);
            }
        }

        public void SendText()
        {
            try
            {
                if (sendTextEnable)
                {
                    //AcceptText += "【" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "】发送数据：" + NeedSendText;
                    logger.Info("【" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "】发送数据：" + NeedSendText);
                    communication.SendData(NeedSendText);
                }
               
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

        }

        private void SendData(SendDataInfo info)
        {
            if (sendTextEnable)
            {
                //AcceptText += "【" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "】发送数据：" + info.Content;
                logger.Info("【" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "】发送数据：" + info.Content);
                SendDataInfo res = communication.SendData(info);
                if (null != res)
                {
                    protocol.OnSendResult(res);
                }
            }
        }
    }
}
