using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FOX.DataModels
{
    public static class EntityHelper
    {
        public static string getConnectionStringName()
        {

            return "FOXConnection";
        }
        public static string getDB5ConnectionStringName()
        {
            return "DB5Connection";
        }
    }
}