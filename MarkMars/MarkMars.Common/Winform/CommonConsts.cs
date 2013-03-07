using System;

namespace MarkMars.Common
{
    public class CommonConsts
    {
        public static readonly String TrueLore_Int32_Regex = @"^-?[0-9]+$"; //整数
        public static readonly String TrueLore_UInt32_Regex = @"^[0-9]+$"; //正整数
        public static readonly String TrueLore_Float_Regex = @"^[0-9]+(\.[0-9]+)?$";//正浮点数
        public static readonly String TrueLore_Float1_Regex = @"^[0-9]+(\.[0-9]{1})?$"; //保存一位小数
        public static readonly String TrueLore_Fraction_Regex = @"\d+/\d+";//分数
        //获取应用目录
        public static readonly String TrueLore_ApplicationPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
    }
}
