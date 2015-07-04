using System;
using System.ComponentModel;

namespace SocialNetwork.Logger.Interface
{
    /// <summary>
    /// Interfase for logging
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Add string to log.
        /// </summary>
        /// <param name="level">log level.</param>
        /// <param name="message">string to write to log.</param>
        void Log(LogLevel level,  [Localizable(false)] String message);

        /// <summary>
        /// Add string to log.
        /// </summary>
        /// <param name="level">log level.</param>
        /// <param name="message">string to write to log.</param>
        /// <param name="args">arguments to formtting string</param>
        void Log(LogLevel level, [Localizable(false)] String message, params object[] args);

        /// <summary>
        /// Add string to log.
        /// </summary>
        /// <typeparam name="T">Type of formattion string argument</typeparam>
        /// <param name="level">log level.</param>
        /// <param name="message">string to write to log.</param>
        /// <param name="arg">argument to formtting string</param>
        void Log<T>(LogLevel level, [Localizable(false)] String message, T arg);

        /// <summary>
        /// Add string to log.
        /// </summary>
        /// <typeparam name="T1">Type of first formattion string argument</typeparam>
        /// <typeparam name="T2">Type of second formattion string argument</typeparam>
        /// <param name="level">log level.</param>
        /// <param name="message">string to write to log.</param>
        /// <param name="arg1">argument to formtting string</param>
        /// <param name="arg2">argument to formtting string</param>
        void Log<T1,T2>(LogLevel level, [Localizable(false)] String message, T1 arg1, T2 arg2);

        /// <summary>
        /// Add string to log.
        /// </summary>
        /// <param name="level">log level.</param>
        /// <param name="formatProvider">formatProvider for formating string</param>
        /// <param name="message">string to write to log.</param>
        /// <param name="args">arguments to formtting string</param>

        void Log(LogLevel level,IFormatProvider formatProvider, [Localizable(false)] String message, params object[] args);
        
        /// <summary>
        /// Add string to log.
        /// </summary>
        /// <param name="level">log level.</param>
        /// <param name="formatProvider">formatProvider for formating string</param>
        /// <param name="message">string to write to log.</param>
        /// <param name="arg">arguments to formtting string</param>
        void Log<T>(LogLevel level, IFormatProvider formatProvider, [Localizable(false)] String message, T arg);
       
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
        void Log<T1, T2>(LogLevel level, IFormatProvider formatProvider, [Localizable(false)] String message, T1 arg1, T2 arg2);
    }
}
