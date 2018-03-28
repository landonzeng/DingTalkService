using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class DepartmentTrees
    {
        [SugarColumn(IsIdentity = true, IsPrimaryKey = true)]
        public int Id { get; set; }
        [SugarColumn(IsNullable = false, Length = 50)]
        public string DD_Id { get; set; }
        private string _parentId = "1";
        [SugarColumn(IsNullable = true, Length = 50)]
        public string DD_ParentId
        {
            get { return _parentId; }
            set { _parentId = value; }
        }
        [SugarColumn(IsNullable = false, Length = 50)]
        public string DepartmentId { get; set; }
        [SugarColumn(IsNullable = false, Length = 100, ColumnDataType = "nvarchar")]
        public string FullName { get; set; }
        [SugarColumn(IsNullable = true, Length = 50)]
        public string ParentDepartmentId { get; set; }
        [SugarColumn(IsNullable = true)]
        public int level { get; set; }
        private DateTime _nowDate = DateTime.Now;
        [SugarColumn(IsNullable = true)]
        public DateTime CreateDate
        {
            get { return _nowDate; }
            set { _nowDate = value; }
        }
    }
}
