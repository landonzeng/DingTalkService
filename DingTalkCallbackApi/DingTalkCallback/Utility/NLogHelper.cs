using NLog;
using System;

namespace Utility
{
    public class NLogHelper
    {
        private Logger _logger;

        public NLogHelper(Logger log)
        {
            _logger = log;
        }

        /// <summary>
        /// 写入文本
        /// </summary>
        /// <param name="logKey">自定义Key</param>
        /// <param name="err">报错信息</param>
        public void Error(string logKey, Exception err)
        {

            try
            {
                if (_logger.IsErrorEnabled)
                {
                    _logger.Error(logKey, err);
                }
                else
                {
                    throw new Exception(logKey + "日志插件没有开启Error");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="exception">报错信息</param>
        public void Error(Exception exception)
        {
            try
            {
                if (_logger.IsErrorEnabled)
                {
                    _logger.Error(exception);
                }
                else
                {
                    throw new Exception("日志插件没有开启Error");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="logKey"></param>
        /// <param name="logModel"></param>
        public void Error(string logKey)
        {
            try
            {
                if (_logger.IsErrorEnabled)
                {
                    _logger.Error(logKey);
                }
                else
                {
                    throw new Exception("日志插件没有开启Error");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Debug(string logKey)
        {
            try
            {
                if (_logger.IsDebugEnabled)
                {
                    _logger.Debug(logKey);
                }
                else
                {
                    throw new Exception("日志插件没有开启Debug");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Info(object msg)
        {
            try
            {
                if (_logger.IsInfoEnabled)
                {
                    _logger.Info(msg);
                }
                else
                {
                    throw new Exception("日志插件没有开启Debug");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
