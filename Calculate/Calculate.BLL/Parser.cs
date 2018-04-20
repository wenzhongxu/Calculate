using Calculate.DAL.Express;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculate.BLL
{
    /// <summary>
    /// 表达式解析器
    /// </summary>
    public class Parser
    {
        #region 参数
        /// <summary>
        ///  操作符栈
        /// </summary>
        private OperatorStack scOp;
        /// <summary>
        ///  表达式对象队列
        /// </summary>
        private ExecutionQueue eqResult;
        /// <summary>
        /// 
        /// </summary>
        private static char[] strDividers = { '+', '-', '*', '/', '(', ')', '<', '>', '\n', '\t', ' ', '!', '&', '|', '=' };
        #endregion


        #region 公开方法
        /// <summary>
        ///  解析表达式
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static ExecutionQueue Execute(string expression, string NotOperator = "")
        {
            var parser = new Parser();
            var result = parser.ParseIt(expression, NotOperator);


            return result;
        }
        /// <summary>
        /// 解析表达式 
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public ExecutionQueue ParseIt(string expression, string NotOperator = "")
        {
            this.scOp = new OperatorStack();
            this.scOp.Clear();

            this.eqResult = new ExecutionQueue();
            string strSrc = expression;

            int pos = 0;
            bool isFirstOperator = true;
            EnmOperators opCurrent;
            while (pos < strSrc.Length)
            {
                opCurrent = this.IsOperator(strSrc, isFirstOperator, ref pos);
                if (opCurrent != EnmOperators.Nop)
                {
                    if (opCurrent != EnmOperators.Blank)
                    {
                        this.OperatorStackManager(opCurrent);

                        if (opCurrent == EnmOperators.RightPar)
                        {
                            isFirstOperator = false;
                        }
                        else
                        {
                            isFirstOperator = true;
                        }
                    }

                    continue;
                }

                var item = this.GetOperand(strSrc, ref pos);
                eqResult.Enqueue(item);
                isFirstOperator = false;
            }

            if (string.IsNullOrEmpty(NotOperator))
            {
                while (scOp.Count > 0)
                {
                    eqResult.Enqueue(new ExecutionItem(scOp.Pop()));
                }
            }

            return this.eqResult;
        }

        #endregion

        #region 私有方法 private 方法
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="isFirstOperator"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        private EnmOperators IsOperator(string s, bool isFirstOperator, ref int pos)
        {
            var curOperator = EnmOperators.Nop;
            char c = s[pos];
            switch (c)
            {
                case ' ':
                case '\n':
                case '\t':
                    pos++;
                    curOperator = EnmOperators.Blank;
                    break;
                case '+':
                    curOperator = isFirstOperator ? EnmOperators.UnPlus : EnmOperators.Plus;
                    pos++;
                    break;
                case '-':
                    curOperator = isFirstOperator ? EnmOperators.UnMinus : EnmOperators.Minus;
                    pos++;
                    break;
                case '*':
                    curOperator = EnmOperators.Mul;
                    pos++;
                    break;
                case '/':
                    curOperator = EnmOperators.Div;
                    pos++;
                    break;
                case '=':
                    curOperator = EnmOperators.Eq;
                    pos++;
                    break;
                case '(':
                    curOperator = EnmOperators.LeftPar;
                    pos++;
                    break;
                case ')':
                    curOperator = EnmOperators.RightPar;
                    pos++;
                    break;
                case '&':
                    curOperator = EnmOperators.And;
                    pos++;
                    break;
                case '|':
                    curOperator = EnmOperators.Or;
                    pos++;
                    break;
                case '!':
                    if ((s.Length > (pos + 1)) && (s[pos + 1] == '='))
                    {
                        curOperator = EnmOperators.NtEq;
                        pos += 2;
                    }
                    else
                    {
                        curOperator = EnmOperators.Not;
                        pos++;
                    }
                    break;
                case '>':
                    if ((s.Length > (pos + 1)) && (s[pos + 1] == '='))
                    {
                        curOperator = EnmOperators.GrEq;
                        pos += 2;
                    }
                    else
                    {
                        curOperator = EnmOperators.Gr;
                        pos++;
                    }
                    break;
                case '<':
                    if ((s.Length > (pos + 1)) && (s[pos + 1] == '='))
                    {
                        curOperator = EnmOperators.LsEq;
                        pos += 2;
                    }
                    else
                    {
                        if ((s.Length > (pos + 1)) && (s[pos + 1] == '>'))
                        {
                            curOperator = EnmOperators.NtEq;
                            pos += 2;
                        }
                        else
                        {
                            curOperator = EnmOperators.Ls;
                            pos++;
                        }
                    }
                    break;
            }

            return curOperator;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="op"></param>
        /// <returns></returns>
        private int GetPrivelege(EnmOperators op)
        {
            switch (op)
            {
                case EnmOperators.LeftPar:
                    return 0;
                case EnmOperators.RightPar:
                    return 1;
                case EnmOperators.Eq:
                case EnmOperators.NtEq:
                case EnmOperators.GrEq:
                case EnmOperators.LsEq:
                case EnmOperators.Gr:
                case EnmOperators.Ls:
                    return 2;
                case EnmOperators.Plus:
                case EnmOperators.Minus:
                case EnmOperators.Or:
                    return 3;
                case EnmOperators.Mul:
                case EnmOperators.Div:
                case EnmOperators.And:
                    return 4;
                case EnmOperators.UnMinus:
                case EnmOperators.UnPlus:
                case EnmOperators.Not:
                    return 5;
                default:
                    return 5;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="op"></param>
        private void OperatorStackManager(EnmOperators op)
        {
            if (scOp.Count == 0)
            {
                scOp.Push(op);
                return;
            }

            if (op == EnmOperators.LeftPar)
            {
                scOp.Push(op);
                return;
            }

            if (op == EnmOperators.RightPar)
            {
                while (scOp.Count > 0)
                {
                    EnmOperators opStack = scOp.Pop();
                    if (opStack != EnmOperators.LeftPar)
                    {
                        eqResult.Enqueue(new ExecutionItem(opStack));
                    }
                    else
                    {
                        return;
                    }
                }

                return;
            }

            while (scOp.Count > 0)
            {
                EnmOperators opStack = scOp.Peek();
                if (GetPrivelege(op) <= GetPrivelege(opStack))
                {
                    eqResult.Enqueue(new ExecutionItem(scOp.Pop()));
                }
                else
                {
                    break;
                }
            }

            scOp.Push(op);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strSrc"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        private ExecutionItem GetOperand(string strSrc, ref int pos)
        {
            char c = strSrc[pos];

            /* todo: ww 针对成对出现的字符，缺失后续字符的判断
             * 如 [A.B]、''、""、#2015-01-01#
            */

            string itemString = string.Empty;
            switch (c)
            {
                case '\"':
                    itemString = this.GetString(strSrc, ref pos);
                    return new ExecutionItem(ItemType.itString, itemString);
                case '\'':
                    itemString = this.GetString2(strSrc, ref pos);
                    return new ExecutionItem(ItemType.itString, itemString);
                case '[':
                    itemString = this.GetVariable(strSrc, ref pos);
                    return new ExecutionItem(ItemType.itVariable, itemString);
                case '#':
                    itemString = this.GetDate(strSrc, ref pos);
                    return new ExecutionItem(ItemType.itDate, itemString);
                case '.':
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    itemString = this.GetNumber(strSrc, ref pos);
                    return new ExecutionItem(ItemType.itDigit, itemString);
            }

            int i = strSrc.IndexOfAny(strDividers, pos);
            if (i < 0)
            {
                i = strSrc.Length;
            }

            string s = strSrc.Substring(pos, i - pos);
            pos = i;
            if ((pos < strSrc.Length) && (strSrc[pos] == '('))
            {
                var paramList = this.GetFunction(strSrc, ref pos);
                return new ExecutionItem(s, paramList);
            }

            if (String.Compare(s, "true", true) == 0)
            {
                return new ExecutionItem(true);
            }

            if (String.Compare(s, "false", true) == 0)
            {
                return new ExecutionItem(false);
            }

            return new ExecutionItem(ItemType.itString, s);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strSrc"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        private string GetString(string strSrc, ref int pos)
        {
            int oldPos = pos;
            pos++;
            while ((pos < strSrc.Length) && (strSrc[pos] != '\"'))
            {
                pos++;
            }
            pos++;

            var result = strSrc.Substring(oldPos + 1, pos - oldPos - 2);
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strSrc"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        private string GetString2(string strSrc, ref int pos)
        {
            int oldPos = pos;
            pos++;
            while ((pos < strSrc.Length) && (strSrc[pos] != '\''))
            {
                pos++;
            }
            pos++;

            var result = strSrc.Substring(oldPos + 1, pos - oldPos - 2);
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strSrc"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        private string GetDate(string strSrc, ref int pos)
        {
            int oldPos = pos;
            pos++;
            while ((pos < strSrc.Length) && (strSrc[pos] != '#'))
            {
                pos++;
            }
            pos++;

            var result = strSrc.Substring(oldPos + 1, pos - oldPos - 2);
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strSrc"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        private string GetVariable(string strSrc, ref int pos)
        {
            int oldPos = pos;
            int lrBr = 1;
            pos++;
            while ((pos < strSrc.Length))
            {
                switch (strSrc[pos])
                {
                    case '[':
                        lrBr++;
                        break;
                    case ']':
                        lrBr--;
                        break;
                }
                pos++;

                if (lrBr == 0)
                {
                    break;
                }
            }

            var result = strSrc.Substring(oldPos + 1, pos - oldPos - 2).Trim(); // ww 去除前后的空格
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strSrc"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        private string GetNumber(string strSrc, ref int pos)
        {
            int oldPos = pos;
            while ((pos < strSrc.Length) && (Char.IsDigit(strSrc[pos]) || (strSrc[pos] == '.')))
            {
                pos++;
            }

            var result = strSrc.Substring(oldPos, pos - oldPos);
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strSrc"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        private StringCollection GetFunction(string strSrc, ref int pos)
        {
            var paramList = new StringCollection();
            string param = string.Empty;

            int oldPos = ++pos;
            bool noStr = true;
            int parentLevel = 1;

            while (!((strSrc[pos] == ')') && (parentLevel == 1) && noStr) && (strSrc.Length > pos))
            {
                switch (strSrc[pos])
                {
                    case '\"':
                        noStr = !noStr;
                        break;
                    case '(':
                        if (noStr)
                        {
                            parentLevel++;
                        }
                        break;
                    case ')':
                        if (noStr)
                        {
                            parentLevel--;
                        }
                        break;
                    case ',':
                        if ((parentLevel == 1) && noStr)
                        {
                            param = strSrc.Substring(oldPos, pos - oldPos);
                            paramList.Add(param);
                            oldPos = pos + 1;
                            break;
                        }
                        break;
                }
                pos++;
            }

            param = strSrc.Substring(oldPos, pos - oldPos);
            if (!string.IsNullOrWhiteSpace(param))
            {
                paramList.Add(param);
            }

            pos++;

            return paramList;
        }
        #endregion

    }
}
