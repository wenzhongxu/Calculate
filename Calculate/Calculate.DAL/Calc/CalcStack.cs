using System;
using System.Collections;

namespace Calculate.DAL.Calc
{
    /// <summary>
    /// 计算栈
    /// </summary>
    public class CalcStack : Stack
    {
        public CalcStack()
        {
        }

        /// <summary>
        /// 返回并移除栈顶的对象
        /// </summary>
        /// <returns></returns>
        public new Variant Pop()
        {
            return (Variant)base.Pop();
        }

        /// <summary>
        /// 返回但不移除栈顶对象
        /// </summary>
        /// <returns></returns>
        public new Variant Peek()
        {
            return (Variant)base.Peek();
        }

        /// <summary>
        /// 将对象压入栈
        /// </summary>
        /// <param name="v"></param>
        public void Push(Variant v)
        {
            base.Push(v);
        }

        private new void Push(Object o) { }
    }
}
