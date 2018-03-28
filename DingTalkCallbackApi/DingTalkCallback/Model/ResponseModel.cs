using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class ResponseModel
    {
        /// <summary>
        /// 消息体签名
        /// </summary>
        public string msg_signature { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        public string timeStamp { get; set; }
        /// <summary>
        /// 随机字符串
        /// </summary>
        public string nonce { get; set; }
        /// <summary>
        /// “success”加密字符串
        /// </summary>
        public string encrypt { get; set; }
    }
}
