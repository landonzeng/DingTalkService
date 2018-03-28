using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HttpRequest;

namespace Business
{
    public class EmployeeBll
    {
        public static GetEmployee Get(string userid)
        {
            GetEmployee model = new GetEmployee();
            AccessToken accessToken = TokenHelp.GetAccessToken();
            string url = DingTalkUrlHelp.GetEmployee(accessToken.Value, userid);
            model = DingTalkHttpHelp<GetEmployee>.Get(url);
            return model;
        }

        public static EmployeeResult Create(string param)
        {
            EmployeeResult model = new EmployeeResult();
            AccessToken accessToken = TokenHelp.GetAccessToken();
            string url = DingTalkUrlHelp.CreateEmployee(accessToken.Value);
            model = DingTalkHttpHelp<EmployeeResult>.Post(url, param);
            if (model.errmsg == "40014")
            {
                accessToken = TokenHelp.GetAccessToken();
                url = DingTalkUrlHelp.CreateEmployee(accessToken.Value);
                model = DingTalkHttpHelp<EmployeeResult>.Post(url, param);
            }
            return model;
        }

        public static Result Update(string param)
        {
            Result model = new Result();
            AccessToken accessToken = TokenHelp.GetAccessToken();
            string url = DingTalkUrlHelp.UpdateEmployee(accessToken.Value);
            model = DingTalkHttpHelp<Result>.Post(url, param);
            if (model.errmsg == "40014")
            {
                accessToken = TokenHelp.GetAccessToken();
                url = DingTalkUrlHelp.UpdateEmployee(accessToken.Value);
                model = DingTalkHttpHelp<Result>.Post(url, param);
            }
            return model;
        }

        public static Result Delete(string userid)
        {
            Result model = new Result();
            AccessToken accessToken = TokenHelp.GetAccessToken();
            string url = DingTalkUrlHelp.DeleteEmployee(accessToken.Value, userid);
            model = DingTalkHttpHelp<Result>.Get(url);
            return model;
        }

        public static Result BatchDelete(string param)
        {
            Result model = new Result();
            AccessToken accessToken = TokenHelp.GetAccessToken();
            string url = DingTalkUrlHelp.BatchDeleteEmployee(accessToken.Value);
            model = DingTalkHttpHelp<Result>.Post(url, param);
            return model;
        }
    }
}
