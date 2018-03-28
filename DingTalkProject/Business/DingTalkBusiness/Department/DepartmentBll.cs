using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HttpRequest;
using Utilities;

namespace Business
{
    public class DepartmentBll
    {
        /// <summary>
        /// 获取部门列表
        /// </summary>
        /// <returns></returns>
        public static GetDepartmentList GetList()
        {
            AccessToken accessToken = TokenHelp.GetAccessToken();
            string url = DingTalkUrlHelp.GetDepartmentList(accessToken.Value);
            GetDepartmentList model = new GetDepartmentList();
            model = DingTalkHttpHelp<GetDepartmentList>.Get(url);
            return model;
        }

        /// <summary>
        /// 获取部门详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static GetDepartment Get(string id)
        {
            AccessToken accessToken = TokenHelp.GetAccessToken();
            string url = DingTalkUrlHelp.GetDepartment(accessToken.Value, id);
            GetDepartment model = new GetDepartment();
            model = DingTalkHttpHelp<GetDepartment>.Get(url);
            return model;
        }
        /// <summary>
        /// 根据部门名称获取钉钉中的部门信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static GetDepartment GetByName(string name)
        {
            AccessToken accessToken = TokenHelp.GetAccessToken();
            GetDepartmentList DepartmentList = GetList();
            if (DepartmentList.errcode == 90002)
            {
                System.Threading.Thread.Sleep(1500);
                DepartmentList = GetList();
            }
            var dep = DepartmentList.department.Where(e => e.name.Equals(name)).ToList().FirstOrDefault();
            if (dep != null)
            {
                GetDepartment model = Get(dep.id);
                return model;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static DepartmentResult Create(DepartmentEntity model)
        {

            DepartmentResult resul = new DepartmentResult();
            AccessToken accessToken = TokenHelp.GetAccessToken();
            string url = DingTalkUrlHelp.CreateDepartment(accessToken.Value);
            resul = DingTalkHttpHelp<DepartmentResult>.Post(url, model.ToJson());
            return resul;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static Result Update(string param)
        {
            Result model = new Result();
            AccessToken accessToken = TokenHelp.GetAccessToken();
            string url = DingTalkUrlHelp.UpdateDepartment(accessToken.Value);
            model = DingTalkHttpHelp<Result>.Post(url, param);
            return model;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Result Delete(string id)
        {
            Result model = new Result();
            AccessToken accessToken = TokenHelp.GetAccessToken();
            string url = DingTalkUrlHelp.DeleteDepartment(accessToken.Value, id);
            model = DingTalkHttpHelp<Result>.Get(url);
            return model;
        }

        /// <summary>
        /// 获取部门成员
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static ByDepIdFroSimpleListResult GetByDepartmentIdForSimpleList(string id)
        {
            ByDepIdFroSimpleListResult model = new ByDepIdFroSimpleListResult();
            AccessToken accessToken = TokenHelp.GetAccessToken();
            string url = DingTalkUrlHelp.GetByDepartmentIdForSimpleList(accessToken.Value, id);
            model = DingTalkHttpHelp<ByDepIdFroSimpleListResult>.Get(url);
            return model;
        }

        /// <summary>
        /// 获取部门成员（详情）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static GetDepartmentForUserList GetByDepartmentIdForUserInfoList(string id)
        {
            GetDepartmentForUserList model = new GetDepartmentForUserList();
            AccessToken accessToken = TokenHelp.GetAccessToken();
            string url = DingTalkUrlHelp.GetByDepartmentIdForUserInfoList(accessToken.Value, id);
            model = DingTalkHttpHelp<GetDepartmentForUserList>.Get(url);
            return model;
        }

        /// <summary>
        /// 获取管理员列表
        /// </summary>
        /// <returns></returns>
        public static Result GetByDepartmentIdForAdmin()
        {
            Result model = new Result();
            AccessToken accessToken = TokenHelp.GetAccessToken();
            string url = DingTalkUrlHelp.GetByDepartmentIdForAdmin(accessToken.Value);
            model = DingTalkHttpHelp<Result>.Get(url);
            return model;
        }
    }
}
