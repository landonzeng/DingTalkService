using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class GetDepartmentList
    {
        public int errcode { get; set; }
        public string errmsg { get; set; }
        public List<Department> department { get; set; }
    }
}
