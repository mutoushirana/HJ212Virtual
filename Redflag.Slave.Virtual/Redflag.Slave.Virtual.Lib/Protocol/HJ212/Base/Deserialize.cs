using Redflag.Slave.Virtual.Lib.Protocol.HJ212.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redflag.Slave.Virtual.Lib.Protocol.HJ212.Base
{
    public class Deserialize
    {
        /// <summary>
        /// 基础反序列化方法
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static HJ212Model DeserializeBase(string context)
        {
            HJ212Model model = new HJ212Model();
            string begin = context.Substring(0, 2);
            string end = context.Substring(context.Length - 2 - 1, 2);
            string dataLenght = context.Substring(2, 4);
            model.CRC = context.Substring(context.Length - 2 - 4, 4);
            model.Body = context.Substring(6, context.Length - 2 - 4 - 4 - 2);
            string[] bodyArray = model.Body.Replace("&&", "&").Split('&');
            string[] bodyHeadArray = bodyArray[0].Split(';');
            model.CP = bodyArray[1];
            foreach (string bodyHeader in bodyHeadArray)
            {
                string[] headArray = bodyHeader.Split('=');
                switch (headArray[0])
                {
                    case "QN":
                        model.QN = headArray[1];
                        break;
                    case "ST":
                        model.ST = headArray[1];
                        break;
                    case "CN":
                        try
                        {
                            model.CN = Convert.ToInt32(headArray[1]);
                        }
                        catch
                        {
                            model.CN = 0;
                        }
                        break;
                    case "MN":
                        model.MN = headArray[1];
                        break;
                    case "Flag":
                        model.Flag = headArray[1];
                        try
                        {
                            int flag = Convert.ToInt32(model.Flag);
                            model.Flag_A = flag % 2;
                            model.Flag_D = (flag >> 1) % 2;
                            model.Flag_V = flag - model.Flag_D * 2 - model.Flag_A;
                        }
                        catch
                        {
                            model.Flag = string.Empty;
                        }
                        break;
                    case "PW":
                        model.PW = headArray[1];
                        break;
                    case "PNUM":
                        model.PNUM = headArray[1];
                        break;
                    case "PNO":
                        model.PNO = headArray[1];
                        break;
                }
            }
            return model;
        }

        /// <summary>
        /// 设置超时时间及重发次数
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static SetTimeOutReSendTimesModel SetTimeOutReSendTimes(string context)
        {
            SetTimeOutReSendTimesModel model = new SetTimeOutReSendTimesModel();
            string[] dataArray = context.Split(';');
            foreach (string data in dataArray)
            {
                string[] cmdArray = data.Split('=');
                if (cmdArray[0] == "ReCount")
                {
                    model.ReCount = Convert.ToInt32(cmdArray[1]);
                }
                if (cmdArray[0] == "OverTime")
                {
                    model.OverTime = Convert.ToInt32(cmdArray[1]);
                }
            }
            return model;
        }

        /// <summary>
        /// 提取现场机时间
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetSceneDeviceTime(string context)
        {
            return GetPolId(context);
        }

        /// <summary>
        /// 提取采样时间周期
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetSampleTimeInterval(string context)
        {
            return GetPolId(context);
        }

        /// <summary>
        /// 提取设备唯一标识
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetSceneDeviceUUID(string context)
        {
            return GetPolId(context);
        }

        /// <summary>
        /// 提取现场机信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static GetSceneDeviceInfoModel GetSceneDeviceInfo(string context)
        {
            GetSceneDeviceInfoModel getSceneDeviceInfoModel = new GetSceneDeviceInfoModel();
            string[] dataArray = context.Split(';');
            foreach (string data in dataArray)
            {
                string[] cmdArray = data.Split('=');
                if (cmdArray[0] == "PolId")
                {
                    getSceneDeviceInfoModel.PolId = cmdArray[1];
                }
                if (cmdArray[0] == "InfoId")
                {
                    getSceneDeviceInfoModel.InfoId = cmdArray[1];
                }
                if (cmdArray[0] == "BeginTime")
                {
                    string year = cmdArray[1].Substring(0, 2);
                    string month = cmdArray[1].Substring(4, 2);
                    string day = cmdArray[1].Substring(6, 2);
                    string hour = cmdArray[1].Substring(8, 2);
                    string minute = cmdArray[1].Substring(10, 2);
                    string second = cmdArray[1].Substring(12, 2);
                    getSceneDeviceInfoModel.BeginTime = Convert.ToDateTime(string.Format("{0}-{1}-{2} {3}:{4}:{5}", year, month, day, hour, minute, second));
                }
                if (cmdArray[0] == "EndTime")
                {
                    string year = cmdArray[1].Substring(0, 2);
                    string month = cmdArray[1].Substring(4, 2);
                    string day = cmdArray[1].Substring(6, 2);
                    string hour = cmdArray[1].Substring(8, 2);
                    string minute = cmdArray[1].Substring(10, 2);
                    string second = cmdArray[1].Substring(12, 2);
                    getSceneDeviceInfoModel.EndTime = Convert.ToDateTime(string.Format("{0}-{1}-{2} {3}:{4}:{5}", year, month, day, hour, minute, second));
                }
            }
            return getSceneDeviceInfoModel;
        }

        /// <summary>
        /// 提取现场机信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static SetSceneDeviceParamModel SetSceneDeviceParam(string context)
        {
            SetSceneDeviceParamModel setSceneDeviceParamModel = new SetSceneDeviceParamModel();
            string[] dataArray = context.Split(';');
            foreach (string data in dataArray)
            {
                string[] cmdArray = data.Split('=');
                if (cmdArray[0] == "PolId")
                {
                    setSceneDeviceParamModel.PolId = cmdArray[1];
                }
                if (cmdArray[0] == "InfoId")
                {
                    setSceneDeviceParamModel.InfoId = cmdArray[1];
                }
                if (cmdArray[0].EndsWith("-Info"))
                {
                    setSceneDeviceParamModel.Value = cmdArray[1];
                }
            }
            return setSceneDeviceParamModel;
        }
        
        /// <summary>
        /// 提取出样时间
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetSampleTime(string context)
        {
            return GetPolId(context);
        }
        
        /// <summary>
        /// 设置现场机时间
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static DateTime? SetSceneDeviceTime(string context)
        {
            DateTime? SystemTime = null;
            string[] dataArray = context.Split(';');
            foreach (string data in dataArray)
            {
                string[] cmdArray = data.Split('=');
                if (cmdArray[0] == "SystemTime")
                {
                    string year = cmdArray[1].Substring(0, 2);
                    string month = cmdArray[1].Substring(4, 2);
                    string day = cmdArray[1].Substring(6, 2);
                    string hour = cmdArray[1].Substring(8, 2);
                    string minute = cmdArray[1].Substring(10, 2);
                    string second = cmdArray[1].Substring(12, 2);
                    SystemTime = Convert.ToDateTime(string.Format("{0}-{1}-{2} {3}:{4}:{5}", year, month, day, hour, minute, second));
                }
            }
            return SystemTime;
        }

        /// <summary>
        /// 设置采样时间周期
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static SetSampleTimeIntervalModel SetSampleTimeInterval(string context)
        {
            SetSampleTimeIntervalModel setSampleTimeIntervalModel = new SetSampleTimeIntervalModel();
            string[] dataArray = context.Split(';');
            foreach (string data in dataArray)
            {
                string[] cmdArray = data.Split('=');
                if (cmdArray[0] == "PolId")
                {
                    setSampleTimeIntervalModel.PolId = cmdArray[1];
                }
                if (cmdArray[0] == "CstartTime")
                {
                    setSampleTimeIntervalModel.CstartTime = cmdArray[1];
                }
                if (cmdArray[0] == "CTime")
                {
                    setSampleTimeIntervalModel.CTime = Convert.ToInt16(cmdArray[1]);
                }
            }
            return setSampleTimeIntervalModel;
        }

        /// <summary>
        /// 设置实时数据间隔
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static int? SetRtdDataInterval(string context)
        {
            int? RtdInterval = null;
            string[] dataArray = context.Split(';');
            foreach (string data in dataArray)
            {
                string[] cmdArray = data.Split('=');
                if (cmdArray[0] == "PolId")
                {
                    RtdInterval = Convert.ToInt32(cmdArray[1]);
                }
            }
            return RtdInterval;
        }

        /// <summary>
        /// 设置分钟数据间隔
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static int? SetMinuteDataInterval(string context)
        {
            int? MinInterval = null;
            string[] dataArray = context.Split(';');
            foreach (string data in dataArray)
            {
                string[] cmdArray = data.Split('=');
                if (cmdArray[0] == "MinInterval")
                {
                    MinInterval = Convert.ToInt32(cmdArray[1]);
                }
            }
            return MinInterval;
        }

        /// <summary>
        /// 用于设置现场机的密码
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string SetSceneDevicePassword(string context)
        {
            string NewPW = null;
            string[] dataArray = context.Split(';');
            foreach (string data in dataArray)
            {
                string[] cmdArray = data.Split('=');
                if (cmdArray[0] == "NewPW")
                {
                    NewPW = cmdArray[1];
                }
            }
            return NewPW;
        }

        /// <summary>
        /// 取污染物日历史数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static HistoryDataModel GetDayData(string context)
        {
            return GetHistoryData(context);
        }

        /// <summary>
        /// 取设备运行时间日历史数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static HistoryDataModel GetDeviceRunTimeDayData(string context)
        {
            return GetHistoryData(context);
        }

        /// <summary>
        /// 取污染物小时数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static HistoryDataModel GetHourData(string context)
        {
            return GetHistoryData(context);
        }

        /// <summary>
        /// 取污染物分钟数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static HistoryDataModel GetMinuteData(string context)
        {
            return GetHistoryData(context);
        }

        /// <summary>
        /// 零点校准量程校准
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string RangeCalibration(string context)
        {
            return GetPolId(context);
        }

        /// <summary>
        /// 即时采样
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string TakeSampleImmediately(string context)
        {
            return GetPolId(context);
        }

        /// <summary>
        /// 启动清洗/反吹
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string StartClear(string context)
        {
            return GetPolId(context);
        }

        /// <summary>
        /// 比对采样
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string CompareSample(string context)
        {
            return GetPolId(context);
        }

        /// <summary>
        /// 取污染物周期数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static HistoryDataModel GetCycleData(string context)
        {
            return GetHistoryData(context);
        }

        /// <summary>
        /// 获取历史数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private static HistoryDataModel GetHistoryData(string context)
        {
            HistoryDataModel historyDataModel = new HistoryDataModel();
            string[] dataArray = context.Split(';');
            foreach (string data in dataArray)
            {
                string[] cmdArray = data.Split('=');
                if (cmdArray[0] == "BeginTime")
                {
                    string year = cmdArray[1].Substring(0, 2);
                    string month = cmdArray[1].Substring(4, 2);
                    string day = cmdArray[1].Substring(6, 2);
                    string hour = cmdArray[1].Substring(8, 2);
                    string minute = cmdArray[1].Substring(10, 2);
                    string second = cmdArray[1].Substring(12, 2);
                    historyDataModel.BeginTime = Convert.ToDateTime(string.Format("{0}-{1}-{2} {3}:{4}:{5}", year, month, day, hour, minute, second));
                }
                if (cmdArray[0] == "EndTime")
                {
                    string year = cmdArray[1].Substring(0, 2);
                    string month = cmdArray[1].Substring(4, 2);
                    string day = cmdArray[1].Substring(6, 2);
                    string hour = cmdArray[1].Substring(8, 2);
                    string minute = cmdArray[1].Substring(10, 2);
                    string second = cmdArray[1].Substring(12, 2);
                    historyDataModel.EndTime = Convert.ToDateTime(string.Format("{0}-{1}-{2} {3}:{4}:{5}", year, month, day, hour, minute, second));
                }
            }
            return historyDataModel;
        }

        /// <summary>
        /// 获取PolId
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetPolId(string context)
        {
            string PolId = string.Empty;
            string[] dataArray = context.Split(';');
            foreach (string data in dataArray)
            {
                string[] cmdArray = data.Split('=');
                if (cmdArray[0] == "PolId")
                {
                    PolId = cmdArray[1];
                }
            }
            return PolId;
        }
    }


}
