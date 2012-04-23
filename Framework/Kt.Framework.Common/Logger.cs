using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Reflection;

namespace Kt.Framework.Common
{
    /// <summary>
    /// 常用的日志方法 
    /// </summary>
    public class Logger
    {
        private static log4net.ILog getLog()
        {
            StackFrame frame = new StackFrame(2);      

            MethodBase method = frame.GetMethod();      //取得调用函数
            log4net.ILog log = log4net.LogManager.GetLogger(method.DeclaringType);
            return log;
        }
        /// <summary>
        /// DEBUG 
        /// </summary>
        /// <param name="message"></param>
        public static void Debug(object message)
        {
            log4net.ILog log = getLog();// log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            log.Debug(message);
        }
        /// <summary>
        /// Error
        /// </summary>
        /// <param name="message"></param>
        public static void Error(object message)
        {
            log4net.ILog log = getLog();
            //log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.Error(message);
        }

        /// <summary>
        /// Error
        /// </summary>
        /// <param name="message"></param>
        public static void Error(object message, Exception exception)
        {
            log4net.ILog log = getLog();

            //log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.Error(message, exception);
        }

        /// <summary>
        /// Fatal
        /// </summary>
        /// <param name="message"></param>
        public static void Fatal(object message)
        {
            log4net.ILog log = getLog();// log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.Fatal(message);
        }
        /// <summary>
        /// Info
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Info(object message, Exception exception)
        {
            log4net.ILog log = getLog();
            //log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.Info(message, exception);
        }

        /// <summary>
        /// Info
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Info(object message)
        {
            log4net.ILog log = getLog();
            //log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.Info(message);
        }
        /// <summary>
        /// Warn
        /// </summary>
        /// <param name="message"></param>
        public static void Warn(object message)
        {
            log4net.ILog log = getLog();
            //log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.Warn(message);
        }
    }
}
