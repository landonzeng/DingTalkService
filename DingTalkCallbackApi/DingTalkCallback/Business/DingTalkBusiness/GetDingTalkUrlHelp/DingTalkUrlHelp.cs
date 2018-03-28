using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace Business
{
    public class DingTalkUrlHelp
    {
        public static string GetTokenUrl
        {
            get { return string.Format(JsonConfigurationHelper.GetAppSettings("DingTalkSettings", "TokenUrl"), JsonConfigurationHelper.GetAppSettings("DingTalkSettings", "CorpID"), JsonConfigurationHelper.GetAppSettings("DingTalkSettings", "CorpSecret")); }
        }

        public static double TokenCacheTime
        {
            get { return Convert.ToDouble(JsonConfigurationHelper.GetAppSettings("DingTalkSettings","TokenCacheTime")); }
        }

        public static string GetDepartmentList(string access_token)
        {
            return string.Format(JsonConfigurationHelper.GetAppSettings("DingTalkSettings","GetDepartmentList"), access_token);
        }

        public static string GetDepartment(string access_token, string id)
        {
            return string.Format(JsonConfigurationHelper.GetAppSettings("DingTalkSettings","GetDepartment"), access_token, id);
        }

        public static string CreateDepartment(string access_token)
        {
            return string.Format(JsonConfigurationHelper.GetAppSettings("DingTalkSettings","CreateDepartment"), access_token);
        }

        public static string UpdateDepartment(string access_token)
        {
            return string.Format(JsonConfigurationHelper.GetAppSettings("DingTalkSettings","UpdateDepartment"), access_token);
        }

        public static string DeleteDepartment(string access_token, string id)
        {
            return string.Format(JsonConfigurationHelper.GetAppSettings("DingTalkSettings","DeleteDepartment"), access_token, id);
        }

        public static string GetUseridByUnionid(string access_token, string unionid)
        {
            return string.Format(JsonConfigurationHelper.GetAppSettings("DingTalkSettings","GetUseridByUnionid"), access_token, unionid);
        }

        public static string GetEmployee(string access_token, string userid)
        {
            return string.Format(JsonConfigurationHelper.GetAppSettings("DingTalkSettings","GetEmployee"), access_token, userid);
        }

        public static string CreateEmployee(string access_token)
        {
            return string.Format(JsonConfigurationHelper.GetAppSettings("DingTalkSettings","CreateEmployee"), access_token);
        }

        public static string UpdateEmployee(string access_token)
        {
            return string.Format(JsonConfigurationHelper.GetAppSettings("DingTalkSettings","UpdateEmployee"), access_token);
        }

        public static string DeleteEmployee(string access_token, string userid)
        {
            return string.Format(JsonConfigurationHelper.GetAppSettings("DingTalkSettings","DeleteEmployee"), access_token, userid);
        }

        public static string BatchDeleteEmployee(string access_token)
        {
            return string.Format(JsonConfigurationHelper.GetAppSettings("DingTalkSettings","BatchDeleteEmployee"), access_token);
        }

        public static string GetByDepartmentIdForSimpleList(string access_token, string department_id)
        {
            return string.Format(JsonConfigurationHelper.GetAppSettings("DingTalkSettings","GetByDepartmentIdForSimpleList"), access_token, department_id);
        }

        public static string GetByDepartmentIdForUserInfoList(string access_token, string department_id)
        {
            return string.Format(JsonConfigurationHelper.GetAppSettings("DingTalkSettings","GetByDepartmentIdForUserInfoList"), access_token, department_id);
        }

        public static string GetByDepartmentIdForAdmin(string access_token)
        {
            return string.Format(JsonConfigurationHelper.GetAppSettings("DingTalkSettings","GetByDepartmentIdForAdmin"), access_token);
        }

        public static string GetRegisterCallBack(string access_token)
        {
            return string.Format(JsonConfigurationHelper.GetAppSettings("DingTalkSettings", "RegisterCallBack"), access_token);
        }

    }
}
