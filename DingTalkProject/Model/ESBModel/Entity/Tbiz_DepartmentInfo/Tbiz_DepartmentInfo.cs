using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Description("部门信息接口")]
    public class Tbiz_DepartmentInfo
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
        /// 部门主键
        /// </summary>
        [DisplayName("部门主键")]
        public string DepartmentId { get; set; }
        /// <summary>
        /// 生效日期
        /// </summary>
        [DisplayName("生效日期")]
        public DateTime Effdt { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        [DisplayName("有效")]
        public string Enabled { get; set; }
        /// <summary>
        /// 部门描述
        /// </summary>
        [DisplayName("部门描述")]
        public string FullName { get; set; }
        /// <summary>
        /// 简称
        /// </summary>
        [DisplayName("简称")]
        public string ShortName { get; set; }
        /// <summary>
        /// 公司id
        /// </summary>
        [DisplayName("公司id")]
        public string CompanyId { get; set; }
        /// <summary>
        /// 部门类型
        /// </summary>
        [DisplayName("部门类型")]
        public string AType { get; set; }
        /// <summary>
        /// 部门子类
        /// </summary>
        [DisplayName("部门子类")]
        public string CdeptStype { get; set; }
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
        /// 实体店ID
        /// </summary>
        [DisplayName("实体店ID")]
        public string CphysiStore { get; set; }
        /// <summary>
        /// 部门负责人ID
        /// </summary>
        [DisplayName("部门负责人ID")]
        public string ManagerId { get; set; }
        /// <summary>
        /// 部门负责人岗位
        /// </summary>
        [DisplayName("部门负责人岗位")]
        public string ManagerPosn { get; set; }
        /// <summary>
        /// 财务编码
        /// </summary>
        [DisplayName("财务编码")]
        public string CfinanceCd { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        [DisplayName("编码")]
        public string Code { get; set; }
        /// <summary>
        /// 部门路径名称
        /// </summary>
        [DisplayName("部门路径名称")]
        public string AllName { get; set; }
        /// <summary>
        /// 批次号，适用于批量传输数据的场景
        /// </summary>
        [DisplayName("批次号，适用于批量传输数据的场景")]
        public string BatchNum { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [DisplayName("创建时间")]
        public DateTime? CreateDate { get; set; }
    }
}
