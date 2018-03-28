using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class DepartmentEntity
    {
        /// <summary>
        /// 必须(是)
        /// 部门id
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 必须(否)
        /// 部门名称。长度限制为1~64个字符
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 必须(否)
        /// 在父部门中的次序值。order值小的排序靠前
        /// </summary>
        public string order { get; set; }
        /// <summary>
        /// 必须(否)
        /// 父部门id。根部门id为1
        /// </summary>
        public string parentid { get; set; }
        /// <summary>
        /// 必须(否)
        /// 是否创建一个关联此部门的企业群
        /// </summary>
        public bool createDeptGroup { get; set; }
        /// <summary>
        /// 必须(否)
        /// 如果有新人加入部门是否会自动加入部门群
        /// </summary>
        public bool autoAddUser { get; set; }
        /// <summary>
        /// 必须(否)
        /// 是否隐藏部门, true表示隐藏, false表示显示
        /// </summary>
        public bool deptHiding { get; set; }
        /// <summary>
        /// 必须(否)
        /// 可以查看指定隐藏部门的其他部门列表，如果部门隐藏，则此值生效，取值为其他的部门id组成的的字符串，使用 | 符号进行分割
        /// </summary>
        public string deptPerimits { get; set; }
        /// <summary>
        /// 必须(否)
        /// 可以查看指定隐藏部门的其他人员列表，如果部门隐藏，则此值生效，取值为其他的人员userid组成的的字符串，使用| 符号进行分割
        /// </summary>
        public string userPerimits { get; set; }
        /// <summary>
        /// 必须(否)
        /// 是否本部门的员工仅可见员工自己, 为true时，本部门员工默认只能看到员工自己
        /// </summary>
        public bool outerDept { get; set; }
        /// <summary>
        /// 必须(否)
        /// 本部门的员工仅可见员工自己为true时，可以配置额外可见部门，值为部门id组成的的字符串，使用|符号进行分割
        /// </summary>
        public string outerPermitDepts { get; set; }
        /// <summary>
        /// 必须(否)
        /// 本部门的员工仅可见员工自己为true时，可以配置额外可见人员，值为userid组成的的字符串，使用|符号进行分割
        /// </summary>
        public string outerPermitUsers { get; set; }
        /// <summary>
        /// 必须(否)
        /// 企业群群主
        /// </summary>
        public string orgDeptOwner { get; set; }
        /// <summary>
        /// 必须(否)
        /// 部门的主管列表,取值为由主管的userid组成的字符串，不同的userid使用’| 符号进行分割
        /// </summary>
        public string deptManagerUseridList { get; set; }
    }
}
