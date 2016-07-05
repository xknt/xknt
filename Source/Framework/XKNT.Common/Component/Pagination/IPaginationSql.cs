using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XKNT.Common.Component
{
    public interface IPaginationSql
    {
        bool IsAll
        {
            get;
            set;
        }
        int BeginRowIndex
        {
            get;
        }
        int EndRowIndex
        {
            get;
        }
        int RowCount
        {
            set;
        }
        string sql
        {
            get;
        }
        string sqlCount
        {
            get;
            set;
        }
        string OrderBy
        {
            get;
        }
    }
}
