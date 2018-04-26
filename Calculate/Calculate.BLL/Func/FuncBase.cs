using Calculate.DAL;
using Calculate.DAL.Func;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculate.BLL.Func
{
    /// <summary>
    /// 扩展函数基类
    /// </summary>
    public abstract class FuncBase : IFuncCalc
    {
        /// <summary>
        /// 函数名称 默认为类名，可重载
        /// </summary>
        public virtual string Name
        {
            get { return this.GetType().Name; }
        }

        /// <summary>
        /// 函数分组
        /// </summary>
        public virtual string Catalog
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// 描述说明
        /// </summary>
        public virtual string Description
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// 参数验证方式
        /// </summary>
        public virtual ParamValidType ValidType
        {
            get { return ParamValidType.Fixed; }
        }

        /// <summary>
        /// 参数个数
        /// </summary>
        public virtual int ParamCount
        {
            get { return 0; }
        }

        public Variant Execute(FunctionDesc args, Calculator context)
        {
            if (args == null || context == null)
            {
                return new Variant(string.Empty);
            }

            // 扩展函数的参数有效性检测
            string msg = string.Empty;
            if (!this.CheckParam(args, ref msg))
            {
                throw new ArgumentException(msg);
            }

            // 执行计算
            try
            {
                var result = this.Calc(args, context);
                return result;
            }
            catch (Exception e)
            {
                // 记录并重新抛出异常
                string message = string.Format("函数:{0} 计算出错", this.Name);
                // log

                var throwException = e is CalcException ? e : new CalcException(message, e);
                throw throwException;
            }
        }

        protected abstract Variant Calc(FunctionDesc args, Calculator context);

        protected virtual bool CustomCheckParam(FunctionDesc args, ref string msg)
        {
            return true;
        }

        private bool CheckParam(FunctionDesc args, ref string errorMsg)
        {
            var passed = true;

            switch (this.ValidType)
            {
                case ParamValidType.Fixed:
                    passed = args.Count == this.ParamCount;
                    if (!passed)
                    {
                        errorMsg = string.Format("函数:{0} 需 {1}个参数，当前为{2}个参数", this.Name, this.ParamCount, args.Count);
                    }
                    break;
                case ParamValidType.Least:
                    passed = args.Count >= this.ParamCount;
                    if (!passed)
                    {
                        errorMsg = string.Format("函数:{0} 至少需{1}个参数，当前为{2}个参数", this.Name, this.ParamCount, args.Count);
                    }
                    break;
                case ParamValidType.Custom:
                    passed = this.CustomCheckParam(args, ref errorMsg);
                    break;
            }

            return passed;
        }

        /// <summary>
        /// 计算表达式的Double值
        /// </summary>
        /// <param name="expr">表达式</param>
        /// <param name="context">计算上下文</param>
        /// <returns>返回表达式的Double值</returns>
        public static double GetDoubleValue(string expr, Calculator context)
        {
            var exprValue = GetExpressionValue(expr);

            // 转换为Double值
            double result = double.NaN;
            if (!double.TryParse(exprValue, out result))
            {
                result = double.NaN;
            }

            return result;
        }

        public static Variant GetExpressionValue(string expr)
        {
            Calculator calculator = new Calculator();
            var exprValue = calculator.Calc(expr);
            return exprValue;
        }
    }
}
