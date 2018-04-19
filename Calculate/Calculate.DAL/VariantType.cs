namespace Calculate.DAL
{
    /// <summary>
    /// 计算值值类型枚举
    /// </summary>
    public enum VariantType
    {
        /// <summary>
        /// 未知
        /// </summary>
        vtUnknow,

        /// <summary>
        /// 布尔型
        /// </summary>
        vtBool,

        /// <summary>
        /// 整型
        /// </summary>
        vtInt,

        /// <summary>
        /// 浮点型
        /// </summary>
        vtDouble,

        /// <summary>
        /// 字符串
        /// </summary>
        vtString,

        /// <summary>
        /// 日期型
        /// </summary>
        vtDateTime,

        /// <summary>
        /// 高精度浮点型
        /// </summary>
        vDecimal
    }
}
