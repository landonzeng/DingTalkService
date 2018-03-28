using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace Model
{
    public class DepartmentResult
    {
        [SugarColumn(IsNullable = true)]
        public int errcode { get; set; }
        [SugarColumn(IsNullable = true, Length = 100)]
        public string errmsg { get; set; }
        [SugarColumn(IsNullable = true, Length = 100)]
        public string id { get; set; }
        [SugarColumn(IsNullable = false, Length = 50)]
        public string ESB_DepartmentID { get; set; }
        [SugarColumn(IsNullable = true, Length = 100)]
        public string ESB_DepartmentName { get; set; }
        private DateTime _nowDate = DateTime.Now;
        [SugarColumn(IsNullable = true)]
        public DateTime CreateDate
        {
            get { return _nowDate; }
            set { _nowDate = value; }
        }
    }
}
