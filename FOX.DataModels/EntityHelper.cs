using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FOX.DataModels
{
    public static class EntityHelper
    {
        public static bool isTalkRehab;
        static EntityHelper()
        {
            isTalkRehab = false;
        }
        public static string getConnectionStringName()
        {
            if (isTalkRehab == true)
            {
                return "TalkRehabConnection";
            }
            else
            {
                return "FOXConnection";
            }
        }
        public static string getDB5ConnectionStringName()
        {
            return "DB5Connection";
        }
    }
}