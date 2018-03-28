using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Description("员工职务接口")]
    public class Tbiz_StaffPosition
    {
        /// <summary>
        /// id
        /// </summary>
        public int? Id { get; set; }
        /// <summary>
        /// 员工ID
        /// </summary>
        [DisplayName("员工ID")]
        public string EmplId { get; set; }
        /// <summary>
        /// 员工记录
        /// </summary>
        [DisplayName("员工记录")]
        public string EmplRcd { get; set; }
        /// <summary>
        /// 生效日期
        /// </summary>
        [DisplayName("生效日期")]
        public DateTime Effdt { get; set; }
        /// <summary>
        /// 生效序号
        /// </summary>
        [DisplayName("生效序号")]
        public string Effseq { get; set; }
        /// <summary>
        /// 部门集合ID
        /// </summary>
        [DisplayName("部门集合ID")]
        public string SetIdDept { get; set; }
        /// <summary>
        /// 部门ID
        /// </summary>
        [DisplayName("部门ID")]
        public string DeptId { get; set; }
        /// <summary>
        /// 职务集合ID
        /// </summary>
        [DisplayName("职务集合ID")]
        public string SetIdJobCode { get; set; }
        /// <summary>
        /// 公司
        /// </summary>
        [DisplayName("公司")]
        public string Company { get; set; }
        /// <summary>
        /// 职务
        /// </summary>
        [DisplayName("职务")]
        public string JobCode { get; set; }
        /// <summary>
        /// 职级代码
        /// </summary>
        [DisplayName("职级代码")]
        public string CrankCode { get; set; }
        /// <summary>
        /// 职级描述
        /// </summary>
        [DisplayName("职级描述")]
        public string CrankDescr { get; set; }
        /// <summary>
        /// 岗位ID
        /// </summary>
        [DisplayName("岗位ID")]
        public string CquartersId { get; set; }
        /// <summary>
        /// 主管ID
        /// </summary>
        [DisplayName("主管ID")]
        public string SupervisorId { get; set; }
        /// <summary>
        /// HR状态
        /// </summary>
        [DisplayName("HR状态")]
        public string HrStatus { get; set; }
        /// <summary>
        /// 工作店(ERP)
        /// </summary>
        [DisplayName("工作店(ERP)")]
        public string CworkShop { get; set; }
        /// <summary>
        /// 入司时间
        /// </summary>
        [DisplayName("入司时间")]
        public string LastHireDt { get; set; }
        /// <summary>
        /// 离职日期
        /// </summary>
        [DisplayName("离职日期")]
        public string TerminationDt { get; set; }
        /// <summary>
        /// 操作
        /// </summary>
        [DisplayName("操作")]
        public string Action { get; set; }

        /// <summary>
        /// 批次号，适用于批量传输数据的场景
        /// </summary>
        [DisplayName("批次号，适用于批量传输数据的场景")]
        public string BatchNum { get; set; }
        public DateTime? CreateDate { get; set; }


    }
}
