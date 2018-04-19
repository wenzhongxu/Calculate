using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculate.DAL.Express
{
    /// <summary>
    /// 表达式对象队列
    /// </summary>
    public class ExecutionQueue:Queue
    {
        public ExecutionQueue()
        {
        }

        private new void Enqueue(object ob)
        {
        }

        public void Enqueue(ExecutionItem ei)
        {
            base.Enqueue(ei);
        }

        public new ExecutionItem Dequeue()
        {
            return (ExecutionItem)base.Dequeue();
        }

        public new ExecutionItem Peek()
        {
            return (ExecutionItem)base.Peek();
        }

        public new ExecutionQueue Clone()
        {
            var result = new ExecutionQueue();
            IEnumerator ieNum = this.GetEnumerator();
            while (ieNum.MoveNext())
            {
                result.Enqueue((ExecutionItem)ieNum.Current);
            }

            return result;
        }
    }
}
