using System.Collections;

namespace Calculate.DAL.Express
{
    /// <summary>
    /// 操作符栈
    /// </summary>
    public class OperatorStack : Stack
    {
        /// <summary>
        /// 操作符栈 构造函数
        /// </summary>
        public OperatorStack()
        {
        }
        /// <summary>
        /// 返回位于 System.Collections.Stack 顶部的对象但不将其移除。
        /// </summary>
        /// <returns></returns>
        public new EnmOperators Peek()
        {
            return (EnmOperators)base.Peek();
        }
        /// <summary>
        /// 移除并返回位于 System.Collections.Stack 顶部的对象。
        /// </summary>
        /// <returns></returns>
        public new EnmOperators Pop()
        {
            return (EnmOperators)base.Pop();
        }
        /// <summary>
        /// 将对象插入 System.Collections.Stack 的顶部。
        /// </summary>
        /// <param name="op"></param>
        public void Push(EnmOperators op)
        {
            base.Push(op);
        }
    }
}
