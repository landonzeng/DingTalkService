using NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Utility
{
    public class NLogFactory
    {
        public static NLogHelper GetLogger(Type type)
        {
            return new NLogHelper(LogManager.GetCurrentClassLogger(type));
        }

        public static NLogHelper GetLogger(string str)
        {
            return new NLogHelper(LogManager.GetLogger(str));
        }
    }
}
