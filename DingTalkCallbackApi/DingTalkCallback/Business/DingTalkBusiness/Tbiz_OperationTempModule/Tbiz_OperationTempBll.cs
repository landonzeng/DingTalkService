using Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class Tbiz_OperationTempBll
    {
        /// <summary>
        /// 根据Type和Operation的值分别取出新增或者变更过的部门或用户信息
        /// </summary>
        /// <param name="Edb"></param>
        /// <param name="type"></param>
        /// <param name="Operation"></param>
        /// <returns></returns>
        public static List<V_OperationObject> GetOperationList(SqlSugarClient Edb, int type, int Operation)
        {
            List<V_OperationObject> OperationList = Edb.Ado.UseStoredProcedure<dynamic>(() =>
            {
                //该存储过程是用于取出变更过的部门或用户信息
                string spName = "SP_OperationObject";
                var p1 = new SugarParameter("@Operation", Operation);
                var p2 = new SugarParameter("@type", type);
                return Edb.Ado.SqlQuery<V_OperationObject>(spName, new SugarParameter[] { p1, p2 });
            });
            return OperationList;
        }
    }
}
