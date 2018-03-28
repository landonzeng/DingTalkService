using Model;
using Newtonsoft.Json;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Utility;

namespace Business
{
    public class DepartmentForDingTalkBll
    {
        /// <summary>
        /// 获取部门列表
        /// </summary>
        public static void GetDepartmentList()
        {
            NLogHelper log = NLogFactory.GetLogger("GetDepartmentList");
            try
            {
                GetDepartmentList model = DepartmentBll.GetList();

                if (model != null)
                {
                    if (model.errcode == 0)
                    {
                        //string json = model.department.ToJson();
                        //Console.Write("成功:\n" + json);
                    }
                    else
                    {
                        log.Error("\r\n DepartmentForDingTalkBll-GetDepartmentList() 新增部门失败,具体原因为：" + model.errmsg + "\r\n");
                        //Console.Write(model.errmsg);
                    }
                }
                else
                {
                    log.Error("\r\n DepartmentForDingTalkBll-GetDepartmentList() 获取部门列表失败,具体原因为：无返回数据\r\n");
                    //Console.Write("无返回数据");
                }
            }
            catch (Exception ex)
            {
                log.Error("\r\n DepartmentForDingTalkBll-GetDepartmentList() 获取部门列表失败,具体原因为：" + ex.Message + "\r\n");
                //Console.Write(ex.Message);
            }
        }

        /// <summary>
        /// 新增部门
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parentid"></param>
        public static void AddDepartment(string name, string parentid)
        {
            NLogHelper log = NLogFactory.GetLogger("AddDepartment");
            try
            {
                DepartmentEntity model = new DepartmentEntity();
                model.name = name;
                model.createDeptGroup = true;
                model.autoAddUser = true;
                model.parentid = parentid;
                model.createDeptGroup = true;

                DepartmentResult Result = DepartmentBll.Create(model);

                if (Result != null)
                {
                    if (Result.errcode == 0)
                    {
                        log.Debug("\r\n------------------------------------------------新增部门------------------------------------------------\n\n\r\n部门id为:" + Result.id + "\n");
                        //Console.Write("创建成功,ID=" + Result.id);
                    }
                    else
                    {
                        log.Error("\r\n DepartmentForDingTalkBll-AddDepartment() 新增部门失败,具体原因为：" + Result.errmsg + "\r\n");
                        //Console.Write(Result.errmsg);
                    }
                }
                else
                {
                    log.Error("\r\n DepartmentForDingTalkBll-AddDepartment() 新增部门失败,具体原因为：无返回数据\r\n");
                    //Console.Write("无返回数据");
                }
            }
            catch (Exception ex)
            {
                log.Error("\r\n DepartmentForDingTalkBll-AddDepartment() 新增部门失败,具体原因为：" + ex.Message + "\r\n");
                //Console.Write(ex.Message);
            }
        }

        /// <summary>
        /// 根据ESB数据为钉钉新增部门
        /// </summary>
        /// <param name="Edb"></param>
        /// <param name="Ddb"></param>
        public static void InsertDepartmentForDingTalk(SqlSugarClient Edb, SqlSugarClient Ddb)
        {

            NLogHelper log = NLogFactory.GetLogger("InsertDepartmentForDingTalk");
            List<DepartmentResult> resultList = new List<DepartmentResult>();
            List<V_OperationObject> OperationList = new List<V_OperationObject>();


            OperationList = Tbiz_OperationTempBll.GetOperationList(Edb, 0, 1);

            log.Info("\r\n------------------------------------------------ESB部门数据导入到钉钉------------------------------------------------\r\n");

            #region 循环插入到钉钉
            foreach (var item in OperationList)
            {
                DepartmentResult dr = Ddb.Queryable<DepartmentResult>().Where(it => it.ESB_DepartmentID.Equals(item.DepartmentId)).First();
                if (dr != null)
                {
                    log.Error("钉钉部门新增时，DepartmentResult中ESB_DepartmentID已存在，ESB_DepartmentID=" + dr.ESB_DepartmentID + "; 部门名称=" + dr.ESB_DepartmentName);
                }

                DepartmentEntity model = new DepartmentEntity();
                model.name = item.FullName;
                model.createDeptGroup = true;
                model.autoAddUser = true;
                model.parentid = "1";
                model.createDeptGroup = true;
                DepartmentResult result = DepartmentBll.Create(model);
                //将ESB中的id和名称存入到钉钉同步数据库中
                result.ESB_DepartmentID = item.DepartmentId;
                result.ESB_DepartmentName = item.FullName;

                if (result != null)
                {
                    if (result.errcode == 0)
                    {
                        //string json = result.id;

                        //log.Debug("\n部门id为:" + result.id + "\n");
                        resultList.Add(result);
                        //Console.Write("成功:" + json + "\n 已插入：" + (Convert.ToInt32(OperationList.IndexOf(item)) + 1) + "条,当前进度：" + (Math.Round(Convert.ToDecimal(Convert.ToDouble((OperationList.IndexOf(item) + 1)) / Convert.ToDouble(OperationList.Count)) * 100, 2, MidpointRounding.AwayFromZero)) + "%\n\n");
                    }
                    else
                    {
                        log.Error("\n DepartmentForDingTalkBll-InsertDepartmentForDingTalk()-foreach (var item in OperationList) 导入部门ESBDepartmentId为:" + item.DepartmentId + "， 错误原因：" + result.errmsg + "\n");
                        //Console.Write("\r\n导入部门ESBDepartmentId为:" + item.DepartmentId + "， 错误原因：" + result.errmsg + "\r\n");
                    }
                }
                else
                {
                    log.Error("\r\n DepartmentForDingTalkBll-InsertDepartmentForDingTalk()-foreach (var item in OperationList) 无返回数据,DepartmentId=" + item.DepartmentId + "\r\n");
                    //Console.Write("\r\n无返回数据\r\n");
                }
            }
            #endregion

            #region 将钉钉中的部门ID记录到项目数据库中
            //正常情况下，resultList记录的所有数据均在钉钉记录数据中不存在，所以直接插入即可
            if (resultList.Count > 0)
            {
                int count = Ddb.Insertable<DepartmentResult>(resultList).ExecuteCommand();
            }

            //当存储过程查出来的数据为0时，说明操作表中的部门插入数据均为无效数据(部门为挂在部门树上)，所以将这部分数据删除
            if (OperationList.Count == 0)
            {
                int count = Edb.Deleteable<Tbiz_OperationTemp>().Where(it => it.Operation == 1 && it.Type == 0).ExecuteCommand();
            }
            #endregion

            #region 部门信息新增到记录表后，同时需要将新增的数据同时添加到部门树记录表中
            List<V_DingTalk_DepartmentTree> DepTreeList = Edb.Ado.SqlQuery<V_DingTalk_DepartmentTree>("exec SP_DingTalk_DepartmentTree");

            List<DepartmentTrees> list = JsonConvert.DeserializeObject<List<DepartmentTrees>>(JsonConvert.SerializeObject(DepTreeList)).OrderBy(e => e.level).ToList();

            var InsertList = list.Where(p => OperationList.Exists(q => q.DepartmentId == p.DepartmentId)).ToList();
            List<DepartmentTrees> InDepTreesList = new List<DepartmentTrees>();
            foreach (var item in InsertList)
            {
                string DD_DepartmentId = "1";
                if (item.ParentDepartmentId != "1000000001")
                {
                    DD_DepartmentId = Ddb.Queryable<DepartmentResult>().Where(it => it.ESB_DepartmentID.Equals(item.ParentDepartmentId)).First().id;
                }
                DepartmentTrees depTree = new DepartmentTrees();
                depTree.DD_Id = item.DD_Id;
                depTree.DD_ParentId = DD_DepartmentId;
                depTree.DepartmentId = item.DepartmentId;
                depTree.FullName = item.FullName;
                depTree.ParentDepartmentId = item.ParentDepartmentId;
                depTree.level = item.level;
                depTree.CreateDate = DateTime.Now;
                InDepTreesList.Add(depTree);

            }
            //将钉钉的id和父id以及ESB部门id记录到钉钉部门树记录表中
            if (InDepTreesList.Count > 0)
            {
                int inCount = Ddb.Insertable<DepartmentTrees>(InDepTreesList).ExecuteCommand();
            }
            #endregion

            #region 根据钉钉记录表中的部门树信息查询出钉钉部门在钉钉中的父部门Id，并更新钉钉部门信息，然后删除ESB同步记录
            foreach (var item in InDepTreesList)
            {
                string DepartmentJson = GetDepartment(item.DD_Id);

                DepartmentEntity model = new DepartmentEntity();
                model = JsonConvert.DeserializeObject<DepartmentEntity>(DepartmentJson);
                model.order = "";
                model.parentid = item.DD_ParentId;
                string param = JsonConvert.SerializeObject(model);
                Result result = DepartmentBll.Update(param);
                if (result != null)
                {
                    if (result.errcode == "0")
                    {
                        //Console.Write("数据更新成功\r\n");
                    }
                    else
                    {
                        log.Error("\n DepartmentForDingTalkBll-InsertDepartmentForDingTalk()-foreach (var item in InDepTreesList) 更新部门ESBDepartmentId为:" + item.DepartmentId + "， 错误原因：" + result.errmsg + "\n");
                        //Console.Write("\n更新部门ESBDepartmentId为:" + item.DepartmentId + "， 错误原因：" + result.errmsg + "\n");
                    }
                }
                else
                {
                    log.Debug("\r\n DepartmentForDingTalkBll-InsertDepartmentForDingTalk()-foreach (var item in InDepTreesList) 无返回数据,DepartmentId=" + item.DepartmentId + "\r\n");
                    //Console.Write("\r\n无返回数据\r\n");
                }
                //删除ESB临时操作记录表中的数据
                int cou = Edb.Deleteable<Tbiz_OperationTemp>().Where(it => it.ObjectId.Equals(item.DepartmentId) && it.Type == 0 && it.Operation == 1).ExecuteCommand();
            }
            #endregion
        }

        /// <summary>
        /// 根据ESB数据为钉钉更新部门
        /// </summary>
        /// <param name="Edb"></param>
        /// <param name="Ddb"></param>
        public static void UpdateDepartmentForDingTalk(SqlSugarClient Edb, SqlSugarClient Ddb)
        {
            NLogHelper log = NLogFactory.GetLogger("UpdateDepartmentForDingTalk");
            List<DepartmentTrees> DD_TreesList = new List<DepartmentTrees>();
            List<V_OperationObject> OperationList = new List<V_OperationObject>();


            //获取ESB中目前已经更新的数据
            OperationList = Tbiz_OperationTempBll.GetOperationList(Edb, 0, 0);

            #region 获取ESB中所有部门的父部门ID
            List<Department_Tree_Temp> DeptTreeslist = Edb.Queryable<V_Effective_Tbiz_DepartmentInfo, V_Effective_Tbiz_DepartmentTreeas>((a, b) => new object[] { JoinType.Left, a.DepartmentId == b.TreeNode })
            .Where((a, b) => a.DepartmentId != "1000000001")
            .Select
            (
                (a, b) =>
                new Department_Tree_Temp
                {
                    DepartmentId = a.DepartmentId,
                    FullName = a.FullName,
                    ParentId = b.ParentNodeName,
                    Enabled = a.Enabled
                }
            ).ToList();
            #endregion

            //根据ESB操作记录中更新的ID，查出目前的部门结构信息
            var deptList = DeptTreeslist.Where(p => OperationList.Exists(q => q.DepartmentId == p.DepartmentId)).OrderBy(it => it.DepartmentId).ToList();

            log.Info("\r\n------------------------------------------------根据ESB更新钉钉部门数据------------------------------------------------\r\n");


            List<Department_Tree_Temp> deptList2 = new List<Department_Tree_Temp>();

            //钉钉中的所有部门信息
            GetDepartmentList DD_DepartmentList = DepartmentBll.GetList();

            foreach (var item in deptList)
            {
                try
                {
                    //根据新的ESB部门信息数据获取钉钉库中的钉钉部门id，更新钉钉组织框架
                    DepartmentResult DD_Department = Ddb.Queryable<DepartmentResult>().Where(it => it.ESB_DepartmentID == item.DepartmentId).First();

                    #region 钉钉部门更新同时对钉钉记录表进行操作
                    if (DD_Department == null && !string.IsNullOrWhiteSpace(item.ParentId))
                    {
                        //部门启用的情况下，钉钉关系表中不存在钉钉Id和esb部门id
                        if (item.Enabled != "I")
                        {
                            //判断钉钉中是否存在改部门名称
                            //Department DD_Depart = DepartmentBll.GetByName(item.FullName);

                            log.Debug("\r\n 根据新的ESB部门信息数据获取钉钉库中的钉钉部门id，更新钉钉组织框架事 DepartmentId=" + item.DepartmentId + "的数据未查到实体，所以对该ESB部门进行新增\r\n");
                            //当钉钉关系记录表中不存在，但是esb中父id存在时，表示该部门之前停用了，目前重新启用，由于钉钉中没有停用概念，一旦停用即删除，所以需要重新添加进去
                            AddDepartmentForDingTalk(Edb, Ddb, item);
                        }
                        else
                        {
                            deptList2.Add(item);
                        }
                        continue;
                    }
                    else
                    {
                        //当钉钉关系记录表中部门状态为禁用 或者 钉钉库关系库中不存在并且esb中父id不存在（从ESB组织树上摘下）时
                        if (item.Enabled == "I" || (DD_Department == null && string.IsNullOrWhiteSpace(item.ParentId)))
                        {
                            //钉钉库关系库中不存在并且esb中父id不存在时
                            if (DD_Department == null)
                            {
                                //检查钉钉中该部门名称信息是否存在
                                var DD_Dept = DD_DepartmentList.department.Where(e => e.name.Equals(item.FullName)).ToList().FirstOrDefault();
                                //钉钉中该部门信息不存在
                                if (DD_Dept == null)
                                {
                                    //检查钉钉Tree关系表中该ESB部门id是否存在
                                    var tree = Ddb.Queryable<DepartmentTrees>().Where(it => it.DepartmentId == item.DepartmentId).First();
                                    if (tree != null)
                                    {
                                        //部门在钉钉中不存在时，删除钉钉部门树记录表                                 
                                        int ddel = Ddb.Deleteable<DepartmentTrees>().Where(it => it.DepartmentId.Equals(item.DepartmentId)).ExecuteCommand();
                                    }
                                    //删除esb中该数据的操作记录
                                    deptList2.Add(item);
                                    continue;
                                }
                                else
                                {
                                    //钉钉中该部门信息存在，但是id不存在时
                                    if (string.IsNullOrWhiteSpace(DD_Dept.id))
                                    {
                                        DD_Department.id = DD_Dept.id;
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                            }
                            //遍历每个id下的所有人员
                            GetDepartmentForUserList UserList = DepartmentBll.GetByDepartmentIdForUserInfoList(DD_Department.id);
                            //所有人都挂在公司下
                            foreach (var Useritem in UserList.userlist)
                            {
                                string EmployeeJson = EmployeeForDingTalkBll.GetEmployee(Useritem.userid);
                                EmployeeEntity Emodel = Newtonsoft.Json.JsonConvert.DeserializeObject<EmployeeEntity>(EmployeeJson);

                                Emodel.department = new List<int>() { 1 };

                                string param = JsonConvert.SerializeObject(Emodel);

                                Result uResult = EmployeeBll.Update(param);

                                if (uResult.errcode != "0")
                                {
                                    log.Error("\r\n DepartmentForDingTalkBll-UpdateDepartmentForDingTalk()-foreach (var Useritem in UserList.userlist) " + uResult.errmsg + "；部门ID为：" + item.DepartmentId + "\r\n");
                                }
                            }
                            //钉钉中删除禁用的部门
                            Result rest = DepartmentBll.Delete(DD_Department.id);
                            if (rest.errcode == "0")
                            {
                                //删除esb同步操作记录
                                int count = Edb.Deleteable<Tbiz_OperationTemp>().Where(it => it.ObjectId.Equals(item.DepartmentId) && it.Type == 0 && it.Operation == 0).ExecuteCommand();
                            }
                            else
                            {
                                if (rest.errcode != "60003")
                                {
                                    log.Error("\r\n DepartmentForDingTalkBll-UpdateDepartmentForDingTalk() 删除禁用部门错误,具体原因为：" + rest.errmsg + "；部门ID为：" + item.DepartmentId + "\r\n");
                                }
                                else
                                {
                                    //部门在钉钉中不存在时，删除钉钉部门树记录表
                                    int dd_count = Ddb.Deleteable<DepartmentResult>().Where(it => it.ESB_DepartmentID.Equals(item.DepartmentId)).ExecuteCommand();
                                    int ddel = Ddb.Deleteable<DepartmentTrees>().Where(it => it.DepartmentId.Equals(item.DepartmentId)).ExecuteCommand();
                                    deptList2.Add(item);
                                }
                            }
                            continue;
                        }
                    }
                    #endregion

                    string Parentid = "1";
                    if (!item.ParentId.Equals("1000000001"))
                    {
                        try
                        {
                            //获取ESB操作记录中当前ESB父部门id对应的钉钉ID
                            Parentid = Ddb.Queryable<DepartmentResult>().Where(it => it.ESB_DepartmentID == item.ParentId).First().id;
                        }
                        catch
                        {
                            //一旦catch，则说明钉钉关系记录表中不存在该父部门
                            Parentid = AddDepartmentParentForDingTalk(Edb, Ddb, item);
                        }
                    }

                    string DepartmentJson = GetDepartment(DD_Department.id);
                    //如果部门不存在钉钉ESB同步记录表中，则添加进钉钉并且加入记录表
                    //log.Debug("\r\n DepartmentUpdate json: " + DepartmentJson + "\r\n");
                    string errcode = Newtonsoft.Json.JsonConvert.DeserializeObject<Result>(DepartmentJson).errcode;
                    //部门不存在
                    if (errcode == "60003")
                    {
                        if (DD_Department != null && !string.IsNullOrWhiteSpace(DD_Department.id))
                        {
                            //删除钉钉中的部门
                            var a = DepartmentBll.Delete(DD_Department.id);
                            //删除钉钉关系表和钉钉树关系表
                            int i = Ddb.Deleteable<DepartmentResult>().Where(it => it.id.Equals(DD_Department.id)).ExecuteCommand();
                            i = Ddb.Deleteable<DepartmentTrees>().Where(it => it.DD_Id.Equals(DD_Department.id)).ExecuteCommand();
                        }

                        log.Debug("\r\n 部门不存在 添加部门前，删除之前的记录 DepartmentId=" + item.DepartmentId + "的数据未查到实体，所以对该ESB部门进行新增\r\n");

                        AddDepartmentForDingTalk(Edb, Ddb, item);
                        continue;
                    }

                    DepartmentEntity model = new DepartmentEntity();

                    model = Newtonsoft.Json.JsonConvert.DeserializeObject<DepartmentEntity>(DepartmentJson);

                    //如果部门名或者部门父ID与钉钉的不同的时候就更新，如果相同就不调用更新接口
                    if (!model.name.Equals(item.FullName) || !model.parentid.Equals(Parentid))
                    {
                        model.name = item.FullName;
                        model.parentid = Parentid;

                        string param = JsonConvert.SerializeObject(model);
                        Result result = DepartmentBll.Update(param);
                        if (result.errcode == "0")
                        {
                            DepartmentTrees DD_Trees = new DepartmentTrees();
                            DD_Trees.DD_ParentId = model.parentid;
                            DD_Trees.DD_Id = model.id;
                            DD_Trees.DepartmentId = item.DepartmentId;
                            DD_Trees.FullName = item.FullName;
                            DD_Trees.ParentDepartmentId = item.ParentId;
                            DD_TreesList.Add(DD_Trees);
                        }
                    }
                    deptList2.Add(item);
                }
                catch
                {
                    continue;
                }
                //Console.Write("\n 已执行：" + (Convert.ToInt32(deptList.IndexOf(item)) + 1) + "条,当前进度：" + (Math.Round(Convert.ToDecimal(Convert.ToDouble((deptList.IndexOf(item) + 1)) / Convert.ToDouble(deptList.Count)) * 100, 2, MidpointRounding.AwayFromZero)) + "%\n");
            }
            int u = 0;
            foreach (var item in DD_TreesList)
            {
                try
                {
                    DepartmentTrees oldDepTree = Ddb.Queryable<DepartmentTrees>().Where(it => it.DepartmentId.Equals(item.DepartmentId)).First();
                    item.level = oldDepTree.level;
                    item.CreateDate = oldDepTree.CreateDate;
                    u = Ddb.Updateable<DepartmentTrees>(item).Where(it => it.DepartmentId == item.DepartmentId).ExecuteCommand();
                }
                catch (Exception ex)
                {
                    log.Error("\r\n 部门更新部门树报错，DD_TreesList中DepartmentId=" + item.DepartmentId + ";   \n" + ex);
                    continue;
                }
            }

            int d = 0;
            foreach (var item in deptList2)
            {
                d = Edb.Deleteable<Tbiz_OperationTemp>().Where(it => it.ObjectId.Equals(item.DepartmentId)).ExecuteCommand();
            }
            //Console.Write("同步完成\r\n");
        }

        /// <summary>
        /// 根据钉钉部门id获取部门详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static string GetDepartment(string Id)
        {
            GetDepartment model = new GetDepartment();
            try
            {
                model = DepartmentBll.Get(Id);
                if (model.errcode == "90002")
                {
                    System.Threading.Thread.Sleep(1500);
                    model = DepartmentBll.Get(Id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return JsonConvert.SerializeObject(model);
        }

        /// <summary>
        /// 初始化后删除临时部门
        /// </summary>
        /// <param name="TempDeptID"></param>
        public static void DeleteTempDDDept(SqlSugarClient Edb, string TempDeptID)
        {
            NLogHelper log = NLogFactory.GetLogger("DeleteTempDDDept");
            try
            {
                List<DepartmentResult> resultList = new List<DepartmentResult>();

                List<V_EmployeeToDingTalk> ESB_EmployeeList = Edb.Queryable<V_EmployeeToDingTalk>().ToList();

                ByDepIdFroSimpleListResult simpleList = DepartmentBll.GetByDepartmentIdForSimpleList(TempDeptID);

                foreach (var item in simpleList.userlist)
                {
                    try
                    {
                        //得到当前临时部门中的用户信息，根据用户信息查询esb中，该用户所在部门，然后更新到目前钉钉中的部门下
                        GetEmployee employee = EmployeeBll.Get(item.userid);

                        if (employee != null)
                        {
                            if (employee.errcode == "0")
                            {

                                V_EmployeeToDingTalk em = ESB_EmployeeList.Where(e => e.Name == employee.name).FirstOrDefault();
                                V_DingTalk_DepartmentTree vd = Edb.Ado.SqlQuery<V_DingTalk_DepartmentTree>("exec SP_DingTalk_DepartmentTree").Where(e => e.DepartmentId == em.ESB_DepartmentId).First();

                                string EmployeeJson = EmployeeForDingTalkBll.GetEmployee(item.userid);

                                EmployeeEntity model = Newtonsoft.Json.JsonConvert.DeserializeObject<EmployeeEntity>(EmployeeJson);

                                model.department = new List<int>(new int[] { Convert.ToInt32(vd.DD_Id) });

                                string param = JsonConvert.SerializeObject(model);

                                Result Result = EmployeeBll.Update(param);
                                if (Result != null)
                                {
                                    if (Result.errcode == "0")
                                    {
                                        //Console.Write("更新成功," + Result.errmsg);
                                    }
                                    else
                                    {
                                        log.Error("\r\n DepartmentForDingTalkBll-DeleteTempDDDept() 初始化后删除临时部门错误,具体原因为：" + Result.errmsg + "\r\n");
                                        //Console.Write(Result.errmsg);
                                    }
                                }
                                else
                                {
                                    log.Error("\r\n DepartmentForDingTalkBll-DeleteTempDDDept() 初始化后删除临时部门错误,具体原因为：无返回数据\r\n");
                                    //Console.Write("无返回数据");
                                }
                            }
                            else
                            {
                                log.Error("\r\n DepartmentForDingTalkBll-DeleteTempDDDept() 初始化后删除临时部门错误,具体原因为：" + employee.errmsg + "\r\n");

                                //Console.Write("\r\n" + employee.errmsg);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error("\r\n DepartmentForDingTalkBll-DeleteTempDDDept() catch" + ex + "\r\n");
                        continue;
                    }
                }
                //最后删除部门
                Result r = DepartmentBll.Delete(TempDeptID);
                if (r.errcode == "0")
                {
                    //Console.Write("\r\n删除成功");
                }
                else
                {
                    if (r.errcode != "60003")
                    {
                        log.Error("\r\n DepartmentForDingTalkBll-DeleteTempDDDept() 初始化后删除临时部门中,最后删除部门失败,具体原因为：" + r.errmsg + "\r\n");
                    }

                    //Console.Write("\r\n" + r.errmsg);
                }
            }
            catch (Exception ex)
            {
                log.Error("\r\n DepartmentForDingTalkBll-DeleteTempDDDept() " + ex + "\r\n");
                //Console.Write(ex.Message);
            }
        }

        /// <summary>
        /// 根据ESB部门ID查出对应的钉钉ID
        /// </summary>
        /// <param name="DTDepartList"></param>
        /// <param name="ESB_DepartmentID"></param>
        /// <returns></returns>
        public static string GetDingTalkDepartmentId(List<DepartmentResult> DTDepartList, string ESB_DepartmentID)
        {
            NLogHelper log = NLogFactory.GetLogger("GetDingTalkDepartmentId");
            try
            {
                DepartmentResult model = DTDepartList.Where(e => e.ESB_DepartmentID == ESB_DepartmentID).FirstOrDefault<DepartmentResult>();
                return model.id;
            }
            catch (Exception ex)
            {
                log.Error("\r\n DepartmentForDingTalkBll-GetDingTalkDepartmentId() ESB_DepartmentID:" + ESB_DepartmentID + "\n 异常详情：" + ex);
                return "1";
            }
        }

        /// <summary>
        /// 根据钉钉id删除钉钉部门
        /// </summary>
        /// <param name="id"></param>
        public static void DeleteDepartment(string id)
        {
            NLogHelper log = NLogFactory.GetLogger("DeleteDepartment");
            try
            {
                Result Result = DepartmentBll.Delete(id);

                if (Result != null)
                {
                    if (Result.errcode == "0")
                    {
                        //Console.Write("删除成功," + Result.errmsg);
                    }
                    else
                    {
                        if (Result.errcode != "60003")
                        {
                            log.Error("\r\n DepartmentForDingTalkBll-DeleteDepartment() 根据钉钉id删除钉钉部门失败,具体原因为：" + Result.errmsg + "\r\n");
                        }

                        //Console.Write(Result.errmsg);
                    }
                }
                else
                {
                    log.Error("\r\n DepartmentForDingTalkBll-DeleteDepartment() 根据钉钉id删除钉钉部门失败,具体原因为：无返回数据\r\n");
                    //Console.Write("无返回数据");
                }
            }
            catch (Exception ex)
            {
                log.Error("\r\n DepartmentForDingTalkBll-DeleteDepartment() 根据钉钉id删除钉钉部门失败,具体原因为：" + ex.Message + "\r\n");
                //Console.Write(ex.Message);
            }
        }

        /// <summary>
        /// 更新钉钉部门信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="parentid"></param>
        public static void UpdateDepartment(string id, string name, string parentid)
        {
            NLogHelper log = NLogFactory.GetLogger("UpdateDepartment");
            try
            {
                string DepartmentJson = GetDepartment(id);

                DepartmentEntity model = new DepartmentEntity();
                model = Newtonsoft.Json.JsonConvert.DeserializeObject<DepartmentEntity>(DepartmentJson);
                model.parentid = parentid;
                model.name = name;

                string param = JsonConvert.SerializeObject(model);

                Result result = DepartmentBll.Update(param);

                if (result != null)
                {
                    if (result.errcode == "0")
                    {
                        //Console.Write("更新成功," + result.errmsg);
                    }
                    else
                    {
                        log.Error("\r\n DepartmentForDingTalkBll-UpdateDepartment() 更新钉钉部门信息失败,具体原因为：" + result.errmsg + "\r\n");
                        //Console.Write(result.errmsg);
                    }
                }
                else
                {
                    log.Error("\r\n DepartmentForDingTalkBll-UpdateDepartment() 更新钉钉部门信息失败,具体原因为：无返回数据\r\n");
                    //Console.Write("无返回数据");
                }
            }
            catch (Exception ex)
            {
                log.Error("\r\n DepartmentForDingTalkBll-UpdateDepartment() 更新钉钉部门信息失败,具体原因为：" + ex.Message + "\r\n");
                //Console.Write(ex.Message);
            }
        }

        /// <summary>
        /// 根据部门id获取部门下所有用户
        /// </summary>
        /// <param name="DepartmentId"></param>
        public static void GetByDepartmentIdForUserInfoList(string DepartmentId)
        {
            NLogHelper log = NLogFactory.GetLogger("GetByDepartmentIdForUserInfoList");
            GetDepartmentForUserList model = DepartmentBll.GetByDepartmentIdForUserInfoList(DepartmentId);

            if (model != null)
            {
                if (model.errcode == 0)
                {
                    //string json = model.userlist.ToJson();
                    //Console.Write("成功:\n" + json);
                }
                else
                {
                    log.Error("\r\n DepartmentForDingTalkBll-GetByDepartmentIdForUserInfoList() 根据部门id获取部门下所有用户失败,具体原因为：" + model.errmsg + "\r\n");
                    //Console.Write(model.errmsg);
                }
            }
            else
            {
                log.Error("\r\n DepartmentForDingTalkBll-GetByDepartmentIdForUserInfoList() 根据部门id获取部门下所有用户失败,具体原因为：无返回数据\r\n");
                //Console.Write("无返回数据");
            }

        }

        /// <summary>
        /// 更新时提示部门不存在，则新增部门
        /// </summary>
        /// <param name="Edb"></param>
        /// <param name="Ddb"></param>
        /// <param name="depTemp"></param>
        public static void AddDepartmentForDingTalk(SqlSugarClient Edb, SqlSugarClient Ddb, Department_Tree_Temp depTemp)
        {
            NLogHelper log = NLogFactory.GetLogger("AddDepartmentForDingTalk");
            //List<DepartmentResult> resultList = new List<DepartmentResult>();
            List<V_OperationObject> OperationList = new List<V_OperationObject>();

            DepartmentResult depRest = Ddb.Queryable<DepartmentResult>().Where(it => it.ESB_DepartmentID.Equals(depTemp.DepartmentId)).First();
            if (depRest != null)
            {
                log.Error("钉钉部门新增时，DepartmentResult中ESB_DepartmentID已存在，ESB_DepartmentID=" + depRest.ESB_DepartmentID + "; 部门名称=" + depRest.ESB_DepartmentName);
            }

            log.Info("\r\n------------------------------------------------ESB部门数据导入到钉钉------------------------------------------------\r\n");

            #region 插入到钉钉
            DepartmentEntity model = new DepartmentEntity();
            model.name = depTemp.FullName;
            model.createDeptGroup = true;
            model.autoAddUser = true;
            model.parentid = "1";
            model.createDeptGroup = true;
            DepartmentResult result = DepartmentBll.Create(model);
            //将ESB中的id和名称存入到钉钉同步数据库中
            result.ESB_DepartmentID = depTemp.DepartmentId;
            result.ESB_DepartmentName = depTemp.FullName;

            if (result != null)
            {
                if (result.errcode == 0)
                {
                    //string json = result.id;

                    //log.Debug("\n部门id为:" + result.id + "\n");
                }
                else
                {
                    // 父部门下该部门名称已存在 如果部门存在，则查询出该部门所在钉钉的id是什么，重新更新钉钉记录表
                    if (result.errcode == 60008)
                    {
                        GetDepartmentList DepartmentList = DepartmentBll.GetList();

                        string TempDDDeptID = DepartmentList.department.Where(e => e.name.Equals(depTemp.FullName)).ToList().FirstOrDefault().id;

                        try
                        {
                            //查询一下钉钉关系记录表中该ESB部门编号存不存在
                            DepartmentResult dr = Ddb.Queryable<DepartmentResult>().Where(it => it.ESB_DepartmentID.Equals(depTemp.DepartmentId)).First();
                            //判定钉钉id是否与钉钉关系记录表中的钉钉id相等，不相等则更新
                            if (!dr.id.Equals(TempDDDeptID))
                            {
                                dr.id = TempDDDeptID;
                                int up = Ddb.Updateable<DepartmentResult>(dr).Where(it => it.ESB_DepartmentID.Equals(depTemp.DepartmentId)).ExecuteCommand();
                            }
                        }
                        catch
                        {

                        }
                    }
                    else
                    {
                        log.Error("\n DepartmentForDingTalkBll-AddDepartmentForDingTalk() 导入部门ESBDepartmentId为:" + depTemp.DepartmentId + "， 错误原因：" + result.errmsg + "\n");
                    }
                    return;
                    //Console.Write("\r\n导入部门ESBDepartmentId为:" + depTemp.DepartmentId + "， 错误原因：" + result.errmsg + "\r\n");
                }
            }
            else
            {
                log.Error("\r\n DepartmentForDingTalkBll-AddDepartmentForDingTalk() 钉钉添加部门失败,具体原因为：无返回数据\r\n");
                return;
                //Console.Write("\r\n无返回数据\r\n");
            }
            #endregion

            //当钉钉创建部门成功后
            try
            {
                #region 将钉钉中的部门ID记录到项目数据库中
                //查一下ESB部门id在不在钉钉同步记录表中，在就删除后重新插入，不在就直接插入
                int c = Ddb.Queryable<DepartmentResult>().Where(it => it.ESB_DepartmentID.Equals(depTemp.DepartmentId)).Count();
                if (c > 0)
                {
                    int d = Ddb.Deleteable<DepartmentResult>().Where(it => it.ESB_DepartmentID.Equals(depTemp.DepartmentId)).ExecuteCommand();
                    //是否要删除钉钉Tree记录表中的记录
                    d = Ddb.Deleteable<DepartmentTrees>().Where(it => it.DepartmentId.Equals(depTemp.DepartmentId)).ExecuteCommand();
                }
                int count = Ddb.Insertable<DepartmentResult>(result).ExecuteCommand();
                #endregion

                #region 部门信息新增到记录表后，同时需要将新增的数据同时添加到部门树记录表中

                //根据钉钉记录表的数据遍历出每个部门的父部门信息等
                V_DingTalk_DepartmentTree DD_depTree = Edb.Ado.SqlQuery<V_DingTalk_DepartmentTree>("exec SP_DingTalk_DepartmentTree").Where(it => it.DepartmentId.Equals(result.ESB_DepartmentID)).First();

                string DD_DepartmentId = "1";

                if (DD_depTree != null)
                {
                    try
                    {
                        //根据ESB数据里的父部门id查询出对应在钉钉记录表中该父部门的钉钉id
                        DD_DepartmentId = Ddb.Queryable<DepartmentResult>().Where(it => it.ESB_DepartmentID.Equals(DD_depTree.ParentDepartmentId)).First().id;
                    }
                    catch
                    {
                        DD_DepartmentId = "1";
                    }
                }

                DepartmentTrees depTree = new DepartmentTrees();
                depTree.DD_Id = DD_depTree.DD_Id;
                depTree.DD_ParentId = DD_DepartmentId;
                depTree.DepartmentId = DD_depTree.DepartmentId;
                depTree.FullName = DD_depTree.FullName;
                depTree.ParentDepartmentId = DD_depTree.ParentDepartmentId;
                depTree.level = DD_depTree.level;
                depTree.CreateDate = DateTime.Now;

                //将钉钉的id和父id以及ESB部门id记录到钉钉部门树记录表中
                int inCount = Ddb.Insertable<DepartmentTrees>(depTree).ExecuteCommand();

                #endregion

                #region 根据钉钉记录表中的部门树信息查询出钉钉部门在钉钉中的父部门Id，并更新钉钉部门信息，然后删除ESB同步记录

                string DepartmentJson = GetDepartment(depTree.DD_Id);

                DepartmentEntity departModel = new DepartmentEntity();
                departModel = Newtonsoft.Json.JsonConvert.DeserializeObject<DepartmentEntity>(DepartmentJson);
                departModel.order = "";
                departModel.parentid = depTree.DD_ParentId;
                string param = JsonConvert.SerializeObject(departModel);
                Result res = DepartmentBll.Update(param);
                if (res != null)
                {
                    if (res.errcode == "0")
                    {
                        //Console.Write("数据更新成功\r\n");
                    }
                    else
                    {
                        log.Error("\n DepartmentForDingTalkBll-AddDepartmentForDingTalk() 更新部门ESBDepartmentId为:" + depTree.DepartmentId + "， 错误原因：" + result.errmsg + "\n");
                        //Console.Write("\n更新部门ESBDepartmentId为:" + depTree.DepartmentId + "， 错误原因：" + result.errmsg + "\n");
                    }
                }
                else
                {
                    log.Error("\r\n DepartmentForDingTalkBll-AddDepartmentForDingTalk() 钉钉添加部门失败,具体原因为：无返回数据\r\n");
                    //Console.Write("\r\n无返回数据\r\n");
                }
            }
            catch (Exception ex)
            {
                log.Error("\r\n DepartmentForDingTalkBll-AddDepartmentForDingTalk() 钉钉添加部门失败,具体原因为：" + ex + "\r\n");
            }
            #endregion
        }

        /// <summary>
        /// 根据信息里的父id来新增钉钉数据
        /// </summary>
        /// <param name="Edb"></param>
        /// <param name="Ddb"></param>
        /// <param name="depTemp"></param>
        /// <returns></returns>
        public static string AddDepartmentParentForDingTalk(SqlSugarClient Edb, SqlSugarClient Ddb, Department_Tree_Temp depTemp)
        {
            NLogHelper log = NLogFactory.GetLogger("AddDepartmentParentForDingTalk");

            List<V_OperationObject> OperationList = new List<V_OperationObject>();

            log.Info("\r\n------------------------------------------------ESB部门数据导入到钉钉------------------------------------------------\r\n");
            try
            {
                //钉钉中的所有部门信息
                GetDepartmentList DD_DepartmentList = DepartmentBll.GetList();

                //检查ESB中是否存在该父部门
                V_Effective_Tbiz_DepartmentInfo vmode = Edb.Queryable<V_Effective_Tbiz_DepartmentInfo>().Where(it => it.DepartmentId.Equals(depTemp.ParentId)).First();
                if (string.IsNullOrWhiteSpace(vmode.DepartmentId))
                {
                    return "1";
                }

                #region 插入到钉钉
                DepartmentEntity model = new DepartmentEntity();
                model.name = vmode.FullName;
                model.createDeptGroup = true;
                model.autoAddUser = true;
                model.parentid = "1";
                model.createDeptGroup = true;
                DepartmentResult result = DepartmentBll.Create(model);
                //将ESB中的id和名称存入到钉钉同步数据库中
                result.ESB_DepartmentID = depTemp.ParentId;
                result.ESB_DepartmentName = depTemp.FullName;

                if (result != null)
                {
                    if (result.errcode == 0)
                    {
                        //string json = result.id;

                        //log.Debug("\n部门id为:" + result.id + "\n");
                    }
                    else
                    {
                        //父部门下该部门名称已存在
                        if (result.errcode == 60008)
                        {
                            try
                            {
                                var DD_Dept = DD_DepartmentList.department.Where(e => e.name.Equals(depTemp.FullName)).ToList().FirstOrDefault();
                                return DD_Dept.parentid.ToString();
                            }
                            catch
                            {
                                return "1";
                            }
                        }
                        log.Error("\n DepartmentForDingTalkBll-AddDepartmentParentForDingTalk() 导入部门ESBDepartmentId为:" + depTemp.DepartmentId + "， 错误原因：" + result.errmsg + "\n");
                        return "1";
                        //Console.Write("\r\n导入部门ESBDepartmentId为:" + depTemp.DepartmentId + "， 错误原因：" + result.errmsg + "\r\n");
                    }
                }
                else
                {
                    log.Error("\r\n DepartmentForDingTalkBll-AddDepartmentParentForDingTalk() 钉钉添加部门失败,具体原因为：无返回数据\r\n");
                    return "1";
                    //Console.Write("\r\n无返回数据\r\n");
                }

                #endregion

                #region 将钉钉中的部门ID记录到项目数据库中
                //正常情况下，resultList记录的所有数据均在钉钉记录数据中不存在，所以直接插入即可
                int count = Ddb.Insertable<DepartmentResult>(result).ExecuteCommand();
                #endregion

                #region 部门信息新增到记录表后，同时需要将新增的数据同时添加到部门树记录表中
                V_DingTalk_DepartmentTree DD_depTree = Edb.Ado.SqlQuery<V_DingTalk_DepartmentTree>("exec SP_DingTalk_DepartmentTree").Where(it => it.DepartmentId.Equals(result.ESB_DepartmentID)).First();

                string DD_DepartmentId = "1";
                if (DD_depTree != null)
                {
                    try
                    {
                        DD_DepartmentId = Ddb.Queryable<DepartmentResult>().Where(it => it.ESB_DepartmentID.Equals(DD_depTree.ParentDepartmentId)).First().id;
                    }
                    catch
                    {
                        DD_DepartmentId = "1";
                    }
                }
                DepartmentTrees depTree = new DepartmentTrees();
                depTree.DD_Id = DD_depTree.DD_Id;
                depTree.DD_ParentId = DD_DepartmentId;
                depTree.DepartmentId = DD_depTree.DepartmentId;
                depTree.FullName = DD_depTree.FullName;
                depTree.ParentDepartmentId = DD_depTree.ParentDepartmentId;
                depTree.level = DD_depTree.level;
                depTree.CreateDate = DateTime.Now;

                //将钉钉的id和父id以及ESB部门id记录到钉钉部门树记录表中
                int inCount = Ddb.Insertable<DepartmentTrees>(depTree).ExecuteCommand();

                #endregion

                #region 根据钉钉记录表中的部门树信息查询出钉钉部门在钉钉中的父部门Id，并更新钉钉部门信息，然后删除ESB同步记录

                string DepartmentJson = GetDepartment(depTree.DD_Id);

                DepartmentEntity departModel = new DepartmentEntity();
                departModel = Newtonsoft.Json.JsonConvert.DeserializeObject<DepartmentEntity>(DepartmentJson);
                departModel.order = "";
                departModel.parentid = depTree.DD_ParentId;
                string param = JsonConvert.SerializeObject(departModel);
                Result res = DepartmentBll.Update(param);
                if (res != null)
                {
                    if (res.errcode == "0")
                    {
                        return DD_DepartmentId;
                        //Console.Write("数据更新成功\r\n");
                    }
                    else
                    {
                        log.Error("\n DepartmentForDingTalkBll-AddDepartmentParentForDingTalk() 更新部门ESBDepartmentId为:" + depTree.DepartmentId + "， 错误原因：" + result.errmsg + "\n");
                        return "1";
                        //Console.Write("\n更新部门ESBDepartmentId为:" + depTree.DepartmentId + "， 错误原因：" + result.errmsg + "\n");
                    }
                }
                else
                {
                    log.Error("\r\n DepartmentForDingTalkBll-AddDepartmentParentForDingTalk() 钉钉添加部门失败,具体原因为：无返回数据\r\n");
                    return "1";
                    //Console.Write("\r\n无返回数据\r\n");
                }
            }
            catch (Exception ex)
            {
                log.Error("\r\n DepartmentForDingTalkBll-AddDepartmentParentForDingTalk() 钉钉添加部门失败,具体原因为：" + ex + "\r\n");
                return "1";
            }
            #endregion
        }

        public static string DD_DepartmentIsNullForDingTalk(SqlSugarClient Edb, SqlSugarClient Ddb, string ESB_DepartmentId)
        {
            NLogHelper log = NLogFactory.GetLogger("DD_DepartmentIsNullForDingTalk");
            List<V_OperationObject> OperationList = new List<V_OperationObject>();
            string DD_Id = "";

            #region 根据ID获取ESB中部门的父部门ID
            var DeptTreeslist = Edb.Queryable<V_Effective_Tbiz_DepartmentInfo, V_Effective_Tbiz_DepartmentTreeas>((a, b) => new object[] { JoinType.Inner, a.DepartmentId == b.TreeNode })
            .Where((a, b) => a.DepartmentId.Equals(ESB_DepartmentId))
            .Select
            (
                (a, b) =>
                new Department_Tree_Temp
                {
                    DepartmentId = a.DepartmentId,
                    FullName = a.FullName,
                    ParentId = b.ParentNodeName
                }
            ).First();
            #endregion

            Department_Tree_Temp depTemp = DeptTreeslist;
            if (depTemp == null)
            {
                return "1";
            }
            #region 插入到钉钉
            DepartmentEntity model = new DepartmentEntity();
            model.name = depTemp.FullName;
            model.createDeptGroup = true;
            model.autoAddUser = true;
            model.parentid = "1";
            model.createDeptGroup = true;
            DepartmentResult result = DepartmentBll.Create(model);
            //将ESB中的id和名称存入到钉钉同步数据库中
            result.ESB_DepartmentID = depTemp.DepartmentId;
            result.ESB_DepartmentName = depTemp.FullName;

            if (result != null)
            {
                if (result.errcode == 0)
                {
                    DD_Id = result.id;
                }
                else
                {
                    log.Error("\n DepartmentForDingTalkBll-DD_DepartmentIsNullForDingTalk() 导入部门ESBDepartmentId为:" + depTemp.DepartmentId + "， 错误原因：" + result.errmsg + "\n");
                    //Console.Write("\r\n导入部门ESBDepartmentId为:" + depTemp.DepartmentId + "， 错误原因：" + result.errmsg + "\r\n");
                }
            }
            else
            {
                log.Error("\r\n DepartmentForDingTalkBll-DD_DepartmentIsNullForDingTalk() 钉钉添加部门失败,具体原因为：无返回数据\r\n");
                //Console.Write("\r\n无返回数据\r\n");
            }

            #endregion

            #region 将钉钉中的部门ID记录到项目数据库中
            //正常情况下，resultList记录的所有数据均在钉钉记录数据中不存在，所以直接插入即可
            int count = Ddb.Insertable<DepartmentResult>(result).ExecuteCommand();
            #endregion

            #region 部门信息新增到记录表后，同时需要将新增的数据同时添加到部门树记录表中
            V_DingTalk_DepartmentTree DD_depTree = Edb.Ado.SqlQuery<V_DingTalk_DepartmentTree>("exec SP_DingTalk_DepartmentTree").Where(it => it.DepartmentId.Equals(result.ESB_DepartmentID)).First();

            string DD_DepartmentId = "1";
            if (DD_depTree != null)
            {
                try
                {
                    DD_DepartmentId = Ddb.Queryable<DepartmentResult>().Where(it => it.ESB_DepartmentID.Equals(DD_depTree.ParentDepartmentId)).First().id;
                }
                catch
                {
                    DD_DepartmentId = "1";
                }
            }
            DepartmentTrees depTree = new DepartmentTrees();
            depTree.DD_Id = DD_depTree.DD_Id;
            depTree.DD_ParentId = DD_DepartmentId;
            depTree.DepartmentId = DD_depTree.DepartmentId;
            depTree.FullName = DD_depTree.FullName;
            depTree.ParentDepartmentId = DD_depTree.ParentDepartmentId;
            depTree.level = DD_depTree.level;
            depTree.CreateDate = DateTime.Now;

            //将钉钉的id和父id以及ESB部门id记录到钉钉部门树记录表中
            int inCount = Ddb.Insertable<DepartmentTrees>(depTree).ExecuteCommand();

            #endregion

            #region 根据钉钉记录表中的部门树信息查询出钉钉部门在钉钉中的父部门Id，并更新钉钉部门信息

            string DepartmentJson = GetDepartment(depTree.DD_Id);

            DepartmentEntity departModel = new DepartmentEntity();
            departModel = Newtonsoft.Json.JsonConvert.DeserializeObject<DepartmentEntity>(DepartmentJson);
            departModel.order = "";
            departModel.parentid = depTree.DD_ParentId;
            string param = JsonConvert.SerializeObject(departModel);
            Result res = DepartmentBll.Update(param);
            if (res != null)
            {
                if (res.errcode == "0")
                {
                    //Console.Write("数据更新成功\r\n");
                }
                else
                {
                    log.Error("\n DepartmentForDingTalkBll-DD_DepartmentIsNullForDingTalk() 更新部门ESBDepartmentId为:" + depTree.DepartmentId + "， 错误原因：" + result.errmsg + "\n");
                    //Console.Write("\n更新部门ESBDepartmentId为:" + depTree.DepartmentId + "， 错误原因：" + result.errmsg + "\n");
                }
            }
            else
            {
                log.Error("\r\n DepartmentForDingTalkBll-DD_DepartmentIsNullForDingTalk() 钉钉添加部门失败,具体原因为：无返回数据\r\n");
                //Console.Write("\r\n无返回数据\r\n");
            }
            #endregion

            return DD_Id;
        }

        public static void DDUpdate(string DepName)
        {
            GetDepartmentList DepartmentList = DepartmentBll.GetList();

            string TempDDDeptID = DepartmentList.department.Where(e => e.name.Equals(DepName)).ToList().FirstOrDefault().id;

            string DepartmentJson = GetDepartment(TempDDDeptID);

            DepartmentEntity model = new DepartmentEntity();
            model = Newtonsoft.Json.JsonConvert.DeserializeObject<DepartmentEntity>(DepartmentJson);

            model.order = "";
            model.parentid = "49051121";
            string param = JsonConvert.SerializeObject(model);
            Result result = DepartmentBll.Update(param);
            if (result != null)
            {
                if (result.errcode == "0")
                {
                    Console.Write("数据更新成功\r\n");
                }
                else
                {

                    Console.Write("\n更新部门ESBDepartmentId为:" + TempDDDeptID + "， 错误原因：" + result.errmsg + "\n");
                }
            }
            else
            {
                Console.Write("\r\n无返回数据\r\n");
            }
        }
    }
}
