using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculate.DAL
{
    /// <summary>
    /// 计算引擎值转化
    /// </summary>
    [Serializable]
    public class Variant : IComparable
    {
        #region 变量
        private VariantType varType;
        private object oValue;
        public VariantType VarType { get { return this.varType; } }
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public Variant()
        {
            this.varType = VariantType.vtUnknow;
        }
        /// <summary>
        /// 对object类型进行类型转化实例
        /// </summary>
        /// <param name="source"></param>
        public Variant(object source)
        {
            if (source is decimal)
            {
                this.oValue = (decimal)source;
                this.varType = VariantType.vDecimal;
                return;
            }
            else if (source is bool)
            {
                this.oValue = (bool)source;
                this.varType = VariantType.vtBool;
                return;
            }
            else if (source is int)
            {
                this.oValue = (int)source;
                this.varType = VariantType.vtInt;
                return;
            }
            else if (source is double)
            {
                this.oValue = (double)source;
                this.varType = VariantType.vtDouble;
                return;
            }
            else if (source is DateTime)
            {
                this.oValue = (DateTime)source;
                this.varType = VariantType.vtDateTime;
                return;
            }
            else if (source is string)
            {
                this.oValue = (string)source;
                this.varType = VariantType.vtString;
                return;
            }
            else if (source is Variant)
            {
                this.oValue = ((Variant)source).oValue;
                this.varType = ((Variant)source).VarType;
                return;
            }
            else if (source is long)
            {
                this.oValue = (int)(long)source;
                this.varType = VariantType.vtInt;
                return;
            }
            else if (source is float)
            {
                this.oValue = (double)(float)source;
                this.varType = VariantType.vtDouble;
                return;
            }

            throw new CalcException("基础数据缺失");
        }
        /// <summary>
        /// 对Variant类型进行类型转化实例
        /// </summary>
        /// <param name="v"></param>
        public Variant(Variant v) : this((object)v) { }
        /// <summary>
        /// 对bool类型进行类型转化实例
        /// </summary>
        /// <param name="b"></param>
        public Variant(bool b) : this((object)b) { }
        /// <summary>
        /// 对int类型进行类型转化实例
        /// </summary>
        /// <param name="i"></param>
        public Variant(int i) : this((object)i) { }
        /// <summary>
        /// 对double类型进行类型转化实例
        /// </summary>
        /// <param name="d"></param>
        public Variant(double d) : this((object)d) { }
        /// <summary>
        /// 对string 类型进行类型转化实例
        /// </summary>
        /// <param name="s"></param>
        public Variant(string s) : this((object)s) { }
        /// <summary>
        /// 对DateTime 类型进行类型转化实例
        /// </summary>
        /// <param name="dt"></param>
        public Variant(DateTime dt) : this((object)dt) { }
        /// <summary>
        /// 对decimal 类型进行类型转化实例
        /// </summary>
        /// <param name="dt"></param>
        public Variant(decimal dt) : this((object)dt) { }
        #endregion

        #region 属性
        /// <summary>
        /// 判断值是否为null或者空白
        /// </summary>
        public bool IsNullOrBlank
        {
            get
            {
                string value = this.ToString();
                bool flag = string.IsNullOrWhiteSpace(value) || string.Compare(value, "NULL", true) == 0;
                return flag;
            }
        }
        /// <summary>
        /// 判断值是否为null
        /// </summary>
        public bool IsNull
        {
            get { return this.NullOrZeroCheck(false); }
        }
        /// <summary>
        /// 判断值是否为null或者零
        /// </summary>
        public bool IsNullOrZero
        {
            get { return this.NullOrZeroCheck(true); }
        }

        #endregion

        #region 重写函数
        /// <summary>
        /// 重写 TOSTRING 函数
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            switch (varType)
            {
                case VariantType.vtUnknow:
                    return "";
                case VariantType.vtBool:
                    return ((bool)oValue).ToString();
                case VariantType.vtInt:
                    return ((int)oValue).ToString();
                case VariantType.vtDouble:
                    return ((double)oValue).ToString();
                case VariantType.vtString:
                    return (string)oValue.ToString();
                case VariantType.vtDateTime:
                    return ((DateTime)oValue).ToString();
                case VariantType.vDecimal:
                    return ((decimal)oValue).ToString();
                default:
                    return "";
            }
        }

        /// <summary>
        /// 该hashcode函数无效，会返回一个object的哈希code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return oValue.GetHashCode();
        }

        /// <summary>
        /// 不能使用equals函数比较。会直接放回false
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public override bool Equals(object o)
        {
            return false;
        }

        #endregion

        /// <summary>
        /// 与obj进行大小比较(obj需要为double类型)
        /// </summary>
        /// <param name="obj">obj需要为double类型</param>
        /// <returns>比obj大则返回-1，比obj小则返回1，其他返回-2</returns>
        public int CompareTo(object obj)
        {
            Variant other = (Variant)obj;
            if (other.VarType == VariantType.vtDouble)
            {
                if (this - other > 0)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            }
            return -2;
        }


        #region 隐式操作
        /// <summary>
        /// Variant 转 bool型
        /// </summary>
        /// <param name="v"></param>
        public static implicit operator bool(Variant v)
        {
            switch (v.VarType)
            {
                case VariantType.vtBool:
                    return (bool)v.oValue;
                case VariantType.vtString: // ww 增加字符类型的逻辑判断
                    if (string.Compare(v.ToString(), "True", true) == 0)
                    {
                        return true;
                    }
                    else if (string.Compare(v.ToString(), "False", true) == 0)
                    {
                        return false;
                    }
                    else
                    {
                        throw new CalcException("基础数据缺失 " + v.VarType + " to bool");
                    }
                default:
                    throw new CalcException("基础数据缺失 " + v.VarType + " to bool");
            }
        }
        /// <summary>
        /// Variant 转 int型
        /// </summary>
        /// <param name="v"></param>
        public static implicit operator int(Variant v)
        {
            switch (v.VarType)
            {
                case VariantType.vtInt:
                    return (int)v.oValue;
                case VariantType.vtDouble:
                    return (int)(double)v.oValue;
                case VariantType.vDecimal:
                    return (int)(decimal)v.oValue;
                case VariantType.vtBool:
                    return ((bool)v.oValue) ? 1 : 0;
                case VariantType.vtString:
                    int dv;
                    if (int.TryParse(v, out dv))
                    {
                        return dv;
                    }
                    else
                    {
                        throw new CalcException("基础数据缺失 " + v.VarType + " to int");
                    }
                default:
                    throw new CalcException("基础数据缺失 " + v.VarType + " to int");
            }
        }
        /// <summary>
        /// Variant 转 double型
        /// </summary>
        /// <param name="v"></param>
        public static implicit operator double(Variant v)
        {
            switch (v.VarType)
            {
                case VariantType.vtInt:
                    return (double)(int)v.oValue;
                case VariantType.vtDouble:
                    return (double)v.oValue;
                case VariantType.vDecimal:
                    return (double)(decimal)v.oValue;
                case VariantType.vtBool:
                    return ((bool)v.oValue) ? 1d : 0d;
                case VariantType.vtString:
                    double dv;
                    if (double.TryParse(v, out dv))
                    {
                        return dv;
                    }
                    else
                    {
                        throw new CalcException("基础数据缺失 " + v.VarType + " to double");
                    }
                default:
                    throw new CalcException("基础数据缺失 " + v.VarType + " to double");
            }
        }
        /// <summary>
        /// Variant 转 DateTime型
        /// </summary>
        /// <param name="v"></param>
        public static implicit operator DateTime(Variant v)
        {
            switch (v.VarType)
            {
                case VariantType.vtDateTime:
                    return (DateTime)v.oValue;
                default:
                    throw new CalcException("基础数据缺失 " + v.VarType + " to DateTime");
            }
        }
        /// <summary>
        /// Variant 转 decimal型
        /// </summary>
        /// <param name="v"></param>
        public static implicit operator decimal(Variant v)
        {
            switch (v.VarType)
            {
                case VariantType.vDecimal:
                    return (decimal)v.oValue;
                default:
                    throw new CalcException("基础数据缺失 " + v.VarType + " to decimal");
            }
        }
        /// <summary>
        /// Variant 转 string型
        /// </summary>
        /// <param name="v"></param>
        public static implicit operator string(Variant v)
        {
            if (v.varType == VariantType.vtUnknow)
                return "";
            else
                return v.ToString();
        }
        /// <summary>
        /// string 转 Variant型
        /// </summary>
        /// <param name="s"></param>
        public static implicit operator Variant(string s)
        {
            return new Variant(s);
        }

        #endregion

        #region 运算操作符
        /// <summary>
        ///  Variant 的操作符 取反
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Variant operator -(Variant a)
        {
            switch (a.varType)
            {
                case VariantType.vtInt:
                    return new Variant(-(int)a.oValue);
                case VariantType.vtDouble:
                    return new Variant(-(double)a.oValue);
                case VariantType.vtBool:
                    return new Variant(!(bool)a.oValue);
                case VariantType.vDecimal:
                    return new Variant(-(decimal)a.oValue);
                default:
                    throw new CalcException("基础数据缺失");
            }
        }
        /// <summary>
        ///  Variant 的操作符 未知
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Variant operator !(Variant a)
        {
            return new Variant(-a);
        }
        /// <summary>
        ///  Variant 的操作符 未知
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Variant operator +(Variant a)
        {
            return new Variant(a);
        }
        /// <summary>
        ///  Variant 的操作符 相加
        /// </summary>
        /// <param name="right">右值</param>
        /// <param name="left">左值</param>
        /// <returns></returns>
        public static Variant operator +(Variant right, Variant left)
        {
            switch (left.varType)
            {
                case VariantType.vtBool:
                    switch (right.varType)
                    {
                        case VariantType.vtString:
                            return new Variant(left.ToString() + right.ToString());
                        default:
                            throw new CalcException("类型相加转换错误");
                    }
                case VariantType.vtInt:
                    switch (right.varType)
                    {
                        case VariantType.vtBool:
                            throw new CalcException("基础数据缺失");
                        case VariantType.vtInt:
                            return new Variant((int)left.oValue + (int)right.oValue);
                        case VariantType.vtDouble:
                            return new Variant((int)left.oValue + (double)right.oValue);
                        case VariantType.vtString:
                            return new Variant(left.ToString() + right.ToString());
                        case VariantType.vDecimal:
                            return new Variant(decimal.Parse(left.oValue.ToString()) + (decimal)right.oValue);
                        case VariantType.vtDateTime:
                            throw new CalcException("基础数据缺失");
                        default:
                            throw new CalcException("基础数据缺失");
                    }
                case VariantType.vtDouble:
                    switch (right.varType)
                    {
                        case VariantType.vtBool:
                            throw new CalcException("基础数据缺失");
                        case VariantType.vtInt:
                            return new Variant((double)left.oValue + (int)right.oValue);
                        case VariantType.vtDouble:
                            return new Variant((double)left.oValue + (double)right.oValue);
                        case VariantType.vDecimal:
                            return new Variant((decimal)(double)left.oValue + (decimal)right.oValue);
                        case VariantType.vtString:
                            return new Variant(left.ToString() + right.ToString());
                        case VariantType.vtDateTime:
                            throw new CalcException("基础数据缺失");
                        default:
                            throw new CalcException("基础数据缺失");
                    }
                case VariantType.vDecimal:
                    switch (right.varType)
                    {
                        case VariantType.vtBool:
                            throw new CalcException("基础数据缺失");
                        case VariantType.vtInt:
                            return new Variant((decimal)left.oValue + (int)right.oValue);
                        case VariantType.vtDouble:
                            return new Variant((decimal)left.oValue + (decimal)(double)right.oValue);
                        case VariantType.vDecimal:
                            return new Variant((decimal)left.oValue + (decimal)right.oValue);

                        case VariantType.vtString:
                            return new Variant(left.ToString() + right.ToString());
                        case VariantType.vtDateTime:
                            throw new CalcException("基础数据缺失");
                        default:
                            throw new CalcException("基础数据缺失");
                    }
                case VariantType.vtString:
                    switch (right.varType)
                    {
                        case VariantType.vtBool:
                        case VariantType.vtInt:
                        case VariantType.vtDouble:
                        case VariantType.vtString:
                        case VariantType.vtDateTime:
                        case VariantType.vDecimal:
                            return new Variant(left.ToString() + right.ToString());
                        default:
                            throw new CalcException("基础数据缺失");
                    }
                default:
                    throw new CalcException("基础数据缺失");
            }
        }
        /// <summary>
        ///  Variant 的操作符 减
        /// </summary>
        /// <param name="right">右值</param>
        /// <param name="left">左值</param>
        /// <returns></returns>
        public static Variant operator -(Variant right, Variant left)
        {
            switch (left.varType)
            {
                case VariantType.vtInt:
                    switch (right.varType)
                    {
                        case VariantType.vtInt:
                            return new Variant((int)left.oValue - (int)right.oValue);
                        case VariantType.vtDouble:
                            return new Variant((int)left.oValue - (double)right.oValue);
                        case VariantType.vDecimal:
                            return new Variant((int)left.oValue - (decimal)right.oValue);
                        default:
                            throw new CalcException("基础数据缺失");
                    }
                case VariantType.vtDouble:
                    switch (right.varType)
                    {
                        case VariantType.vtInt:
                            return new Variant((double)left.oValue - (int)right.oValue);
                        case VariantType.vtDouble:
                            return new Variant((double)left.oValue - (double)right.oValue);
                        case VariantType.vDecimal:
                            return new Variant((decimal)(double)left.oValue - (decimal)right.oValue);

                        default:
                            throw new CalcException("基础数据缺失");
                    }
                case VariantType.vDecimal:
                    switch (right.varType)
                    {
                        case VariantType.vtInt:
                            return new Variant((decimal)left.oValue - (int)right.oValue);
                        case VariantType.vtDouble:
                            return new Variant((decimal)left.oValue - (decimal)(double)right.oValue);
                        case VariantType.vDecimal:
                            return new Variant((decimal)left.oValue - (decimal)right.oValue);
                        default:
                            throw new CalcException("基础数据缺失");
                    }

                default:
                    throw new CalcException("基础数据缺失");
            }
        }
        /// <summary>
        ///  Variant 的操作符 乘 
        /// </summary>
        /// <param name="right">右值</param>
        /// <param name="left">左值</param>
        /// <returns></returns>
        public static Variant operator *(Variant right, Variant left)
        {
            switch (left.varType)
            {
                case VariantType.vtString:
                    switch (right.varType)
                    {
                        case VariantType.vtInt:
                            return new Variant(double.Parse(left.oValue.ToString()) * (int)right.oValue);
                        case VariantType.vtDouble:
                            return new Variant(double.Parse(left.oValue.ToString()) * (double)right.oValue);
                        case VariantType.vDecimal:
                            return new Variant(decimal.Parse(left.oValue.ToString()) * (decimal)right.oValue);

                        default:
                            throw new CalcException("基础数据缺失");
                    }
                case VariantType.vtInt:
                    switch (right.varType)
                    {
                        case VariantType.vtInt:
                            return new Variant((int)left.oValue * (int)right.oValue);
                        case VariantType.vtDouble:
                            return new Variant((int)left.oValue * (double)right.oValue);
                        case VariantType.vDecimal:
                            return new Variant((int)left.oValue * (decimal)right.oValue);
                        default:
                            throw new CalcException("基础数据缺失");
                    }
                case VariantType.vtDouble:
                    switch (right.varType)
                    {
                        case VariantType.vtInt:
                            return new Variant((double)left.oValue * (int)right.oValue);
                        case VariantType.vtDouble:
                            return new Variant((double)left.oValue * (double)right.oValue);
                        case VariantType.vDecimal:
                            return new Variant((decimal)(double)left.oValue * (decimal)right.oValue);

                        case VariantType.vtString:
                            return new Variant((double)left.oValue * double.Parse(right.oValue.ToString()));
                        default:
                            throw new CalcException("基础数据缺失");
                    }
                case VariantType.vDecimal:
                    switch (right.varType)
                    {
                        case VariantType.vtInt:
                            return new Variant((decimal)left.oValue * decimal.Parse(right.oValue.ToString()));
                        case VariantType.vtDouble:
                            return new Variant((decimal)left.oValue * (decimal)(double)(right.oValue));
                        case VariantType.vDecimal:
                            return new Variant((decimal)left.oValue * (decimal)(right.oValue));
                        case VariantType.vtString:
                            return new Variant((decimal)left.oValue * decimal.Parse(right.oValue.ToString()));
                        default:
                            throw new CalcException("基础数据缺失");
                    }
                default:
                    throw new CalcException("基础数据缺失");
            }
        }
        /// <summary>
        ///  Variant 的操作符 除 
        /// </summary>
        /// <param name="right">右值</param>
        /// <param name="left">左值</param>
        /// <returns></returns>
        public static Variant operator /(Variant right, Variant left)
        {
            switch (left.varType)
            {
                case VariantType.vtInt:
                    switch (right.varType)
                    {
                        case VariantType.vtInt:
                            if ((int)right.oValue == 0)
                                //return "分母为零";
                                throw new CalcException("分母为零，分子为[" + ((decimal)(int)left.oValue).ToString() + "]");
                            else
                                return new Variant((int)left.oValue / (int)right.oValue);
                        case VariantType.vtDouble:
                            if ((double)right.oValue == 0)
                                throw new CalcException("分母为零，分子为[" + ((decimal)(int)left.oValue).ToString() + "]");
                            else
                                return new Variant((int)left.oValue / (double)right.oValue);
                        case VariantType.vDecimal:
                            if ((decimal)right.oValue == 0)
                                throw new CalcException("分母为零，分子为[" + ((decimal)(int)left.oValue).ToString() + "]");
                            else
                                return new Variant((int)left.oValue / (decimal)right.oValue);

                        default:
                            throw new CalcException("基础数据缺失");
                    }
                case VariantType.vtDouble:
                    switch (right.varType)
                    {
                        case VariantType.vtInt:
                            if ((int)right.oValue == 0)
                                throw new CalcException("分母为零，分子为[" + ((decimal)(double)left.oValue).ToString() + "]");
                            else
                                return new Variant((double)left.oValue / (int)right.oValue);
                        case VariantType.vtDouble:
                            if ((double)right.oValue == 0)
                            {
                                throw new CalcException("分母为零，分子为[" + ((decimal)(double)left.oValue).ToString() + "]");
                            }
                            else
                                return new Variant((double)left.oValue / (double)right.oValue);
                        case VariantType.vDecimal:
                            if ((decimal)right.oValue == 0)
                                throw new CalcException("分母为零，分子为[" + ((decimal)(double)left.oValue).ToString() + "]");
                            else
                                return new Variant((decimal)(double)left.oValue / (decimal)right.oValue);
                        default:
                            throw new CalcException("基础数据缺失");
                    }
                case VariantType.vDecimal:
                    switch (right.varType)
                    {
                        case VariantType.vtInt:
                            if ((int)right.oValue == 0)
                                throw new CalcException("分母为零，分子为[" + ((decimal)left.oValue).ToString() + "]");
                            else
                                return new Variant((decimal)left.oValue / (int)right.oValue);
                        case VariantType.vDecimal:
                            if ((decimal)right.oValue == 0)
                                throw new CalcException("分母为零，分子为[" + ((decimal)left.oValue).ToString() + "]");
                            else
                                return new Variant((decimal)left.oValue / (decimal)right.oValue);
                        case VariantType.vtDouble:
                            if ((decimal)(double)right.oValue == 0)
                                throw new CalcException("分母为零，分子为[" + ((decimal)left.oValue).ToString() + "]");
                            else
                                return new Variant((decimal)left.oValue / (decimal)(double)right.oValue);
                        default:
                            throw new CalcException("基础数据缺失");
                    }
                default:
                    throw new CalcException("基础数据缺失");
            }
        }
        /// <summary>
        ///  Variant 的操作符 大于
        /// </summary>
        /// <param name="right">右值</param>
        /// <param name="left">左值</param>
        /// <returns></returns>
        public static Variant operator >(Variant right, Variant left)
        {
            switch (left.varType)
            {
                case VariantType.vtBool:
                    switch (right.varType)
                    {
                        case VariantType.vtBool:
                            if ((bool)left.oValue && !(bool)right.oValue)
                                return new Variant(true);
                            else
                                return new Variant(false);
                        default:
                            throw new CalcException("基础数据缺失");
                    }
                case VariantType.vtInt:
                    switch (right.varType)
                    {
                        case VariantType.vtString:
                            if (right.ToString().ToUpper() == "NULL" || right.ToString().ToUpper() == "")
                                return new Variant(false);
                            else
                                throw new CalcException("基础数据缺失");

                        case VariantType.vtBool:
                            throw new CalcException("基础数据缺失");
                        case VariantType.vtInt:
                            return new Variant((int)left.oValue > (int)right.oValue);
                        case VariantType.vtDouble:
                            return new Variant((int)left.oValue > (double)right.oValue);
                        case VariantType.vDecimal:
                            return new Variant((int)left.oValue > (decimal)right.oValue);
                        default:
                            throw new CalcException("基础数据缺失");
                    }
                case VariantType.vtDouble:
                    switch (right.varType)
                    {
                        case VariantType.vtString:
                            if (right.ToString().ToUpper() == "NULL" || right.ToString().ToUpper() == "")
                                return new Variant(false);
                            else
                                throw new CalcException("基础数据缺失");
                        case VariantType.vtInt:
                            return new Variant((double)left.oValue > (int)right.oValue);
                        case VariantType.vtDouble:
                            return new Variant((double)left.oValue > (double)right.oValue);
                        case VariantType.vDecimal:
                            return new Variant((decimal)(double)left.oValue > (decimal)right.oValue);
                        default:
                            throw new CalcException("基础数据缺失");
                    }
                case VariantType.vDecimal:
                    switch (right.varType)
                    {
                        case VariantType.vtString:
                            if (right.ToString().ToUpper() == "NULL" || right.ToString().ToUpper() == "")
                                return new Variant(false);
                            else
                                throw new CalcException("基础数据缺失");
                        case VariantType.vtInt:
                            return new Variant((decimal)left.oValue > (int)right.oValue);
                        case VariantType.vtDouble:
                            return new Variant((decimal)left.oValue > (decimal)(double)right.oValue);
                        case VariantType.vDecimal:
                            return new Variant((decimal)left.oValue > (decimal)right.oValue);
                        default:
                            throw new CalcException("基础数据缺失");
                    }
                case VariantType.vtString:
                    switch (right.varType)
                    {
                        case VariantType.vtString:
                            if (String.Compare(left.ToString(), right.ToString(), true) > 0)
                                return new Variant(true);
                            else
                                return new Variant(false);
                        case VariantType.vtDouble:
                        case VariantType.vDecimal:
                        case VariantType.vtInt:
                            if (left.ToString().ToUpper() == "NULL" || left.ToString().ToUpper() == "")
                            {
                                return new Variant(false);
                            }
                            else
                                throw new CalcException("基础数据缺失");
                        default:
                            throw new CalcException("基础数据缺失");
                    }
                case VariantType.vtDateTime:
                    switch (right.varType)
                    {
                        case VariantType.vtDateTime:
                            return new Variant((DateTime)left.oValue > (DateTime)right.oValue);
                        default:
                            throw new CalcException("基础数据缺失");
                    }
                default:
                    throw new CalcException("基础数据缺失");
            }
        }
        /// <summary>
        ///  Variant 的操作符 小于
        /// </summary>
        /// <param name="right">右值</param>
        /// <param name="left">左值</param>
        /// <returns></returns>
        public static Variant operator <(Variant right, Variant left)
        {
            switch (left.varType)
            {
                case VariantType.vtBool:
                    switch (right.varType)
                    {
                        case VariantType.vtBool:
                            if (!(bool)left.oValue && (bool)right.oValue)
                                return new Variant(true);
                            else
                                return new Variant(false);
                        default:
                            throw new CalcException("基础数据缺失");
                    }
                case VariantType.vtInt:
                    switch (right.varType)
                    {
                        case VariantType.vtString:
                            if (right.ToString().ToUpper() == "NULL" || right.ToString().ToUpper() == "")
                                return new Variant(true);
                            else
                                throw new CalcException("基础数据缺失");

                        case VariantType.vtBool:
                            throw new CalcException("基础数据缺失");
                        case VariantType.vtInt:
                            return new Variant((int)left.oValue < (int)right.oValue);
                        case VariantType.vtDouble:
                            return new Variant((int)left.oValue < (double)right.oValue);
                        case VariantType.vDecimal:
                            return new Variant((int)left.oValue < (decimal)right.oValue);
                        default:
                            throw new CalcException("基础数据缺失");
                    }
                case VariantType.vtDouble:
                    switch (right.varType)
                    {
                        case VariantType.vtString:
                            if (right.ToString().ToUpper() == "NULL" || right.ToString().ToUpper() == "")
                                return new Variant(true);
                            else
                                throw new CalcException("基础数据缺失");
                        case VariantType.vtInt:
                            return new Variant((double)left.oValue < (int)right.oValue);
                        case VariantType.vtDouble:
                            return new Variant((double)left.oValue < (double)right.oValue);
                        case VariantType.vDecimal:
                            return new Variant((decimal)(double)left.oValue < (decimal)right.oValue);
                        default:
                            throw new CalcException("基础数据缺失");
                    }
                case VariantType.vDecimal:
                    switch (right.varType)
                    {
                        case VariantType.vtString:
                            if (right.ToString().ToUpper() == "NULL" || right.ToString().ToUpper() == "")
                                return new Variant(true);
                            else
                                throw new CalcException("基础数据缺失");
                        case VariantType.vtInt:
                            return new Variant((decimal)left.oValue < (int)right.oValue);
                        case VariantType.vtDouble:
                            return new Variant((decimal)left.oValue < (decimal)(double)right.oValue);
                        case VariantType.vDecimal:
                            return new Variant((decimal)left.oValue < (decimal)right.oValue);
                        default:
                            throw new CalcException("基础数据缺失");
                    }
                case VariantType.vtString:
                    switch (right.varType)
                    {
                        case VariantType.vtDouble:
                        case VariantType.vtInt:
                        case VariantType.vDecimal:
                            if (left.ToString().ToUpper() == "NULL" || left.ToString().ToUpper() == "")
                            {
                                return new Variant(false);
                            }
                            else
                                throw new CalcException("基础数据缺失");
                        case VariantType.vtString:
                            if (String.Compare(left.ToString(), right.ToString(), true) < 0)
                                return new Variant(true);
                            else
                                return new Variant(false);
                        default:
                            throw new CalcException("基础数据缺失");
                    }
                case VariantType.vtDateTime:
                    switch (right.varType)
                    {
                        case VariantType.vtDateTime:
                            return new Variant((DateTime)left.oValue < (DateTime)right.oValue);
                        default:
                            throw new CalcException("基础数据缺失");
                    }
                default:
                    throw new CalcException("基础数据缺失");
            }
        }
        /// <summary>
        ///  Variant 的操作符 大于等于
        /// </summary>
        /// <param name="right">右值</param>
        /// <param name="left">左值</param>
        /// <returns></returns>
        public static Variant operator >=(Variant right, Variant left)
        {
            switch (left.varType)
            {
                case VariantType.vtBool:
                    switch (right.varType)
                    {
                        case VariantType.vtBool:
                            if (!(bool)left.oValue && (bool)right.oValue)
                                return new Variant(false);
                            else
                                return new Variant(true);
                        default:
                            throw new CalcException("基础数据缺失");
                    }
                case VariantType.vtInt:
                    switch (right.varType)
                    {
                        case VariantType.vtString:
                            if (right.ToString().ToUpper() == "NULL" || right.ToString().ToUpper() == "")
                                return new Variant(false);
                            else
                                throw new CalcException("基础数据缺失");

                        case VariantType.vtBool:
                            throw new CalcException("基础数据缺失");
                        case VariantType.vtInt:
                            return new Variant((int)left.oValue >= (int)right.oValue);
                        case VariantType.vtDouble:
                            return new Variant((int)left.oValue >= (double)right.oValue);
                        case VariantType.vDecimal:
                            return new Variant((int)left.oValue >= (decimal)right.oValue);
                        default:
                            throw new CalcException("基础数据缺失");
                    }
                case VariantType.vDecimal:
                    switch (right.varType)
                    {
                        case VariantType.vtString:
                            if (right.ToString().ToUpper() == "NULL" || right.ToString().ToUpper() == "")
                                return new Variant(false);
                            else
                                throw new CalcException("基础数据缺失");
                        case VariantType.vtInt:
                            return new Variant((decimal)left.oValue >= (int)right.oValue);
                        case VariantType.vtDouble:
                            return new Variant((decimal)left.oValue >= (decimal)(double)right.oValue);
                        case VariantType.vDecimal:
                            return new Variant((decimal)left.oValue >= (decimal)right.oValue);


                        default:
                            throw new CalcException("基础数据缺失");
                    }
                case VariantType.vtDouble:
                    switch (right.varType)
                    {
                        case VariantType.vtString:
                            if (right.ToString().ToUpper() == "NULL" || right.ToString().ToUpper() == "")
                                return new Variant(false);
                            else
                                throw new CalcException("基础数据缺失");

                        case VariantType.vtInt:
                            return new Variant((double)left.oValue >= (int)right.oValue);
                        case VariantType.vtDouble:
                            return new Variant((double)left.oValue >= (double)right.oValue);
                        case VariantType.vDecimal:
                            return new Variant((decimal)(double)left.oValue >= (decimal)right.oValue);
                        default:
                            throw new CalcException("基础数据缺失");
                    }
                case VariantType.vtString:
                    switch (right.varType)
                    {
                        case VariantType.vtDouble:
                        case VariantType.vtInt:
                        case VariantType.vDecimal:
                            if (left.ToString().ToUpper() == "NULL" || left.ToString().ToUpper() == "")
                            {
                                return new Variant(true);
                            }
                            else
                                throw new CalcException("基础数据缺失");
                        case VariantType.vtString:
                            if (String.Compare(left.ToString(), right.ToString(), true) >= 0)
                                return new Variant(true);
                            else
                                return new Variant(false);
                        default:
                            throw new CalcException("基础数据缺失");
                    }
                case VariantType.vtDateTime:
                    switch (right.varType)
                    {
                        case VariantType.vtDateTime:
                            return new Variant((DateTime)left.oValue >= (DateTime)right.oValue);
                        default:
                            throw new CalcException("基础数据缺失");
                    }
                default:
                    throw new CalcException("基础数据缺失");
            }
        }
        /// <summary>
        ///  Variant 的操作符 小于等于 
        /// </summary>
        /// <param name="right">右值</param>
        /// <param name="left">左值</param>
        /// <returns></returns>
        public static Variant operator <=(Variant right, Variant left)
        {
            switch (left.varType)
            {
                case VariantType.vtBool:
                    switch (right.varType)
                    {
                        case VariantType.vtBool:
                            if ((bool)left.oValue && !(bool)right.oValue)
                                return new Variant(false);
                            else
                                return new Variant(true);
                        default:
                            throw new CalcException("基础数据缺失");
                    }
                case VariantType.vtInt:
                    switch (right.varType)
                    {
                        case VariantType.vtString:
                            if (right.ToString().ToUpper() == "NULL" || right.ToString().ToUpper() == "")
                                return new Variant(true);
                            else
                                throw new CalcException("基础数据缺失");

                        case VariantType.vtBool:
                            throw new CalcException("基础数据缺失");
                        case VariantType.vtInt:
                            return new Variant((int)left.oValue <= (int)right.oValue);
                        case VariantType.vtDouble:
                            return new Variant((int)left.oValue <= (double)right.oValue);
                        case VariantType.vDecimal:
                            return new Variant((int)left.oValue <= (decimal)right.oValue);
                        default:
                            throw new CalcException("基础数据缺失");
                    }
                case VariantType.vtDouble:
                    switch (right.varType)
                    {
                        case VariantType.vtString:
                            if (right.ToString().ToUpper() == "NULL" || right.ToString().ToUpper() == "")
                                return new Variant(true);
                            else
                                throw new CalcException("基础数据缺失");

                        case VariantType.vtInt:
                            return new Variant((double)left.oValue <= (int)right.oValue);
                        case VariantType.vtDouble:
                            return new Variant((double)left.oValue <= (double)right.oValue);
                        case VariantType.vDecimal:
                            return new Variant((decimal)(double)left.oValue <= (decimal)right.oValue);
                        default:
                            throw new CalcException("基础数据缺失");
                    }
                case VariantType.vDecimal:
                    switch (right.varType)
                    {
                        case VariantType.vtString:
                            if (right.ToString().ToUpper() == "NULL" || right.ToString().ToUpper() == "")
                                return new Variant(true);
                            else
                                throw new CalcException("基础数据缺失");

                        case VariantType.vtInt:
                            return new Variant((decimal)left.oValue <= (int)right.oValue);
                        case VariantType.vtDouble:
                            return new Variant((decimal)left.oValue <= (decimal)(double)right.oValue);
                        case VariantType.vDecimal:
                            return new Variant((decimal)left.oValue <= (decimal)right.oValue);
                        default:
                            throw new CalcException("基础数据缺失");
                    }
                case VariantType.vtString:
                    switch (right.varType)
                    {
                        /////////////////////////////////格式刷配置NULL时的比较
                        case VariantType.vtDouble:
                        case VariantType.vDecimal:
                        case VariantType.vtInt:
                            if (left.ToString().ToUpper() == "NULL" || left.ToString().ToUpper() == "")
                            {
                                return new Variant(false);
                            }
                            else
                                throw new CalcException("基础数据缺失");
                        //////////////////////////////////////
                        case VariantType.vtString:
                            if (String.Compare(left.ToString(), right.ToString(), false) <= 0)
                                return new Variant(true);
                            else
                                return new Variant(false);
                        default:
                            throw new CalcException("基础数据缺失");
                    }
                case VariantType.vtDateTime:
                    switch (right.varType)
                    {
                        case VariantType.vtDateTime:
                            return new Variant((DateTime)left.oValue <= (DateTime)right.oValue);
                        default:
                            throw new CalcException("基础数据缺失");
                    }
                default:
                    throw new CalcException("基础数据缺失");
            }
        }
        /// <summary>
        ///  Variant 的操作符 相等
        /// </summary>
        /// <param name="right">右值</param>
        /// <param name="left">左值</param>
        /// <returns></returns>
        public static Variant operator ==(Variant right, Variant left)
        {
            switch (left.varType)
            {
                case VariantType.vtBool:
                    switch (right.varType)
                    {
                        case VariantType.vtBool:
                            if ((bool)left.oValue ^ (bool)right.oValue)
                                return new Variant(false);
                            else
                                return new Variant(true);
                        default:
                            throw new CalcException("基础数据缺失");
                    }
                case VariantType.vtInt:
                    switch (right.varType)
                    {
                        case VariantType.vtBool:
                            throw new CalcException("基础数据缺失");
                        case VariantType.vtInt:
                            return new Variant((int)left.oValue == (int)right.oValue);
                        case VariantType.vtDouble:
                            return new Variant((int)left.oValue == (double)right.oValue);
                        case VariantType.vDecimal:
                            return new Variant((int)left.oValue == (decimal)right.oValue);
                        default:
                            throw new CalcException("基础数据缺失");
                    }
                case VariantType.vDecimal:
                    switch (right.varType)
                    {
                        case VariantType.vtInt:
                            return new Variant((decimal)left.oValue == (int)right.oValue);
                        case VariantType.vDecimal:
                            return new Variant((decimal)left.oValue == (decimal)right.oValue);
                        case VariantType.vtDouble:
                            return new Variant((decimal)left.oValue == (decimal)(double)right.oValue);
                        default:
                            throw new CalcException("基础数据缺失");
                    }
                case VariantType.vtDouble:
                    switch (right.varType)
                    {
                        case VariantType.vtInt:
                            return new Variant((double)left.oValue == (int)right.oValue);
                        case VariantType.vtDouble:
                            return new Variant((double)left.oValue == (double)right.oValue);
                        case VariantType.vDecimal:
                            return new Variant((decimal)(double)left.oValue == (decimal)right.oValue);
                        default:
                            throw new CalcException("基础数据缺失");
                    }
                case VariantType.vtString:
                    switch (right.varType)
                    {
                        case VariantType.vtString:
                            if (String.Compare(left.ToString(), right.ToString(), true) == 0)
                                return new Variant(true);
                            else
                                return new Variant(false);
                        default:
                            throw new CalcException("基础数据缺失");
                    }
                case VariantType.vtDateTime:
                    switch (right.varType)
                    {
                        case VariantType.vtDateTime:
                            return new Variant((DateTime)left.oValue == (DateTime)right.oValue);
                        default:
                            throw new CalcException("基础数据缺失");
                    }
                default:
                    throw new CalcException("基础数据缺失");
            }
        }
        /// <summary>
        ///  Variant 的操作符 不相等
        /// </summary>
        /// <param name="right">右值</param>
        /// <param name="left">左值</param>
        /// <returns></returns>
        public static Variant operator !=(Variant right, Variant left)
        {
            switch (left.varType)
            {
                case VariantType.vtBool:
                    switch (right.varType)
                    {
                        case VariantType.vtBool:
                            if ((bool)left.oValue ^ (bool)right.oValue)
                                return new Variant(true);
                            else
                                return new Variant(false);
                        default:
                            throw new CalcException("基础数据缺失");
                    }
                case VariantType.vtInt:
                    switch (right.varType)
                    {
                        case VariantType.vtBool:
                            throw new CalcException("基础数据缺失");
                        case VariantType.vtInt:
                            return new Variant((int)left.oValue != (int)right.oValue);
                        case VariantType.vtDouble:
                            return new Variant((int)left.oValue != (double)right.oValue);
                        case VariantType.vDecimal:
                            return new Variant((int)left.oValue != (decimal)right.oValue);
                        default:
                            throw new CalcException("基础数据缺失");
                    }
                case VariantType.vtDouble:
                    switch (right.varType)
                    {
                        case VariantType.vtInt:
                            return new Variant((double)left.oValue != (int)right.oValue);
                        case VariantType.vtDouble:
                            return new Variant((double)left.oValue != (double)right.oValue);
                        case VariantType.vDecimal:
                            return new Variant((decimal)(double)left.oValue != (decimal)right.oValue);
                        default:
                            throw new CalcException("基础数据缺失");
                    }
                case VariantType.vDecimal:
                    switch (right.varType)
                    {
                        case VariantType.vtInt:
                            return new Variant((decimal)left.oValue != (int)right.oValue);
                        case VariantType.vtDouble:
                            return new Variant((decimal)left.oValue != (decimal)(double)right.oValue);
                        case VariantType.vDecimal:
                            return new Variant((decimal)left.oValue != (decimal)right.oValue);
                        default:
                            throw new CalcException("基础数据缺失");
                    }
                case VariantType.vtString:
                    switch (right.varType)
                    {
                        case VariantType.vtString:
                            if (String.Compare(left.ToString(), right.ToString(), true) != 0)
                                return new Variant(true);
                            else
                                return new Variant(false);
                        default:
                            throw new CalcException("基础数据缺失");
                    }
                case VariantType.vtDateTime:
                    switch (right.varType)
                    {
                        case VariantType.vtDateTime:
                            return new Variant((DateTime)left.oValue <= (DateTime)right.oValue);
                        default:
                            throw new CalcException("基础数据缺失");
                    }
                default:
                    throw new CalcException("基础数据缺失");
            }
        }

        #endregion

        #region 私有方法
        /// <summary>
        ///  判断该对象值是空还是零
        /// </summary>
        /// <param name="includeZero">是否包括零</param>
        /// <returns></returns>
        private bool NullOrZeroCheck(bool includeZero)
        {
            string value = this.ToString();

            bool flag = string.IsNullOrWhiteSpace(value);
            if (!flag)
            {
                if (double.TryParse(value, out double dv))
                {
                    flag = double.IsNaN(dv);
                    if (!flag && includeZero)
                    {
                        flag = Math.Abs(dv) <= 0.00000001;
                    }
                }
            }

            return flag;
        }

        #endregion
    }
}
