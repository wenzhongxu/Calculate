using Calculate.DAL;
using Calculate.DAL.Func;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculate.BLL.Func
{
    public interface IFuncCalc
    {
        /// <summary>
        /// 函数名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 函数分组
        /// </summary>
        string Catalog { get; }

        /// <summary>
        /// 描述说明
        /// </summary>
        string Description { get; }

        /// <summary>
        /// 参数验证方式
        /// </summary>
        ParamValidType ValidType { get; }

        /// <summary>
        /// 参数个数
        /// </summary>
        int ParamCount { get; }

        /// <summary>
        /// 计算入口
        /// </summary>
        /// <param name="args">传入的参数</param>
        /// <param name="CalcEng">计算上下文</param>
        /// <returns></returns>
        Variant Execute(FunctionDesc args, Calculator context);
    }
}
