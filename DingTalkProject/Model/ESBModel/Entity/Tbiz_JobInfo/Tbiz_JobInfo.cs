using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Description("岗位信息接口")]
    public class Tbiz_JobInfo
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
        /// 岗位ID
        /// </summary>
        [DisplayName("岗位ID")]
        public string CquartersId { get; set; }
        /// <summary>
        /// 生效日期
        /// </summary>
        [DisplayName("生效日期")]
        public DateTime Effdt { get; set; }
        /// <summary>
        /// 岗位名称
        /// </summary>
        [DisplayName("岗位名称")]
        public string Descr { get; set; }
        /// <summary>
        /// 业务类型
        /// </summary>
        [DisplayName("业务类型")]
        public string CbusinessType { get; set; }
        /// <summary>
        /// 业务子类
        /// </summary>
        [DisplayName("业务子类")]
        public string CbusinessSub { get; set; }
        /// <summary>
        /// 条线划分
        /// </summary>
        [DisplayName("条线划分")]
        public string CdividingLine { get; set; }
        /// <summary>
        /// 岗位序列
        /// </summary>
        [DisplayName("岗位序列")]
        public string CquteOrder { get; set; }
        /// <summary>
        /// 岗位子序列
        /// </summary>
        [DisplayName("岗位子序列")]
        public string CquteSonOrder { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        [DisplayName("状态")]
        public string Cstate { get; set; }
        /// <summary>
        /// 职务代码
        /// </summary>
        [DisplayName("职务代码")]
        public string JobCode { get; set; }
        /// <summary>
        /// 岗位简称
        /// </summary>
        [DisplayName("岗位简称")]
        public string CquteShortName { get; set; }
        /// <summary>
        /// 批次号，适用于批量传输数据的场景
        /// </summary>
        [DisplayName("批次号，适用于批量传输数据的场景")]
        public string BatchNum { get; set; }
        public DateTime? CreateDate { get; set; }

    }
}
