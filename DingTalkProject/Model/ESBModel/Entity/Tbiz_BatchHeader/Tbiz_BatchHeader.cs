using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Description("集合ID表")]
    public class Tbiz_BatchHeader
    {
        /// <summary>
        /// Id
        /// </summary>
        public int? Id { get; set; }
        /// <summary>
        /// 传输ID
        /// </summary>
        [DisplayName("传输ID")]
        public string TransferId { get; set; }
        /// <summary>
        /// 批次号，适用于批量传输数据的场景
        /// </summary>
        [DisplayName("批次号，适用于批量传输数据的场景ID")]
        public string BatchNum { get; set; }
        /// <summary>
        /// 传输时间
        /// </summary>
        [DisplayName("传输时间")]
        public string TransferDateTime { get; set; }
        /// <summary>
        /// 同步类型,AL：全量；AD：增量
        /// </summary>
        [DisplayName("同步类型,AL：全量；AD：增量")]
        public string PushDataType { get; set; }
        /// <summary>
        /// 数据类型(业务json类型)
        /// </summary>
        [DisplayName("数据类型(业务json类型)")]
        public string Type { get; set; }
        /// <summary>
        /// 批次行数据量
        /// </summary>
        public int RowsCount { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [DisplayName("创建时间")]
        public DateTime? CreateDate { get; set; }
    }
}
