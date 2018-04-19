using Calculate.DAL;
using Calculate.DAL.Calc;
using Calculate.DAL.Express;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculate.BLL
{
    /// <summary>
    /// 计算器
    /// </summary>
    public class Calculator
    {

        #region 参数
        public readonly CalcStack calcStack;

        #endregion

        #region 构造函数
        public Calculator()
        {
            this.calcStack = new CalcStack();

        }
        #endregion

        #region public方法
        /// <summary>
        /// 计算表达式
        /// </summary>
        /// <param name="expr">表达式</param>
        /// <returns></returns>
        public Variant Calc(string expr)
        {
            try
            {
                var queue = new ExecutionQueue();
                var calcResult = this.DoCalc(queue);
                var result = this.GetVariant(calcResult);

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region private方法
        private Variant DoCalc(ExecutionQueue executionQueue)
        {
            if (executionQueue.Count == 0)
            {
                return new Variant("");
            }

            ExecutionItem executionItem;

            while (executionQueue.Count > 0)
            {
                executionItem = executionQueue.Dequeue();
                switch (executionItem.ItemType)
                {
                    case ItemType.itBool:
                        var value = string.Compare(executionItem.ItemString, "True", true) == 0;
                        calcStack.Push(new Variant(value));
                        break;
                    case ItemType.itString:
                        calcStack.Push(new Variant(executionItem.ItemString));
                        break;
                    case ItemType.itDigit:
                        if (int.TryParse(executionItem.ItemString, out int iv))
                        {
                            calcStack.Push(new Variant(iv));
                        }
                        else
                        {
                            if (!double.TryParse(executionItem.ItemString, out double dv))
                            {
                                throw new CalcException(string.Format("数值类型转换错误:{0}", executionItem.ItemString));
                            }

                            calcStack.Push(new Variant(dv));
                        }
                        break;
                    case ItemType.itDate:
                        if (!DateTime.TryParse(executionItem.ItemString, out DateTime dt))
                        {
                            throw new CalcException(string.Format("日期类型转换错误:{0}", executionItem.ItemString));
                        }
                        calcStack.Push(new Variant(dt));
                        break;
                    case ItemType.itOperator:
                        this.DoOperator(executionItem.ItemOperator, executionQueue);
                        break;
                    case ItemType.itFunction:
                        //this.doFunction(executionItem);
                        break;
                    case ItemType.itVariable:
                        calcStack.Push(new Variant(executionItem.ItemString));
                        break;
                    default:
                        throw new CalcException("队列存取错误");
                }
            }

            return calcStack.Pop();
        }

        /// <summary>
        /// 运算操作符计算
        /// </summary>
        /// <param name="enmOperators"></param>
        /// <param name="executionQueue"></param>
        private void DoOperator(EnmOperators enmOperators, ExecutionQueue executionQueue)
        {
            switch (enmOperators)
            {
                case EnmOperators.UnMinus:
                case EnmOperators.Nop:
                case EnmOperators.Not:
                case EnmOperators.UnPlus:
                    if (calcStack.Count < 1)
                    {
                        throw new CalcException("堆栈为空 " + enmOperators);
                    }
                    break;
                default:
                    if (calcStack.Count < 2)
                    {
                        throw new CalcException("堆栈为空 " + enmOperators);
                    }
                    break;
            }

            try
            {
                Variant variant = null;
                Variant varleft = null;
                Variant varright = null;
                Variant operandValue = null;
                switch (enmOperators)
                {
                    case EnmOperators.Not:
                        variant = calcStack.Pop();

                        // 逻辑非处理
                        calcStack.Push(operandValue);
                        break;
                    case EnmOperators.UnMinus:
                        variant = calcStack.Pop();

                        // 取反处理
                        calcStack.Push(operandValue);
                        break;
                    case EnmOperators.UnPlus:
                        break;
                    case EnmOperators.Plus: // 相加处理
                        GetTwoOperandValue(ref varleft, ref varright);
                        calcStack.Push(varright + varleft);
                        break;
                    case EnmOperators.Minus:// 相减处理
                        GetTwoOperandValue(ref varleft, ref varright);
                        calcStack.Push(varleft - varright);
                        break;
                    case EnmOperators.Mul: // 相乘处理
                        GetTwoOperandValue(ref varleft, ref varright);
                        calcStack.Push(varleft * varright);
                        break;
                    case EnmOperators.Div:// 相除处理
                        GetTwoOperandValue(ref varleft, ref varright);
                        calcStack.Push(varleft / varright);
                        break;
                    case EnmOperators.Gr: // 大于处理
                        GetTwoOperandValue(ref varleft, ref varright);
                        calcStack.Push(varleft > varright);
                        break;
                    case EnmOperators.Ls:// 小于处理
                        GetTwoOperandValue(ref varleft, ref varright);
                        calcStack.Push(varleft < varright);
                        break;
                    case EnmOperators.GrEq: // 大于等于处理
                        GetTwoOperandValue(ref varleft, ref varright);
                        calcStack.Push(varleft >= varright);
                        break;
                    case EnmOperators.LsEq:// 小于等于处理
                        GetTwoOperandValue(ref varleft, ref varright);
                        calcStack.Push(varleft <= varright);
                        break;
                    case EnmOperators.Eq:// 等于处理
                        GetTwoOperandValue(ref varleft, ref varright);
                        calcStack.Push(varleft == varright);
                        break;
                    case EnmOperators.NtEq: // 不等于处理
                        GetTwoOperandValue(ref varleft, ref varright);
                        calcStack.Push(varleft != varright);
                        break;
                    default:
                        throw new CalcException("操作符 " + enmOperators + " 错误");
                }
            }
            catch (Exception e)
            {
                string msg = string.Format("基础数据缺失:{0}", e.Message);
                // 此处应有日志记录异常
                throw new CalcException(msg);
            }
        }

        /// <summary>
        /// 取两个操作数的值
        /// </summary>
        /// <param name="leftValue">左边操作数</param>
        /// <param name="rightValue">右边操作数</param>
        private void GetTwoOperandValue(ref Variant leftValue, ref Variant rightValue)
        {
            var leftoperand = calcStack.Pop();
            var rightoperand = calcStack.Pop();

            leftValue = this.GetVariant(leftoperand);
            rightValue = this.GetVariant(rightoperand);
        }

        /// <summary>
        /// 获取表达式的值
        /// </summary>
        /// <param name="operand">表达式</param>
        /// <returns></returns>
        private Variant GetVariant(string operand)
        {
            if (string.IsNullOrWhiteSpace(operand) ||
                string.Compare(operand, "NULL", true) == 0 ||
                string.Compare(operand, "True", true) == 0 ||
                string.Compare(operand, "False", true) == 0)
            {
                return new Variant(operand);
            }

            if (double.TryParse(operand, out double dv))
            {
                return new Variant(dv);
            }

            return new Variant(operand);
        }

        #endregion
    }
}
