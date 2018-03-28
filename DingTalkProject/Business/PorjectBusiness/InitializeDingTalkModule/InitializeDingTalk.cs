using Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Business
{
    public class InitializeDingTalk
    {
        static LogHelper log = LogFactory.GetLogger("InitializeDingTalk");

        public static void Initialize()
        {
            SqlSugarClient Ddb = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = Config.DingTalkConnectionString,
                DbType = DbType.SqlServer,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute
            });

            SqlSugarClient Edb = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = Config.ESBConnectionString,
                DbType = DbType.SqlServer,
                IsAutoCloseConnection = true
            });

            #region 数据初始化
            string TempDeptID = FirstInitialize(Ddb);
            #endregion

            ImputEsbDepartment(Edb, Ddb);
            ImputEsbDepartmentTree(Edb, Ddb);
            UpdateEsbDepartmentParentID(Ddb);
            DepartmentImputDingTalk(Ddb);
            EmployeeImputDingTalk(Edb, Ddb);

            //删除临时部门
            DepartmentForDingTalkBll.DeleteTempDDDept(Edb, TempDeptID);
        }

        public static string FirstInitialize(SqlSugarClient Ddb)
        {

            Ddb.CodeFirst.InitTables(typeof(Tbiz_ExistData));
            Ddb.DbMaintenance.TruncateTable("Tbiz_ExistData");

            //创建一个临时部门，用于存放管理员
            DepartmentEntity Deptmodel = new DepartmentEntity();
            Deptmodel.name = "TempDepartment";
            Deptmodel.parentid = "1";
            Deptmodel.sourceIdentifier = "";
            DepartmentResult DepResult = DepartmentBll.Create(Deptmodel);
            string TempDDDeptID = DepResult.id;

            //查出所有部门id
            GetDepartmentList DepartmentList = DepartmentBll.GetList();
            if (string.IsNullOrWhiteSpace(TempDDDeptID))
            {
                TempDDDeptID = DepartmentList.department.Where(e => e.name.Equals("TempDepartment")).ToList().FirstOrDefault().id;
            }
            var list = DepartmentList.department.Where(e => !e.id.Equals("1") && !e.id.Equals(TempDDDeptID)).ToList();

            foreach (var item in list)
            {
                //遍历每个id下的所有人员
                GetDepartmentForUserList UserList = DepartmentBll.GetByDepartmentIdForUserInfoList(item.id);
                foreach (var Useritem in UserList.userlist)
                {
                    try
                    {
                        //删除所有人员
                        Result Result = EmployeeBll.Delete(Useritem.userid);
                        //当删除失败时，将该人员转移到临时部门中
                        if (Result.errcode != "0")
                        {
                            Tbiz_ExistData model = new Tbiz_ExistData();
                            model.ObjectId = Useritem.userid;
                            model.Type = 1;
                            model.CreateDate = DateTime.Now;
                            Ddb.Insertable<Tbiz_ExistData>(model).ExecuteCommand();

                            string EmployeeJson = EmployeeForDingTalkBll.GetEmployee(Useritem.userid);
                            EmployeeEntity Emodel = Newtonsoft.Json.JsonConvert.DeserializeObject<EmployeeEntity>(EmployeeJson);
                            for (int i = 0; i < Emodel.department.Count; i++)
                            {
                                if (!Emodel.department[i].ToString().Equals("1"))
                                {
                                    Emodel.department.Remove(Emodel.department[i]);
                                }
                            }
                            Emodel.department.Add(Convert.ToInt32(TempDDDeptID));

                            string param = Emodel.ToJson();

                            Result uResult = EmployeeBll.Update(param);

                            if (uResult.errcode != "0")
                            {
                                Console.Write("\r\n" + uResult.errmsg + "\r\n");
                            }

                        }

                    }
                    catch (Exception e)
                    {
                        Console.Write("\r\n InitializeDingTalk-FirstInitialize() " + e + "\r\n");
                        log.Error(e);
                    }
                }
                //最后删除公司下的所有部门
                Result r = DepartmentBll.Delete(item.id);
                if (r.errcode != "0")
                {
                    Tbiz_ExistData model = new Tbiz_ExistData();
                    model.ObjectId = item.id;
                    model.Type = 0;
                    model.CreateDate = DateTime.Now;
                    Ddb.Insertable<Tbiz_ExistData>(model).ExecuteCommand();

                    Console.Write("\r\n" + r.errmsg + "\r\n");
                }
                Console.Write("\r\n 当前进度" + Math.Round(Convert.ToDecimal((Convert.ToDecimal(list.IndexOf(item)) / list.Count())), 2, MidpointRounding.AwayFromZero) * 100 + "%");
            }
            return TempDDDeptID;
        }

        public static void ImputEsbDepartment(SqlSugarClient Edb, SqlSugarClient Ddb)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                watch = CommonHelper.TimerStart();
                LogHelper log = LogFactory.GetLogger("ImputEsbDepartment");
                List<DepartmentResult> resultList = new List<DepartmentResult>();


                var Departmentlist = Edb.Queryable<V_Effective_Tbiz_DepartmentInfo, V_Effective_Tbiz_DepartmentTreeas>((a, b) => new object[] { JoinType.Inner, a.DepartmentId == b.TreeNode })
                .Where((a, b) => a.DepartmentId != "1000000001")
                .Select
                (
                    (a, b) =>
                    new V_Effective_Tbiz_DepartmentInfo
                    {
                        DepartmentId = a.DepartmentId,
                        FullName = a.FullName
                    }
                ).OrderBy("DepartmentId").ToList();

                log.Info("\r\n------------------------------------------------ESB部门数据导入------------------------------------------------\r\n");
                foreach (var item in Departmentlist)
                {
                    DepartmentEntity model = new DepartmentEntity();
                    model.name = item.FullName;
                    model.createDeptGroup = true;
                    model.autoAddUser = true;
                    model.parentid = "1";
                    model.createDeptGroup = true;
                    model.sourceIdentifier = item.DepartmentId;
                    DepartmentResult result = DepartmentBll.Create(model);
                    //将ESB中的id和名称存入到钉钉同步数据库中
                    result.ESB_DepartmentID = item.DepartmentId;
                    result.ESB_DepartmentName = item.FullName;

                    if (result != null)
                    {
                        if (result.errcode == 0)
                        {
                            string json = result.id;

                            log.Debug("\n部门id为:" + result.id + "\n");
                            resultList.Add(result);
                            Console.Write("成功:" + json + "\n 已插入：" + (Convert.ToInt32(Departmentlist.IndexOf(item)) + 1) + "条,当前进度：" + (Math.Round(Convert.ToDecimal(Convert.ToDouble((Departmentlist.IndexOf(item) + 1)) / Convert.ToDouble(Departmentlist.Count)), 2, MidpointRounding.AwayFromZero) * 100) + "%\n\n");
                        }
                        else
                        {
                            Console.Write("\r\n" + result.errmsg + "\r\n");
                        }
                    }
                    else
                    {
                        Console.Write("\r\n无返回数据\r\n");
                    }
                }

                Ddb.CodeFirst.InitTables(typeof(DepartmentResult));
                //删除目前记录的数据，初始化
                Ddb.DbMaintenance.TruncateTable("DepartmentResult");
                int count = Ddb.Insertable<DepartmentResult>(resultList).ExecuteCommand();
                log.Info("\n钉钉接口对接成功后，插入到本地共:" + count + " 条数据,共耗时：" + CommonHelper.TimerEnd(watch) + "毫秒\n");
                Console.Write("\r\n钉钉接口对接成功后，插入到本地共 " + count + " 条数据,共耗时：" + CommonHelper.TimerEnd(watch) + "毫秒\r\n");
            }
            catch (Exception ex)
            {
                Console.Write("\r\n" + ex.Message + "\r\n");
            }
        }

        public static void ImputEsbDepartmentTree(SqlSugarClient Edb, SqlSugarClient Ddb)
        {
            List<V_DingTalk_DepartmentTree> DepTreeList = Edb.Ado.SqlQuery<V_DingTalk_DepartmentTree>("exec SP_DingTalk_DepartmentTree");
            List<DepartmentTrees> list = JsonHelper.JonsToList<DepartmentTrees>(DepTreeList.ToJson()).OrderBy(e => e.level).ToList();

            Ddb.CodeFirst.InitTables(typeof(DepartmentTrees));

            Ddb.DbMaintenance.TruncateTable("DepartmentTrees");
            int count = Ddb.Insertable<DepartmentTrees>(list).ExecuteCommand();

            Console.Write("\r\n执行完成\r\n");
        }

        public static void UpdateEsbDepartmentParentID(SqlSugarClient Ddb)
        {
            Ddb.CodeFirst.InitTables(typeof(DepartmentTrees));

            var newlist = Ddb.Queryable<DepartmentTrees, DepartmentTrees>((st, sc) => new object[] { JoinType.Inner, st.ParentDepartmentId == sc.DepartmentId })
                .Select
                (
                    (st, sc) =>
                    new DepartmentTrees
                    {
                        Id = st.Id,
                        DD_Id = st.DD_Id,
                        DD_ParentId = sc.DD_Id,
                        DepartmentId = st.DepartmentId,
                        FullName = st.FullName,
                        ParentDepartmentId = st.ParentDepartmentId,
                        CreateDate = st.CreateDate,
                        level = st.level
                    }
                ).ToList();

            Ddb.Updateable<DepartmentTrees>(newlist).ExecuteCommand();
            Console.Write("\r\n执行完成\r\n");
        }

        public static void DepartmentImputDingTalk(SqlSugarClient Ddb)
        {
            LogHelper log = LogFactory.GetLogger("ImputEsbDepartment");

            Ddb.CodeFirst.InitTables(typeof(DepartmentTrees));

            int treeCount = Ddb.Queryable<DepartmentTrees>().Count();
            int maxlevel = Ddb.Queryable<DepartmentTrees>().Max(it => it.level);
            int countRow = 0;
            log.Info("\r\n------------------------------------------------钉钉组织框架根据ESB部门树数据调整------------------------------------------------\r\n");
            for (int i = 0; i <= maxlevel; i++)
            {
                List<DepartmentTrees> list = new List<DepartmentTrees>();
                list = Ddb.Queryable<DepartmentTrees>().Where(it => it.level == i).ToList();
                foreach (var item in list)
                {
                    countRow++;
                    try
                    {
                        string DepartmentJson = DepartmentForDingTalkBll.GetDepartment(item.DD_Id);

                        DepartmentEntity model = new DepartmentEntity();
                        model = Newtonsoft.Json.JsonConvert.DeserializeObject<DepartmentEntity>(DepartmentJson);
                        model.parentid = item.DD_ParentId;
                        model.order = list.IndexOf(item).ToString();
                        model.sourceIdentifier = item.DepartmentId;
                        string param = model.ToJson();
                        Result result = DepartmentBll.Update(param);
                        if (result != null)
                        {
                            if (result.errcode == "0")
                            {
                                Console.Write("第" + i + "级数据更新成功," + result.errmsg + ",已更新：" + countRow + "条,当前进度：" + (Math.Round(Convert.ToDecimal(Convert.ToDouble(countRow) / Convert.ToDouble(treeCount)), 2, MidpointRounding.AwayFromZero) * 100) + "%\n\n");
                            }
                            else
                            {
                                Console.Write(result.errmsg);
                            }
                        }
                        else
                        {
                            Console.Write("\r\n无返回数据\r\n");
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error("\r\n InitializeDingTalk-DepartmentImputDingTalk() " + ex.Message + "\r\n");
                        Console.Write("\r\n" + ex.Message + "\r\n");
                    }
                }
            }
            Console.Write("更新成功,共更新 " + countRow + " 条数据");
        }

        public static void EmployeeImputDingTalk(SqlSugarClient Edb, SqlSugarClient Ddb)
        {
            LogHelper log = LogFactory.GetLogger("EmployeeImputDingTalk");
            try
            {
                List<DepartmentResult> resultList = new List<DepartmentResult>();

                List<V_EmployeeToDingTalk> ESB_EmployeeList = Edb.Queryable<V_EmployeeToDingTalk>().ToList();

                List<DepartmentResult> DTDepartList = Ddb.Queryable<DepartmentResult>().ToList();

                foreach (var item in ESB_EmployeeList)
                {
                    try
                    {
                        if (item.Enabled == 1)
                        {
                            EmployeeEntity model = new EmployeeEntity();
                            model.userid = item.UserId;
                            model.name = item.Name;
                            model.department = new List<int>(new int[] { Convert.ToInt32(DepartmentForDingTalkBll.GetDingTalkDepartmentId(DTDepartList, item.ESB_DepartmentId)) });
                            model.position = item.PositionName;
                            model.mobile = item.Mobile;
                            model.tel = item.Telephone;
                            model.workPlace = "";
                            model.remark = "";
                            model.email = item.Email;
                            model.jobnumber = item.UserId;
                            model.isHide = false;
                            model.isSenior = false;

                            string param = model.ToJson();

                            EmployeeResult Result = EmployeeBll.Create(param);
                            if (Result != null)
                            {
                                if (Result.errcode == "0")
                                {
                                    Console.Write("创建成功,UserId=" + Result.userid + "\r\n");
                                }
                                else
                                {
                                    Console.Write(Result.errmsg + "\r\n");
                                }
                            }
                            else
                            {
                                Console.Write("无返回数据");
                            }
                        }
                        else
                        {
                            Result Result = EmployeeBll.Delete(item.UserId);
                            if (Result != null)
                            {
                                if (Result.errcode == "0")
                                {
                                    Console.Write("删除成功," + Result.errmsg + "\r\n");
                                }
                                else
                                {
                                    Console.Write(Result.errmsg + "\r\n");
                                }
                            }
                            else
                            {
                                Console.Write("无返回数据");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error("\r\n InitializeDingTalk-EmployeeImputDingTalk() " + ex);
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("\r\n InitializeDingTalk-EmployeeImputDingTalk() " + ex);
                Console.Write(ex.Message);
            }
        }

        private static void DeleteEmp()
        {
            SqlSugarClient Ddb = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = Config.DingTalkConnectionString,
                DbType = DbType.SqlServer,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute
            });

            string TempDDDeptID = "";
            //查出所有部门id
            GetDepartmentList DepartmentList = DepartmentBll.GetList();
            if (string.IsNullOrWhiteSpace(TempDDDeptID))
            {
                TempDDDeptID = DepartmentList.department.Where(e => e.name.Equals("TempDepartment")).ToList().FirstOrDefault().id;
            }
            var list = DepartmentList.department.Where(e => !e.id.Equals("1") && !e.id.Equals(TempDDDeptID)).ToList();

            foreach (var item in list)
            {
                //遍历每个id下的所有人员
                GetDepartmentForUserList UserList = DepartmentBll.GetByDepartmentIdForUserInfoList(item.id);
                foreach (var Useritem in UserList.userlist)
                {
                    try
                    {
                        //删除所有人员
                        Result Result = EmployeeBll.Delete(Useritem.userid);
                        //当删除失败时，将该人员转移到临时部门中
                        if (Result.errcode != "0")
                        {
                            Tbiz_ExistData model = new Tbiz_ExistData();
                            model.ObjectId = Useritem.userid;
                            model.Type = 1;
                            model.CreateDate = DateTime.Now;
                            Ddb.Insertable<Tbiz_ExistData>(model).ExecuteCommand();

                            string EmployeeJson = EmployeeForDingTalkBll.GetEmployee(Useritem.userid);
                            EmployeeEntity Emodel = Newtonsoft.Json.JsonConvert.DeserializeObject<EmployeeEntity>(EmployeeJson);
                            for (int i = 0; i < Emodel.department.Count; i++)
                            {
                                if (!Emodel.department[i].ToString().Equals("1"))
                                {
                                    Emodel.department.Remove(Emodel.department[i]);
                                }
                            }
                            Emodel.department.Add(Convert.ToInt32(TempDDDeptID));

                            string param = Emodel.ToJson();

                            Result uResult = EmployeeBll.Update(param);

                            if (uResult.errcode != "0")
                            {
                                Console.Write("\r\n" + uResult.errmsg + "\r\n");
                            }

                        }

                    }
                    catch (Exception e)
                    {
                        Console.Write("\r\n" + e + "\r\n");
                        log.Error("\r\n InitializeDingTalk-DeleteEmp() " + e);
                    }
                }

                Console.Write("\r\n 当前进度" + Math.Round(Convert.ToDecimal((Convert.ToDecimal(list.IndexOf(item)) / list.Count())), 2, MidpointRounding.AwayFromZero) * 100 + "%");
            }
        }
    }
}
