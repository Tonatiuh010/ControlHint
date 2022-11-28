using System;

namespace Engine.Constants {
    public class SQL {
        
        public const string CTL_ACCESS = "CTL_ACCESS";
        public const string CTL_HINT = "CTL_HINT";
        public const string CTL_DOCS = "CTL_DOCS";

        #region CTL_ACCESS            
        public const string SET_DEPARTMENT = "SET_DEPARTMENT";
        public const string SET_EMPLOYEE = "SET_EMPLOYEE";
        public const string SET_DOWN_EMPLOYEE = "SET_DOWN_EMPLOYEE";
        public const string SET_JOB = "SET_JOB";
        public const string SET_EMPLOYEE_ACCESS = "SET_ACCESS_EMPLOYEE";
        public const string SET_ACCESS_LEVEL = "SET_ACCESS_LEVEL";
        public const string SET_SHIFT = "SET_SHIFT";
        public const string SET_POSITION = "SET_POSITION";
        public const string SET_EMPLOYEE_HINT = "SET_EMPLOYEE_HINT";
        public const string SET_CONTROL_CHECK = "SET_CONTROL_CHECK";
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
        public const string GET_FLOW = "GET_FLOW";
        public const string GET_FLOW_PARAMETERS = "GET_FLOW_PARAMETERS";
        public const string GET_TRANSACTIONS = "GET_TRANSACTIONS";
        public const string GET_DEV_CONNECTION = "GET_DEV_CONNECTION";
        public const string SET_API = "SET_API";
        public const string GET_USR_ACCESS = "GET_USR_ACCESS";
        public const string SET_HINT_CONFIG = "SET_HINT_CONFIG";
        public const string SET_URL_ENDPOINT = "SET_URL_ENDPOINT";
        public const string SET_ENDPOINT_PARAMETER = "SET_ENDPOINT_PARAMETER";
        public const string SET_FLOW_DETAIL = "SET_FLOW_DETAIL";
        public const string SET_DEV_CONNECTION = "SET_DEV_CONNECTION";
        public const string SET_DEV_FLOW = "SET_DEV_FLOW";
        public const string SET_USR_ACCESS = "SET_USR_ACCESS";
        #endregion

        #region CTL_DOCS
        public const string SET_APPROVER = "SET_APPROVER";
        public const string SET_DOCS_APPROVER = "SET_DOCS_APPROVER";
        public const string SET_DOCS_FILE = "SET_DOCS_FILE";
        public const string SET_DOCS_FLOW = "SET_DOCS_FLOW";
        public const string SET_DOCS_TYPE = "SET_DOCS_TYPE";
        public const string SET_DOCUMENT = "SET_DOCUMENT";

        public const string GET_DOCUMENTS = "GET_DOCUMENTS";
        public const string GET_FLOWS = "GET_FLOWS";
        public const string GET_FLOWS_APPROVERS = "GET_FLOWS_APPROVERS";
        public const string GET_APPROVERS = "GET_APPROVERS";
        public const string GET_DOC_TXN = "GET_DOC_TXN";
        public const string SET_DOCUMENT_TXN = "SET_DOCUMENT_TXN";
        public const string SET_APPROVER_TXN = "SET_APPROVER_TXN";
        #endregion

    }
}