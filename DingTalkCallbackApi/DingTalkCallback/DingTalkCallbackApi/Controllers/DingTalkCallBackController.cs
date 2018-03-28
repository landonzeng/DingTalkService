
using Business;
using Microsoft.AspNetCore.Mvc;
using Model;
using Newtonsoft.Json;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utility;

namespace DingTalkCallbackApi.Controllers
{
    [Route("api/DingTalkCallBack")]
    public class DingTalkCallBackController : Controller
    {
        NLogHelper _log = NLogFactory.GetLogger("DingTalkCallBack");

        [HttpGet]
        public string Test()
        {
            //var Edb = DBHelper.Edb;

            //List<string> list = new List<string>() { "8158407", "1111111" };

            //var user = Edb.Queryable<V_EmployeeToDingTalk>().Where(it => list.Contains(it.UserId) && it.Enabled == 1).ToList();

            return "test";
        }


        /// <summary>
        /// 接收钉钉回调数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ResponseModel Post(string signature, string timestamp, string nonce, [FromBody]RequestModel model)
        {


            ResponseModel result = new ResponseModel();
            var Ddb = DBHelper.Ddb;
            var Edb = DBHelper.Edb;

            _log.Info("请求参数为: signature=" + signature + "&timestamp=" + timestamp + "&nonce=" + nonce + ", FromBody=" + JsonHelper.JsonSerializer(model) + "\n");

            DingTalkCrypt dingTalkCrypt = new DingTalkCrypt(JsonConfigurationHelper.GetAppSettings("DingTalkSettings", "CallBack_Token"), JsonConfigurationHelper.GetAppSettings("DingTalkSettings", "CallBack_SuiteKey"), JsonConfigurationHelper.GetAppSettings("DingTalkSettings", "CorpID"));
            int decryptMsgcount = 1;
            int encryptMsgCount = 1;
            string dd_result = "";
            string sEncryptMsg = "";
            string msg_signature = "";

            decryptMsgcount = dingTalkCrypt.DecryptMsg(signature, timestamp, nonce, model.encrypt, ref dd_result);

            result.timeStamp = timestamp;
            result.nonce = nonce;
            try
            {
                if (decryptMsgcount == 0)
                {
                    ContactsEventModel contactsEventModel = JsonConvert.DeserializeObject<ContactsEventModel>(dd_result);
                    if (contactsEventModel.EventType == "check_url")
                    {
                        encryptMsgCount = dingTalkCrypt.EncryptMsg("success", timestamp, nonce, ref sEncryptMsg, ref msg_signature);
                        result.encrypt = sEncryptMsg;
                        result.msg_signature = msg_signature;
                        _log.Error(JsonConvert.SerializeObject(result));
                        return result;
                    }

                    List<DingTalkCallBackLog> LogList = new List<DingTalkCallBackLog>();
                    int sqlExeCount = 0;
                    if (contactsEventModel.UserId != null)
                    {
                        foreach (var item in contactsEventModel.UserId)
                        {
                            DingTalkCallBackLog log = new DingTalkCallBackLog
                            {
                                UserId = item,
                                EventType = contactsEventModel.EventType,
                                TimeStamp = contactsEventModel.TimeStamp
                            };
                            LogList.Add(log);
                        }
                        if (LogList.Count > 0)
                        {
                            sqlExeCount = Ddb.Insertable(LogList).ExecuteCommand();
                        }
                        if (sqlExeCount > 0)
                        {
                            encryptMsgCount = dingTalkCrypt.EncryptMsg("success", timestamp, nonce, ref sEncryptMsg, ref msg_signature);
                            if (encryptMsgCount == 0)
                            {
                                List<DingTalkCallBackOperation> userList = new List<DingTalkCallBackOperation>();
                                //日志记录成功后，根据钉钉回调的员工，检查hr中该员工状态为在职的人员
                                var user = Edb.Queryable<V_EmployeeToDingTalk>().Where(it => contactsEventModel.UserId.Contains(it.UserId) && it.Enabled == 1).ToList();
                                foreach (var item in user)
                                {
                                    //将员工为在职的人员重新添加到钉钉中
                                    bool b = EmployeeForDingTalkBll.AddEmployee(Edb, Ddb, item);
                                    DingTalkCallBackOperation DD_User = new DingTalkCallBackOperation();
                                    DD_User.EventType = contactsEventModel.EventType;
                                    DD_User.TimeStamp = contactsEventModel.TimeStamp;
                                    DD_User.UserId = item.UserId;
                                    DD_User.CreateDate = DateTime.Now;
                                    if (b)
                                    {
                                        DD_User.IsOperation = 1;
                                    }
                                    userList.Add(DD_User);
                                }
                                if (userList.Count > 0)
                                {
                                    sqlExeCount = Ddb.Insertable(userList).ExecuteCommand();
                                    _log.Debug(sqlExeCount + "条数据执行成功\n");
                                }
                                result.encrypt = sEncryptMsg;
                                result.msg_signature = msg_signature;
                            }
                            else
                            {
                                _log.Error("将消息加密,返回加密后字符串失败，返回加密前的参数为：" + dd_result + "，返回编码为: " + encryptMsgCount + "\n");
                            }
                        }
                    }
                    else
                    {
                        _log.Error("钉钉传递的UserList为空，返回解密前的参数为：" + JsonHelper.JsonSerializer(model) + "，返回编码为: " + decryptMsgcount + "\n");
                    }
                }
                else
                {
                    _log.Error("回调失败，请求参数为: signature = " + signature + " & timestamp = " + timestamp + " & nonce = " + nonce + ", FromBody = " + JsonHelper.JsonSerializer(model) + "\n错误详情如下：\n");
                }
            }
            catch (Exception ex)
            {
                _log.Error("回调失败，请求参数为: signature = " + signature + " & timestamp = " + timestamp + " & nonce = " + nonce + ", FromBody = " + JsonHelper.JsonSerializer(model) + "\n错误详情如下：\n" + ex);
            }

            return result;
        }
    }
}