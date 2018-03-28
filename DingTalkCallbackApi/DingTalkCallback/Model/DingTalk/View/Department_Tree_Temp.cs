using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Department_Tree_Temp
    {
        /// <summary>
        /// 部门id
        /// </summary>
        public string DepartmentId { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// 父id
        /// </summary>
        public string ParentId { get; set; }
        /// <summary>
        /// 是否有效(A:有效；I无效)
        /// </summary>
        public string Enabled { get; set; }
    }
}
