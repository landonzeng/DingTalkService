using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class V_DingTalk_DepartmentTree
    {
        public string DD_Id { get; set; }
        public string DepartmentId { get; set; }
        public string FullName { get; set; }
        public string ParentDepartmentId { get; set; }
        public int level { get; set; }
    }
}
