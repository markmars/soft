using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MarkMars.Common
{
    public class 身份证号码
    {
        /// <summary>
        /// 是不是合法的18位身份证号码
        /// </summary>
        /// <param name="strId"></param>
        /// <returns></returns>
        public static bool IsValidIDNumber(string strId)
        {
            Regex regId = new Regex(@"^\d{17}(\d|x)$", RegexOptions.IgnoreCase);
            if (strId.Length != 18)
            {
                return false;
            }
            else
            {
                if (!regId.IsMatch(strId))
                {
                    return false;
                }
                else
                {
                    int iS = 0;
                    int[] iW = new int[] { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };
                    string LastCode = "10X98765432";
                    for (int i = 0; i < 17; i++)
                    {
                        iS += int.Parse(strId.Substring(i, 1)) * iW[i];
                    }
                    int iY = iS % 11;
                    if (strId.Substring(17, 1) != LastCode.Substring(iY, 1))
                        return false;
                    return true;
                }
            }
        }

        /// <summary>
        /// 把15位身份证号码升级为18位
        /// </summary>
        /// <param name="perIDSrc"></param>
        /// <returns></returns>
        public static string UpgradeFrom15to18(string perIDSrc)
        {
            int iS = 0;

            //加权因子常数 
            int[] iW = new int[] { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };
            //校验码常数 
            string LastCode = "10X98765432";
            //新身份证号 
            string perIDNew;

            perIDNew = perIDSrc.Substring(0, 6);
            //填在第6位及第7位上填上‘1’，‘9’两个数字 
            perIDNew += "19";

            perIDNew += perIDSrc.Substring(6, 9);

            //进行加权求和 
            for (int i = 0; i < 17; i++)
            {
                iS += int.Parse(perIDNew.Substring(i, 1)) * iW[i];
            }

            //取模运算，得到模值 
            int iY = iS % 11;
            //从LastCode中取得以模为索引号的值，加到身份证的最后一位，即为新身份证号。 
            perIDNew += LastCode.Substring(iY, 1);

            return perIDNew;
        }
    }
}
