using System;
using System.ComponentModel;

namespace Model
{
    [Description("个人信息接口")]
    public class Tbiz_PersonalInfo
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
        /// 用户主键
        /// </summary>
        [DisplayName("用户主键")]
        public string UserId { get; set; }
        /// <summary>
        /// 使用格式
        /// </summary>
        [DisplayName("使用格式")]
        public string CountryNmForm { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        [DisplayName("姓名")]
        public string RealName { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        [DisplayName("性别")]
        public string Gender { get; set; }
        /// <summary>
        /// 婚姻状况
        /// </summary>
        [DisplayName("婚姻状况")]
        public string MarStatus { get; set; }
        /// <summary>
        /// 结婚日期
        /// </summary>
        [DisplayName("结婚日期")]
        public string MarStatusDt { get; set; }
        /// <summary>
        /// 最高教育程度
        /// </summary>
        [DisplayName("最高教育程度")]
        public string HighestEducLvl { get; set; }
        /// <summary>
        /// 出生日期
        /// </summary>
        [DisplayName("出生日期")]
        public string Birthday { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        [DisplayName("手机")]
        public string Mobile { get; set; }
        /// <summary>
        /// 固定电话
        /// </summary>
        [DisplayName("固定电话")]
        public string Telephone { get; set; }
        /// <summary>
        /// 居住地址
        /// </summary>
        [DisplayName("居住地址")]
        public string CaddressHome { get; set; }
        /// <summary>
        /// 主证件地址（默认身份证地址）
        /// </summary>
        [DisplayName("主证件地址（默认身份证地址）")]
        public string CaddressNid { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        [DisplayName("用户ID")]
        public string Operator { get; set; }
        /// <summary>
        /// 账户锁定标示
        /// </summary>
        [DisplayName("账户锁定标示")]
        public string AcctLock { get; set; }
        /// <summary>
        /// 公司主键
        /// </summary>
        [DisplayName("公司主键")]
        public string CompanyId { get; set; }
        /// <summary>
        /// 树名称
        /// </summary>
        [DisplayName("树名称")]
        public string NationalId { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        [DisplayName("邮箱")]
        public string Email { get; set; }
        /// <summary>
        /// 公司邮箱
        /// </summary>
        [DisplayName("公司邮箱")]
        public string CcompEmail { get; set; }
        /// <summary>
        /// 批次号，适用于批量传输数据的场景
        /// </summary>
        [DisplayName("批次号，适用于批量传输数据的场景")]
        public string BatchNum { get; set; }
        public DateTime? CreateDate { get; set; }

    }
}
