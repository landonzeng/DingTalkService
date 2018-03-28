using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class EmployeeEntity
    {
        /// <summary>
        /// 员工唯一标识ID（不可修改），企业内必须唯一。长度为1~64个字符，如果不传，服务器将自动生成一个userid
        /// 是否必须(否)
        /// </summary>
        public string userid { get; set; }
        /// <summary>
        /// 成员名称。长度为1~64个字符
        /// 是否必须(是)
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 在对应的部门中的排序, Map结构的json字符串, key是部门的Id, value是人员在这个部门的排序值
        /// 是否必须(否)
        /// </summary>
        public string orderInDepts { get { return ""; } }
        /// <summary>
        /// 数组类型，数组里面值为整型，成员所属部门id列表
        /// 是否必须(是)
        /// </summary>
        public List<int> department { get; set; }
        /// <summary>
        /// 职位信息。长度为0~64个字符
        /// 是否必须(否)
        /// </summary>
        public string position { get; set; }
        /// <summary>
        /// 手机号码。企业内必须唯一
        /// 是否必须(是)
        /// </summary>
        public string mobile { get; set; }
        /// <summary>
        /// 分机号，长度为0~50个字符
        /// 是否必须(否)
        /// </summary>
        public string tel { get; set; }
        /// <summary>
        /// 办公地点，长度为0~50个字符
        /// 是否必须(否)
        /// </summary>
        public string workPlace { get; set; }
        /// <summary>
        /// 备注，长度为0~1000个字符
        /// 是否必须(否)
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// 邮箱。长度为0~64个字符。企业内必须唯一
        /// 是否必须(否)
        /// </summary>
        public string email { get; set; }
        /// <summary>
        /// 员工工号。对应显示到OA后台和客户端个人资料的工号栏目。长度为0~64个字符
        /// 是否必须(否)
        /// </summary>
        public string jobnumber { get; set; }
        /// <summary>
        /// 是否号码隐藏, true表示隐藏, false表示不隐藏。隐藏手机号后，手机号在个人资料页隐藏，但仍可对其发DING、发起钉钉免费商务电话。
        /// 是否必须(否)
        /// </summary>
        public bool isHide { get; set; }
        /// <summary>
        /// 是否高管模式，true表示是，false表示不是。开启后，手机号码对所有员工隐藏。普通员工无法对其发DING、发起钉钉免费商务电话。高管之间不受影响。
        /// 是否必须(否)
        /// </summary>
        public bool isSenior { get; set; }
    }
}
