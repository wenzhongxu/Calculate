using System;
using System.Runtime.Serialization;

namespace Calculate.DAL
{
    /// <summary>
    ///  计算引擎异常
    /// </summary>
    [Serializable]
    public class CalcException : Exception
    {
        /// <summary>
        /// 计算引擎异常
        /// </summary>
        public CalcException() :
            base()
        {
        }
        /// <summary>
        /// 计算引擎异常
        /// </summary>
        /// <param name="message"></param>
        public CalcException(string message)
            : base(message)
        {
        }
        /// <summary>
        /// 计算引擎异常
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public CalcException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
        /// <summary>
        /// 计算引擎异常
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected CalcException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
