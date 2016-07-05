using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace XKNT.Common.Helper
{
    public class StringHelper
    {
        #region 指定长度截取字符串
        /// <summary>
        /// 指定长度截取字符串
        /// </summary>
        /// <param name="orgText">字符串</param>
        /// <param name="max">最大长度</param>
        /// <returns></returns>
        public static string CutString(object obj, int max)
        {
            if (obj == null) return string.Empty;
            string str = obj.ToString();
            if (string.IsNullOrEmpty(str) || str.Trim() == string.Empty) return string.Empty;
            string result = string.Empty;
            int nByte = 0;
            char[] cArr = str.ToCharArray();
            int len = cArr.Length;
            for (int i = 0; i < len; i++)
            {
                int keyCode = Convert.ToInt32(cArr[i]);
                if (keyCode < 0 || keyCode >= 128)
                {
                    nByte += 2; // 2byte
                }
                else
                {
                    nByte++;
                }
                if (nByte <= max)
                {
                    result += str.Substring(i, 1);
                }
                else
                {
                    result = result.Trim() + "...";
                    break;
                }
            }
            return result;
        }
        #endregion

        #region 过滤非法字串
        public static string Filter(object str)
        {
            if (str == null || string.IsNullOrEmpty(str.ToString()))
            {
                return string.Empty;
            }
            string result = str.ToString();
            result = result.Replace("'", "''");
            result = result.Replace("%27", "''");
            result = result.Replace("%3B", "；");
            result = result.Replace("--", "——");
            result = result.Replace("<", "&lt;");
            result = result.Replace(">", "&gt;");
            return result.Trim();
        }
        #endregion

        #region 获取随机数（指定字符长度）
        public static string GetRndCode(int num)
        {
            string str = "0123456789"; //"ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            StringBuilder sb = new StringBuilder();
            var rnd = new Random(Guid.NewGuid().GetHashCode());
            for (int i = 0; i < num; i++)
            {
                sb.Append(str[rnd.Next(0, str.Length - 1)]);
            }
            return sb.ToString();
        }
        public static string GetStrByRnd(Random rnd, int num, string str)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < num; i++)
            {
                sb.Append(str[rnd.Next(0, str.Length - 1)]);
            }
            return sb.ToString();
        }
        public static string[] GetStrByRnd(int n)
        {
            string str = "0123456789";
            var rnd = new Random(Guid.NewGuid().GetHashCode());
            string[] arr = new string[n];
            for (int i = 0; i < n; i++)
            {
                arr[i] = GetStrByRnd(rnd, 16, str);
            }
            return arr;
        }
        public static string GetStrByRnd()
        {
            string str = "0123456789";
            var rnd = new Random(Guid.NewGuid().GetHashCode());
            return GetStrByRnd(rnd, 16, str);
        }
        #endregion

        #region 将流转换成字符串
        private const int BufferSize = 1024 * 8;
        /// <summary>
        /// 将流转换成字符串
        /// </summary>
        /// <param name="s">文件留</param>
        /// <returns>流的字符形式</returns>
        public static string ToBase64String(Stream s)
        {
            byte[] buff = null;
            StringBuilder rtnvalue = new StringBuilder();
            using (System.IO.BinaryReader br = new System.IO.BinaryReader(s))
            {
                do
                {
                    buff = br.ReadBytes(BufferSize);
                    rtnvalue.Append(Convert.ToBase64String(buff));
                } while (buff.Length != 0);
                br.Close();
            }
            return rtnvalue.ToString(); ;
        }
        #endregion

        #region 获取客户端IP
        public static string ClientIP
        {
            get
            {
                string result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrEmpty(result))
                {
                    result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
                if (string.IsNullOrEmpty(result))
                {
                    result = HttpContext.Current.Request.UserHostAddress;
                }
                return result;
            }
        }
        #endregion

        #region 获取网站根目录
        public static string RootPath
        {
            get
            {
                HttpContext context = HttpContext.Current;
                string urlPort = string.Empty;
                if (!context.Request.Url.IsDefaultPort)
                {
                    urlPort = ":" + context.Request.Url.Port.ToString();
                }
                //UrlBase的后面部分例如localhost/web
                string urlSuffix = context.Request.Url.Host + urlPort;
                if (context.Request.ApplicationPath != "/")
                {
                    urlSuffix += context.Request.ApplicationPath;
                }
                //UrlBase是安全的套接字还是普通的http
                string urlPrefix = "http://";
                if (context.Request.IsSecureConnection)
                {
                    urlPrefix = "https://";
                }
                return urlPrefix + urlSuffix;
            }
        }
        #endregion

        #region 计算时差
        public static string DateDiff(object DateTime, DateTime DateTime2)
        {
            string dateDiff = null;
            try
            {
                if (DateTime != null)
                {
                    DateTime DateTime1 = Convert.ToDateTime(DateTime);
                    //TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
                    //TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
                    //TimeSpan ts = ts1.Subtract(ts2).Duration();
                    TimeSpan ts = DateTime2 - DateTime1;
                    if (ts.Days >= 1)
                    {
                        dateDiff = DateTime1.Month.ToString() + "月" + DateTime1.Day.ToString() + "日";
                    }
                    else
                    {
                        if (ts.Hours > 1)
                        {
                            dateDiff = ts.Hours.ToString() + "小时前";
                        }
                        else
                        {
                            if (ts.Minutes >= 1)
                            {
                                dateDiff = ts.Minutes.ToString() + "分钟前";
                            }
                            else
                            {
                                dateDiff = ts.Seconds.ToString() + "秒前";
                            }
                        }
                    }
                }
            }
            catch
            { }
            return dateDiff;
        }
        #endregion

        #region 获取SQL中的in条件
        public static string GetSqlIn(IEnumerable<string> list)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var str in list)
            {
                sb.AppendFormat("'{0}',", Filter(str));
            }
            return sb.ToString().TrimEnd(',');
        }
        public static string GetSqlIn(IEnumerable<int> list)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var str in list)
            {
                sb.AppendFormat("{0},", str);
            }
            return sb.ToString().TrimEnd(',');
        }
        public static string GetSqlIn(IEnumerable<Int64> list)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var str in list)
            {
                sb.AppendFormat("{0},", str);
            }
            return sb.ToString().TrimEnd(',');
        }
        public static string GetSqlIn(IEnumerable<Guid> list)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var str in list)
            {
                sb.AppendFormat("'{0}',", str);
            }
            return sb.ToString().TrimEnd(',');
        }
        #endregion

        #region 拼音检索
        /// <summary>
        /// 得到首字母 
        /// </summary>
        /// <param name="cnChar"></param>
        /// <returns></returns>
        static public string getSpell(string cnChar)
        {
            byte[] arrCN = Encoding.Default.GetBytes(cnChar);
            if (arrCN.Length > 1)
            {
                int area = (short)arrCN[0];
                int pos = (short)arrCN[1];
                int code = (area << 8) + pos;
                int[] areacode = { 45217, 45253, 45761, 46318, 46826, 47010, 47297, 47614, 48119, 48119, 49062, 49324, 49896, 50371, 50614, 50622, 50906, 51387, 51446, 52218, 52698, 52698, 52698, 52980, 53689, 54481 };
                for (int i = 0; i < 26; i++)
                {
                    int max = 55290;
                    if (i != 25) max = areacode[i + 1];
                    if (areacode[i] <= code && code < max)
                    {
                        return Encoding.Default.GetString(new byte[] { (byte)(65 + i) });
                    }
                }
                return "";
            }
            else
                return cnChar;
        }
        /// <summary>
        /// 拼音检索
        /// </summary>
        /// <param name="strText"></param>
        /// <returns></returns>
        public static string GetChineseSpell(string strText)
        {
            int len = strText.Length;
            string myStr = "";
            for (int i = 0; i < len; i++)
            {
                myStr += getSpell(strText.Substring(i, 1));
            }
            return myStr;
        }
        #endregion

        #region 生成随机数字
        /// <summary>
        /// 生成4位验证码 长度不够则左边补0
        /// </summary>
        /// <param name="length">长度</param>
        /// <returns></returns>
        public static string GetCode(int length)
        {
            string code = string.Empty;
            //随即数生成4位
            Random rnd = new Random((int)DateTime.Now.Ticks);
            code = rnd.Next(0, 9999).ToString().PadLeft(length, '0');
            return code;
        }

        /// <summary>
        /// 生成6位验证码 长度不够则左边补0
        /// </summary>
        /// <param name="length">长度</param>
        /// <returns></returns>
        public static string GetCodeByLength(int length)
        {
            string code = string.Empty;
            //随即数生成6位
            Random rnd = new Random((int)DateTime.Now.Ticks);
            code = rnd.Next(0, 999999).ToString().PadLeft(length, '0');
            return code;
        }

        /// <summary>
        /// 生成6位验证码 长度不够则左边补0
        /// </summary>
        /// <param name="length">长度</param>
        /// <returns></returns>
        public static string GetTwoLengthCode(int length)
        {
            string code = string.Empty;
            //随即数生成2位
            Random rnd = new Random((int)DateTime.Now.Ticks);
            code = rnd.Next(0, 99).ToString().PadLeft(length, '0');
            return code;
        }

        #endregion

        #region MD5加密
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string MD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(System.Text.Encoding.Default.GetBytes(str));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                sBuilder.Append(result[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
        #endregion

        #region 获取GET请求的参数 组成数组

        /// <summary>
        /// 获取GET过来通知消息，并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        public static SortedDictionary<string, string> GetRequestGet()
        {
            int i = 0;
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
            NameValueCollection coll;
            coll = HttpContext.Current.Request.QueryString;

            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], HttpContext.Current.Request.QueryString[requestItem[i]]);
            }
            return sArray;
        }

        #endregion

        #region 获取POST请求的参数 组成数组
        /// <summary>
        /// 获取POST过来通知消息，并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        public static SortedDictionary<string, string> GetRequestPost()
        {
            int i = 0;
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
            NameValueCollection coll;
            coll = HttpContext.Current.Request.Form;

            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], HttpContext.Current.Request.Form[requestItem[i]]);
            }
            return sArray;
        }
        #endregion

        #region 生成不重复的随机数 (数字)
        public static string NewGuid
        {
            get
            {
                return System.Guid.NewGuid().ToString("N");
            }
        }
        private static Random GetRandomByRNG()
        {
            byte[] bytes = new byte[4];

            //使用加密服务提供程序 (CSP)提供的实现来实现加密随机数生成器 (RNG)。无法继承此类。
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(bytes);//用经过加密的强随机值序列填充字节数组。

            return new Random(BitConverter.ToInt32(bytes, 0));
        }
        /// <summary>
        /// 生成时间+流水号
        /// </summary>
        /// <returns></returns>
        public static string SerialNumber
        {
            get
            {
                var rnd = GetRandomByRNG();
                return DateTime.Now.ToString("yyyyMMddHHmmss") + rnd.Next(0, 9999).ToString().PadLeft(4, '0');
            }
        }
        #endregion

        #region 加解密
        private static string key = "c2s2bcheckstand";
        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="encryptString"></param>
        /// <returns></returns>
        public static string DesEncrypt(string encryptString)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key.Substring(0, 8));
            byte[] keyIV = keyBytes;
            byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, provider.CreateEncryptor(keyBytes, keyIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Convert.ToBase64String(mStream.ToArray());
        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="decryptString"></param>
        /// <returns></returns>
        public static string DesDecrypt(string decryptString)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key.Substring(0, 8));
            byte[] keyIV = keyBytes;
            byte[] inputByteArray = Convert.FromBase64String(decryptString);
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, provider.CreateDecryptor(keyBytes, keyIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Encoding.UTF8.GetString(mStream.ToArray());
        }
        #endregion

        #region 生成单号(订单号传空，配货单传P，任务单传R，退款单传K，退换货单传H，入库单传RK，出库单传CK，进货单传JH)
        /// <summary>
        /// 生成单号(订单号传空，配货单传P，任务单传R，退款单传K，退换货单传H，入库单传RK，出库单传CK，进货单传JH)
        /// </summary>
        /// <param name="type">订单号传空，配货单传P，任务单传R，退款单传K，退换货单传H，入库单传RK，进货单传JH，补贴结算BJ</param>
        /// <param name="type">订单号传空，配货单传P，任务单传R，退款单传K，退换货单传H，入库单传RK，出库单传CK，进货单传JH</param>
        /// <returns></returns>
        public static string GetNumber(string type)
        {
            Random rnd = new Random();
            return type + DateTime.Now.Year.ToString().Substring(3) + DateTime.Now.ToString("MMddHHmmssfff") + rnd.Next(0, 99).ToString().PadLeft(2, '0');
        }
        #endregion

        #region  返回时间段字符串
        public static string GetTimeStr(string time)
        {
            string str = "";
            DateTime dt = DateTime.Now; //当前时间
            //当日
            if (time == "day")
            {
                str = TypeHelper.GetDateString("{0:yyyy-MM-dd}", dt);
            }
            if (time == "week")//当周
            {

                str = TypeHelper.GetDateString("{0:yyyy-MM-dd}", dt.AddDays(1 - Convert.ToInt32(dt.DayOfWeek.ToString("d")))) + "至" + TypeHelper.GetDateString("{0:yyyy-MM-dd}", dt);

            }
            if (time == "month")//当月
            {
                str = TypeHelper.GetDateString("{0:yyyy-MM-dd}", dt.AddDays(1 - dt.Day)) + "至" + TypeHelper.GetDateString("{0:yyyy-MM-dd}", dt);
            }
            return str;
        }


        /// <summary>
        /// 计算某日起始日期（礼拜一的日期）
        /// </summary>
        /// <param name="someDate">该周中任意一天</param>
        /// <returns>返回礼拜一日期，后面的具体时、分、秒和传入值相等</returns>
        public static DateTime GetMondayDate(DateTime someDate)
        {
            int i = someDate.DayOfWeek - DayOfWeek.Monday;
            if (i == -1) i = 6;// i值 > = 0 ，因为枚举原因，Sunday排在最前，此时Sunday-Monday=-1，必须+7=6。
            TimeSpan ts = new TimeSpan(i, 0, 0, 0);
            return someDate.Subtract(ts);
        }
        /// <summary>
        /// 计算某日结束日期（礼拜日的日期）
        /// </summary>
        /// <param name="someDate">该周中任意一天</param>
        /// <returns>返回礼拜日日期，后面的具体时、分、秒和传入值相等</returns>
        public static DateTime GetSundayDate(DateTime someDate)
        {
            int i = someDate.DayOfWeek - DayOfWeek.Sunday;
            if (i != 0) i = 7 - i;// 因为枚举原因，Sunday排在最前，相减间隔要被7减。
            TimeSpan ts = new TimeSpan(i, 0, 0, 0);
            return someDate.Add(ts);
        }

        #endregion




    }
}
