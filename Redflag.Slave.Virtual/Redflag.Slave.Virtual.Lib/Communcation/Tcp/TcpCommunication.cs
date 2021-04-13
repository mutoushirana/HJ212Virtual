using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Redflag.Slave.Virtual.Lib.Communcation
{
    public class TcpCommunication : ICommunication
    {
        private TcpClient client;
        private string host;
        private int port;
        private NetworkStream networkStream;
        private bool connectionState = false;

        public TcpCommunication(string host, int port)
        {
            this.host = host;
            this.port = port;
        }

        public override void Connect()
        {
            client = new TcpClient();
            client.BeginConnect(host, port, new AsyncCallback(AsynConnect), client);
        }

        public override void ReConnect()
        {
            client.BeginConnect(host, port, new AsyncCallback(AsynConnect), client);
        }

        public override void Close()
        {
            if (null != client)
            {
                client.Close();
                connectionState = false;
                OnStateChange?.Invoke(false);
            }
        }

        public override SendDataInfo SendData(SendDataInfo data)
        {
            data.SendTimes += 1;
            try
            {
                if (connectionState)
                {
                    if (networkStream.CanWrite && !string.IsNullOrEmpty(data.Content))
                    {
                        byte[] buff = Encoding.UTF8.GetBytes(data.Content);
                        networkStream.Write(buff, 0, buff.Length);
                        networkStream.Flush();
                        return null;
                    }
                }
                else
                {
                    client.BeginConnect(host, port, new AsyncCallback(AsynConnect), client);
                }
                return data;
            }
            catch (Exception ex)
            {
                OnError?.Invoke(ex);
                return null;
            }
        }

        public override void SendData(string data)
        {
            if (connectionState)
            {
                if (networkStream.CanWrite && !string.IsNullOrEmpty(data))
                {
                    byte[] buff = Encoding.UTF8.GetBytes(data);
                    networkStream.Write(buff, 0, buff.Length);
                    networkStream.Flush();
                }
            }
            else
            {
                client.BeginConnect(host, port, new AsyncCallback(AsynConnect), client);
            }
        }


        private void AsynConnect(IAsyncResult iAsyncResult)
        {
            try
            {
                client.EndConnect(iAsyncResult);
                networkStream = client.GetStream();
                byte[] buff = new byte[1024 * 4];
                networkStream.BeginRead(buff, 0, buff.Length, new AsyncCallback(AsynReceiveData), buff);
                connectionState = true;
                OnStateChange?.Invoke(true);
            }
            catch (Exception ex)
            {
                OnError?.Invoke(ex);
            }
            
        }

        private void AsynReceiveData(IAsyncResult iAsyncResult)
        {
            try
            {
                byte[] buff = iAsyncResult.AsyncState as byte[];
                if (null != buff)
                {
                    string context = Encoding.UTF8.GetString(buff);
                    OnDataReceive?.Invoke(context);
                }
                buff = new byte[1024 * 4];
                networkStream.BeginRead(buff, 0, buff.Length, new AsyncCallback(AsynReceiveData), buff);
            }
            catch(Exception ex)
            {
                OnError?.Invoke(ex);
            }
           
        }


    }
}
