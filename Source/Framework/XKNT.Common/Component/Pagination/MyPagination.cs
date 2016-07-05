using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using XKNT.Common.Helper;

namespace XKNT.Common.Component.Pagination
{
    /// <summary>
    /// 分页帮助类
    /// </summary>
    public class MyPagination : IPaginationSql
    {
        public string BGetRouteValue(Controller cb, string key)
        {
            if (cb.RouteData.Values.ContainsKey(key))
            {
                return cb.RouteData.Values[key].ToString();
            }
            else if (!string.IsNullOrEmpty(cb.Request[key]))
            {
                return cb.Request[key];
            }
            return "";
        }

        public MyPagination()
        {

        }
        public MyPagination(Controller cb)
        {
            this.PageIndex = TypeHelper.ToInt(BGetRouteValue(cb, "PageIndex"));
            this.PageRowCount = TypeHelper.ToInt(BGetRouteValue(cb, "PageRowCount"));
            this.OrderBy = BGetRouteValue(cb, "OrderBy");
        }

        string _GoPage = "goPage";
        /// <summary>
        ///默认 goPage
        /// </summary>
        public string GoPage
        {
            get { return _GoPage; }
            set { _GoPage = value; }
        }
        string _PaginationObject = "_PO";
        public string PaginationObject
        {
            get { return _PaginationObject; }
            set { _PaginationObject = value; }
        }
        public string PaginationID
        {
            get;
            set;
        }
        public string Path = "../Controls/Partial/MyPaginationControl";
        public string AjaxPath = "../Controls/Partial/MyAjaxPaginationControl";
        public string Url = "";
        public bool IsAjax = false;
        private int _PageCount;
        private int _PageRowCount = 10;
        private int _PageIndex = 1;
        private int _RowCount;
        /// <summary>
        /// 页数量
        /// </summary>
        public int PageCount
        {
            get { return _PageCount; }
        }
        /// <summary>
        /// 每页记录数
        /// </summary>
        public int PageRowCount
        {
            get { return _PageRowCount; }
            set
            {
                if (value > 0)
                {
                    _PageRowCount = value;
                }
            }
        }
        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex
        {
            get
            {
                if (_PageIndex == 0) return 1;
                return _PageIndex;
            }
            set { _PageIndex = value; }
        }
        /// <summary>
        /// 总记录数
        /// </summary>
        public int RowCount
        {
            get
            {
                return _RowCount;
            }
            set
            {
                _RowCount = value;
                _PageCount = (_RowCount - 1) / PageRowCount + 1;
            }
        }
        /// <summary>
        /// 开始记录号
        /// </summary>
        public int BeginRowIndex
        {
            get
            {
                return _PageRowCount * (PageIndex - 1) + 1;
            }
        }
        /// <summary>
        /// 是否是当前页
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool IsCurrentPage(int index)
        {
            return PageIndex == index;
        }
        /// <summary>
        /// 截止记录号
        /// </summary>
        public int EndRowIndex
        {
            get { return _PageRowCount * (PageIndex); }
        }
        /// <summary>
        /// 是否显示上一页 (分页控件） 
        /// </summary>
        public bool IsPrePage
        {
            get { return PageIndex > 1; }
        }
        /// <summary>
        /// 是否显示下一页 (分页控件)
        /// </summary>
        public bool IsNextPage
        {
            get { return PageIndex < PageCount; }
        }
        /// <summary>
        /// 转化为JSON
        /// </summary>
        /// <returns></returns>

        int _ViewPageCount;
        /// <summary>
        /// 分页链接数量
        /// </summary>
        public int ViewPageCount
        {
            get
            {
                if (_ViewPageCount == 0) return 10;
                return _ViewPageCount;
            }
            set { _ViewPageCount = value; }
        }
        /// <summary>
        ///  获取分页链接 分页索引列表
        /// </summary>
        /// <returns></returns>
        public List<int> GetPageList()
        {
            List<int> list = new List<int>();
            list.Add(PageIndex);
            int ViewCountNext = ViewPageCount / 2; //当前页后的 分页数
            for (int i = 0; i < ViewCountNext; i++)
            {
                if (list[i] + 1 <= PageCount)
                {
                    list.Add(list[i] + 1);
                }
                else
                {
                    break;
                }
            }
            int ViewCountPre = ViewPageCount - list.Count;//当前页前的分页数
            for (int i = 0; i < ViewCountPre; i++)
            {
                if (list[0] - 1 > 0)
                {
                    list.Insert(0, list[0] - 1);
                }
                else
                {
                    break;
                }
            }
            int ViewCountNext1 = ViewPageCount - list.Count;//总分页数不够，有后面补上
            for (int i = list.Count - 1; i < ViewPageCount - 1; i++)
            {
                if (list[i] + 1 <= PageCount)
                {
                    list.Add(list[i] + 1);
                }
                else
                {
                    break;
                }
            }
            return list;
        }


        public string sql
        {
            get;
            set;
        }


        public string sqlCount
        {
            get;
            set;
        }
        public string OrderBy
        {
            get;
            set;
        }

        public string getOrderFunction(string columnName, int orderType = 1)
        {
            string defaultOrderTypeStr = "asc";
            string orderTypeStr = "desc";
            if (orderType == 0)
            {
                defaultOrderTypeStr = "desc";
                orderTypeStr = "asc";
            }
            string orderByColumn = string.Format("{0} {1}", columnName, defaultOrderTypeStr);//  默认排序升序
            if (OrderBy.Equals(orderByColumn))
            {
                orderByColumn = string.Format("{0} {1}", columnName, orderTypeStr);//降序
            }
            return string.Format("getOrderPage('{0}')", orderByColumn);
        }
        public bool IsAll
        {
            get;

            set;
        }
    }
}
