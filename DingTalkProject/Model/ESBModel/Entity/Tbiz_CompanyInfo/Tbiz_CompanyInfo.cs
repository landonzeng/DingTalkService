

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Description("公司信息接口")]
    public class Tbiz_CompanyInfo
    {
        /// <summary>
        /// id
        /// </summary>
        public int? Id { get; set; }
        /// <summary>
        /// 公司id
        /// </summary>
        [DisplayName("公司id")]
        public string CompanyId { get; set; }
        /// <summary>
        /// 生效日期
        /// </summary>
        [DisplayName("生效日期")]
        public DateTime Effdt { get; set; }
        /// <summary>
        /// 状态(有效A/无效I)
        /// </summary>
        [DisplayName("状态(有效A/无效I)")]
        public string Enabled { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [DisplayName("描述")]
        public string FullName { get; set; }
        /// <summary>
        /// 简短描述
        /// </summary>
        [DisplayName("简短描述")]
        public string DesctShort { get; set; }
        /// <summary>
        /// 国家地区
        /// </summary>
        [DisplayName("国家地区")]
        public string Country { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        [DisplayName("地址")]
        public string Address { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        [DisplayName("城市")]
        public string City { get; set; }
        /// <summary>
        /// 州/省
        /// </summary>
        [DisplayName("州/省")]
        public string Province { get; set; }
        /// <summary>
        /// 邮政编码
        /// </summary>
        [DisplayName("邮政编码")]
        public string PostalCode { get; set; }
        /// <summary>
        /// 法人类型
        /// </summary>
        [DisplayName("法人类型")]
        public string LegalType { get; set; }

        /// <summary>
        /// 批次号，适用于批量传输数据的场景
        /// </summary>
        [DisplayName("批次号，适用于批量传输数据的场景")]
        public string BatchNum { get; set; }
        public DateTime? CreateDate { get; set; }

    }
}
