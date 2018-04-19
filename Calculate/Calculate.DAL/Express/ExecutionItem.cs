using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculate.DAL.Express
{
    /// <summary>
    /// 表达式解析对象
    /// </summary>
    public class ExecutionItem
    {
        #region 属性
        /// <summary>
        /// 对象表达式
        /// </summary>
        public string ItemString { get; private set; }
        /// <summary>
        /// 对象类型
        /// </summary>
        public ItemType ItemType { get; private set; }
        /// <summary>
        /// 对象操作类型
        /// </summary>
        public EnmOperators ItemOperator { get; private set; }
        /// <summary>
        /// 对象参数
        /// </summary>
        public StringCollection ItemParams { get; private set; }
        #endregion

        #region 构造函数
        /// <summary>
        /// 表达式解析对象实例
        /// </summary>
        public ExecutionItem()
        {
            this.ItemString = "";
            this.ItemType = ItemType.itUnknow;
            this.ItemOperator = EnmOperators.Blank;
            this.ItemParams = null;
        }
        /// <summary>
        /// 表达式解析对象实例
        /// </summary>
        /// <param name="b"></param>
        public ExecutionItem(bool b)
        {
            this.ItemString = b.ToString();
            this.ItemType = ItemType.itBool;
            this.ItemOperator = EnmOperators.Blank;
            this.ItemParams = null;
        }
        /// <summary>
        /// 表达式解析对象实例
        /// </summary>
        /// <param name="itType">对象类型</param>
        /// <param name="s">对象表达式</param>
        public ExecutionItem(ItemType itType, string s)
        {
            this.ItemString = s;
            this.ItemType = itType;
            this.ItemOperator = EnmOperators.Blank;
            this.ItemParams = null;
        }
        /// <summary>
        /// 表达式解析对象实例 （运算操作符）
        /// </summary>
        /// <param name="op">对象操作符</param>
        public ExecutionItem(EnmOperators op)
        {
            this.ItemString = op.ToString();
            this.ItemType = ItemType.itOperator;
            this.ItemOperator = op;
            this.ItemParams = null;
        }
        /// <summary>
        /// 表达式解析对象实例 （函数）
        /// </summary>
        /// <param name="s">对象表达式</param>
        /// <param name="param">函数参数</param>
        public ExecutionItem(string s, StringCollection param)
        {
            this.ItemString = s;
            this.ItemType = ItemType.itFunction;
            this.ItemOperator = EnmOperators.Blank;
            this.ItemParams = param;
        }
        #endregion
    }
}
