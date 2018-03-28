using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using HttpCode.Core;
using Utility;

namespace Business
{
    public class DingTalkHttpHelp<T> where T : new()
    {
        public static T Get(string url)
        {
            if (url != "")
            {
                try
                {
                    HttpResults result = HttpHelper.Get(url,"application/json");
                    T model = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(result.Html);
                    return model;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                return default(T);
            }
        }

        public static T Post(string url, string param = null)
        {
            if (url != "")
            {
                try
                {
                    HttpResults result = HttpHelper.Post(url, param, "application/json");
                    T model = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(result.Html);
                    return model;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                return default(T);
            }
        }
    }
}
