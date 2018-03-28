using System;

namespace Model
{
    public class Tbiz_ErroUpdateEmployeeInfo
    {
        public string UserId { get; set; }
        /// <summary>
        /// 更新前钉钉中的手机号
        /// </summary>
        public string OldMobile { get; set; }
        /// <summary>
        /// 新手机号
        /// </summary>
        public string NewMobile { get; set; }
        /// <summary>
        /// 钉钉返回错误编码
        /// </summary>
        public string ErroCode { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
    }
}
