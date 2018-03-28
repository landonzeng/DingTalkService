
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Description("级联关系基础数据接口")]
    public class Tbiz_Cascadedata
    {
        /// <summary>
        /// id
        /// </summary>
        public int? Id { get; set; }
        /// <summary>
        /// 集合ID
        /// </summary>
        [DisplayName("集合ID")]
        public string SetId { get; set; }
        /// <summary>
        /// 对象分类
        /// </summary>
        [DisplayName("对象分类")]
        public string Category { get; set; }
        /// <summary>
        /// 父值
        /// </summary>
        [DisplayName("父值")]
        public string CfatherVal { get; set; }
        /// <summary>
        /// 父值描述
        /// </summary>
        [DisplayName("父值描述")]
        public string CfatherDescr { get; set; }
        /// <summary>
        /// 子值
        /// </summary>
        [DisplayName("子值")]
        public string CsonVal { get; set; }
        /// <summary>
        /// 子值描述
        /// </summary>
        [DisplayName("子值描述")]
        public string CsonDescr { get; set; }
        /// <summary>
        /// 部门编码
        /// </summary>
        [DisplayName("部门编码")]
        public string CdeptIdType { get; set; }
        /// <summary>
        /// 部门类型描述
        /// </summary>
        [DisplayName("部门类型描述")]
        public string Descr4 { get; set; }
        /// <summary>
        /// 批次号，适用于批量传输数据的场景
        /// </summary>
        [DisplayName("批次号，适用于批量传输数据的场景")]
        public string BatchNum { get; set; }

        public DateTime? CreateDate { get; set; }
    }
}
