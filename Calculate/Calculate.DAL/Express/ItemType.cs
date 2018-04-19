namespace Calculate.DAL.Express
{
    /// <summary>
    /// 表达式对象类型
    /// </summary>
    public enum ItemType
    {
        /// <summary>
        /// 未知
        /// </summary>
        itUnknow,

        /// <summary>
        /// 字符串常量
        /// </summary>
        itString,

        /// <summary>
        /// 数值常量
        /// </summary>
        itDigit,

        /// <summary>
        /// 日期常量
        /// </summary>
        itDate,

        /// <summary>
        /// 逻辑常量：true,false
        /// </summary>
        itBool,

        /// <summary>
        /// 变量（财务指标、表达式）
        /// </summary>
        itVariable,

        /// <summary>
        /// 扩展函数
        /// </summary>
        itFunction,

        /// <summary>
        /// 操作符
        /// </summary>
        itOperator
    }
}
