using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 访问票据
    /// </summary>
    public class AccessToken
    {
         /// <summary>  
        /// 票据的值  
        /// </summary>  
        public String Value { get; set; }

        /// <summary>
        /// 设置默认时间
        /// </summary>
        private DateTime _begin = DateTime.Parse("1970-01-01");

        /// <summary>  
        /// 票据的开始时间  
        /// </summary>  
        public DateTime Begin
        {
            get { return _begin; }
            set { _begin = value; }
        }
    }
}
