using System;
using SocialNetwork.Logger.Interface;

namespace SocialNetwork.Logger.NLogLogger
{
    /// <summary>
    /// Represent logger by using NLog library
    /// </summary>
    public class NLogLogger : ILogger
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        
        /// <summary>
        /// Add string to log.
        /// </summary>
        /// <param name="level">log level.</param>
        /// <param name="message">string to write to log.</param>
        public void Log(LogLevel level, string message)
        {
           Logger.Log(ToNLogLevel(level), message);
        }

        /// <summary>
        /// Add string to log.
        /// </summary>
        /// <param name="level">log level.</param>
        /// <param name="message">string to write to log.</param>
        /// <param name="args">arguments to formtting string</param>
        public void Log(LogLevel level, string message, params object[] args)
        {
            Logger.Log(ToNLogLevel(level), message,args);
        }

        /// <summary>
        /// Add string to log.
        /// </summary>
        /// <typeparam name="T">Type of formattion string argument</typeparam>
        /// <param name="level">log level.</param>
        /// <param name="message">string to write to log.</param>
        /// <param name="arg">argument to formtting string</param>
        public void Log<T>(LogLevel level, string message, T arg)
        {
            Logger.Log(ToNLogLevel(level), message,arg);
        }

        /// <summary>
        /// Add string to log.
        /// </summary>
        /// <typeparam name="T1">Type of first formattion string argument</typeparam>
        /// <typeparam name="T2">Type of second formattion string argument</typeparam>
        /// <param name="level">log level.</param>
        /// <param name="message">string to write to log.</param>
        /// <param name="arg1">argument to formtting string</param>
        /// <param name="arg2">argument to formtting string</param>
        public void Log<T1, T2>(LogLevel level, string message, T1 arg1, T2 arg2)
        {
            Logger.Log(ToNLogLevel(level), message, arg1,arg2);
        }

        /// <summary>
        /// Add string to log.
        /// </summary>
        /// <param name="level">log level.</param>
        /// <param name="formatProvider">formatProvider for formating string</param>
        /// <param name="message">string to write to log.</param>
        /// <param name="args">arguments to formtting string</param>
        public void Log(LogLevel level, IFormatProvider formatProvider, string message, params object[] args)
        {
            Logger.Log(ToNLogLevel(level),formatProvider, message, args);
        }

        /// <summary>
        /// Add string to log.
        /// </summary>
        /// <param name="level">log level.</param>
        /// <param name="formatProvider">formatProvider for formating string</param>
        /// <param name="message">string to write to log.</param>
        /// <param name="arg">arguments to formtting string</param>
        public void Log<T>(LogLevel level, IFormatProvider formatProvider, string message, T arg)
        {
            Logger.Log(ToNLogLevel(level), formatProvider, message, arg);
        }

        /// <summary>
        /// Add string to log.
        /// </summary>
        /// <typeparam name="T1">Type of first formattion string argument</typeparam>
        /// <typeparam name="T2">Type of second formattion string argument</typeparam>
        /// <param name="level">log level.</param>
        /// <param name="formatProvider">formatProvider for formating string</param>
        /// <param name="message">string to write to log.</param>
        /// <param name="arg1">argument to formtting string</param>
        /// <param name="arg2">argument to formtting string</param>
        public void Log<T1, T2>(LogLevel level, IFormatProvider formatProvider, string message, T1 arg1, T2 arg2)
        {
            Logger.Log(ToNLogLevel(level), formatProvider, message, arg1,arg2);
        }

        private NLog.LogLevel ToNLogLevel(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Trace:
                    return NLog.LogLevel.Trace;
                case LogLevel.Debug:
                    return NLog.LogLevel.Debug;
                case LogLevel.Info:
                    return NLog.LogLevel.Info;
                case LogLevel.Warn:
                    return NLog.LogLevel.Warn;
                case LogLevel.Error:
                    return NLog.LogLevel.Error;
                case LogLevel.Fatal:
                    return NLog.LogLevel.Fatal;
                default:
                    throw new ArgumentOutOfRangeException("logLevel");
            }
        }
    }
}
