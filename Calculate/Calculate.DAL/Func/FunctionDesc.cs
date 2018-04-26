using System.Collections;

namespace Calculate.DAL.Func
{
    /// <summary>
    /// 扩展函数参数验证方式
    /// </summary>
    public enum ParamValidType
    {
        /// <summary>
        /// 固定个数
        /// </summary>
        Fixed,

        /// <summary>
        /// 至少几个参数
        /// </summary>
        Least,

        /// <summary>
        /// 自定义
        /// </summary>
        Custom
    }

    public class FunctionDesc : ArrayList
    {
        public string FunctionName { get; private set; }

        public FunctionDesc()
            : this(string.Empty)
        {
        }

        public FunctionDesc(string s)
        {
            this.FunctionName = s;
        }

        private new int Add(object obj)
        {
            return 0;
        }

        public int Add(Variant v)
        {
            return base.Add(v);
        }

        public new Variant this[int index]
        {
            get { return (Variant)base[index]; }
            set { base[index] = value; }
        }
    }
}
