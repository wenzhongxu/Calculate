using Calculate.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculate.BLL
{
    /// <summary>
    /// 表达式计算——运算符运算和函数运算
    /// </summary>
    public class ExpCalculate
    {
        /// <summary>
        /// 计算器
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public Variant Calc(string exp)
        {
            Calculator calc = new Calculator();

            Variant relust = calc.Calc(exp);

            return relust;
        }
    }
}
