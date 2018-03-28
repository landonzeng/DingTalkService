using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Utilities
{
    /// <summary>
    /// Log4Net日志 工厂
    /// 版本：2.0
    /// <author>
    ///		<name>shecixiong</name>
    ///		<date>2014.03.03</date>
    /// </author>
    /// </summary>
    public class LogFactory
    {
        static LogFactory()
        {
            if (HttpContext.Current != null) //web访问方式
            {
                FileInfo configFile = new FileInfo(HttpContext.Current.Server.MapPath("/XmlConfig/log4net.config"));

                log4net.Config.XmlConfigurator.Configure(configFile);
            }
            else
            {
                FileInfo configFile = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "XmlConfig" + System.IO.Path.DirectorySeparatorChar + "log4net.config");
                log4net.Config.XmlConfigurator.Configure(configFile);
            }
        }

        public static LogHelper GetLogger(Type type)
        {
            return new LogHelper(LogManager.GetLogger(type));
        }

        public static LogHelper GetLogger(string str)
        {
            return new LogHelper(LogManager.GetLogger(str));
        }
    }
}
