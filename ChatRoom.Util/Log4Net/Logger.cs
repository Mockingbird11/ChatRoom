using log4net;
using log4net.Config;
using log4net.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ChatRoom.DataAcces.Log4Net
{
    public class Logger
    {
        private ILog logger;
        public Logger()
        {
            if (logger == null)
            {
                ILoggerRepository loggerRepository = null;
                if (LogManager.GetAllRepositories().Count() > 0)
                {
                    loggerRepository = LogManager.GetRepository("NETCoreRepository");
                }
                else
                {
                    loggerRepository = LogManager.CreateRepository("NETCoreRepository");
                }
                //log4net从log4net.config文件中读取配置信息
                XmlConfigurator.Configure(loggerRepository, new FileInfo("log4net.config"));
                logger = LogManager.GetLogger(loggerRepository.Name, "InfoLogger");
            }
        }

        /// <summary>
        /// 普通日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public void Info(string message, Exception exception = null)
        {
            if (exception == null)
                logger.Info(message);
            else
                logger.Info(message, exception);
        }

        /// <summary>
        /// 告警日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public void Warn(string message, Exception exception = null)
        {
            if (exception == null)
                logger.Warn(message);
            else
                logger.Warn(message, exception);
        }

        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public void Error(string message, Exception exception = null)
        {
            if (exception == null)
                logger.Error(message);
            else
                logger.Error(message, exception);
        }
    }
}
