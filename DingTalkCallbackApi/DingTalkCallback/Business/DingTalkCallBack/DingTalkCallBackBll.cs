using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Utility;

namespace Business
{
    public class DingTalkCallBackBll
    {
        public static string RegisterDingTalkCallBack()
        {
            RegisterModel regModel = new RegisterModel();
            AccessToken token = TokenHelp.GetAccessToken();
            string RegisterCallBackUrl = DingTalkUrlHelp.GetRegisterCallBack(token.Value);
            regModel.call_back_tag = new List<string>() { "user_leave_org" };
            regModel.token = "加解密需要用到的token，ISV(服务提供商)推荐使用注册套件时填写的token，普通企业可以随机填写";
            regModel.aes_key = "数据加密密钥。用于回调数据的加密，长度固定为43个字符，从a-z, A-Z, 0-9共62个字符中选取,您可以随机生成，ISV(服务提供商)推荐使用注册套件时填写的EncodingAESKey";
            regModel.url = "钉钉中设置的接收事件回调的url";
            string res = HttpHelper.Post(RegisterCallBackUrl, JsonConvert.SerializeObject(regModel, Formatting.Indented), "application/json").Html;
            return res;
        }

    }
}
