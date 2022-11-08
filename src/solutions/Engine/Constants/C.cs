using System;

namespace Engine.Constants {
    public class C {

        public const string GLOBAL_USER = "API_CTL";
        public const string PROCESS_USER = "CTL_DATASET";

        public const string OK = "OK";
        public const string ERROR = "ERROR";
        public const string COMPLETE = "COMPLETE";
        public const string PENDING = "PENDING";
        public const string ROLLABACK = "ROLLBACK";

        public const string URL = "URL";
        public const string RESOURCE = "RESOURCE";
        public const string INFO = "INFO";

        public const string ENABLED = "ENABLED";
        public const string DISABLED = "DISABLED";

        public const string EmployeeCsv = "employees.csv";
        public const string AccessCsv = "access.csv";        
        public const string JobsCsv = "jobs.csv";
        public const string DepartamentsCsv = "department.csv";
        public const string PositionsCsv = "positions.csv";

        #region HUB Methods
        public const string HUB_DEVICE_MONITOR = "DeviceMonitor";
        public const string HUB_DEVICE_INFO = "DeviceInfo";
        #endregion

        /* ACTIONS Device */

        public const string REGISTER_FINGER = "REGISTER_FINGER";
        public const string GET_HINTS = "GET_HINTS";
        public const string SET_HINTS = "SET_HINTS";


        /* Connection Strings */
        public const string ACCESS_DB = "DB_ACCESS";
        public const string HINT_DB = "DB_HINT";        
    }
}