using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Description("离职信息接口")]
    public class Tbiz_DepartureInfo
    {
        /// <summary>
        /// id
        /// </summary>
        public int? Id { get; set; }
        /// <summary>
        /// 员工ID
        /// </summary>
        [DisplayName("员工ID")]
        public string EmployeeId { get; set; }
        /// <summary>
        /// 集合 ID
        /// </summary>
        [DisplayName("集合 ID")]
        public string Setid { get; set; }
        /// <summary>
        /// 流程状态
        /// </summary>
        [DisplayName("流程状态")]
        public string CapproveStatus { get; set; }
        /// <summary>
        /// 离职状态
        /// </summary>
        [DisplayName("离职状态")]
        public string IsDimission { get; set; }
        /// <summary>
        /// 批次号，适用于批量传输数据的场景
        /// </summary>
        [DisplayName("批次号，适用于批量传输数据的场景")]
        public string BatchNum { get; set; }
        public DateTime? CreateDate { get; set; }

    }
}
