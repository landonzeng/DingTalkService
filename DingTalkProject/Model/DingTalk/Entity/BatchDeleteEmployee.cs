using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class BatchDeleteEmployee
    {
        /// <summary>
        /// 员工UserID列表。列表长度在1到20之间
        /// </summary>
        public List<string> useridlist { get; set; }
    }
}
