using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities
{
    /// <summary>
    /// 返回消息 Modify by rafa 2016 6 13
    /// </summary>
    public class JsonMessage
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// 结果编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 结果消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 数据结果集合
        /// </summary>
        public string JsonData { get; set; }

        public override string ToString()
        {
            return JsonHelper.ToJson(this);
        }
    }
}
