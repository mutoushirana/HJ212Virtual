using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Redflag.Slave.Virtual.Lib.Protocol.HJ212.Config;
using System.IO;
using Newtonsoft.Json;
using Redflag.Slave.Virtual.Lib.Protocol.HJ212.Base;
using Redflag.Slave.Virtual.Lib.Protocol.HJ212;
using Redflag.Slave.Virtual.Lib.Protocol.HJ212.Model;
using Redflag.Slave.Virtual.Lib.Protocol.HJ212.Enum;
using Redflag.Slave.Virtual.Lib.Protocol.HJ212.Const;
using System.Runtime.InteropServices;
using Redflag.Slave.Virtual.Lib.Protocol.HJ212.Win32;

namespace Redflag.Slave.Virtual.Lib.Protocol
{
    public class HJ212Protocol : IProtocol
    {
        private Timer rtdDataTimer;
        private Timer minuterDataTimer;
        private Timer hourDataTimer;
        private Timer cycleDataTimer;
        private Timer dayDataTimer;
        private Timer heartBeatTimer;
        private Timer deviceRunStateTimer;
        private HJ212Config config;
        private DateTime restartTime;
        private List<SendDataInfo> messageArray = new List<SendDataInfo>();
        private Timer messageTimer;
        public HJ212Protocol()
        {
            base.ProtocolName = "HJ212";
            string configStr = File.ReadAllText(string.Format("{0}.json", ProtocolName));
            config = JsonConvert.DeserializeObject<HJ212Config>(configStr);
        }

        #region methods

        /// <summary>
        /// 开始任务
        /// </summary>
        public override void BeginTask()
        {
            restartTime = DateTime.Now;
            if (config.HeartBeat)
            {
                heartBeatTimer = new Timer(new TimerCallback(HeartBeatCallBack), null, 0, config.HeartBeatInterval * 1000);
            }
            if (config.RtdData)
            {
                rtdDataTimer = new Timer(new TimerCallback(RtdDataCallBack), null, 0, config.RtdInterval * 1000);
            }
            if (config.MinuteData)
            {
                minuterDataTimer = new Timer(new TimerCallback(MinuteDataCallBack), null, (config.MinInterval - DateTime.Now.Minute % config.MinInterval) * 60 * 1000, config.MinInterval * 60 * 1000);
            }
            if (config.HourData)
            {
                int span = DateTime.Now.Minute - config.HourDataDelay;
                hourDataTimer = new Timer(new TimerCallback(HourDataCallBack), null, span > 0 ? (60 - span) * 60 * 1000 : (0 - span) * 60 * 1000, 60 * 60 * 1000);
            }
            if (config.CycleData)
            {
                int span = DateTime.Now.Minute - config.CycleDataDelay;
                cycleDataTimer = new Timer(new TimerCallback(RtdDataCallBack), null, ((config.CycleInterval - DateTime.Now.Hour % config.CycleInterval) * 60 - span > 0 ? 60 - span : span) * 60 * 1000, config.CycleInterval * 60 * 60 * 1000);
            }
            if (config.DayData)
            {
                int span = Convert.ToInt32((DateTime.Now - new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)).TotalMinutes) - config.DayDataDelay;
                dayDataTimer = new Timer(new TimerCallback(RtdDataCallBack), null, (span > 0 ? 24 * 60 - span : 0 - span) * 60 * 1000, 24 * 60 * 60 * 1000);
            }
            if (config.DeviceRunState)
            {
                deviceRunStateTimer = new Timer(new TimerCallback(DeviceRunStateCallBack), null, 0, config.DeviceRunStateInterval * 1000);
            }
            messageTimer = new Timer(new TimerCallback(MessageCallBack),null, 0, config.MessageInterval);
            if (config.RestartTime)
            {
                UploadComputerPowerOnTime();
            }
            if (config.SceneDeviceTimeCalibration)
            {
                SceneDeviceTimeCalibration();
            }
        }

        /// <summary>
        /// 连接断开
        /// </summary>
        public override void ConnectStop()
        {
            if (null != heartBeatTimer)
            {
                heartBeatTimer.Dispose();
            }
            if (null != heartBeatTimer)
            {
                rtdDataTimer.Dispose();
            }
            if (null != heartBeatTimer)
            {
                minuterDataTimer.Dispose();
            }
            if (null != heartBeatTimer)
            {
                hourDataTimer.Dispose();
            }
            if (null != heartBeatTimer)
            {
                cycleDataTimer.Dispose();
            }
            if (null != heartBeatTimer)
            {
                dayDataTimer.Dispose();
            }
            if (null != heartBeatTimer)
            {
                deviceRunStateTimer.Dispose();
            }
            if (null != messageTimer)
            {
                messageTimer.Dispose();
            }
        }

        /// <summary>
        /// 数据接收
        /// </summary>
        /// <param name="data"></param>
        public override void DataReceive(string data)
        {
            try
            {
                if (string.IsNullOrEmpty(data))
                {
                    return;
                }
                HJ212Model model = Deserialize.DeserializeBase(data);
                CommandType type = Util.GetCommandType((MiddleCode)model.CN);
                switch (type)
                {
                    case CommandType.Notice:
                        NoticeResponse(model);
                        break;
                    case CommandType.Request:
                        CommandResult res = RequestResponse(model);
                        if (res != CommandResult.Ready)
                        {
                            return;
                        }
                        break;
                    case CommandType.Upload:
                        RequestResponse(model, CommandResult.Forbidden);
                        return;
                    case CommandType.Other:
                        return;
                    case CommandType.None:
                        RequestResponse(model, CommandResult.Forbidden);
                        return;
                    default:
                        RequestResponse(model, CommandResult.Forbidden);
                        return;
                }
                switch ((MiddleCode)model.CN)
                {
                    case MiddleCode.SetTimeOutReSendTimes:
                        SetTimeOutReSendTimes(model);
                        break;
                    case MiddleCode.GetSceneDeviceTime:
                        UploadSceneDeviceTime(model);
                        break;
                    case MiddleCode.SetSceneDeviceTime:
                        SetSceneDeviceTime(model);
                        break;
                    case MiddleCode.GetRtdDataInterval:
                        UploadRtdDataInterval(model);
                        break;
                    case MiddleCode.SetRtdDataInterval:
                        SetRtdDataInterval(model);
                        break;
                    case MiddleCode.GetMinuteDataInterval:
                        UploadMinuteDataInterval(model);
                        break;
                    case MiddleCode.SetMinuteDataInterval:
                        SetMinuteDataInterval(model);
                        break;
                    case MiddleCode.SetSceneDevicePassword:
                        SetSceneDevicePassword(model);
                        break;
                    case MiddleCode.GetRtdData:
                        GetRtdData(model);
                        break;
                    case MiddleCode.StopRtdData:
                        StopRtdData(model);
                        break;
                    case MiddleCode.GetDeviceRunState:
                        GetDeviceRunState(model);
                        break;
                    case MiddleCode.StopDeviceRunState:
                        StopDeviceRunState(model);
                        break;
                    case MiddleCode.GetDayData:
                        UploadDayData(model);
                        break;
                    case MiddleCode.GetDeviceRunTimeDayData:
                        UploadDeviceRunTimeDayData(model);
                        break;
                    case MiddleCode.GetMinuteData:
                        UploadMinuteData(model);
                        break;
                    case MiddleCode.GetHourData:
                        UploadHourData(model);
                        break;
                    case MiddleCode.RangeCalibration:
                        RangeCalibration(model);
                        break;
                    case MiddleCode.GetCycleData:
                        UploadCycleData(model);
                        break;
                    case MiddleCode.TakeSampleImmediately:
                        TakeSampleImmediately(model);
                        break;
                    case MiddleCode.StartClear:
                        StartClear(model);
                        break;
                    case MiddleCode.CompareSample:
                        CompareSample(model);
                        break;
                    case MiddleCode.LeaveSuperstandardSample:
                        UploadSuperstandardSample(model);
                        break;
                    case MiddleCode.SetSampleTimeInterval:
                        SetSampleTimeInterval(model);
                        break;
                    case MiddleCode.GetSampleTimeInterval:
                        UploadSampleTimeInterval(model);
                        break;
                    case MiddleCode.GetSampleTime:
                        UploadSampleTime(model);
                        break;
                    case MiddleCode.GetSceneDeviceUUID:
                        UploadSceneDeviceUUID(model);
                        break;
                    case MiddleCode.GetSceneDeviceInfo:
                        UploadSceneDeviceInfo(model);
                        break;
                    case MiddleCode.SetSceneDeviceParam:
                        SetSceneDeviceParam(model);
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// 数据发送结果
        /// </summary>
        /// <param name="data"></param>
        public override void OnSendResult(SendDataInfo data)
        {
            if (data.SendTimes < config.ReCount)
            {
                SendData(data);
            }
        }

        #endregion

        #region private methonds

        /// <summary>
        /// 设置超时时间及重发次数
        /// </summary>
        private void SetTimeOutReSendTimes(HJ212Model model)
        {
            string content = string.Empty;
            SetTimeOutReSendTimesModel setTimeOutReSendTimesModel = Deserialize.SetTimeOutReSendTimes(model.CP);
            if (null == setTimeOutReSendTimesModel.ReCount || null == setTimeOutReSendTimesModel.OverTime)
            {
                content = Serialize.ExecuteResponse(config, model.QN, ExecuteResult.CommandError);
            }
            else
            {
                config.ReCount = (int)setTimeOutReSendTimesModel.ReCount;
                config.OverTime = (int)setTimeOutReSendTimesModel.OverTime;
                ReWriteConfig();
                content = Serialize.ExecuteResponse(config, model.QN, ExecuteResult.Success);
            }
            SendData(content);
        }

        /// <summary>
        /// 上传现场机时间
        /// </summary>
        private void UploadSceneDeviceTime(HJ212Model model)
        {
            string content = string.Empty;
            string PolId = Deserialize.GetSceneDeviceTime(model.CP);
            if(string.IsNullOrEmpty(PolId))
            {
                content = Serialize.ExecuteResponse(config, model.QN, ExecuteResult.CommandError);
                SendData(content);
            }
            else
            {
                content = Serialize.UploadSceneDeviceTime(config, model.QN, PolId, DateTime.Now);
                SendData(content);
                content = Serialize.ExecuteResponse(config, model.QN, ExecuteResult.Success);
                SendData(content);
            }
        }

        /// <summary>
        /// 设置现场机时间
        /// </summary>
        private void SetSceneDeviceTime(HJ212Model model)
        {
            string content = string.Empty;
            DateTime? SystemTime = Deserialize.SetSceneDeviceTime(model.CP);
            if(null == SystemTime)
            {
                content = Serialize.ExecuteResponse(config, model.QN, ExecuteResult.CommandError);
            }
            else
            {
                bool res = Win32Util.SetSystemTime((DateTime)SystemTime);
                if (res)
                {
                    content = Serialize.ExecuteResponse(config, model.QN, ExecuteResult.Success);
                }
                else
                {
                    content = Serialize.ExecuteResponse(config, model.QN, ExecuteResult.Fail);
                }
            }
            SendData(content);
        }

        /// <summary>
        /// 现场机时间校准请求
        /// </summary>
        /// <param name="model"></param>
        private void SceneDeviceTimeCalibration()
        {
            foreach (ParamCodeConfig param in config.ParamCodes)
            {
                string content = Serialize.SceneDeviceTimeCalibration(config, Util.GetTimeStamp(), param.ParamCode);
                SendData(content);
                break;
            }
        }

        /// <summary>
        /// 上传实时数据间隔
        /// </summary>
        private void UploadRtdDataInterval(HJ212Model model)
        {
            string content  = Serialize.UploadRtdDataInterval(config, model.QN,config.RtdInterval);
            SendData(content);
            content = Serialize.ExecuteResponse(config, model.QN, ExecuteResult.Success);
            SendData(content);
        }

        /// <summary>
        /// 设置实时数据间隔
        /// </summary>
        private void SetRtdDataInterval(HJ212Model model)
        {
            string content = string.Empty;
            int? RtdInterval = Deserialize.SetRtdDataInterval(model.CP);
            if (null == RtdInterval)
            {
                content = Serialize.ExecuteResponse(config, model.QN, ExecuteResult.CommandError);
            }
            else
            {
                config.RtdInterval = (int)RtdInterval;
                ReWriteConfig();
                if (null != rtdDataTimer)
                {
                    rtdDataTimer.Dispose();
                }
                if (config.RtdData)
                {
                    rtdDataTimer = new Timer(new TimerCallback(RtdDataCallBack), null, 0, config.RtdInterval * 1000);
                }
                content = Serialize.ExecuteResponse(config, model.QN, ExecuteResult.Success);
            }
            SendData(content);
        }

        /// <summary>
        /// 用于设置现场机的密码
        /// </summary>
        private void SetSceneDevicePassword(HJ212Model model)
        {
            string content = string.Empty;
            string NewPW = Deserialize.SetSceneDevicePassword(model.CP);
            if (null == NewPW)
            {
                content = Serialize.ExecuteResponse(config, model.QN, ExecuteResult.CommandError);
            }
            else
            {
                config.PW = NewPW;
                ReWriteConfig();
                content = Serialize.ExecuteResponse(config, model.QN, ExecuteResult.Success);
            }
            SendData(content);
        }

        /// <summary>
        /// 上传分钟数据间隔
        /// </summary>
        private void UploadMinuteDataInterval(HJ212Model model)
        {
            string content = Serialize.UploadMinuteDataInterval(config, model.QN, config.MinInterval);
            SendData(content);
            content = Serialize.ExecuteResponse(config, model.QN, ExecuteResult.Success);
            SendData(content);
        }

        /// <summary>
        /// 设置分钟数据间隔
        /// </summary>
        private void SetMinuteDataInterval(HJ212Model model)
        {
            string content = string.Empty;
            int? MinInterval = Deserialize.SetMinuteDataInterval(model.CP);
            if (null == MinInterval)
            {
                content = Serialize.ExecuteResponse(config, model.QN, ExecuteResult.CommandError);
            }
            else
            {
                config.MinInterval = (int)MinInterval;
                ReWriteConfig();
                if(null != minuterDataTimer)
                {
                    minuterDataTimer.Dispose();
                }
                if (config.MinuteData)
                {
                    minuterDataTimer = new Timer(new TimerCallback(MinuteDataCallBack), null, (config.MinInterval - DateTime.Now.Minute % config.MinInterval) * 60 * 1000, config.MinInterval * 60 * 1000);
                }
                content = Serialize.ExecuteResponse(config, model.QN, ExecuteResult.Success);
            }
            SendData(content);
        }

        /// <summary>
        /// 取污染物实时数据
        /// </summary>
        private void GetRtdData(HJ212Model model)
        {
            string content = string.Empty;
            if (!config.RtdData)
            {
                config.RtdData = true;
                ReWriteConfig();
                rtdDataTimer = new Timer(new TimerCallback(RtdDataCallBack), null, 0, config.RtdInterval * 1000);
            }
            content = Serialize.ExecuteResponse(config, model.QN, ExecuteResult.Success);
            SendData(content);
        }

        /// <summary>
        /// 停止察看污染物实时数据
        /// </summary>
        private void StopRtdData(HJ212Model model)
        {
            string content = string.Empty;
            if (config.RtdData)
            {
                config.RtdData = false;
                ReWriteConfig();
                rtdDataTimer.Dispose();
            }
            content = Serialize.ExecuteResponse(config, model.QN, ExecuteResult.Success);
            SendData(content);
        }

        /// <summary>
        /// 取设备运行状态数据
        /// </summary>
        private void GetDeviceRunState(HJ212Model model)
        {
            string content = string.Empty;
            if (!config.DeviceRunState)
            {
                config.DeviceRunState = true;
                ReWriteConfig();
                deviceRunStateTimer = new Timer(new TimerCallback(DeviceRunStateCallBack), null, 0, config.DeviceRunStateInterval * 1000);
            }
            content = Serialize.ExecuteResponse(config, model.QN, ExecuteResult.Success);
            SendData(content);
        }

        /// <summary>
        /// 停止察看设备运行状态
        /// </summary>
        private void StopDeviceRunState(HJ212Model model)
        {
            string content = string.Empty;
            if (config.RtdData)
            {
                config.DeviceRunState = false;
                ReWriteConfig();
                deviceRunStateTimer.Dispose();
            }
            content = Serialize.ExecuteResponse(config, model.QN, ExecuteResult.Success);
            SendData(content);
        }

        /// <summary>
        /// 上传污染物日历史数据
        /// </summary>
        private void UploadDayData(HJ212Model model)
        {
            string content = string.Empty;
            HistoryDataModel historyDataModel = Deserialize.GetDayData(model.CP);
            if (null == historyDataModel.BeginTime || null == historyDataModel.EndTime)
            {
                content = Serialize.ExecuteResponse(config, model.QN, ExecuteResult.CommandError);
            }
            else
            {
                for(DateTime dt = (DateTime)historyDataModel.BeginTime; dt <= historyDataModel.EndTime; dt = dt.AddDays(1))
                {
                    string qn = Util.GetTimeStamp();
                    IDictionary<string, DataModel> datas = new Dictionary<string, DataModel>();
                    foreach (ParamCodeConfig param in config.ParamCodes)
                    {
                        DataModel dataModel = new DataModel();
                        dataModel.Rtd = Util.GetRandom(param.DefaultValue, param.Max, param.Min);
                        dataModel.Min = Util.GetMin((double)dataModel.Rtd, param.DefaultValue - param.Min);
                        dataModel.Max = Util.GetMax((double)dataModel.Rtd, param.DefaultValue + param.Max);
                        if (param.CouValue)
                        {
                            dataModel.Cou = dataModel.Rtd  * 60 * 24;
                        }
                        model.Flag = param.DataFlag;
                        datas.Add(param.ParamCode, dataModel);
                    }
                    IEnumerable<string> contents = Serialize.UploadDayData(config, qn, dt, datas);
                    foreach (string c in contents)
                    {
                        SendData(c);
                    }
                }
                content = Serialize.ExecuteResponse(config, model.QN, ExecuteResult.Success);
            }
            SendData(content);
        }

        /// <summary>
        /// 上传设备运行时间日历史数据
        /// </summary>
        private void UploadDeviceRunTimeDayData(HJ212Model model)
        {
            string content = string.Empty;
            HistoryDataModel historyDataModel = Deserialize.GetDeviceRunTimeDayData(model.CP);
            if (null == historyDataModel.BeginTime || null == historyDataModel.EndTime)
            {
                content = Serialize.ExecuteResponse(config, model.QN, ExecuteResult.CommandError);
            }
            else
            {
                for (DateTime dt = (DateTime)historyDataModel.BeginTime; dt <= historyDataModel.EndTime; dt = dt.AddDays(1))
                {
                    string qn = Util.GetTimeStamp();
                    IList<DeviceRunTimeModel> datas = new List<DeviceRunTimeModel>();
                    foreach (DeviceConfig param in config.Devices)
                    {
                        DeviceRunTimeModel deviceRunTimeModel = new DeviceRunTimeModel();
                        deviceRunTimeModel.DeviceNumber = param.DeviceNumber;
                        deviceRunTimeModel.DeviceRunTime = Util.GetRunHourRandom();
                        datas.Add(deviceRunTimeModel);
                    }
                    string c = Serialize.UploadDeviceRunTimeDayData(config, qn, dt, datas);
                    SendData(content);
                }
                content = Serialize.ExecuteResponse(config, model.QN, ExecuteResult.Success);
            }
            SendData(content);
        }

        /// <summary>
        /// 上传污染物分钟数据
        /// </summary>
        private void UploadMinuteData(HJ212Model model)
        {
            string content = string.Empty;
            HistoryDataModel historyDataModel = Deserialize.GetMinuteData(model.CP);
            if (null == historyDataModel.BeginTime || null == historyDataModel.EndTime)
            {
                content = Serialize.ExecuteResponse(config, model.QN, ExecuteResult.CommandError);
            }
            else
            {
                for (DateTime dt = (DateTime)historyDataModel.BeginTime; dt <= historyDataModel.EndTime; dt = dt.AddMinutes(config.MinInterval))
                {
                    string qn = Util.GetTimeStamp();
                    IDictionary<string, DataModel> datas = new Dictionary<string, DataModel>();
                    foreach (ParamCodeConfig param in config.ParamCodes)
                    {
                        DataModel dataModel = new DataModel();
                        dataModel.Rtd = Util.GetRandom(param.DefaultValue, param.Max, param.Min);
                        dataModel.Min = Util.GetMin((double)dataModel.Rtd, param.DefaultValue - param.Min);
                        dataModel.Max = Util.GetMax((double)dataModel.Rtd, param.DefaultValue + param.Max);
                        if (param.CouValue)
                        {
                            dataModel.Cou = dataModel.Rtd * config.MinInterval;
                        }
                        model.Flag = param.DataFlag;
                        datas.Add(param.ParamCode, dataModel);
                    }
                    IEnumerable<string> contents = Serialize.UploadMinuteData(config, qn, dt, datas);
                    foreach (string c in contents)
                    {
                        SendData(c);
                    }
                }
                content = Serialize.ExecuteResponse(config, model.QN, ExecuteResult.Success);
            }
            SendData(content);
        }

        /// <summary>
        /// 上传污染物小时数据
        /// </summary>
        private void UploadHourData(HJ212Model model)
        {
            string content = string.Empty;
            HistoryDataModel historyDataModel = Deserialize.GetHourData(model.CP);
            if (null == historyDataModel.BeginTime || null == historyDataModel.EndTime)
            {
                content = Serialize.ExecuteResponse(config, model.QN, ExecuteResult.CommandError);
            }
            else
            {
                for (DateTime dt = (DateTime)historyDataModel.BeginTime; dt <= historyDataModel.EndTime; dt = dt.AddHours(1))
                {
                    string qn = Util.GetTimeStamp();
                    IDictionary<string, DataModel> datas = new Dictionary<string, DataModel>();
                    foreach (ParamCodeConfig param in config.ParamCodes)
                    {
                        DataModel dataModel = new DataModel();
                        dataModel.Rtd = Util.GetRandom(param.DefaultValue, param.Max, param.Min);
                        dataModel.Min = Util.GetMin((double)dataModel.Rtd, param.DefaultValue - param.Min);
                        dataModel.Max = Util.GetMax((double)dataModel.Rtd, param.DefaultValue + param.Max);
                        if (param.CouValue)
                        {
                            dataModel.Cou = dataModel.Rtd * 60;
                        }
                        model.Flag = param.DataFlag;
                        datas.Add(param.ParamCode, dataModel);
                    }
                    IEnumerable<string> contents = Serialize.UploadHourData(config, qn, dt, datas);
                    foreach (string c in contents)
                    {
                        SendData(c);
                    }
                }
                content = Serialize.ExecuteResponse(config, model.QN, ExecuteResult.Success);
            }
            SendData(content);
        }

        /// <summary>
        /// 上传数采仪开机时间
        /// </summary>
        private void UploadComputerPowerOnTime()
        {
            string content = Serialize.UploadComputerPowerOnTime(config, Util.GetTimeStamp(), DateTime.Now,restartTime);
            SendData(content);
        }

        /// <summary>
        /// 上传污染物周期数据
        /// </summary>
        private void UploadCycleData(HJ212Model model)
        {
            string content = string.Empty;
            HistoryDataModel historyDataModel = Deserialize.GetCycleData(model.CP);
            if (null == historyDataModel.BeginTime || null == historyDataModel.EndTime)
            {
                content = Serialize.ExecuteResponse(config, model.QN, ExecuteResult.CommandError);
            }
            else
            {
                for (DateTime dt = (DateTime)historyDataModel.BeginTime; dt <= historyDataModel.EndTime; dt = dt.AddHours(config.CycleInterval))
                {
                    string qn = Util.GetTimeStamp();
                    IDictionary<string, DataModel> datas = new Dictionary<string, DataModel>();
                    foreach (ParamCodeConfig param in config.ParamCodes)
                    {
                        DataModel dataModel = new DataModel();
                        dataModel.Rtd = Util.GetRandom(param.DefaultValue, param.Max, param.Min);
                        dataModel.Min = Util.GetMin((double)dataModel.Rtd, param.DefaultValue - param.Min);
                        dataModel.Max = Util.GetMax((double)dataModel.Rtd, param.DefaultValue + param.Max);
                        if (param.CouValue)
                        {
                            dataModel.Cou = dataModel.Rtd * 60 * config.CycleInterval;
                        }
                        model.Flag = param.DataFlag;
                        datas.Add(param.ParamCode, dataModel);
                    }
                    IEnumerable<string> contents = Serialize.UploadCycleData(config, qn, dt, datas);
                    foreach (string c in contents)
                    {
                        SendData(c);
                    }
                }
                content = Serialize.ExecuteResponse(config, model.QN, ExecuteResult.Success);
            }
            SendData(content);
        }

        /// <summary>
        /// 零点校准量程校准
        /// </summary>
        private void RangeCalibration(HJ212Model model)
        {
            string content = string.Empty;
            string PolId = Deserialize.RangeCalibration(model.CP);
            if (string.IsNullOrEmpty(PolId))
            {
                content = Serialize.ExecuteResponse(config, model.QN, ExecuteResult.CommandError);
            }
            else
            {
                content = Serialize.ExecuteResponse(config, model.QN, ExecuteResult.Success);
            }
            SendData(content);
        }

        /// <summary>
        /// 即时采样
        /// </summary>
        private void TakeSampleImmediately(HJ212Model model)
        {
            string content = string.Empty;
            string PolId = Deserialize.TakeSampleImmediately(model.CP);
            if (string.IsNullOrEmpty(PolId))
            {
                content = Serialize.ExecuteResponse(config, model.QN, ExecuteResult.CommandError);
            }
            else
            {
                content = Serialize.ExecuteResponse(config, model.QN, ExecuteResult.Success);
            }
            SendData(content);
        }

        /// <summary>
        /// 启动清洗/反吹
        /// </summary>
        private void StartClear(HJ212Model model)
        {
            string content = string.Empty;
            string PolId = Deserialize.StartClear(model.CP);
            if (string.IsNullOrEmpty(PolId))
            {
                content = Serialize.ExecuteResponse(config, model.QN, ExecuteResult.CommandError);
            }
            else
            {
                content = Serialize.ExecuteResponse(config, model.QN, ExecuteResult.Success);
            }
            SendData(content);
        }

        /// <summary>
        /// 比对采样
        /// </summary>
        private void CompareSample(HJ212Model model)
        {
            string content = string.Empty;
            string PolId = Deserialize.CompareSample(model.CP);
            if (string.IsNullOrEmpty(PolId))
            {
                content = Serialize.ExecuteResponse(config, model.QN, ExecuteResult.CommandError);
            }
            else
            {
                content = Serialize.ExecuteResponse(config, model.QN, ExecuteResult.Success);
            }
            SendData(content);
        }

        /// <summary>
        /// 上传超标留样信息
        /// </summary>
        private void UploadSuperstandardSample(HJ212Model model)
        {
            string content = Serialize.UploadSuperstandardSample(config, model.QN, DateTime.Now , config.VaseNo);
            SendData(content);
            content = Serialize.ExecuteResponse(config, model.QN, ExecuteResult.Success);
            SendData(content);
        }

        /// <summary>
        /// 设置采样时间周期
        /// </summary>
        private void SetSampleTimeInterval(HJ212Model model)
        {
            string content = string.Empty;
            SetSampleTimeIntervalModel setSampleTimeIntervalModel = Deserialize.SetSampleTimeInterval(model.CP);
            if (string.IsNullOrEmpty(setSampleTimeIntervalModel.PolId) || 
                string.IsNullOrEmpty(setSampleTimeIntervalModel.CstartTime) || 
                setSampleTimeIntervalModel.CTime == null)
            {
                content = Serialize.ExecuteResponse(config, model.QN, ExecuteResult.CommandError);
            }
            else
            {
                ParamCodeConfig param = config.ParamCodes.FirstOrDefault(x => x.ParamCode == setSampleTimeIntervalModel.PolId);
                if(null != param)
                {
                    param.CstartTime = setSampleTimeIntervalModel.CstartTime;
                    param.CTime = (int)setSampleTimeIntervalModel.CTime;
                    ReWriteConfig();
                }
                content = Serialize.ExecuteResponse(config, model.QN, ExecuteResult.Success);
            }
            SendData(content);
        }

        /// <summary>
        /// 设置现场机参数
        /// </summary>
        private void SetSceneDeviceParam(HJ212Model model)
        {
            string content = string.Empty;
            SetSceneDeviceParamModel setSceneDeviceParam = Deserialize.SetSceneDeviceParam(model.CP);
            if (string.IsNullOrEmpty(setSceneDeviceParam.PolId) ||
                string.IsNullOrEmpty(setSceneDeviceParam.InfoId) ||
                setSceneDeviceParam.Value == null)
            {
                content = Serialize.ExecuteResponse(config, model.QN, ExecuteResult.CommandError);
            }
            else
            {
                ParamCodeConfig param = config.ParamCodes.FirstOrDefault(x => x.ParamCode == setSceneDeviceParam.PolId);
                if(null != param)
                {
                    InfoConfig info = param.Infos.FirstOrDefault(x => x.InfoId == setSceneDeviceParam.InfoId);
                    if(null == info)
                    {
                        info = new InfoConfig();
                        info.InfoId = setSceneDeviceParam.InfoId;
                        info.Value = setSceneDeviceParam.Value;
                    }
                    else
                    {
                        info.Value = setSceneDeviceParam.Value;
                    }
                }
                ReWriteConfig();
                content = Serialize.ExecuteResponse(config, model.QN, ExecuteResult.Success);
            }
            SendData(content);
        }

        /// <summary>
        /// 上传采样时间周期
        /// </summary>
        private void UploadSampleTimeInterval(HJ212Model model)
        {
            string content = string.Empty;
            string PolId = Deserialize.GetSampleTimeInterval(model.CP);
            if (string.IsNullOrEmpty(PolId))
            {
                content = Serialize.ExecuteResponse(config, model.QN, ExecuteResult.CommandError);
            }
            else
            {
                ParamCodeConfig param = config.ParamCodes.FirstOrDefault(x => x.ParamCode == PolId);
                if (null != param)
                {
                    string c = Serialize.UploadSampleTimeInterval(config, model.QN, param.ParamCode, param.CstartTime, param.CTime);
                    SendData(c);
                }
                content = Serialize.ExecuteResponse(config, model.QN, ExecuteResult.Success);
            }
            SendData(content);
        }

        /// <summary>
        /// 上传出样时间
        /// </summary>
        private void UploadSampleTime(HJ212Model model)
        {
            string content = string.Empty;
            string PolId = Deserialize.GetSampleTime(model.CP);
            if (string.IsNullOrEmpty(PolId))
            {
                content = Serialize.ExecuteResponse(config, model.QN, ExecuteResult.CommandError);
            }
            else
            {
                ParamCodeConfig param = config.ParamCodes.FirstOrDefault(x => x.ParamCode == PolId);
                if (null != param)
                {
                    string c = Serialize.UploadSampleTime(config, model.QN, param.ParamCode, param.Stime);
                    SendData(c);
                }
                content = Serialize.ExecuteResponse(config, model.QN, ExecuteResult.Success);
            }
            SendData(content);
        }

        /// <summary>
        /// 提取设备唯一标识
        /// </summary>
        private void UploadSceneDeviceUUID(HJ212Model model)
        {
            string content = string.Empty;
            string PolId = Deserialize.GetSceneDeviceUUID(model.CP);
            if (string.IsNullOrEmpty(PolId))
            {
                content = Serialize.ExecuteResponse(config, model.QN, ExecuteResult.CommandError);
            }
            else
            {
                ParamCodeConfig param = config.ParamCodes.FirstOrDefault(x => x.ParamCode == PolId);
                if (null != param)
                {
                    string c = Serialize.UploadSceneDeviceUUID(config, model.QN, param.ParamCode, param.SN);
                    SendData(c);
                }
                content = Serialize.ExecuteResponse(config, model.QN, ExecuteResult.Success);
            }
            SendData(content);
        }

        /// <summary>
        /// 提取现场机信息
        /// </summary>
        private void UploadSceneDeviceInfo(HJ212Model model)
        {
            string content = string.Empty;
            GetSceneDeviceInfoModel getSceneDeviceInfoModel = Deserialize.GetSceneDeviceInfo(model.CP);
            if (string.IsNullOrEmpty(getSceneDeviceInfoModel.PolId)||string.IsNullOrEmpty(getSceneDeviceInfoModel.InfoId))
            {
                content = Serialize.ExecuteResponse(config, model.QN, ExecuteResult.CommandError);
            }
            else
            {
                string c = Serialize.UploadSceneDeviceInfo(config, model.QN, getSceneDeviceInfoModel.PolId, getSceneDeviceInfoModel.InfoId, new Random().NextDouble(), DateTime.Now);
                content = Serialize.ExecuteResponse(config, model.QN, ExecuteResult.Success);
            }
            SendData(content);
        }

        /// <summary>
        /// 通知应答
        /// </summary>
        /// <param name="model"></param>
        private void NoticeResponse(HJ212Model model)
        {
            if (model.Flag_A == 1)
            {
                string content = Serialize.NoticeResponse(config, model.QN);
                SendData(content);
            }
        }
        
        /// <summary>
        /// 请求应答
        /// </summary>
        /// <param name="model"></param>
        private CommandResult RequestResponse(HJ212Model model)
        {
            CommandResult res = CommandResult.Ready;
            if (string.IsNullOrEmpty(model.QN))
            {
                res = CommandResult.QNError;
            }
            else if (config.PW != model.PW)
            {
                res = CommandResult.PWError;
            }
            else if (config.MN != model.MN)
            {
                res = CommandResult.MNError;
            }
            else if (config.ST != model.ST)
            {
                res = CommandResult.STError;
            }
            else if (string.IsNullOrEmpty(model.Flag))
            {
                res = CommandResult.FlagError;
            }
            else if (0 == model.CN)
            {
                res = CommandResult.CNError;
            }
            else if (config.CheckCRC && model.CRC != Util.GetHj212Crc16(model.Body))
            {
                res = CommandResult.CRCError;
            }
            if (model.Flag_A == 1)
            {
                string content = Serialize.RequestResponse(config, model.QN, res);
                SendData(content);
            }
            return res;
        }

        /// <summary>
        /// 请求应答
        /// </summary>
        /// <param name="model"></param>
        private void RequestResponse(HJ212Model model, CommandResult res)
        {
            if (model.Flag_A == 1)
            {
                string content = Serialize.RequestResponse(config, model.QN, res);
                SendData(content);
            }
        }

        /// <summary>
        /// 执行结果
        /// </summary>
        /// <param name="model"></param>
        private void ExecuteResponse(HJ212Model model, ExecuteResult res)
        {
            string content = Serialize.ExecuteResponse(config, model.QN, res);
            SendData(content);
        }

        /// <summary>
        /// 数据应答
        /// </summary>
        /// <param name="model"></param>
        private void DataResponse(HJ212Model model)
        {
            string content = Serialize.DataResponse(config, model.QN);
            SendData(content);
        }

        /// <summary>
        /// 心跳任务
        /// </summary>
        /// <param name="state"></param>
        private void HeartBeatCallBack(object state)
        {
            string content = Serialize.HeartBeat(config, Util.GetTimeStamp());
            SendData(content);
        }

        /// <summary>
        /// 上传污染物实时数据
        /// </summary>
        /// <param name="state"></param>
        private void RtdDataCallBack(object state)
        {
            DateTime datatime = DateTime.Now;
            string qn = Util.GetTimeStamp();
            IDictionary<string, DataModel> datas = new Dictionary<string, DataModel>();
            foreach (ParamCodeConfig param in config.ParamCodes)
            {
                DataModel model = new DataModel();
                model.Rtd = Util.GetRandom(param.DefaultValue, param.Max, param.Min);
                model.Flag = param.DataFlag;
                datas.Add(param.ParamCode, model);
            }
            IEnumerable<string> contents = Serialize.UploadRtdData(config, qn, datatime, datas);
            foreach (string content in contents)
            {
                SendData(content);
            }
        }

        /// <summary>
        /// 上传污染物分钟数据
        /// </summary>
        /// <param name="state"></param>
        private void MinuteDataCallBack(object state)
        {
            DateTime datatime = DateTime.Now;
            string qn = Util.GetTimeStamp();
            IDictionary<string, DataModel> datas = new Dictionary<string, DataModel>();
            foreach (ParamCodeConfig param in config.ParamCodes)
            {
                DataModel model = new DataModel();
                model.Rtd = Util.GetRandom(param.DefaultValue, param.Max, param.Min);
                model.Min = Util.GetMin((double)model.Rtd, param.DefaultValue - param.Min);
                model.Max = Util.GetMax((double)model.Rtd, param.DefaultValue + param.Max);
                if (param.CouValue)
                {
                    model.Cou = model.Rtd * config.MinInterval;
                }
                model.Flag = param.DataFlag;
                datas.Add(param.ParamCode, model);
            }
            IEnumerable<string> contents = Serialize.UploadMinuteData(config, qn, datatime, datas);
            foreach (string content in contents)
            {
                SendData(content);
            }
        }

        /// <summary>
        /// 上传污染物小时数据
        /// </summary>
        /// <param name="state"></param>
        private void HourDataCallBack(object state)
        {
            DateTime datatime = DateTime.Now;
            string qn = Util.GetTimeStamp();
            IDictionary<string, DataModel> datas = new Dictionary<string, DataModel>();
            foreach (ParamCodeConfig param in config.ParamCodes)
            {
                DataModel model = new DataModel();
                model.Rtd = Util.GetRandom(param.DefaultValue, param.Max, param.Min);
                model.Min = Util.GetMin((double)model.Rtd, param.DefaultValue - param.Min);
                model.Max = Util.GetMax((double)model.Rtd, param.DefaultValue + param.Max);
                if (param.CouValue)
                {
                    model.Cou = model.Rtd * 60;
                }
                model.Flag = param.DataFlag;
                datas.Add(param.ParamCode, model);
            }
            IEnumerable<string> contents = Serialize.UploadHourData(config, qn, datatime, datas);
            foreach (string content in contents)
            {
                SendData(content);
            }
        }

        /// <summary>
        /// 上传污染物周期数据
        /// </summary>
        /// <param name="state"></param>
        private void CycleDataCallBack(object state)
        {
            DateTime datatime = DateTime.Now;
            string qn = Util.GetTimeStamp();
            IDictionary<string, DataModel> datas = new Dictionary<string, DataModel>();
            foreach (ParamCodeConfig param in config.ParamCodes)
            {
                DataModel model = new DataModel();
                model.Rtd = Util.GetRandom(param.DefaultValue, param.Max, param.Min);
                model.Min = Util.GetMin((double)model.Rtd, param.DefaultValue - param.Min);
                model.Max = Util.GetMax((double)model.Rtd, param.DefaultValue + param.Max);
                if (param.CouValue)
                {
                    model.Cou = model.Rtd * config.CycleInterval * 60;
                }
                model.Flag = param.DataFlag;
                datas.Add(param.ParamCode, model);
            }
            IEnumerable<string> contents = Serialize.UploadCycleData(config, qn, datatime, datas);
            foreach (string content in contents)
            {
                SendData(content);
            }
        }

        /// <summary>
        /// 上传污染物周期数据
        /// </summary>
        /// <param name="state"></param>
        private void DeviceRunStateCallBack(object state)
        {
            DateTime datatime = DateTime.Now;
            string qn = Util.GetTimeStamp();
            IList<DeviceRunStateModel> datas = new List<DeviceRunStateModel>();
            foreach (DeviceConfig param in config.Devices)
            {
                DeviceRunStateModel model = new DeviceRunStateModel();
                model.DeviceNumber = param.DeviceNumber;
                model.DeviceRunState = param.DeviceRunState;
                datas.Add(model);
            }
            string content = Serialize.UploadDeviceRunState(config, qn, datatime, datas);
            SendData(content);
        }

        /// <summary>
        /// 上传污染物日历史数据
        /// </summary>
        /// <param name="state"></param>
        private void DayDataCallBack(object state)
        {
            DateTime datatime = DateTime.Now;
            string qn = Util.GetTimeStamp();
            IDictionary<string, DataModel> datas = new Dictionary<string, DataModel>();
            foreach (ParamCodeConfig param in config.ParamCodes)
            {
                DataModel model = new DataModel();
                model.Rtd = Util.GetRandom(param.DefaultValue, param.Max, param.Min);
                model.Min = Util.GetMin((double)model.Rtd, param.DefaultValue - param.Min);
                model.Max = Util.GetMax((double)model.Rtd, param.DefaultValue + param.Max);
                if (param.CouValue)
                {
                    model.Cou = model.Rtd * config.CycleInterval * 60 * 24;
                }
                model.Flag = param.DataFlag;
                datas.Add(param.ParamCode, model);
            }
            IEnumerable<string> contents = Serialize.UploadDayData(config, qn, datatime, datas);
            foreach (string content in contents)
            {
                SendData(content);
            }
        }

        /// <summary>
        /// 消息发送任务
        /// </summary>
        /// <param name="state"></param>
        private void MessageCallBack(object state)
        {
            if (messageArray.Count > 0)
            {
                SendDataInfo info = messageArray.FirstOrDefault();
                if (null != info)
                {
                    OnDataSend.Invoke(info);
                }
                messageArray.Remove(info);
            }
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="content"></param>
        private void SendData(string content)
        {
            SendDataInfo info = new SendDataInfo();
            info.DataId = Guid.NewGuid().ToString();
            info.Content = content;
            messageArray.Add(info);
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="data"></param>
        private void SendData(SendDataInfo info)
        {
            messageArray.Add(info);
        }

        /// <summary>
        /// 重新保存配置文件
        /// </summary>
        private void ReWriteConfig()
        {
            string text = JsonConvert.SerializeObject(config);
            StreamWriter fileStream = new StreamWriter(string.Format("{0}.json", ProtocolName), false);
            fileStream.Write(text);
            fileStream.Close();
        }

        #endregion
    }
}
