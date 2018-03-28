using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 集合ID表
    /// </summary>
    [Description("集合ID表")]
    public class Tbiz_GroupOperation
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
        /// 描述
        /// </summary>
        [DisplayName("描述")]
        public string Descr { get; set; }
        /// <summary>
        /// 简短描述
        /// </summary>
        [DisplayName("简短描述")]
        public string DescrShort { get; set; }
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
