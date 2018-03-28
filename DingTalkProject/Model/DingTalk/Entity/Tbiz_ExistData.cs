using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Tbiz_ExistData
    {
        /// <summary>
        /// 用户Id/部门Id
        /// </summary>
        [SugarColumn(IsNullable = true, Length = 50, ColumnDescription = "用户Id/部门Id")]
        public string ObjectId { get; set; }
        /// <summary>
        /// 类型(0部门、1用户)
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "类型(0部门、1用户)")]
        public int Type { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "创建时间")]
        public DateTime CreateDate { get; set; }
    }
}
