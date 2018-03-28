using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HttpRequest;
using System.Net;
using System.Configuration;
using Utilities;

namespace Business
{
    public class TokenHelp
    {
        /// <summary>
        /// 创建静态字段，保证全局一致
        /// </summary>
        public static AccessToken AccessToken = new AccessToken();

        /// <summary>
        /// 获取/更新票据
        /// </summary>
        /// <param name="forced">true:强制更新.false:按缓存是否到期来更新</param>
        /// <returns></returns>
        public static AccessToken GetAccessToken(bool forced = false)
        {

            AccessToken model = AccessToken;
            //TokenCacheTime是缓存时间(常量，也可放到配置文件中)，这样在有效期内则直接从缓存中获取票据，不需要再向服务器中获取。
            if (!forced && AccessToken.Begin.AddSeconds(DingTalkUrlHelp.TokenCacheTime) >= DateTime.Now)
            {
                //没有强制更新，并且没有超过缓存时间
                return model;
            }

            string apiurl = DingTalkUrlHelp.GetTokenUrl;

            try
            {
                HttpRequest.HttpHelper.HttpResult result = HttpRequest.HttpHelper.Get(apiurl);

                Get_AccessToken tokenResult = Newtonsoft.Json.JsonConvert.DeserializeObject<Get_AccessToken>(result.ToStringResult());

                if (tokenResult != null)
                {
                    if (tokenResult.errcode == 0)
                    {
                        AccessToken.Value = tokenResult.access_token;
                        AccessToken.Begin = DateTime.Now;
                        return model;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper log = LogFactory.GetLogger("GetAccessToken");
                log.Error(ex);
                throw ex;
            }
            return model;
        }
    }
}
