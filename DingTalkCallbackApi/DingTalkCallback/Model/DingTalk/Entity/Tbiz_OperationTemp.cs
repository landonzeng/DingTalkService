using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Tbiz_OperationTemp
    {
        /// <summary>
        /// 员工ID/部门ID
        /// </summary>
        public string ObjectId { get; set; }
        /// <summary>
        /// 类型(0部门、1员工)
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 操作(0更新、1插入)
        /// </summary>
        public int Operation { get; set; }
        /// <summary>
        /// 处理优先级，越小优先级越大
        /// </summary>
        public int OrderByNum { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
    }
}
