using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Calculate.BLL.Func
{
    /// <summary>
    /// 扩展函数管理器
    /// </summary>
    public class FuncManager
    {
        public IFuncCalc Func(string name)
        {
            IFuncCalc funcCalc = null;
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                var types = assembly.GetTypes();

                foreach (var type in types)
                {
                    if (type.Name == name)
                    {
                        Type t = Type.GetType(type.ToString());
                        funcCalc = Activator.CreateInstance(t) as IFuncCalc;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return funcCalc;
        }
    }
}
