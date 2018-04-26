using Calculate.BLL.Func;
using Calculate.DAL;
using Calculate.DAL.Func;
using System.Collections.Generic;
using System.Linq;

namespace Calculate.BLL.Functions
{
    public class Sum : FuncBase
    {
        public override ParamValidType ValidType
        {
            get { return ParamValidType.Least; }
        }

        public override int ParamCount
        {
            get { return 1; }
        }

        protected override Variant Calc(FunctionDesc args, Calculator context)
        {
            var sampleList = new List<double>();

            string expr = string.Empty;
            for (int i = 0; i < args.Count; i++)
            {
                expr = args[i];
                try
                {
                    var exprValue = GetDoubleValue(expr, context);
                    if (!double.IsNaN(exprValue))
                    {
                        sampleList.Add(exprValue);
                    }
                }
                catch (System.Exception ex)
                {

                }
            }

            var result = sampleList.Count > 0 ? sampleList.Sum() : double.NaN;
            return new Variant(result);
        }
    }
}
