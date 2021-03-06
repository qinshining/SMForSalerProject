using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SMProject
{
    class DataValidate
    {
        /// <summary>
        /// 判断是否非负整数
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static bool IsInteger(string txt)
        {
            Regex regex = new Regex(@"^\d+$");
            return regex.IsMatch(txt);
        }
        /// <summary>
        /// 判断是否非负数值
        /// </summary>
        /// <returns></returns>
        public static bool IsDecimalNum(string txt)
        {
            Regex regex = new Regex(@"^\d+\.?\d*$");
            return regex.IsMatch(txt);
        }
    }
}
