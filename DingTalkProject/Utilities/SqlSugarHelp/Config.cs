using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class Config
    {
        public static string ESBConnectionString = ConfigurationManager.ConnectionStrings["ESBConnectionstr"].ConnectionString;
        public static string DingTalkConnectionString = ConfigurationManager.ConnectionStrings["DingTalkConnectionstr"].ConnectionString;
    }
}
