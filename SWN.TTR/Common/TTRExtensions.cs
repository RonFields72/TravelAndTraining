using SWN.TTR.Validation;
using System;

namespace SWN.TTR.Common
{
    /// <summary>
    /// Contains extension methods for TTR.
    /// </summary>
    public static class TTRExtensions
    {
        /// <summary>
        /// Converts a null value to empty string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>Returns an empty string for null values; otherwise, it returns the value.</returns>
        public static string NullToEmptyString(this System.String value)
        {
            return (value == null ? string.Empty : value);
        }

        /// <summary>
        /// Converts a boolean flag to "Yes" and "No".
        /// </summary>
        /// <param name="value">if set to <c>true</c> "Yes"; otherwise "No".</param>
        /// <returns>"Yes" or "No".</returns>
        public static string ToYesNo(this System.Boolean value)
        {
            return (value ? "Yes" : "No");
        }

        /// <summary>
        /// Determines whether the specified value is default.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// 	<c>true</c> if the specified value is default; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsDefault(this System.DateTime value)
        {
            if (System.DateTime.Compare(value, BizObjectValidator.DEFAULT_DATETIME) == 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Determines whether the specified value is between the given start date and end date.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="inclusive">if set to <c>true</c> the start and end dates are included; otherwise, they are excluded.</param>
        /// <returns>
        /// 	<c>true</c> if the specified value is between the start and end dates; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsBetween(this System.DateTime value, System.DateTime startDate, System.DateTime endDate, bool inclusive)
        {
            if (inclusive)
            {
                if (DateTime.Compare(value, startDate) >= 0 &&
                    DateTime.Compare(value, endDate) <= 0)
                    return true;
                else
                    return false;
            }
            else
            {
                if (DateTime.Compare(value, startDate) > 0 &&
                    DateTime.Compare(value, endDate) < 0)
                    return true;
                else
                    return false;
            }            
        }
    }
}