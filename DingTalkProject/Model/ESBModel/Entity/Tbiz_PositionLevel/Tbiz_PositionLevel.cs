using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Description("职务级别接口")]
    public class Tbiz_PositionLevel
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
        /// 角色编码
        /// </summary>
        [DisplayName("角色编码")]
        public string Code { get; set; }
        /// <summary>
        /// 职级代码
        /// </summary>
        [DisplayName("职级代码")]
        public string CrankCode { get; set; }
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
        /// 角色名称
        /// </summary>
        [DisplayName("角色名称")]
        public string FullName { get; set; }
        /// <summary>
        /// 职级简短描述
        /// </summary>
        [DisplayName("职级简短描述")]
        public string DescrShort { get; set; }
        /// <summary>
        /// 控股职级；此列用于表示城市公司的职级与控股统一要求的职级的对应关系
        /// </summary>
        [DisplayName("控股职级；此列用于表示城市公司的职级与控股统一要求的职级的对应关系")]
        public string CholdingRank { get; set; }
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
