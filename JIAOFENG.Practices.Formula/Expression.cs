using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace JIAOFENG.Practices.Formula
{
    /// <summary>
    /// 表达式抽象类
    /// </summary>
    public abstract class Expression
    {
        #region 属性和字段

        private int index;
        protected int priority;
        protected int length;
        protected int childCountMax;
        protected int childCountMin;
        private Type valueType;
        private object expressionValue;
        public string ExpressionName;
        private List<Expression> childList = new List<Expression>();

        /// <summary>
        /// 列序号
        /// </summary>
        public int Index
        {
            get { return index; }
        }

        public int Priority
        {
            get { return priority; }
        }

        public int Length
        {
            get { return length; }
        }

        public Type ValueType
        {
            get { return valueType; }
            set { valueType = value; }
        }

        public object Value
        {
            get { return expressionValue; }
            set { expressionValue = value; }
        }

        public List<Expression> ChildList
        {
            get { return childList; }
        }

        #endregion

        public Expression(int index,int length)
        {
            this.index = index;
            this.length = length;
            this.SetPriority();
            this.SetChildCount();
        }

        #region 方法

        public abstract void Erpreter(DataTable vars);
        public abstract void SetPriority();
        public abstract void SetChildCount();

        #endregion

        #region 方法
        /// <summary>
        /// 重写ToString方法
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            //可以根据需要修改以显示不同的信息
            return this.GetType().Name + "_" + this.Value.ToString() + "_" + ValueType.ToString();
        }

        /// <summary>
        /// 检查下级数量
        /// </summary>
        /// <param name="errorInformation">下级数量不符时显示的错误信息</param>
        internal void CheckChildCount(string errorInformation)
        {
            if (this.ChildList.Count < this.childCountMin || this.ChildList.Count > this.childCountMax)
            {
                throw new SyntaxException(this.Index, this.Length, errorInformation);
            }
        }
        #endregion

        #region 转换公式值类型

        /// <summary>
        /// 将公式值转换为字符串类型
        /// </summary>
        internal string ChangeToString()
        {
            string strValue;
            strValue = (string)(this.Value = this.Value.ToString());
            this.ValueType = typeof(string);
            return strValue;
        }

        /// <summary>
        /// 将公式值转换为数字类型
        /// </summary>
        /// <param name="errorInformation">无法转换成数字时显示的错误信息</param>
        internal double ChangeToDouble(string errorInformation)
        {
            double dblValue;
            if (this.ValueType != typeof(double))
            {
                if (double.TryParse(this.Value.ToString(), out dblValue))
                    this.ValueType = typeof(double);
                else
                    throw new SyntaxException(this.index, this.length, errorInformation);
            }
            else
            {
                dblValue = Convert.ToDouble(this.Value);
            }
            return dblValue;
        }

        /// <summary>
        /// 将公式值转换为逻辑值
        /// </summary>
        internal bool ChangeToBoolean()
        {
            bool blnValue = false;
            if (this.ValueType == typeof(string))
            {
                switch (this.Value.ToString().Trim().ToLower())
                {
                    case "true":
                        blnValue = (bool)(this.Value = true);
                        break;
                    case "false":
                    case "":
                    default:
                        blnValue = (bool)(this.Value = false);
                        break;
                }
                this.ValueType = typeof(bool);
            }
            else if (this.ValueType == typeof(double))
            {
                blnValue = (bool)((Convert.ToInt32(this.Value) != 0) ? (this.Value = true) : (this.Value = false));
                this.ValueType = typeof(bool);
            }
            else if (this.ValueType == typeof(bool))
            {
                blnValue = (bool)this.Value;
            }
            else
            {
                blnValue = bool.Parse(this.Value.ToString().Trim());
            }
            return blnValue;
        }

        #endregion
    }
}
