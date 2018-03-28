using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Description("职务信息接口")]
    public class Tbiz_PositionInfo
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
        /// 职务编码
        /// </summary>
        [DisplayName("职务编码")]
        public string Code { get; set; }
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
        /// 职务名称
        /// </summary>
        [DisplayName("职务名称")]
        public string FullName { get; set; }
        /// <summary>
        /// 职务简短描述
        /// </summary>
        [DisplayName("职务简短描述")]
        public string DescrShort { get; set; }
        /// <summary>
        /// 控股的职务代码；此列用于表示城市公司的职务与控股统一要求的职务的对应关系
        /// </summary>
        [DisplayName("控股的职务代码；此列用于表示城市公司的职务与控股统一要求的职务的对应关系")]
        public string CholdingNm { get; set; }
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
