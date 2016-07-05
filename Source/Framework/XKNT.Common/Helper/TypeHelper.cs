using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace XKNT.Common.Helper
{
    public abstract class TypeHelper
    {
        #region 日期类型
        /// <summary>
        /// 判断是否为日期类型
        /// </summary>
        /// <param name="strDate"></param>
        /// <returns></returns>
        public static bool IsDate(string strDate)
        {
            DateTime result;
            return (DateTime.TryParse(strDate, out result));
        }

        /// <summary>
        /// 判断是否为数据库日期类型
        /// </summary>
        /// <param name="strDate"></param>
        /// <returns></returns>
        public static bool IsSqlDate(string strDate)
        {
            DateTime result;

            if (DateTime.TryParse(strDate, out result))
            {
                if (result >= DateTime.Parse("1900-01-01"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 把一个string转成DateTime类型，如果转不了，返回returnDate
        /// </summary>
        /// <param name="strDate"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime ToDate(string strDate, DateTime returnDate)
        {
            DateTime result;

            if (DateTime.TryParse(strDate, out result))
            {
                return result;
            }

            return returnDate;
        }

        /// <summary>
        /// 把一个string转成DateTime类型，如果转不了，返回null
        /// </summary>
        /// <param name="strDate"></param>
        /// <returns></returns>
        public static DateTime? ToDateOrNull(string strDate)
        {
            DateTime result;

            if (DateTime.TryParse(strDate, out result))
            {
                return result;
            }

            return null;
        }

        /// <summary>
        /// 获得日期格式字符串
        /// </summary>
        /// <param name="format">日期格式 {0:yyyyMMdd}</param>
        /// <param name="date">日期</param>
        /// <returns></returns>
        public static string GetDateString(string format, object date)
        {
            if (date == null || date.ToString() == "")
            {
                return string.Empty;
            }
            DateTime result;

            if (DateTime.TryParse(date.ToString(), out result))
            {
                return string.Format(format, result);
            }
            else
            {
                return string.Empty;
            }
        }

        public static string GetDateString(object date)
        {
            return GetDateString("{0:yyyy-MM-dd HH:mm:ss }", date);
        }

        public static string GetShortDate(object date)
        {
            return GetDateString("{0:yyyy-MM-dd}", date);
        }
        ///   <summary>
        ///   去除HTML标记
        ///   </summary>
        ///   <param   name=”NoHTML”>包括HTML的源码   </param>
        ///   <returns>已经去除后的文字</returns>
        public static string NoHTML(string Htmlstring)
        {
            //删除脚本
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "",
            RegexOptions.IgnoreCase);
            //删除HTML 
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "",
            RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "",
            RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"–>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!–.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"",
            RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&",
            RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<",
            RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">",
            RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", "   ",
            RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            Htmlstring.Replace("<", "");
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("\r\n", "");
            Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();
            return Htmlstring;
        }
        public static string GetDateToMinute(object date)
        {
            return GetDateString("{0:yyyy-MM-dd HH:mm}", date);
        }

        public static string GetYearMonth(object date)
        {
            return GetDateString("{0:yyyyMM}", date);
        }
        #endregion

        #region DataTable and DataSet

        /// <summary>
        /// 检查数据集ds是否为空
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static bool ContainData(DataSet ds)
        {
            return (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0);
        }
        /// <summary>
        /// 检查数据集dt是否为空
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static bool ContainData(DataTable dt)
        {
            return (dt != null && dt.Rows.Count > 0);
        }
        /// <summary>
        /// 把一个object转成DataTable类型，如果转不了或出错，返回null
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DataTable ToDataTable(object obj)
        {
            if (obj == null) return null;

            DataTable dt = null;
            if (obj is DataSet && ContainData((DataSet)obj))
            {
                dt = (obj as DataSet).Tables[0];
            }
            else if (obj is DataTable && ContainData((DataTable)obj))
            {
                dt = (DataTable)obj;
            }

            return dt;
        }
        #endregion

        #region 整型
        /// <summary>
        /// 把一个object转成int类型，如果转不了，返回i
        /// </summary>
        /// <param name="num">要转换的数据</param>
        /// <param name="i">返回值</param>
        /// <returns></returns>
        public static int ToInt(object num, int i)
        {
            if (num == null || num.ToString() == "") return i;

            int result;
            if (Int32.TryParse(num.ToString(), out result))
            {
                return result;
            }
            else
            {
                return i;
            }
        }
        public static int ToInt0(object num)
        {
            return ToInt(num, 0);
        }
        public static int ToInt(object num)
        {
            return ToInt(num, -1);
        }

        public static Int64 ToLong(object num, Int64 i)
        {
            if (num == null || num.ToString() == "") return i;

            Int64 result;
            if (Int64.TryParse(num.ToString(), out result))
            {
                return result;
            }
            else
            {
                return i;
            }
        }
        public static Int64 ToLong0(object num)
        {
            return ToLong(num, 0);
        }
        public static Int64 ToLong(object num)
        {
            return ToLong(num, -1);
        }

        #endregion

        public static object DBNullToNull(object obj)
        {
            return obj is DBNull ? null : obj;
        }

        public static string NullToEmpty(object obj)
        {
            return obj == null ? string.Empty : obj.ToString().Trim();
        }

        public static bool ContainData<T>(IEnumerable<T> list) where T : class
        {
            return list != null && list.Any();
        }
    }
}
