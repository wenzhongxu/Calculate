namespace Calculate.DAL.Express
{

    /// <summary>
    /// 操作符类型
    /// </summary>
    public enum EnmOperators
    {

        Nop,

        /// <summary>
        /// 加
        /// </summary>
        Plus,

        /// <summary>
        /// 减
        /// </summary>
        Minus,

        /// <summary>
        /// 乘
        /// </summary>
        Mul,

        /// <summary>
        /// 除
        /// </summary>
        Div,

        /// <summary>
        /// 取反
        /// </summary>
        UnMinus,

        UnPlus,

        /// <summary>
        /// 等于
        /// </summary>
        Eq,

        /// <summary>
        /// 大于
        /// </summary>
        Gr,

        /// <summary>
        /// 小于
        /// </summary>
        Ls,

        /// <summary>
        /// 大于等于
        /// </summary>
        GrEq,

        /// <summary>
        /// 小于等于
        /// </summary>
        LsEq,

        /// <summary>
        /// 不等于
        /// </summary>
        NtEq,

        /// <summary>
        /// 左括号
        /// </summary>
        LeftPar,

        /// <summary>
        /// 右括号
        /// </summary>
        RightPar,

        /// <summary>
        /// 空白
        /// </summary>
        Blank,

        /// <summary>
        /// 逻辑非
        /// </summary>
        Not,

        /// <summary>
        /// 逻辑与
        /// </summary>
        And,

        /// <summary>
        /// 逻辑或
        /// </summary>
        Or
    }
}
