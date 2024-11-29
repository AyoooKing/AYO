using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using AYO.Link.Enums;

namespace AYO.Link.Common
{
    public static class StringExtensions
    {
        /// <summary>
        /// 扩展方法：将字符串转换为标题大小写（每个单词的首字母大写）
        /// </summary>
        /// <param name="str">需要的字符串</param>
        /// <returns>返回结果</returns>
        public static string ToTitleCase(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str; // 如果字符串为空或null，返回原字符串
            }

            // 使用TextInfo来转换每个单词的首字母为大写
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            return textInfo.ToTitleCase(str.ToLower()); // 转换并返回
        }

        /// <summary>
        /// 将十六进制字符串转换为ASCII字符串
        /// </summary>
        /// <param name="str">十六进制字符</param>
        /// <param name="encoding">编码规则</param>
        /// <returns></returns>
        public static string HexToText(this string str, Encoding encoding)
        {
            string rText = String.Empty;
            var bytes = new List<byte>();

            for (int i = 0; i < str.Length; i += 2)
            {
                var val = Convert.ToByte(str.Substring(i, 2), 16);
                if (val <= 31 || val == 127)
                {
                    if (bytes.Any())
                    {
                        rText += encoding.GetString(bytes.ToArray());
                        bytes.Clear();
                    }
                    rText += $"{EnumHelper.GetEnumDescription((ASCIIEnum)val)}";
                }
                else
                {
                    bytes.Add(val);
                }
            }

            if (bytes.Any())
            {
                rText += encoding.GetString(bytes.ToArray());
                bytes.Clear();
            }

            return rText;
        }
    }
}