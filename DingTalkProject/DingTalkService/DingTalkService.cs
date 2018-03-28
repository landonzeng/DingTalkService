using Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utilities;

namespace DingTalkService
{
    public partial class DingTalkService : ServiceBase
    {
        public DingTalkService()
        {
            InitializeComponent();
        }

        Thread _serviceThread = null;

        protected override void OnStart(string[] args)
        {
            LogHelper log = LogFactory.GetLogger("OnStart");
            log.Debug("服务开启时间" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n-----------------------------------------------------------------------------");
            _serviceThread = new Thread(TimerTask)
            {
                IsBackground = true
            };
            _serviceThread.Start();
        }

        protected override void OnStop()
        {
            LogHelper log = LogFactory.GetLogger("OnStop");
            log.Debug("服务结束时间：" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n-----------------------------------------------------------------------------");
            _serviceThread.Abort();
        }

        private void TimerTask()
        {
            LogHelper log = LogFactory.GetLogger("TimerTask");
            while (true)
            {
                try
                {
                    #region 业务处理
                    try
                    {
                        SynchronousDingTalk.Synchronous();
                        Thread.Sleep(Convert.ToInt32(ConfigurationManager.AppSettings["ThreadSleepTime"]));//线程休眠
                    }
                    catch (Exception ex)
                    {
                        if (!ex.Message.Contains("正在中止线程"))
                        {
                            log.Error("\n错误Message为：" + ex.Message + "； InnerException为：" + ex.InnerException + "； StackTrace为：" + ex.StackTrace + "；  服务发生错误：\r\n" + ex + "\r\n-----------------------------------------------------------------------------");
                        }
                        Thread.Sleep(Convert.ToInt32(ConfigurationManager.AppSettings["ThreadSleepTime"])); //线程休眠
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    if (!ex.Message.Contains("正在中止线程"))
                    {
                        log.Error("\n错误Message为：" + ex.Message + "；  服务线程错误：\r\n" + ex + "\r\n-----------------------------------------------------------------------------");
                    }
                    Thread.Sleep(Convert.ToInt32(ConfigurationManager.AppSettings["ThreadSleepTime"])); //线程休眠
                }
            }
        }
    }
}
