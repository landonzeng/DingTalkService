using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class GetDepartmentForUserList
    {
        /// <summary>
        /// 
        /// </summary>
        public int errcode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string errmsg { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string hasMore { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<DepartmentForUserList> userlist { get; set; }
    }

    public class DepartmentForUserList
    {
        /// <summary>
        /// 
        /// </summary>
        public string userid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string dingId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string mobile { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string tel { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string workPlace { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string order { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool isAdmin { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool isBoss { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool isHide { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool isLeader { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool active { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<int> department { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string position { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string email { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string avatar { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string jobnumber { get; set; }
    }
}
