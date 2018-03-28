using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class V_EmployeeToDingTalk
    {
        /// <summary>
        /// ESB_DepartmentId
        /// </summary>
        public string ESB_DepartmentId { get; set; }
        /// <summary>
        /// 工号/用户id
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Telephone { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 职务
        /// </summary>
        public string PositionName { get; set; }
        /// <summary>
        /// 是否启用 (1启用 0禁用)
        /// </summary>
        public int Enabled { get; set; }
    }
}
