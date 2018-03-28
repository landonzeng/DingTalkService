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
        public static List<V_OperationObject> GetOperationList(SqlSugarClient Edb, int type, int Operation)
        {
            List<V_OperationObject> OperationList = Edb.Ado.UseStoredProcedure<dynamic>(() =>
            {
                string spName = "SP_OperationObject";
                var p1 = new SugarParameter("@Operation", Operation);
                var p2 = new SugarParameter("@type", type);
                return Edb.Ado.SqlQuery<V_OperationObject>(spName, new SugarParameter[] { p1, p2 });
            });
            return OperationList;
        }
    }
}
