using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Description("实体店信息接口")]
    public class Tbiz_ShopInfo
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
        /// 实体店ID
        /// </summary>
        [DisplayName("实体店ID")]
        public string CphyStoId { get; set; }
        /// <summary>
        /// 生效日期
        /// </summary>
        [DisplayName("生效日期")]
        public DateTime Effdt { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [DisplayName("描述")]
        public string Cdescription { get; set; }
        /// <summary>
        /// 简短描述
        /// </summary>
        [DisplayName("简短描述")]
        public string CsimDescr { get; set; }
        /// <summary>
        /// 店面状态
        /// </summary>
        [DisplayName("店面状态")]
        public string CshopStation { get; set; }
        /// <summary>
        /// 开店日期
        /// </summary>
        [DisplayName("开店日期")]
        public string CstrShopDt { get; set; }
        /// <summary>
        /// 国家地区
        /// </summary>
        [DisplayName("国家地区")]
        public string Country { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        [DisplayName("地址")]
        public string Caddress { get; set; }
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 批次号，适用于批量传输数据的场景
        /// </summary>
        [DisplayName("批次号，适用于批量传输数据的场景")]
        public string BatchNum { get; set; }

    }
}
