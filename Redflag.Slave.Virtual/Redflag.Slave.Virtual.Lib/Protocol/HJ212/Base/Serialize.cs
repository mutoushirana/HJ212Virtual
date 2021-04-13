using Redflag.Slave.Virtual.Lib.Protocol.HJ212.Config;
using Redflag.Slave.Virtual.Lib.Protocol.HJ212.Enum;
using Redflag.Slave.Virtual.Lib.Protocol.HJ212.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Redflag.Slave.Virtual.Lib.Protocol.HJ212.Base
{
    public class Serialize
    {

        /// <summary>
        /// 上传现场机时间
        /// </summary>
        /// <param name="config"></param>
        /// <param name="qn"></param>
        /// <returns></returns>
        public static string UploadSceneDeviceTime(HJ212Config config, string QN,string PolId, DateTime SystemTime)
        {
            string CP = string.Format("PolId={0};SystemTime={1}", PolId, SystemTime.ToString("yyyyMMddHHmmss"));
            return MakeContent(config, MiddleCode.UploadSceneDeviceTime, CP, QN);
        }

        /// <summary>
        /// 上传设备运行状态数据
        /// </summary>
        /// <param name="config"></param>
        /// <param name="qn"></param>
        /// <returns></returns>
        public static string UploadDeviceRunState(HJ212Config config, string QN, DateTime datatime, IEnumerable<DeviceRunStateModel> datas)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(string.Format("DataTime={0}",datatime.ToString("yyyyMMddHHmm00")));
            foreach (DeviceRunStateModel data in datas)
            {
                builder.Append(string.Format(";SB{0}-RS={1}", data.DeviceNumber, data.DeviceRunState));
            }
            return MakeContent(config, MiddleCode.UploadDeviceRunState, builder.ToString(), QN);
        }

        /// <summary>
        /// 上传设备运行时间日历史数据
        /// </summary>
        /// <param name="config"></param>
        /// <param name="qn"></param>
        /// <returns></returns>
        public static string UploadDeviceRunTimeDayData(HJ212Config config, string QN, DateTime datatime, IEnumerable<DeviceRunTimeModel> datas)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(datatime.ToString("yyyyMMdd000000"));
            foreach (DeviceRunTimeModel data in datas)
            {
                builder.Append(string.Format(";SB{0}-RT={1}", data.DeviceNumber, data.DeviceRunTime));
            }
            return MakeContent(config, MiddleCode.UploadDeviceRunTimeDayData, builder.ToString(), QN);
        }

        /// <summary>
        /// 现场机时间校准请求
        /// </summary>
        /// <param name="config"></param>
        /// <param name="qn"></param>
        /// <param name="PolId"></param>
        /// <returns></returns>
        public static string SceneDeviceTimeCalibration(HJ212Config config, string QN, string PolId)
        {
            string CP = string.Format("PolId={0}", PolId);
            return MakeContent(config, MiddleCode.SceneDeviceTimeCalibration, CP, QN);
        }

        /// <summary>
        /// 上传实时数据间隔
        /// </summary>
        /// <param name="config"></param>
        /// <param name="qn"></param>
        /// <returns></returns>
        public static string UploadRtdDataInterval(HJ212Config config, string QN, int RtdInterval)
        {
            string CP = string.Format("RtdInterval={0}", RtdInterval);
            return MakeContent(config, MiddleCode.UploadRtdDataInterval, CP, QN);
        }

        /// <summary>
        /// 上传分钟数据间隔
        /// </summary>
        /// <param name="config"></param>
        /// <param name="qn"></param>
        /// <returns></returns>
        public static string UploadMinuteDataInterval(HJ212Config config, string QN, int RtdInterval)
        {
            string CP = string.Format("MinInterval={0}", RtdInterval);
            return MakeContent(config, MiddleCode.UploadMinuteDataInterval, CP, QN);
        }

        /// <summary>
        /// 上传数采仪开机时间
        /// </summary>
        /// <param name="config"></param>
        /// <param name="qn"></param>
        /// <returns></returns>
        public static string UploadComputerPowerOnTime(HJ212Config config, string QN, DateTime DataTime, DateTime RestartTime)
        {
            string CP = string.Format("DataTime={0};RestartTime={1}", DataTime.ToString("yyyyMMddHHmmss"), RestartTime.ToString("yyyyMMddHHmmss"));
            return MakeContent(config, MiddleCode.UploadComputerPowerOnTime, CP, QN);
        }

        /// <summary>
        /// 上传超标留样信息
        /// </summary>
        /// <param name="config"></param>
        /// <param name="qn"></param>
        /// <returns></returns>
        public static string UploadSuperstandardSample(HJ212Config config, string QN, DateTime DataTime, int VaseNo)
        {
            string CP = string.Format("DataTime={0};VaseNo={1}", DataTime.ToString("yyyyMMddHHmmss"), VaseNo);
            return MakeContent(config, MiddleCode.UploadSuperstandardSample, CP, QN);
        }

        /// <summary>
        /// 上传采样时间周期
        /// </summary>
        /// <param name="config"></param>
        /// <param name="qn"></param>
        /// <returns></returns>
        public static string UploadSampleTimeInterval(HJ212Config config, string QN, string PolId, string CstartTime, int CTime)
        {
            string CP = string.Format("PolId={0};CstartTime={1};CTime={2}", PolId, CstartTime, CTime);
            return MakeContent(config, MiddleCode.UploadSampleTimeInterval, CP, QN);
        }

        /// <summary>
        /// 上传出样时间
        /// </summary>
        /// <param name="config"></param>
        /// <param name="qn"></param>
        /// <returns></returns>
        public static string UploadSampleTime(HJ212Config config, string QN, string PolId, int Stime)
        {
            string CP = string.Format("PolId={0};Stime={1}", PolId, Stime);
            return MakeContent(config, MiddleCode.UploadSampleTime, CP, QN);
        }

        /// <summary>
        /// 上传设备唯一标识
        /// </summary>
        /// <param name="config"></param>
        /// <param name="qn"></param>
        /// <returns></returns>
        public static string UploadSceneDeviceUUID(HJ212Config config, string QN, string PolId, string SN)
        {
            string CP = string.Format("PolId={0};{0}-SN={1}", PolId, SN);
            return MakeContent(config, MiddleCode.UploadSceneDeviceUUID, CP, QN);
        }

        /// <summary>
        /// 上传现场机信息
        /// </summary>
        /// <param name="config"></param>
        /// <param name="qn"></param>
        /// <returns></returns>
        public static string UploadSceneDeviceInfo(HJ212Config config, string QN, string PolId, string InfoId, object Value, DateTime DataTime)
        {
            string CP = string.Format("DataTime={0};PolId={1};{2}-Info={3}", DataTime.ToString("yyyyMMddHHmmss"), PolId, InfoId, Value);
            return MakeContent(config, MiddleCode.UploadSceneDeviceInfo, CP, QN);
        }

        /// <summary>
        /// 上传污染物实时数据
        /// </summary>
        /// <param name="config"></param>
        /// <param name="qn"></param>
        /// <param name="datatime"></param>
        /// <param name="datas"></param>
        /// <returns></returns>
        public static IEnumerable<string> UploadRtdData(HJ212Config config, string qn, DateTime datatime, IDictionary<string, DataModel> datas)
        {
            List<string> contentList = new List<string>();
            IEnumerable<string> cpList = SerializeData(datatime.ToString("yyyyMMddHHmm00"), datas);
            foreach (string cp in cpList)
            {
                contentList.Add(MakeContent(config, MiddleCode.UploadRtdData, cp, qn));
            }
            return contentList;
        }

        /// <summary>
        /// 上传污染物分钟数据
        /// </summary>
        /// <param name="config"></param>
        /// <param name="qn"></param>
        /// <param name="datatime"></param>
        /// <param name="datas"></param>
        /// <returns></returns>
        public static IEnumerable<string> UploadMinuteData(HJ212Config config, string qn, DateTime datatime, IDictionary<string, DataModel> datas)
        {
            List<string> contentList = new List<string>();
            IEnumerable<string> cpList = SerializeData(datatime.ToString("yyyyMMddHHmm00"), datas);
            foreach (string cp in cpList)
            {
                contentList.Add(MakeContent(config, MiddleCode.UploadMinuteData, cp, qn));
            }
            return contentList;
        }

        /// <summary>
        /// 上传污染物小时数据
        /// </summary>
        /// <param name="config"></param>
        /// <param name="qn"></param>
        /// <param name="datatime"></param>
        /// <param name="datas"></param>
        /// <returns></returns>
        public static IEnumerable<string> UploadHourData(HJ212Config config, string qn, DateTime datatime, IDictionary<string, DataModel> datas)
        {
            List<string> contentList = new List<string>();
            IEnumerable<string> cpList = SerializeData(datatime.ToString("yyyyMMddHH0000"), datas);
            foreach (string cp in cpList)
            {
                contentList.Add(MakeContent(config, MiddleCode.UploadHourData, cp, qn));
            }
            return contentList;
        }

        /// <summary>
        /// 上传污染物日历史数据
        /// </summary>
        /// <param name="config"></param>
        /// <param name="qn"></param>
        /// <param name="datatime"></param>
        /// <param name="datas"></param>
        /// <returns></returns>
        public static IEnumerable<string> UploadDayData(HJ212Config config, string qn, DateTime datatime, IDictionary<string, DataModel> datas)
        {
            List<string> contentList = new List<string>();
            IEnumerable<string> cpList = SerializeData(datatime.ToString("yyyyMMdd000000"), datas);
            foreach (string cp in cpList)
            {
                contentList.Add(MakeContent(config, MiddleCode.UploadDayData, cp, qn));
            }
            return contentList;
        }

        /// <summary>
        /// 上传污染物周期历史数据
        /// </summary>
        /// <param name="config"></param>
        /// <param name="qn"></param>
        /// <param name="datatime"></param>
        /// <param name="datas"></param>
        /// <returns></returns>
        public static IEnumerable<string> UploadCycleData(HJ212Config config, string qn, DateTime datatime, IDictionary<string, DataModel> datas)
        {
            List<string> contentList = new List<string>();
            IEnumerable<string> cpList = SerializeData(datatime.ToString("yyyyMMddHH0000"), datas);
            foreach (string cp in cpList)
            {
                contentList.Add(MakeContent(config, MiddleCode.UploadCycleData, cp, qn));
            }
            return contentList;
        }

        /// <summary>
        /// 通知应答
        /// </summary>
        /// <param name="config"></param>
        /// <param name="qn"></param>
        /// <returns></returns>
        public static string NoticeResponse(HJ212Config config, string qn)
        {
            return MakeContent(config, MiddleCode.NoticeResponse, string.Empty, qn);
        }

        /// <summary>
        /// 数据应答
        /// </summary>
        /// <param name="config"></param>
        /// <param name="qn"></param>
        /// <returns></returns>
        public static string DataResponse(HJ212Config config, string qn)
        {
            return MakeContent(config, MiddleCode.DataResponse, string.Empty, qn);
        }

        /// <summary>
        /// 请求应答
        /// </summary>
        /// <param name="config"></param>
        /// <param name="qn"></param>
        /// <returns></returns>
        public static string RequestResponse(HJ212Config config, string qn, CommandResult res)
        {
            string cp = string.Format("QnRtn={0}", (int)res);
            return MakeContent(config, MiddleCode.RequestResponse, cp, qn);
        }

        /// <summary>
        /// 执行结果
        /// </summary>
        /// <param name="config"></param>
        /// <param name="qn"></param>
        /// <returns></returns>
        public static string ExecuteResponse(HJ212Config config, string qn, ExecuteResult res)
        {
            string cp = string.Format("ExeRtn={0}", (int)res);
            return MakeContent(config, MiddleCode.ExecuteResponse, cp, qn);
        }

        /// <summary>
        /// 心跳包
        /// </summary>
        /// <param name="config"></param>
        /// <param name="qn"></param>
        /// <returns></returns>
        public static string HeartBeat(HJ212Config config, string qn)
        {
            return MakeContent(config, MiddleCode.HeartBeat, string.Empty, qn);
        }


        /// <summary>
        /// 序列化数据
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        private static IEnumerable<string> SerializeData(string dataTime, IDictionary<string, DataModel> datas)
        {
            List<string> cpList = new List<string>();
            string cp = string.Format("DataTime={0}",dataTime);
            foreach (KeyValuePair<string, DataModel> data in datas)
            {
                PropertyInfo[] propertyInfos = data.Value.GetType().GetProperties();
                StringBuilder builder = new StringBuilder();
                bool first = true;
                foreach (PropertyInfo info in propertyInfos)
                {
                    if (info.PropertyType == typeof(double?))
                    {
                        object value = info.GetValue(data.Value);
                        if (null != value)
                        {
                            if (first)
                            {
                                builder.Append(string.Format("{0}-{1}={2}", data.Key, info.Name, value));
                                first = false;
                            }
                            else
                            {
                                builder.Append(string.Format(",{0}-{1}={2}", data.Key, info.Name, value));
                            }

                        }
                    }
                    if (info.PropertyType == typeof(string))
                    {
                        object value = info.GetValue(data.Value);
                        if (null != value && !string.IsNullOrEmpty(value.ToString()))
                        {
                            if (first)
                            {
                                builder.Append(string.Format("{0}-{1}={2}", data.Key, info.Name, value));
                                first = false;
                            }
                            else
                            {
                                builder.Append(string.Format(",{0}-{1}={2}", data.Key, info.Name, value));
                            }
                        }
                    }
                }
                if (cp.Length + builder.ToString().Length > 942)
                {
                    cpList.Add(cp);
                    cp = string.Empty;
                }
                else
                {
                    cp = cp + ';' + builder.ToString();
                }
                builder = new StringBuilder();
            }
            cpList.Add(cp);
            return cpList;
        }

        /// <summary>
        /// 组成212协议
        /// </summary>
        /// <param name="config"></param>
        /// <param name="code"></param>
        /// <param name="cp"></param>
        /// <param name="qn"></param>
        /// <returns></returns>
        private static string MakeContent(HJ212Config config, MiddleCode code, string cp, string qn)
        {
            string rdata = string.Format("QN={0};ST={1};CN={2};PW={3};MN={4};Flag={5};CP=&&{6}&&", qn, config.ST, (int)code, config.PW, config.MN, config.Flag, cp);
            string dataLength = Util.GetDataLenght(rdata);
            string crc = Util.GetHj212Crc16(rdata);
            return string.Format("##{0}{1}{2}\r\n", dataLength, rdata, crc);
        }
    }
}
