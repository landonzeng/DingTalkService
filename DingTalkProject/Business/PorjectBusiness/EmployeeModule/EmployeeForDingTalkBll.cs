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
    public class EmployeeForDingTalkBll
    {
        public static void EmployeeInsertForDingTalk(SqlSugarClient Edb, SqlSugarClient Ddb, List<V_EmployeeToDingTalk> ESB_EmployeeList)
        {
            LogHelper log = LogFactory.GetLogger("EmployeeInsertForDingTalk");
            List<V_OperationObject> OperationList = new List<V_OperationObject>();
            List<V_EmployeeToDingTalk> EmpList = new List<V_EmployeeToDingTalk>();

            OperationList = Tbiz_OperationTempBll.GetOperationList(Edb, 1, 1);
            List<V_EmployeeToDingTalk> EmployeeList = ESB_EmployeeList.Where(p => OperationList.Exists(q => q.UserId == p.UserId)).ToList();

            log.Info("\r\n------------------------------------------------ESB人员数据导入到钉钉------------------------------------------------\r\n");

            #region 循环插入到钉钉
            foreach (var item in EmployeeList)
            {

                if (item.Enabled == 1)
                {
                    InsertForDingTalk(Edb, Ddb, EmpList, item);
                }
                else
                {
                    DeleteForDingTalk(Ddb, EmpList, item);
                }
            }

            int d = 0;
            foreach (var item in EmpList)
            {
                d = Edb.Deleteable<Tbiz_OperationTemp>().Where(it => it.ObjectId.Equals(item.UserId)).ExecuteCommand();
            }
            #endregion
        }

        public static void EmployeeUpdateForDingTalk(SqlSugarClient Edb, SqlSugarClient Ddb, List<V_EmployeeToDingTalk> ESB_EmployeeList)
       {
            LogHelper log = LogFactory.GetLogger("EmployeeUpdateForDingTalk");
            List<V_EmployeeToDingTalk> EmpList = new List<V_EmployeeToDingTalk>();
            //获取ESB中目前已经更新的数据
            List<V_OperationObject> OperationList = Tbiz_OperationTempBll.GetOperationList(Edb, 1, 0);
            //根据ESB操作记录中更新的ID，查出目前的用户信息
            List<V_EmployeeToDingTalk> EmployeeList = ESB_EmployeeList.Where(p => OperationList.Exists(q => q.UserId == p.UserId)).ToList();

            log.Info("\r\n------------------------------------------------根据ESB更新钉钉中的人员信息------------------------------------------------\r\n");

            #region 循环更新到钉钉
            foreach (var item in EmployeeList)
            {

                if (item.Enabled == 1)
                {
                    string EmployeeJson = GetEmployee(item.UserId);
                    //用户ID在钉钉中不存在，即：离职用户重新录用时，应将该用户重新添加进钉钉
                    if (EmployeeJson.Equals("-1"))
                    {
                        AddEmployee(Edb, Ddb, item);
                        EmpList.Add(item);
                        continue;
                    }
                    else
                    {
                        EmployeeEntity model = Newtonsoft.Json.JsonConvert.DeserializeObject<EmployeeEntity>(EmployeeJson);
                        string oldMobile = model.mobile;
                        string DD_DepartmentId = "1";
                        DepartmentResult DD_DepModel = Ddb.Queryable<DepartmentResult>().With(SqlWith.NoLock).Where(it => it.ESB_DepartmentID.Equals(item.ESB_DepartmentId)).First();
                        if (DD_DepModel == null)
                        {
                            try
                            {
                                DD_DepartmentId = DepartmentForDingTalkBll.DD_DepartmentIsNullForDingTalk(Edb, Ddb, item.ESB_DepartmentId);
                            }
                            catch (Exception ex)
                            {
                                DD_DepartmentId = DepartmentForDingTalkBll.DD_DepartmentIsNullForDingTalk(Edb, Ddb, item.ESB_DepartmentId);
                            }
                        }
                        else
                        {
                            DD_DepartmentId = DD_DepModel.id;
                        }
                        model.userid = item.UserId;
                        model.name = item.Name;
                        model.department = new List<int>(new int[] { Convert.ToInt32(DD_DepartmentId) });
                        model.position = item.PositionName;
                        model.mobile = item.Mobile;
                        model.tel = item.Telephone;
                        model.email = item.Email;
                        model.jobnumber = item.UserId;
                        string param = model.ToJson();

                        Result Result = EmployeeBll.Update(param);
                        if (Result != null)
                        {
                            if (Result.errcode == "0")
                            {
                                EmpList.Add(item);
                                //Console.Write("更新成功," + Result.errmsg);
                            }
                            else
                            {
                                //UserID不存在
                                if (Result.errcode == "60111")
                                {
                                    AddEmployee(Edb, Ddb, item);
                                    EmpList.Add(item);
                                    continue;
                                }
                                //更新手机号出错时
                                else if (Result.errcode == "40022" || Result.errcode == "40021" || Result.errcode == "60104" || Result.errcode == "60121")
                                {
                                    //40021	更换的号码已注册过钉钉，无法使用该号码
                                    //40022 企业中的手机号码和登陆钉钉的手机号码不一致,暂时不支持修改用户信息,可以删除后重新添加
                                    //60104	手机号码在公司中已存在
                                    //60121	找不到该用户

                                    string Deletecode = EmployeeBll.Delete(model.userid).errcode;

                                    string Createcode = EmployeeBll.Create(model.ToJson()).errcode;

                                    if (Createcode != "0")
                                    {
                                        Createcode = EmployeeBll.Create(model.ToJson()).errcode;
                                        if (Createcode != "0" && Createcode != "40021")
                                        {
                                            log.Error("\r\n EmployeeUpdateForDingTalk - 行号135 更新钉钉人员信息时，成功删除员工信息，但是创建员工信息时报错，错误编码如下：" + Createcode + ", 员工编号为：" + model.userid);
                                        }
                                    }

                                    EmpList.Add(item);

                                    Task.Factory.StartNew(() =>
                                    {
                                        InsertErroUpdateEmployee(Ddb, model.userid, oldMobile, item.Mobile, Result.errcode);
                                        if (Deletecode != "0")
                                        {
                                            InsertErroUpdateEmployee(Ddb, model.userid, oldMobile, item.Mobile, "删除失败，错误编号：" + Deletecode);
                                        }
                                        if (Createcode != "0")
                                        {
                                            InsertErroUpdateEmployee(Ddb, model.userid, oldMobile, item.Mobile, "执行删除后创建失败，错误编号：" + Createcode);
                                        }
                                    });

                                    //Console.Write("更新成功\r\n");
                                }
                                else
                                {
                                    //手机号码不合法
                                    if (Result.errcode == "60103")
                                    {
                                        model.mobile = oldMobile;
                                        Result = EmployeeBll.Update(model.ToJson());
                                        if (Result.errcode == "0")
                                        {
                                            EmpList.Add(item);
                                        }
                                        else
                                        {
                                            log.Error("\r\n EmployeeForDingTalkBll-EmployeeUpdateForDingTalk() 失败后不更新手机号，还是失败，具体信息： " + Result.errmsg + "; UserId=" + item.UserId);
                                        }
                                    }
                                    //部门在钉钉中不存在的时候
                                    else if (Result.errcode == "60003")
                                    {
                                        model.department = new List<int>(new int[] { 1 });

                                        param = model.ToJson();

                                        Result r = EmployeeBll.Update(param);
                                        if (r.errcode == "0")
                                        {
                                            EmpList.Add(item);
                                        }
                                        else if (r.errcode == "40022" || r.errcode == "40021" || r.errcode == "60104" || r.errcode == "60121")
                                        {
                                            //40021	更换的号码已注册过钉钉，无法使用该号码
                                            //40022 企业中的手机号码和登陆钉钉的手机号码不一致,暂时不支持修改用户信息,可以删除后重新添加
                                            //60104	手机号码在公司中已存在
                                            //60121	找不到该用户

                                            string Deletecode = EmployeeBll.Delete(model.userid).errcode;

                                            string Createcode = EmployeeBll.Create(model.ToJson()).errcode;

                                            if (Createcode != "0")
                                            {
                                                Createcode = EmployeeBll.Create(model.ToJson()).errcode;
                                                if (Createcode != "0" && Createcode != "40021")
                                                {
                                                    log.Error("\r\n EmployeeUpdateForDingTalk - 行号199 更新钉钉人员信息时，成功删除员工信息，但是创建员工信息时报错，错误编码如下：" + Createcode);
                                                }
                                            }

                                            EmpList.Add(item);

                                            Task.Factory.StartNew(() =>
                                            {
                                                InsertErroUpdateEmployee(Ddb, model.userid, oldMobile, item.Mobile, r.errcode);
                                                if (Deletecode != "0")
                                                {
                                                    InsertErroUpdateEmployee(Ddb, model.userid, oldMobile, item.Mobile, "删除失败，错误编号：" + Deletecode);
                                                }
                                                if (Createcode != "0")
                                                {
                                                    InsertErroUpdateEmployee(Ddb, model.userid, oldMobile, item.Mobile, "执行删除后创建失败，错误编号：" + Createcode);
                                                }
                                            });

                                            //Console.Write("更新成功\r\n");
                                        }
                                        else
                                        {
                                            log.Debug("\r\n EmployeeForDingTalkBll-EmployeeUpdateForDingTalk() 钉钉中部，部门id=" + DD_DepartmentId + ",已将该人员挂在公司下，" + Result.errmsg + "; UserId=" + item.UserId);

                                            Task.Factory.StartNew(() =>
                                            {
                                                InsertErroUpdateEmployee(Ddb, model.userid, oldMobile, item.Mobile, "更新用户时失败，错误编号：" + Result.errcode);
                                            });
                                        }
                                    }
                                    else
                                    {
                                        log.Error("\r\n EmployeeForDingTalkBll-EmployeeUpdateForDingTalk() " + Result.errmsg + "; UserId=" + item.UserId);

                                        Task.Factory.StartNew(() =>
                                        {
                                            InsertErroUpdateEmployee(Ddb, model.userid, oldMobile, item.Mobile, "更新用户时失败，错误编号：" + Result.errcode);
                                        });
                                    }
                                }
                            }
                        }
                        else
                        {
                            //Console.Write("无返回数据");
                        }
                    }
                }
                else
                {
                    Result Result = EmployeeBll.Delete(item.UserId);
                    if (Result != null)
                    {
                        //找不到该用户
                        if (Result.errcode == "0" || Result.errcode == "60121")
                        {
                            EmpList.Add(item);
                            //Console.Write("删除成功," + Result.errmsg + "\r\n");
                        }
                        else
                        {
                            log.Error("\r\n EmployeeForDingTalkBll-EmployeeUpdateForDingTalk() " + Result.errmsg + "; UserId=" + item.UserId);
                            //Console.Write("\r\n" + Result.errmsg + "; UserId=" + item.UserId);
                        }
                    }
                    else
                    {
                        //Console.Write("无返回数据");
                    }
                }
            }
            int d = 0;
            foreach (var item in EmpList)
            {
                d = Edb.Deleteable<Tbiz_OperationTemp>().Where(it => it.ObjectId.Equals(item.UserId)).ExecuteCommand();
            }
            #endregion
        }

        public static void InsertForDingTalk(SqlSugarClient Edb, SqlSugarClient Ddb, List<V_EmployeeToDingTalk> EmpList, V_EmployeeToDingTalk item)
        {
            LogHelper log = LogFactory.GetLogger("InsertEmployeeForDingTalk");
            string DD_DepartmentId = "1";
            if (!item.ESB_DepartmentId.Equals("1000000001"))
            {
                DepartmentResult DD_DepModel = Ddb.Queryable<DepartmentResult>().With(SqlWith.NoLock).Where(it => it.ESB_DepartmentID.Equals(item.ESB_DepartmentId)).First();
                if (DD_DepModel == null)
                {
                    DD_DepartmentId = DepartmentForDingTalkBll.DD_DepartmentIsNullForDingTalk(Edb, Ddb, item.ESB_DepartmentId);
                }
                else
                {
                    DD_DepartmentId = DD_DepModel.id;
                }
                try
                {
                    DD_DepartmentId = Ddb.Queryable<DepartmentResult>().With(SqlWith.NoLock).Where(it => it.ESB_DepartmentID.Equals(item.ESB_DepartmentId)).First().id;
                }
                catch (Exception ex)
                {
                    Console.Write("根据ESB_DepartmentId获取对应的钉钉id时报错,该ID为" + item.ESB_DepartmentId + "，错误信息为：" + ex + "\r\n");
                }
            }
            EmployeeEntity model = new EmployeeEntity();
            model.userid = item.UserId;
            model.name = item.Name;
            model.department = new List<int>(new int[] { Convert.ToInt32(DD_DepartmentId) });
            model.position = item.PositionName;
            model.mobile = item.Mobile;
            model.tel = item.Telephone;
            model.workPlace = "";
            model.remark = "";
            model.email = item.Email;
            model.jobnumber = item.UserId;
            model.isSenior = false;

            string param = model.ToJson();

            EmployeeResult Result = EmployeeBll.Create(param);
            if (Result != null)
            {
                if (Result.errcode == "0")
                {
                    EmpList.Add(item);
                    //Console.Write("创建成功,UserId=" + Result.userid + "\r\n");
                }
                else
                {
                    //UserID在公司中已存在
                    if (Result.errcode == "60102")
                    {
                        string EmployeeJson = GetEmployee(item.UserId);
                        model = Newtonsoft.Json.JsonConvert.DeserializeObject<EmployeeEntity>(EmployeeJson);

                        model.userid = item.UserId;
                        model.name = item.Name;
                        model.department = new List<int>(new int[] { Convert.ToInt32(DD_DepartmentId) });
                        model.position = item.PositionName;
                        model.mobile = item.Mobile;
                        model.tel = item.Telephone;
                        model.email = item.Email;
                        model.jobnumber = item.UserId;
                        param = model.ToJson();

                        Result res = EmployeeBll.Update(param);
                        if (res != null)
                        {
                            if (res.errcode == "0")
                            {
                                EmpList.Add(item);
                                //Console.Write("更新成功," + Result.errmsg);
                            }
                            else
                            {
                                log.Error("\r\n EmployeeForDingTalkBll-InsertForDingTalk() " + Result.errmsg + "; UserId=" + item.UserId);
                                //Console.Write("\r\n" + Result.errmsg + "; UserId=" + item.UserId);
                            }
                        }
                        else
                        {
                            //Console.Write("无返回数据");
                        }
                    }
                    else
                    {
                        if (Result.errcode == "60104")
                        {
                            log.Error("\r\n EmployeeForDingTalkBll-InsertForDingTalk() " + Result.errmsg + "; UserId=" + item.UserId);
                        }
                        else if (Result.errcode == "40026")
                        {
                            //该外部联系人已存在
                            AddEmployee(Edb, Ddb, item);
                            EmpList.Add(item);
                        }
                        else
                        {
                            log.Error("\r\n EmployeeForDingTalkBll-InsertForDingTalk() " + Result.errmsg + "; UserId=" + item.UserId);
                        }
                        //Console.Write("\r\n" + Result.errmsg + "; UserId=" + item.UserId);
                    }
                }
            }
            else
            {
                //Console.Write("无返回数据");
            }
        }

        public static void DeleteForDingTalk(SqlSugarClient Ddb, List<V_EmployeeToDingTalk> EmpList, V_EmployeeToDingTalk item)
        {
            LogHelper log = LogFactory.GetLogger("InsertEmployeeForDingTalk");

            Result Result = EmployeeBll.Delete(item.UserId);
            if (Result != null)
            {
                if (Result.errcode == "0")
                {
                    EmpList.Add(item);
                    //Console.Write("删除成功," + Result.errmsg + "\r\n");
                }
                else
                {
                    log.Error("\r\n EmployeeForDingTalkBll-DeleteForDingTalk() " + Result.errmsg + "; UserId=" + item.UserId);
                    //Console.Write("\r\n" + Result.errmsg + "; UserId=" + item.UserId);
                }
            }
            else
            {
                //Console.Write("无返回数据");
            }
        }

        public static string GetEmployee(string userid)
        {
            LogHelper log = LogFactory.GetLogger("GetEmployee");
            string json = "";
            try
            {
                GetEmployee Result = EmployeeBll.Get(userid);

                if (Result != null)
                {
                    if (Result.errcode == "0")
                    {
                        json = Result.ToJson();
                        //Console.Write(json + "\r\n");
                    }
                    else if (Result.errcode == "90002")
                    {
                        System.Threading.Thread.Sleep(1500);
                        json = GetEmployee(userid);
                    }
                    else
                    {
                        if (Result.errcode != "60121")
                        {
                            //Console.Write(Result.errmsg);
                            log.Error("\r\n EmployeeForDingTalkBll-GetEmployee() " + Result.errmsg + "\r\n");
                        }
                        return "-1";
                    }
                }
                else
                {
                    //Console.Write("无返回数据");
                }
            }
            catch (Exception ex)
            {
                log.Error("\r\n EmployeeForDingTalkBll-GetEmployee() " + ex + "\r\n");
                //Console.Write(ex.Message);
            }
            return json;
        }

        public static void AddEmployee(SqlSugarClient Edb, SqlSugarClient Ddb, V_EmployeeToDingTalk item)
        {
            LogHelper log = LogFactory.GetLogger("AddEmployee");
            try
            {
                EmployeeEntity model = new EmployeeEntity();
                string DD_DepartmentId = "1";
                if (!item.ESB_DepartmentId.Equals("1000000001"))
                {
                    try
                    {
                        DepartmentResult DD_DepModel = Ddb.Queryable<DepartmentResult>().With(SqlWith.NoLock).Where(it => it.ESB_DepartmentID.Equals(item.ESB_DepartmentId)).First();
                        if (DD_DepModel == null)
                        {
                            DD_DepartmentId = DepartmentForDingTalkBll.DD_DepartmentIsNullForDingTalk(Edb, Ddb, item.ESB_DepartmentId);
                        }
                        else
                        {
                            DD_DepartmentId = DD_DepModel.id;
                        }
                    }
                    catch
                    {
                        DD_DepartmentId = "1";
                    }
                }

                model.userid = item.UserId;
                model.name = item.Name;
                model.department = new List<int>(new int[] { Convert.ToInt32(DD_DepartmentId) });
                model.position = item.PositionName;
                model.mobile = item.Mobile;
                model.tel = item.Telephone;
                model.workPlace = "";
                model.remark = "";
                model.email = item.Email;
                model.jobnumber = item.UserId;
                model.isSenior = false;

                string param = model.ToJson();

                EmployeeResult Result = EmployeeBll.Create(param);
                if (Result != null)
                {
                    if (Result.errcode == "0")
                    {
                        //Console.Write("创建成功,UserId=" + Result.userid);
                    }
                    //该外部联系人已存在 ||	手机号码在公司中已存在
                    else if (Result.errcode == "40026" || Result.errcode == "60104")
                    {
                        string res = EmployeeBll.Delete(model.userid).errcode;
                        if (res != "0")
                        {
                            log.Error("\r\n EmployeeForDingTalkBll-AddEmployee() 手机号码在公司中已存在删除时报错，错误编号：" + res);
                        }
                        EmployeeResult Result2 = EmployeeBll.Create(model.ToJson());
                        if (Result2.errcode != "0")
                        {
                            if (Result2.errcode == "40022")
                            {
                                string Deletecode = EmployeeBll.Delete(model.userid).errcode;

                                string Createcode = EmployeeBll.Create(model.ToJson()).errcode;

                                if (Createcode != "0")
                                {
                                    Createcode = EmployeeBll.Create(model.ToJson()).errcode;
                                    if (Createcode != "0" && Createcode != "40021")
                                    {
                                        log.Error("\r\n AddEmployee - 行号507 成功删除员工信息，但是创建员工信息时报错，错误编码如下：" + Createcode);
                                    }
                                }
                                //手机号码在公司中已存在
                                if (Result2.errcode == "60104")
                                {
                                    log.Debug("\r\n EmployeeForDingTalkBll-AddEmployee() 手机号码在公司中已存在 时报错，信息如下" + Result2.errmsg + "，错误编码为：" + Result2.errcode + "， Json参数为：" + param);
                                }
                                else
                                {
                                    log.Error("\r\n EmployeeForDingTalkBll-AddEmployee() 该外部联系人已存在 ||	手机号码在公司中已存在 时报错，信息如下" + Result2.errmsg + "，错误编码为：" + Result2.errcode + "， Json参数为：" + param);
                                }

                                Task.Factory.StartNew(() =>
                                {
                                    InsertErroUpdateEmployee(Ddb, model.userid, "", item.Mobile, Result.errcode);
                                    if (Deletecode != "0")
                                    {
                                        InsertErroUpdateEmployee(Ddb, model.userid, "", item.Mobile, "新增用户时,删除失败，错误编号：" + Deletecode);
                                    }
                                    if (Createcode != "0")
                                    {
                                        InsertErroUpdateEmployee(Ddb, model.userid, "", item.Mobile, "新增用户时,执行删除后创建失败，错误编号：" + Createcode);
                                    }
                                });
                            }
                        }
                    }
                    else
                    {
                        if (Result.errcode == "60103")
                        {
                            log.Debug("\r\n EmployeeForDingTalkBll-AddEmployee() " + Result.errmsg + "，错误编码为：" + Result.errcode + "，手机号为" + item.Mobile + "，用户id为" + item.UserId + " Json参数为：" + param);
                        }
                        else
                        {
                            log.Error("\r\n EmployeeForDingTalkBll-AddEmployee() " + Result.errmsg + "，错误编码为：" + Result.errcode);
                        }

                        Task.Factory.StartNew(() =>
                        {
                            InsertErroUpdateEmployee(Ddb, model.userid, "", item.Mobile, "新增用户时创建失败，错误编号：" + Result.errcode);
                        });
                    }
                }
                else
                {
                    //Console.Write("无返回数据");
                }
            }
            catch (Exception ex)
            {
                log.Error("\r\n EmployeeForDingTalkBll-AddEmployee() " + ex + "\r\n");
                //Console.Write(ex.Message);
            }
        }

        public static void DeleteEmployee(string UserId)
        {
            LogHelper log = LogFactory.GetLogger("DeleteEmployee");
            try
            {
                Result Result = EmployeeBll.Delete(UserId);
                if (Result != null)
                {
                    if (Result.errcode == "0")
                    {
                        //Console.Write("\r\n更新成功," + Result.errmsg + "\r\n");
                    }
                    else
                    {
                        log.Error("\r\n EmployeeForDingTalkBll-DeleteEmployee() " + Result.errmsg);
                        //Console.Write("\r\n" + Result.errmsg + "\r\n");
                    }
                }
                else
                {
                    //Console.Write("\r\n无返回数据\r\n");
                }
            }
            catch (Exception ex)
            {
                log.Error("\r\n EmployeeForDingTalkBll-DeleteEmployee() " + ex);
                //Console.Write("\r\n" + ex.Message + "\r\n");
            }
        }

        public static void BatchDeleteEmployee(List<string> list)
        {
            LogHelper log = LogFactory.GetLogger("BatchDeleteEmployee");
            try
            {
                BatchDeleteEmployee model = new Model.BatchDeleteEmployee();

                model.useridlist = list;

                string param = model.ToJson();

                Result Result = EmployeeBll.BatchDelete(param);
                if (Result != null)
                {
                    if (Result.errcode == "0")
                    {
                        //Console.Write("删除成功," + Result.errmsg);
                    }
                    else
                    {
                        log.Error("\r\n EmployeeForDingTalkBll-BatchDeleteEmployee() " + Result.errmsg);
                        //Console.Write(Result.errmsg);
                    }
                }
                else
                {
                    //Console.Write("无返回数据");
                }
            }
            catch (Exception ex)
            {
                log.Error("\r\n EmployeeForDingTalkBll-BatchDeleteEmployee() " + ex);
                //Console.Write(ex.Message);
            }
        }

        public static void UpdateEmployeePhoneByUserId(string userid, string MobilePhone)
        {
            SqlSugarClient Edb = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = Config.ESBConnectionString,
                DbType = DbType.SqlServer,
                IsAutoCloseConnection = true
            });

            SqlSugarClient Ddb = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = Config.DingTalkConnectionString,
                DbType = DbType.SqlServer,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute
            });
            Ddb.CodeFirst.InitTables(typeof(DepartmentResult));

            LogHelper log = LogFactory.GetLogger("UpdateEmployeePhoneByUserId");

            try
            {
                GetEmployee emp = EmployeeBll.Get(userid);
                EmployeeEntity model = JsonHelper.JsonToModel<EmployeeEntity>(emp.ToJson());
                string oldMobile = model.mobile;

                model.mobile = MobilePhone;

                string param = model.ToJson();

                Result Result = EmployeeBll.Update(param);
                if (Result != null)
                {
                    if (Result.errcode == "0")
                    {
                        //Console.Write("更新成功\r\n");
                    }
                    else if (Result.errcode == "40022" || Result.errcode == "40021")
                    {
                        //40021	更换的号码已注册过钉钉，无法使用该号码
                        //40022 企业中的手机号码和登陆钉钉的手机号码不一致,暂时不支持修改用户信息,可以删除后重新添加

                        string Deletecode = EmployeeBll.Delete(model.userid).errcode;

                        string Createcode = EmployeeBll.Create(model.ToJson()).errcode;

                        if (Createcode != "0")
                        {
                            Createcode = EmployeeBll.Create(model.ToJson()).errcode;
                            if (Createcode != "0" && Createcode != "40021")
                            {
                                log.Error("\r\n UpdateEmployeePhoneByUserId - 行号666 更新钉钉人员信息时，成功删除员工信息，但是创建员工信息时报错，错误编码如下：" + Createcode);
                            }
                        }

                        if (Deletecode != "0")
                        {
                            InsertErroUpdateEmployee(Ddb, model.userid, oldMobile, MobilePhone, "删除失败，错误编号：" + Deletecode);
                        }
                        if (Createcode != "0")
                        {
                            InsertErroUpdateEmployee(Ddb, model.userid, oldMobile, MobilePhone, "执行删除后创建失败，错误编号：" + Createcode);
                        }

                        //Console.Write("更新成功,Userid=" + m.userid + "\r\n");
                    }
                    else
                    {
                        log.Error("\r\n EmployeeForDingTalkBll-UpdateEmployeePhoneByUserId() " + Result.errmsg);

                        Task.Factory.StartNew(() =>
                        {
                            InsertErroUpdateEmployee(Ddb, model.userid, oldMobile, MobilePhone, "更新用户时失败，错误编号：" + Result.errcode);
                        });
                    }
                }
                else
                {
                    //Console.Write("无返回数据");
                }
            }
            catch (Exception ex)
            {
                log.Error("\r\n EmployeeForDingTalkBll-UpdateEmployeePhoneByUserId() " + ex);
                //Console.Write(ex.Message);
            }
        }

        public static void InsertErroUpdateEmployee(SqlSugarClient Ddb, string UserId, string OldMobile, string NewMobile, string ErroCode)
        {
            LogHelper log = LogFactory.GetLogger("InsertErroUpdateEmployee");
            try
            {
                Tbiz_ErroUpdateEmployeeInfo model = new Tbiz_ErroUpdateEmployeeInfo();
                model.UserId = UserId;
                model.OldMobile = OldMobile;
                model.NewMobile = NewMobile;
                model.ErroCode = ErroCode;
                model.CreateDate = DateTime.Now;
                int count = Ddb.Insertable<Tbiz_ErroUpdateEmployeeInfo>(model).ExecuteCommand();
            }
            catch (Exception ex)
            {
                log.Error("异步插入更新用户信息错误记录表时报错，详情：" + ex);
            }
        }
    }
}
