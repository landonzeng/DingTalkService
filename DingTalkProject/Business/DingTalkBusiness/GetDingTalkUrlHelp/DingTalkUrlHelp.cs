using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class DingTalkUrlHelp
    {
        public static string GetTokenUrl
        {
            get { return string.Format(ConfigurationManager.AppSettings["TokenUrl"].ToString(), ConfigurationManager.AppSettings["CorpID"].ToString(), ConfigurationManager.AppSettings["CorpSecret"].ToString()); }
        }

        public static double TokenCacheTime
        {
            get { return Convert.ToDouble(ConfigurationManager.AppSettings["TokenCacheTime"].ToString()); }
        }

        public static string GetDepartmentList(string access_token)
        {
            return string.Format(ConfigurationManager.AppSettings["GetDepartmentList"].ToString(), access_token);
        }

        public static string GetDepartment(string access_token, string id)
        {
            return string.Format(ConfigurationManager.AppSettings["GetDepartment"].ToString(), access_token, id);
        }

        public static string CreateDepartment(string access_token)
        {
            return string.Format(ConfigurationManager.AppSettings["CreateDepartment"].ToString(), access_token);
        }

        public static string UpdateDepartment(string access_token)
        {
            return string.Format(ConfigurationManager.AppSettings["UpdateDepartment"].ToString(), access_token);
        }

        public static string DeleteDepartment(string access_token, string id)
        {
            return string.Format(ConfigurationManager.AppSettings["DeleteDepartment"].ToString(), access_token, id);
        }

        public static string GetUseridByUnionid(string access_token, string unionid)
        {
            return string.Format(ConfigurationManager.AppSettings["GetUseridByUnionid"].ToString(), access_token, unionid);
        }

        public static string GetEmployee(string access_token, string userid)
        {
            return string.Format(ConfigurationManager.AppSettings["GetEmployee"].ToString(), access_token, userid);
        }

        public static string CreateEmployee(string access_token)
        {
            return string.Format(ConfigurationManager.AppSettings["CreateEmployee"].ToString(), access_token);
        }

        public static string UpdateEmployee(string access_token)
        {
            return string.Format(ConfigurationManager.AppSettings["UpdateEmployee"].ToString(), access_token);
        }

        public static string DeleteEmployee(string access_token, string userid)
        {
            return string.Format(ConfigurationManager.AppSettings["DeleteEmployee"].ToString(), access_token, userid);
        }

        public static string BatchDeleteEmployee(string access_token)
        {
            return string.Format(ConfigurationManager.AppSettings["BatchDeleteEmployee"].ToString(), access_token);
        }

        public static string GetByDepartmentIdForSimpleList(string access_token, string department_id)
        {
            return string.Format(ConfigurationManager.AppSettings["GetByDepartmentIdForSimpleList"].ToString(), access_token, department_id);
        }

        public static string GetByDepartmentIdForUserInfoList(string access_token, string department_id)
        {
            return string.Format(ConfigurationManager.AppSettings["GetByDepartmentIdForUserInfoList"].ToString(), access_token, department_id);
        }

        public static string GetByDepartmentIdForAdmin(string access_token)
        {
            return string.Format(ConfigurationManager.AppSettings["GetByDepartmentIdForAdmin"].ToString(), access_token);
        }
    }
}
