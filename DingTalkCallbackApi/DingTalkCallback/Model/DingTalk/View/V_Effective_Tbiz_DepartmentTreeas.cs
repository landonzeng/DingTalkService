using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class V_Effective_Tbiz_DepartmentTreeas
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// SetId
        /// </summary>
        public string SetId { get; set; }
        /// <summary>
        /// TreeName
        /// </summary>
        public string TreeName { get; set; }
        /// <summary>
        /// Effdt
        /// </summary>
        public DateTime Effdt { get; set; }
        /// <summary>
        /// TreeNode
        /// </summary>
        public string TreeNode { get; set; }
        /// <summary>
        /// ParentNodeName
        /// </summary>
        public string ParentNodeName { get; set; }
        /// <summary>
        /// TreeLevelNum
        /// </summary>
        public string TreeLevelNum { get; set; }
        /// <summary>
        /// BatchNum
        /// </summary>
        public string BatchNum { get; set; }
        /// <summary>
        /// CreateDate
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// times
        /// </summary>
        //public int times { get; set; }

    }
}
