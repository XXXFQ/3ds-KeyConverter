using System;

namespace KeyConverter.Utils
{
    internal static class StringExtensions
    {
        /// <summary>
        /// 16進文字列か判定する
        /// </summary>
        /// <returns>16進文字列の場合trueを返す。そうでない場合はfalseを返す。</returns>
        public static bool IsHexString(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }
            foreach (char c in str)
            {
                if (!Uri.IsHexDigit(c))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
