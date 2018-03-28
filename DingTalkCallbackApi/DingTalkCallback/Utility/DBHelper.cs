using SqlSugar;
using Model;
using Microsoft.Extensions.Options;

namespace Utility
{
    public class DBHelper
    {
        public static SqlSugarClient Ddb
        {
            get
            {
                return new SqlSugarClient(new ConnectionConfig()
                {
                    ConnectionString = JsonConfigurationHelper.GetAppSettings("ConnectionStrings", "DingTalkConnectionstr"),
                    DbType = DbType.SqlServer,
                    IsAutoCloseConnection = true
                }
                );
            }
        }

        public static SqlSugarClient Edb
        {
            get
            {
                return new SqlSugarClient(new ConnectionConfig()
                {
                    ConnectionString = JsonConfigurationHelper.GetAppSettings("ConnectionStrings", "ESBConnectionstr"),
                    DbType = DbType.SqlServer,
                    IsAutoCloseConnection = true
                }
                );
            }
        }        
    }
}
