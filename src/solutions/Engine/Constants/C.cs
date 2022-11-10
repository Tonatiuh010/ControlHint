using System;
using System.Text.Json;

namespace Engine.Constants {
    public class C {

        public const string GLOBAL_USER = "API_CTL";
        public const string PROCESS_USER = "CTL_DATASET";

        public const string OK = "OK";
        public const string ERROR = "ERROR";
        public const string COMPLETE = "COMPLETE";
        public const string PENDING = "PENDING";
        public const string ROLLABACK = "ROLLBACK";
        public const string NOT_MATCH = "NOT_MATCH";

        public const string URL = "URL";
        public const string RESOURCE = "RESOURCE";
        public const string INFO = "INFO";
        public const string NO_CALLBACK = "NO_CALLBACK";

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
        public const string HUB_DEVICE_SIGNAL = "DeviceSignal";
        #endregion

        /* ACTIONS Device */
        public const string REGISTER_FINGER = "REGISTER_FINGER";
        public const string GET_HINTS = "GET_HINTS";
        public const string DELETE_HINT = "DELETE_HINT";
        public const string DELETE_HINTS = "DELETE_HINTS";


        /* Connection Strings */
        public const string ACCESS_DB = "DB_ACCESS";
        public const string HINT_DB = "DB_HINT";

        /* Json Options */
        public static readonly JsonSerializerOptions CustomJsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }
}