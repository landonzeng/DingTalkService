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
    public class SynchronousDingTalk
    {
        public static void Synchronous()
        {
            LogHelper log = LogFactory.GetLogger("Synchronous");
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();

                #region SqlSugarClient初始化
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
                #endregion

                #region 查出目前新增的部门，导入到钉钉组织框架
                DepartmentForDingTalkBll.InsertDepartmentForDingTalk(Edb, Ddb);
                #endregion

                #region 查出目前更新的部门，更新钉钉组织框架
                DepartmentForDingTalkBll.UpdateDepartmentForDingTalk(Edb, Ddb);
                #endregion

                //部门List           
                List<V_EmployeeToDingTalk> ESB_EmployeeList = Edb.Queryable<V_EmployeeToDingTalk>().ToList();

                #region 查出目前新增的人员，导入到钉钉组织框架
                EmployeeForDingTalkBll.EmployeeInsertForDingTalk(Edb, Ddb, ESB_EmployeeList);
                #endregion

                #region 查出目前更新的人员，更新钉钉中的人员信息
                EmployeeForDingTalkBll.EmployeeUpdateForDingTalk(Edb, Ddb, ESB_EmployeeList);
                #endregion

                log.Info("\n钉钉接口同步对接成功，共耗时：" + CommonHelper.TimerEnd(watch) + "毫秒\n");

                //Console.Write("\n钉钉接口同步对接成功，共耗时：" + CommonHelper.TimerEnd(watch) + "毫秒\n");

            }
            catch (Exception ex)
            {
                log.Error("\r\n SynchronousDingTalk-Synchronous()" + ex);
                //Console.Write(ex.Message);
            }
        }
    }
}
