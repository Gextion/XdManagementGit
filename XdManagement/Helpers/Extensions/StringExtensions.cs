using System;
using System.Text;

namespace EficienciaEnergetica.Helpers.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// To Base64
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToBase64(this string value)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(value);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        /// <summary>
        /// From Base64
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string FromBase64(this string value)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(value);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        /// <summary>
        /// Cast String To Int
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static int ToInt(this string Value, int DefaultValue = 0)
        {
            int NewValue = DefaultValue;

            if (!string.IsNullOrEmpty(Value))
            {   
                int.TryParse(Value, out NewValue);
            }

            return NewValue;
        }

        /// <summary>
        /// Cast String To Long
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static long ToLong(this string Value, long DefaultValue = 0)
        {
            long NewValue = DefaultValue;

            if (!string.IsNullOrEmpty(Value))
            {
                long.TryParse(Value, out NewValue);
            }

            return NewValue;
        }
    }
}