using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redflag.Slave.Virtual.Lib.Protocol.HJ212.Enum
{
    public enum MiddleCode
    {
        //设置超时时间及重发次数
        SetTimeOutReSendTimes = 1000,

        //提取现场机时间
        GetSceneDeviceTime = 1011,

        //上传现场机时间
        UploadSceneDeviceTime = 1011,

        //设置现场机时间
        SetSceneDeviceTime = 1012,

        //现场机时间校准请求
        SceneDeviceTimeCalibration = 1013,

        //提取实时数据间隔
        GetRtdDataInterval = 1061,

        //上传实时数据间隔
        UploadRtdDataInterval = 1061,

        //设置实时数据间隔
        SetRtdDataInterval = 1062,

        //提取分钟数据间隔
        GetMinuteDataInterval = 1063,

        //上传分钟数据间隔
        UploadMinuteDataInterval = 1063,

        //设置分钟数据间隔
        SetMinuteDataInterval = 1064,

        //用于设置现场机的密码
        SetSceneDevicePassword = 1072,

        //取污染物实时数据
        GetRtdData = 2011,

        //上传污染物实时数据
        UploadRtdData = 2011,

        //停止察看污染物实时数据
        StopRtdData = 2012,

        //取设备运行状态数据
        GetDeviceRunState = 2021,

        //上传设备运行状态数据
        UploadDeviceRunState = 2021,

        //停止察看设备运行状态
        StopDeviceRunState = 2022,

        //取污染物日历史数据
        GetDayData = 2031,

        //上传污染物日历史数据
        UploadDayData = 2031,

        //取设备运行时间日历史数据
        GetDeviceRunTimeDayData = 2041,

        //上传设备运行时间日历史数据
        UploadDeviceRunTimeDayData = 2041,

        //取污染物分钟数据
        GetMinuteData = 2051,

        //上传污染物分钟数据
        UploadMinuteData = 2051,

        //取污染物小时数据
        GetHourData = 2061,

        //上传污染物小时数据
        UploadHourData = 2061,

        //上传数采仪开机时间
        UploadComputerPowerOnTime = 2081,

        //零点校准量程校准
        RangeCalibration = 3011,

        //即时采样
        TakeSampleImmediately = 3012,

        //启动清洗/反吹
        StartClear = 3013,

        //比对采样
        CompareSample = 3014,

        //超标留样
        LeaveSuperstandardSample = 3015,

        //上传超标留样信息
        UploadSuperstandardSample = 3015,

        //设置采样时间周期
        SetSampleTimeInterval = 3016,

        //提取采样时间周期
        GetSampleTimeInterval = 3017,

        //上传采样时间周期
        UploadSampleTimeInterval = 3017,

        //提取出样时间
        GetSampleTime = 3018,

        //上传出样时间
        UploadSampleTime = 3018,

        //提取设备唯一标识
        GetSceneDeviceUUID = 3019,

        //上传设备唯一标识
        UploadSceneDeviceUUID = 3019,

        //提取现场机信息
        GetSceneDeviceInfo = 3020,

        //上传现场机信息
        UploadSceneDeviceInfo = 3020,

        //设置现场机参数
        SetSceneDeviceParam = 3021,

        //取污染物周期数据
        GetCycleData = 8051,

        //上传污染物周期数据
        UploadCycleData = 8051,

        //请求应答
        RequestResponse = 9011,

        //执行结果
        ExecuteResponse = 9012,

        //通知应答
        NoticeResponse = 9013,

        //数据应答
        DataResponse = 9014,

        //心跳
        HeartBeat = 9021,

    }
}
