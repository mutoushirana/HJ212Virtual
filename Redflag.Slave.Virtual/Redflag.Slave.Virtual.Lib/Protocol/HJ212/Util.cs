using Redflag.Slave.Virtual.Lib.Protocol.HJ212.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redflag.Slave.Virtual.Lib.Protocol.HJ212
{
    public class Util
    {
        private static Random random = new Random();

        /// <summary>
        /// 获取CRC校验码
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetHj212Crc16(string context)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(context);
            int crcRegister = 0xFFFF;
            for (int i = 0; i < bytes.Length; i++)
            {
                crcRegister = (crcRegister >> 8) ^ bytes[i];
                for (int j = 0; j < 8; j++)
                {
                    int check = crcRegister & 0x0001;
                    crcRegister >>= 1;
                    if (check == 0x0001)
                    {
                        crcRegister ^= 0xA001;
                    }
                }
            }

            string result = string.Format("{0:X}", crcRegister);
            for (int i = result.Length; i < 4; i++)
            {
                result = "0" + result;
            }
            return result;
        }

        /// <summary>
        /// 获取字符串长度
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string GetDataLenght(string data)
        {
            string length = data.Length.ToString();
            while (length.Length < 4)
            {
                length = '0' + length;
            }
            return length;
        }

        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static string GetTimeStamp()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssfff");
        }

        /// <summary>
        /// 获取随机值
        /// </summary>
        /// <param name="defualtValue"></param>
        /// <param name="maxValue"></param>
        /// <param name="minValue"></param>
        /// <returns></returns>
        public static double GetRandom(double defaultValue , double maxValue , double minValue)
        {
            double min = defaultValue - minValue;
            double span = maxValue + minValue;
            return min + random.NextDouble() * span;
        }

        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <param name="defualtValue"></param>
        /// <param name="minValue"></param>
        /// <returns></returns>
        public static double GetMin(double defaultValue, double minValue)
        {
            return defaultValue - random.NextDouble() * (defaultValue - minValue);
        }

        /// <summary>
        /// 获取随机值
        /// </summary>
        /// <param name="defualtValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public static double GetMax(double defaultValue, double maxValue)
        {
            return defaultValue + random.NextDouble() * (defaultValue + maxValue);
        }

        /// <summary>
        /// 获取日运行随机值
        /// </summary>
        /// <param name="defualtValue"></param>
        /// <param name="maxValue"></param>
        /// <param name="minValue"></param>
        /// <returns></returns>
        public static double GetRunHourRandom()
        {
            return Math.Round(24 * random.NextDouble(),1);
        }

        /// <summary>
        /// 获取命令类型
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static CommandType GetCommandType(MiddleCode code)
        {
            switch (code)
            {
                //设置超时时间及重发次数
                case MiddleCode.SetTimeOutReSendTimes:
                    return CommandType.Request;

                //提取现场机时间
                case MiddleCode.GetSceneDeviceTime:
                    return CommandType.Request;

                //设置现场机时间
                case MiddleCode.SetSceneDeviceTime:
                    return CommandType.Request;

                //现场机时间校准请求
                case MiddleCode.SceneDeviceTimeCalibration:
                    return CommandType.Notice;

                //提取实时数据间隔
                case MiddleCode.GetRtdDataInterval:
                    return CommandType.Request;

                //设置实时数据间隔
                case MiddleCode.SetRtdDataInterval:
                    return CommandType.Request;

                //提取分钟数据间隔
                case MiddleCode.GetMinuteDataInterval:
                    return CommandType.Request;

                //设置分钟数据间隔
                case MiddleCode.SetMinuteDataInterval:
                    return CommandType.Request;

                //用于设置现场机的密码
                case MiddleCode.SetSceneDevicePassword:
                    return CommandType.Request;

                //取污染物实时数据
                case MiddleCode.GetRtdData:
                    return CommandType.Request;

                //停止察看污染物实时数据
                case MiddleCode.StopRtdData:
                    return CommandType.Notice;

                //取设备运行状态数据
                case MiddleCode.GetDeviceRunState:
                    return CommandType.Request;


                //停止察看设备运行状态
                case MiddleCode.StopDeviceRunState:
                    return CommandType.Request;

                //取污染物日历史数据
                case MiddleCode.GetDayData:
                    return CommandType.Request;

                //取设备运行时间日历史数据
                case MiddleCode.GetDeviceRunTimeDayData:
                    return CommandType.Request;

                //取污染物分钟数据
                case MiddleCode.GetMinuteData:
                    return CommandType.Request;

                //取污染物小时数据
                case MiddleCode.GetHourData:
                    return CommandType.Request;

                //上传数采仪开机时间
                case MiddleCode.UploadComputerPowerOnTime:
                    return CommandType.Upload;

                //零点校准量程校准
                case MiddleCode.RangeCalibration:
                    return CommandType.Request;

                //即时采样
                case MiddleCode.TakeSampleImmediately:
                    return CommandType.Request;

                //启动清洗/反吹
                case MiddleCode.StartClear:
                    return CommandType.Request;

                //比对采样
                case MiddleCode.CompareSample:
                    return CommandType.Request;

                //超标留样
                case MiddleCode.LeaveSuperstandardSample:
                    return CommandType.Request;

                //设置采样时间周期
                case MiddleCode.SetSampleTimeInterval:
                    return CommandType.Request;

                //提取采样时间周期
                case MiddleCode.GetSampleTimeInterval:
                    return CommandType.Request;

                //提取出样时间
                case MiddleCode.GetSampleTime:
                    return CommandType.Request;

                //提取设备唯一标识
                case MiddleCode.GetSceneDeviceUUID:
                    return CommandType.Request;

                //提取现场机信息
                case MiddleCode.GetSceneDeviceInfo:
                    return CommandType.Request;

                //设置现场机参数
                case MiddleCode.SetSceneDeviceParam:
                    return CommandType.Request;

                //取污染物周期数据
                case MiddleCode.GetCycleData:
                    return CommandType.Request;

                //请求应答
                case MiddleCode.RequestResponse:
                    return CommandType.Other;

                //执行结果
                case MiddleCode.ExecuteResponse:
                    return CommandType.Other;

                //通知应答
                case MiddleCode.NoticeResponse:
                    return CommandType.Other;

                //数据应答
                case MiddleCode.DataResponse:
                    return CommandType.Other;

                //心跳
                case MiddleCode.HeartBeat:
                    return CommandType.Other;

                default:
                    return CommandType.None;
            }
        }
    }
}
