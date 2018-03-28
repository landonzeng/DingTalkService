using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class GetEmployee
    {
        /// <summary>
        /// 
        /// </summary>
        public string errcode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string errmsg { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string userid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string name { get; set; }
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
        public string mobile { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string email { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string active { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string orderInDepts { get { return ""; } }
        /// <summary>
        /// 
        /// </summary>
        public string isAdmin { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string isBoss { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string dingId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string unionid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string isLeaderInDepts { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string isHide { get; set; }
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
        public string avatar { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string jobnumber { get; set; }
    }
}
