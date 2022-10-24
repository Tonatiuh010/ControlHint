using System;

namespace Engine.Constants {
    public class SQL {
        
        public const string CTL_ACCESS = "CTL_ACCESS";
        public const string CTL_HINT = "CTL_HINT";        

        #region CTL_ACCESS 
        public const string DB_ACC = "CTL_ACCESS";       
        public const string SET_DEPARTMENT = "SET_DEPARTMENT";
        public const string SET_EMPLOYEE = "SET_EMPLOYEE";
        public const string SET_DOWN_EMPLOYEE = "SET_DOWN_EMPLOYEE";
        public const string SET_JOB = "SET_JOB";
        public const string SET_EMPLOYEE_ACCESS = "SET_ACCESS_EMPLOYEE";
        public const string SET_ACCESS_LEVEL = "SET_ACCESS_LEVEL";
        public const string SET_SHIFT = "SET_SHIFT";
        public const string SET_POSITION = "SET_POSITION";

        public const string GET_ACCESS_LEVEL = "SELECT * from access_level";
        public const string GET_EMPLOYEE_DETAIL = "GET_EMPLOYEE_DETAIL";
        public const string GET_EMPLOYEE_ACCESS_LEVEL = "GET_EMPLOYEE_ACCESS_LEVEL";
        public const string GET_DEPARTMENTS = "GET_DEPARTMENTS";
        public const string GET_JOBS = "GET_JOBS";
        public const string GET_SHIFTS = "GET_SHIFTS";        
        public const string GET_POSITIONS = "GET_POSITIONS";
        public const string GET_CHECKS = "GET_CHECKS";        
        public const string GET_CHECK_DETAILS = "GET_CHECK_DETAILS";
        #endregion

        #region CTL_HINT

        #endregion

    }
}