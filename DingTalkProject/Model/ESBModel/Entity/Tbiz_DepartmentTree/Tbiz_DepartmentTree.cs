using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Description("部门树结构接口")]
    public class Tbiz_DepartmentTree
    {
        /// <summary>
        /// id
        /// </summary>
        public int? Id { get; set; }
        /// <summary>
        /// 集合 ID
        /// </summary>
        [DisplayName("集合 ID")]
        public string SetId { get; set; }
        /// <summary>
        /// 树名称
        /// </summary>
        [DisplayName("树名称")]
        public string TreeName { get; set; }
        /// <summary>
        /// 生效日期
        /// </summary>
        [DisplayName("生效日期")]
        public DateTime Effdt { get; set; }
        /// <summary>
        /// 部门ID
        /// </summary>
        [DisplayName("部门ID")]
        public string TreeNode { get; set; }
        /// <summary>
        /// 父部门ID
        /// </summary>
        [DisplayName("父部门ID")]
        public string ParentNodeName { get; set; }
        /// <summary>
        /// 树层级
        /// </summary>
        [DisplayName("树层级")]
        public string TreeLevelNum { get; set; }
        /// <summary>
        /// 批次号，适用于批量传输数据的场景
        /// </summary>
        [DisplayName("批次号，适用于批量传输数据的场景")]
        public string BatchNum { get; set; }
        public DateTime? CreateDate { get; set; }

    }
}
