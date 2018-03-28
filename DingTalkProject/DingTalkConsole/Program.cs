using Business;
using Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace DingTalkConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            #region 测试
            //GetDepartmentList();
            //GetDepartment2("44700154");
            //UpdateDepartment();
            //AddDepartment();
            //AddEmployee();
            //GetByDepartmentIdForUserInfoList();
            //GetEmployee("landon1");
            //UpdateEmployee();
            //DeleteEmployee();
            //DeleteDepartment("44715488");
            //BatchDeleteEmployee();
            #endregion

            #region 重新初始化
            //DeleteDepartment("44967021");
            //AddDepartment();
            #endregion


            //ImputEsbDepartment("45186346");
            //ImputEsbDepartmentTree();
            //UpdateEsbDepartmentParentID();
            //DepartmentImputDingTalk();

            //EmployeeImputDingTalk();

            //InitializeDingTalk.Initialize();


            SynchronousDingTalk.Synchronous();



            //OperationRepeatData3();

            //OperationRepeatData2();
            //DeleteDingTalkAndDataTable("50272272");

            //GetDepartmentList DepartmentList = DepartmentBll.GetList();

            //var dddd = DepartmentList.department.Where(e => e.id.Equals("50164009")).ToList().FirstOrDefault();
            //GetDepartmentForUserList UserList = DepartmentBll.GetByDepartmentIdForUserInfoList(dddd.id);

            //Console.Write("\n" + dddd.name + " DD_ID=" + dddd.id + "，其下共有" + UserList.userlist.Count + "个人！\r\n");

            //DepartmentUserMove("50028910", 48859723);
            //DeleteDingTalkAndDataTable("50028910");

            //OperationRepeatData4();

            //DepartmentUserMove("51194463", 48803826);

            //DepartmentUserMove("50859360", 48902711);
            //OperationRepeatData();


            //OperationRepeatData4();



            Console.Write("\n同步完成！\r\n");
            Console.ReadKey();
        }

        private static void DeleteDingTalkAndDataTable(string id)
        {
            SqlSugarClient Ddb = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = Config.DingTalkConnectionString,
                DbType = DbType.SqlServer,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute
            });
            //删除钉钉中的部门
            var a = DepartmentBll.Delete(id);
            //删除钉钉关系表和钉钉树关系表
            int i = Ddb.Deleteable<DepartmentResult>().Where(it => it.id.Equals(id)).ExecuteCommand();
            i = Ddb.Deleteable<DepartmentTrees>().Where(it => it.DD_Id.Equals(id)).ExecuteCommand();
        }

        /// <summary>
        /// 将id部门下的人员转到parentid部门下
        /// </summary>
        /// <param name="id"></param>
        /// <param name="parentid"></param>
        private static void DepartmentUserMove(string id, int parentid)
        {
            GetDepartmentList DepartmentList = DepartmentBll.GetList();

            var dddd = DepartmentList.department.Where(e => e.id.Equals(id)).ToList().FirstOrDefault();
            if (dddd != null)
            {
                //钉钉中存在该部门，检查该部门下是否有人，没人的并且重复的部门删除钉钉中该部门，并且删除钉钉关系表和钉钉树关系表
                GetDepartmentForUserList UserList = DepartmentBll.GetByDepartmentIdForUserInfoList(dddd.id);
                Console.Write("\n" + dddd.name + " DD_ID=" + dddd.id + "，其下共有" + UserList.userlist.Count + "个人！\r\n");
                foreach (var Useritem in UserList.userlist)
                {
                    string EmployeeJson = EmployeeForDingTalkBll.GetEmployee(Useritem.userid);
                    EmployeeEntity Emodel = JsonHelper.JsonToModel<EmployeeEntity>(EmployeeJson);
                    Emodel.department = new List<int>() { parentid };
                    string param = Emodel.ToJson();

                    Result uResult = EmployeeBll.Update(param);
                    if (uResult.errcode != "0")
                    {
                        Console.Write("\r\n" + uResult.errmsg + "\r\n");
                    }
                }
            }
        }

        private static void OperationRepeatData()
        {
            GetDepartmentList DepartmentList = DepartmentBll.GetList();

            string sql = @"
                            
                        select *   from DepartmentResult where ESB_DepartmentID in(
                        select ESB_DepartmentID from DepartmentResult group by ESB_DepartmentID having count(1)>1
                        )
                        order by ESB_DepartmentID

                        ";

            SqlSugarClient Ddb = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = Config.DingTalkConnectionString,
                DbType = DbType.SqlServer,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute
            });

            List<DepartmentResult> list = Ddb.Ado.SqlQuery<DepartmentResult>(sql);
            foreach (var item in list)
            {
                var dddd = DepartmentList.department.Where(e => e.id.Equals(item.id)).ToList().FirstOrDefault();
                if (dddd != null)
                {
                    //钉钉中存在该部门，检查该部门下是否有人，没人的并且重复的部门删除钉钉中该部门，并且删除钉钉关系表和钉钉树关系表
                    GetDepartmentForUserList UserList = DepartmentBll.GetByDepartmentIdForUserInfoList(dddd.id);
                    Console.Write("\n ESB_DepartmentID=" + item.ESB_DepartmentID + "，部门名=" + dddd.name + " DD_ID=" + dddd.id + "，其下共有" + UserList.userlist.Count + "个人！\r\n");
                    if (UserList.userlist.Count == 0)
                    {
                        //删除钉钉中的部门
                        var a = DepartmentBll.Delete(dddd.id);
                        //删除钉钉关系表和钉钉树关系表
                        int i = Ddb.Deleteable<DepartmentResult>().Where(it => it.id.Equals(item.id)).ExecuteCommand();
                        i = Ddb.Deleteable<DepartmentTrees>().Where(it => it.DD_Id.Equals(item.id)).ExecuteCommand();
                    }
                }
                else
                {
                    //当钉钉中不存在该部门时，只删除关系表中重复的数据
                    int i = Ddb.Deleteable<DepartmentResult>().Where(it => it.id.Equals(item.id)).ExecuteCommand();
                    i = Ddb.Deleteable<DepartmentTrees>().Where(it => it.DD_Id.Equals(item.id)).ExecuteCommand();
                }

            }
            Console.Write("\r\n处理完毕");
        }

        private static void OperationRepeatData2()
        {
            GetDepartmentList DepartmentList = DepartmentBll.GetList();

            string sql = @"
                            select * from DepartmentResult where ESB_DepartmentID in(
                            '1000003530',
                            '1000003531',
                            '1000003532',
                            '1000003534',
                            '1000003535',
                            '1000003536',
                            '1000003537',
                            '1000003538'
                            )
                        ";

            SqlSugarClient Ddb = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = Config.DingTalkConnectionString,
                DbType = DbType.SqlServer,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute
            });

            List<DepartmentResult> list = Ddb.Ado.SqlQuery<DepartmentResult>(sql);
            foreach (var item in list)
            {
                var dddd = DepartmentList.department.Where(e => e.id.Equals(item.id)).ToList().FirstOrDefault();
                if (dddd != null)
                {
                    DepartmentEntity model = new DepartmentEntity();
                    string DepartmentJson = GetDepartment(item.id);
                    model = JsonHelper.JsonToModel<DepartmentEntity>(DepartmentJson);

                    //如果部门名或者部门父ID与钉钉的不同的时候就更新，如果相同就不调用更新接口
                    if (!model.name.Equals(item.ESB_DepartmentName))
                    {
                        model.name = item.ESB_DepartmentName;
                        model.sourceIdentifier = item.ESB_DepartmentID;
                        string param = model.ToJson();
                        Result result = DepartmentBll.Update(param);
                    }
                }


            }
            Console.Write("\r\n处理完毕");
        }

        private static void OperationRepeatData3()
        {
            GetDepartmentList DepartmentList = DepartmentBll.GetList();

            string sql = @"
                            with ta as(
                            select *   from DepartmentResult where ESB_DepartmentID in(
                            select ESB_DepartmentID from DepartmentResult group by ESB_DepartmentID having count(1)>1
                            )
                            ),tb as(
                            select a.* from [PM.WebApi].dbo.V_Effective_Tbiz_DepartmentInfo a inner join [PM.WebApi].dbo.V_Effective_Tbiz_DepartmentTreeas b
                            on a.DepartmentId=b.TreeNode
                            ),tc as(
                            select tb.fullname a,ta.* from ta left join tb 
                            on ta.ESB_DepartmentName=tb.FullName
                            where tb.fullname is null
                            ),td as(
                            select a.* from [PM.WebApi].dbo.V_Effective_Tbiz_DepartmentInfo a inner join tc
                            on tc.ESB_DepartmentId=a.DepartmentId
                            )
                            select distinct td.fullname aa,tc.* from tc right join  td
                            on tc.ESB_DepartmentId=td.DepartmentId
                            order by td.fullname
                        ";
            string sql2 = @"
                            with ta as(
                            select *   from DepartmentResult where ESB_DepartmentID in(
                            select ESB_DepartmentID from DepartmentResult group by ESB_DepartmentID having count(1)>1
                            )
                            ),tb as(
                            select a.* from [PM.WebApi].dbo.V_Effective_Tbiz_DepartmentInfo a inner join [PM.WebApi].dbo.V_Effective_Tbiz_DepartmentTreeas b
                            on a.DepartmentId=b.TreeNode
                            ),tc as(
                            select tb.fullname a,ta.* from ta left join tb 
                            on ta.ESB_DepartmentName=tb.FullName
                            where tb.fullname is null
                            ),td as(
                            select a.* from [PM.WebApi].dbo.V_Effective_Tbiz_DepartmentInfo a inner join tc
                            on tc.ESB_DepartmentId=a.DepartmentId
                            ),te as(
                            select a.* from td inner join DepartmentResult a
                            on td.DepartmentID=a.ESB_DepartmentID and td.FullName=a.ESB_DepartmentName
                            )select '' aa,'' a  ,* from te
                            order by ESB_DepartmentID
                        ";

            SqlSugarClient Ddb = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = Config.DingTalkConnectionString,
                DbType = DbType.SqlServer,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute
            });

            List<DepartmentResult> list = Ddb.Ado.SqlQuery<DepartmentResult>(sql);

            List<DepartmentResult> list2 = Ddb.Ado.SqlQuery<DepartmentResult>(sql2);
            foreach (var item in list)
            {
                var dddd = DepartmentList.department.Where(e => e.id.Equals(item.id)).ToList().FirstOrDefault();
                if (dddd != null)
                {
                    //钉钉中存在该部门，检查该部门下是否有人，没人的并且重复的部门删除钉钉中该部门，并且删除钉钉关系表和钉钉树关系表
                    GetDepartmentForUserList UserList = DepartmentBll.GetByDepartmentIdForUserInfoList(dddd.id);
                    Console.Write("\n" + dddd.name + " DD_ID=" + dddd.id + "，其下共有" + UserList.userlist.Count + "个人！\r\n");
                    string id = list2.Where(it => it.ESB_DepartmentID.Equals(item.ESB_DepartmentID)).FirstOrDefault().id;
                    if (UserList.userlist.Count != 0)
                    {
                        //将该部门下的人迁移到正确的部门下
                        foreach (var Useritem in UserList.userlist)
                        {

                            string EmployeeJson = EmployeeForDingTalkBll.GetEmployee(Useritem.userid);
                            EmployeeEntity Emodel = JsonHelper.JsonToModel<EmployeeEntity>(EmployeeJson);
                            Emodel.department = new List<int>() { Convert.ToInt32(id) };
                            string param = Emodel.ToJson();

                            Result uResult = EmployeeBll.Update(param);
                            if (uResult.errcode != "0")
                            {
                                Console.Write("\r\n" + uResult.errmsg + "\r\n");
                            }
                        }


                        //删除钉钉中的部门
                        var a = DepartmentBll.Delete(dddd.id);
                        //删除钉钉关系表和钉钉树关系表
                        int i = Ddb.Deleteable<DepartmentResult>().Where(it => it.id.Equals(item.id)).ExecuteCommand();
                        i = Ddb.Deleteable<DepartmentTrees>().Where(it => it.DD_Id.Equals(item.id)).ExecuteCommand();
                    }
                }

            }
            Console.Write("\r\n处理完毕");
        }

        private static void OperationRepeatData4()
        {
            GetDepartmentList DepartmentList = DepartmentBll.GetList();

            string sql = @"with ta as(
                        select * from DepartmentTrees where DepartmentId in(
                            select DepartmentId from DepartmentTrees group by DepartmentId having count(1)>1
                            )
                        )
                        select a.ESB_DepartmentName aa,ta.* from ta left join  DepartmentResult a
                        on ta.DD_Id=a.id
                        where a.ESB_DepartmentName is null";

            SqlSugarClient Ddb = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = Config.DingTalkConnectionString,
                DbType = DbType.SqlServer,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute
            });

            List<DepartmentTrees> list = Ddb.Ado.SqlQuery<DepartmentTrees>(sql);
            foreach (var item in list)
            {
                var dddd = DepartmentList.department.Where(e => e.id.Equals(item.DD_Id)).ToList().FirstOrDefault();
                if (dddd != null)
                {
                    //钉钉中存在该部门，检查该部门下是否有人，没人的并且重复的部门删除钉钉中该部门，并且删除钉钉关系表和钉钉树关系表
                    GetDepartmentForUserList UserList = DepartmentBll.GetByDepartmentIdForUserInfoList(dddd.id);
                    Console.Write("\n" + dddd.name + " DD_ID=" + dddd.id + "，其下共有" + UserList.userlist.Count + "个人！\r\n");
                    if (UserList.userlist.Count == 0)
                    {
                        //删除钉钉中的部门
                        var a = DepartmentBll.Delete(dddd.id);
                        //删除钉钉关系表和钉钉树关系表
                        int i = Ddb.Deleteable<DepartmentResult>().Where(it => it.id.Equals(item.DD_Id)).ExecuteCommand();
                        i = Ddb.Deleteable<DepartmentTrees>().Where(it => it.DD_Id.Equals(item.DD_Id)).ExecuteCommand();
                    }
                }
                else
                {

                    //当钉钉中不存在该部门时，只删除关系表中重复的数据
                    //int i = Ddb.Deleteable<DepartmentResult>().Where(it => it.id.Equals(item.id)).ExecuteCommand();
                    //int i = Ddb.Deleteable<DepartmentTrees>().Where(it => it.Id.Equals(item.Id)).ExecuteCommand();
                }


            }
            Console.Write("\r\n处理完毕");
        }

        #region 测试
        private static void GetDepartmentList()
        {
            try
            {
                GetDepartmentList model = DepartmentBll.GetList();

                if (model != null)
                {
                    if (model.errcode == 0)
                    {
                        var d = model.department.Where(e => e.name.Contains("IT部")).First();
                        string json = model.department.ToJson();

                        Console.Write("成功:\n" + json);
                    }
                    else
                    {
                        Console.Write(model.errmsg);
                    }
                }
                else
                {
                    Console.Write("无返回数据");
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        private static void GetDepartment2(string id)
        {
            GetDepartment model = new GetDepartment();
            try
            {
                model = DepartmentBll.Get(id);

                if (model != null)
                {
                    if (model.errcode == "0")
                    {
                        string json = model.ToJson();

                        Console.Write("成功:\n" + json);
                    }
                    else
                    {
                        Console.Write(model.errmsg);
                    }
                }
                else
                {
                    Console.Write("无返回数据");
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        private static void AddDepartment()
        {
            try
            {
                LogHelper log = LogFactory.GetLogger("AddDepartment");
                DepartmentEntity model = new DepartmentEntity();
                model.id = "123";
                model.name = "Test 钉钉数据对接测试2";
                model.createDeptGroup = true;
                model.autoAddUser = true;
                model.parentid = "1";
                model.createDeptGroup = true;
                model.sourceIdentifier = "";
                DepartmentResult Result = DepartmentBll.Create(model);

                if (Result != null)
                {
                    if (Result.errcode == 0)
                    {
                        log.Info("\r\n------------------------------------------------新增部门------------------------------------------------\n\n\r\n部门id为:" + Result.id + "\n");
                        Console.Write("创建成功,ID=" + Result.id);
                    }
                    else
                    {
                        Console.Write(Result.errmsg);
                    }
                }
                else
                {
                    Console.Write("无返回数据");
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        private static void DeleteDepartment(string id)
        {
            try
            {

                Result Result = DepartmentBll.Delete(id);

                if (Result != null)
                {
                    if (Result.errcode == "0")
                    {
                        Console.Write("删除成功," + Result.errmsg);
                    }
                    else
                    {
                        if (Result.errcode != "60003")
                        {
                            Console.Write(Result.errmsg);
                        }

                    }
                }
                else
                {
                    Console.Write("无返回数据");
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        private static string GetDepartment(string Id)
        {
            GetDepartment model = new GetDepartment();
            try
            {
                model = DepartmentBll.Get(Id);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return model.ToJson();
        }

        //private static void UpdateDepartment()
        //{
        //    try
        //    {
        //        string DepartmentJson = GetDepartment("44224071");

        //        DepartmentEntity model = new DepartmentEntity();
        //        model = Newtonsoft.Json.JsonConvert.DeserializeObject<DepartmentEntity>(DepartmentJson);
        //        //model.parentid = "44074983";
        //        //IT部 4422793
        //        model.parentid = "1";

        //        string param = model.ToJson();

        //        Result result = DepartmentBll.Update(param);

        //        if (result != null)
        //        {
        //            if (result.errcode == "0")
        //            {
        //                Console.Write("更新成功," + result.errmsg);
        //            }
        //            else
        //            {
        //                Console.Write(result.errmsg);
        //            }
        //        }
        //        else
        //        {
        //            Console.Write("无返回数据");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.Write(ex.Message);
        //    }
        //}

        private static void GetByDepartmentIdForUserInfoList()
        {

            //44698157, 44224071 
            GetDepartmentForUserList model = DepartmentBll.GetByDepartmentIdForUserInfoList("44224071");

            if (model != null)
            {
                if (model.errcode == 0)
                {
                    string json = model.userlist.ToJson();

                    Console.Write("成功:\n" + json);
                }
                else
                {
                    Console.Write(model.errmsg);
                }
            }
            else
            {
                Console.Write("无返回数据");
            }

        }

        private static string GetEmployee(string userid)
        {
            string json = "";
            try
            {
                GetEmployee Result = EmployeeBll.Get(userid);

                if (Result != null)
                {
                    if (Result.errcode == "0")
                    {
                        json = Result.ToJson();
                    }
                    else
                    {
                        Console.Write(Result.errmsg);
                    }
                }
                else
                {
                    Console.Write("无返回数据");
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return json;
        }

        private static void AddEmployee()
        {
            try
            {
                EmployeeEntity model = new EmployeeEntity();
                model.userid = "wangzezheng";
                model.name = "王五";
                model.department = new List<int>(new int[] { 123456 });
                model.position = "软件工程师";
                model.mobile = "137*****334";
                model.tel = "";
                model.workPlace = "";
                model.remark = "";
                model.email = "";
                model.jobnumber = "";
                model.isHide = false;
                model.isSenior = false;

                string param = model.ToJson();

                EmployeeResult Result = EmployeeBll.Create(param);
                if (Result != null)
                {
                    if (Result.errcode == "0")
                    {
                        Console.Write("创建成功,UserId=" + Result.userid);
                    }
                    else
                    {
                        Console.Write(Result.errmsg);
                    }
                }
                else
                {
                    Console.Write("无返回数据");
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        //private static void UpdateEmployee()
        //{
        //    try
        //    {
        //        string EmployeeJson = GetEmployee("016630184325983798");
        //        EmployeeEntity model = Newtonsoft.Json.JsonConvert.DeserializeObject<EmployeeEntity>(EmployeeJson);

        //        model.remark = "测试";
        //        model.email = "landonzeng@qq.com";

        //        string param = model.ToJson();

        //        Result Result = EmployeeBll.Update(param);
        //        if (Result != null)
        //        {
        //            if (Result.errcode == "0")
        //            {
        //                Console.Write("更新成功," + Result.errmsg);
        //            }
        //            else
        //            {
        //                Console.Write(Result.errmsg);
        //            }
        //        }
        //        else
        //        {
        //            Console.Write("无返回数据");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.Write(ex.Message);
        //    }
        //}

        private static void DeleteEmployee()
        {
            try
            {

                Result Result = EmployeeBll.Delete("landon1");
                if (Result != null)
                {
                    if (Result.errcode == "0")
                    {
                        Console.Write("更新成功," + Result.errmsg);
                    }
                    else
                    {
                        Console.Write(Result.errmsg);
                    }
                }
                else
                {
                    Console.Write("无返回数据");
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        private static void BatchDeleteEmployee()
        {
            try
            {
                BatchDeleteEmployee model = new Model.BatchDeleteEmployee();

                List<string> list = new List<string>();
                list.Add("landonzeng");
                list.Add("landon");
                model.useridlist = list;

                string param = model.ToJson();

                Result Result = EmployeeBll.BatchDelete(param);
                if (Result != null)
                {
                    if (Result.errcode == "0")
                    {
                        Console.Write("删除成功," + Result.errmsg);
                    }
                    else
                    {
                        Console.Write(Result.errmsg);
                    }
                }
                else
                {
                    Console.Write("无返回数据");
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        private static void ImputEsbDepartment(string parentid)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                watch = CommonHelper.TimerStart();
                LogHelper log = LogFactory.GetLogger("ImputEsbDepartment");
                List<DepartmentResult> resultList = new List<DepartmentResult>();
                SqlSugarClient Edb = new SqlSugarClient(new ConnectionConfig()
                {
                    ConnectionString = Config.ESBConnectionString,
                    DbType = DbType.SqlServer,
                    IsAutoCloseConnection = true
                });

                var Departmentlist = Edb.Queryable<V_Effective_Tbiz_DepartmentInfo, V_Effective_Tbiz_DepartmentTreeas>((a, b) => new object[] { JoinType.Inner, a.DepartmentId == b.TreeNode })
                .Select
                (
                    (a, b) =>
                    new V_Effective_Tbiz_DepartmentInfo
                    {
                        DepartmentId = a.DepartmentId,
                        FullName = a.FullName
                    }
                ).ToList();

                log.Info("\r\n------------------------------------------------ESB部门数据导入------------------------------------------------\r\n");
                foreach (var item in Departmentlist)
                {
                    DepartmentEntity model = new DepartmentEntity();
                    model.name = item.FullName;
                    model.createDeptGroup = true;
                    model.autoAddUser = true;
                    model.parentid = parentid;
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
                SqlSugarClient Ddb = new SqlSugarClient(new ConnectionConfig()
                {
                    ConnectionString = Config.DingTalkConnectionString,
                    DbType = DbType.SqlServer,
                    IsAutoCloseConnection = true,
                    InitKeyType = InitKeyType.Attribute
                });
                Ddb.CodeFirst.InitTables(typeof(DepartmentResult));
                //删除目前记录的数据，初始化
                Ddb.Deleteable<DepartmentResult>().ExecuteCommand();
                int count = Ddb.Insertable<DepartmentResult>(resultList).ExecuteCommand();
                log.Info("\n钉钉接口对接成功后，插入到本地共:" + count + " 条数据,共耗时：" + CommonHelper.TimerEnd(watch) + "毫秒\n");
                Console.Write("\r\n钉钉接口对接成功后，插入到本地共 " + count + " 条数据,共耗时：" + CommonHelper.TimerEnd(watch) + "毫秒\r\n");
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        private static void ImputEsbDepartmentTree()
        {
            SqlSugarClient Edb = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = Config.ESBConnectionString,
                DbType = DbType.SqlServer,
                IsAutoCloseConnection = true
            });

            List<V_DingTalk_DepartmentTree> DepTreeList = Edb.Ado.SqlQuery<V_DingTalk_DepartmentTree>("exec SP_DingTalk_DepartmentTree");
            List<DepartmentTrees> list = JsonHelper.JonsToList<DepartmentTrees>(DepTreeList.ToJson()).OrderBy(e => e.level).ToList();

            SqlSugarClient Ddb = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = Config.DingTalkConnectionString,
                DbType = DbType.SqlServer,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute
            });
            Ddb.CodeFirst.InitTables(typeof(DepartmentTrees));

            Ddb.DbMaintenance.TruncateTable("DepartmentTrees");
            int count = Ddb.Insertable<DepartmentTrees>(list).ExecuteCommand();

            Console.Write("\r\n执行完成\r\n");
        }

        private static void UpdateEsbDepartmentParentID()
        {
            SqlSugarClient Ddb = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = Config.DingTalkConnectionString,
                DbType = DbType.SqlServer,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute
            });
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

        //private static void DepartmentImputDingTalk()
        //{
        //    LogHelper log = LogFactory.GetLogger("ImputEsbDepartment");
        //    SqlSugarClient Ddb = new SqlSugarClient(new ConnectionConfig()
        //    {
        //        ConnectionString = Config.DingTalkConnectionString,
        //        DbType = DbType.SqlServer,
        //        IsAutoCloseConnection = true,
        //        InitKeyType = InitKeyType.Attribute
        //    });
        //    Ddb.CodeFirst.InitTables(typeof(DepartmentTrees));

        //    int treeCount = Ddb.Queryable<DepartmentTrees>().Count();
        //    int maxlevel = Ddb.Queryable<DepartmentTrees>().Max(it => it.level);
        //    int countRow = 0;
        //    log.Info("\r\n------------------------------------------------钉钉组织框架根据ESB部门树数据调整------------------------------------------------\r\n");
        //    for (int i = 1; i <= maxlevel; i++)
        //    {
        //        List<DepartmentTrees> list = new List<DepartmentTrees>();
        //        list = Ddb.Queryable<DepartmentTrees>().Where(it => it.level == i).ToList();
        //        foreach (var item in list)
        //        {
        //            countRow++;
        //            try
        //            {
        //                string DepartmentJson = GetDepartment(item.DD_Id);

        //                DepartmentEntity model = new DepartmentEntity();
        //                model = Newtonsoft.Json.JsonConvert.DeserializeObject<DepartmentEntity>(DepartmentJson);
        //                model.parentid = item.DD_ParentId;
        //                model.order = list.IndexOf(item).ToString();
        //                string param = model.ToJson();
        //                Result result = DepartmentBll.Update(param);
        //                if (result != null)
        //                {
        //                    if (result.errcode == "0")
        //                    {
        //                        Console.Write("第" + i + "级数据更新成功," + result.errmsg + ",已更新：" + countRow + "条,当前进度：" + (Math.Round(Convert.ToDecimal(Convert.ToDouble(countRow) / Convert.ToDouble(treeCount)), 2, MidpointRounding.AwayFromZero) * 100) + "%\n\n");
        //                    }
        //                    else
        //                    {
        //                        Console.Write(result.errmsg);
        //                    }
        //                }
        //                else
        //                {
        //                    Console.Write("\r\n无返回数据\r\n");
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                log.Error("\r\n" + ex.Message + "\r\n");
        //                Console.Write(ex.Message);
        //            }
        //        }
        //    }
        //    Console.Write("更新成功,共更新 " + countRow + " 条数据");
        //}

        private static void DeleteEmployee(string UserId)
        {
            try
            {

                Result Result = EmployeeBll.Delete(UserId);
                if (Result != null)
                {
                    if (Result.errcode == "0")
                    {
                        Console.Write("\r\n更新成功," + Result.errmsg + "\r\n");
                    }
                    else
                    {
                        Console.Write("\r\n" + Result.errmsg + "\r\n");
                    }
                }
                else
                {
                    Console.Write("\r\n无返回数据\r\n");
                }
            }
            catch (Exception ex)
            {
                Console.Write("\r\n" + ex.Message + "\r\n");
            }
        }

        private static void EmployeeImputDingTalk()
        {
            LogHelper log = LogFactory.GetLogger("EmployeeImputDingTalk");
            try
            {
                List<DepartmentResult> resultList = new List<DepartmentResult>();
                SqlSugarClient Edb = new SqlSugarClient(new ConnectionConfig()
                {
                    ConnectionString = Config.ESBConnectionString,
                    DbType = DbType.SqlServer,
                    IsAutoCloseConnection = true
                });

                List<V_EmployeeToDingTalk> ESB_EmployeeList = Edb.Queryable<V_EmployeeToDingTalk>().ToList();

                var _DepartmentId = new[] { "1000000360", "1000000575", "1000000010" };

                ESB_EmployeeList = ESB_EmployeeList.Where(it => _DepartmentId.Contains(it.ESB_DepartmentId) && it.Enabled.Equals(1)).ToList();

                SqlSugarClient Ddb = new SqlSugarClient(new ConnectionConfig()
                {
                    ConnectionString = Config.DingTalkConnectionString,
                    DbType = DbType.SqlServer,
                    IsAutoCloseConnection = true
                });
                List<DepartmentResult> DTDepartList = Ddb.Queryable<DepartmentResult>().ToList();


                foreach (var item in ESB_EmployeeList)
                {
                    try
                    {
                        EmployeeEntity model = new EmployeeEntity();
                        model.userid = item.UserId;
                        model.name = item.Name;
                        model.department = new List<int>(new int[] { Convert.ToInt32(GetDingTalkDepartmentId(DTDepartList, item.ESB_DepartmentId)) });
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
                                Console.Write("创建成功,UserId=" + Result.userid);
                            }
                            else
                            {
                                Console.Write(Result.errmsg);
                            }
                        }
                        else
                        {
                            Console.Write("无返回数据");
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Console.Write(ex.Message);
            }
        }

        private static string GetDingTalkDepartmentId(List<DepartmentResult> DTDepartList, string ESB_DepartmentID)
        {
            DepartmentResult model = DTDepartList.Where(e => e.ESB_DepartmentID == ESB_DepartmentID).FirstOrDefault<DepartmentResult>();
            return model.id;
        }
        #endregion
    }
}
