using System;

namespace Engine.Constants {
    public class SQL {

        public const string DB_CLASS = "CLASSROOM";


        #region Procedures
        public const string GET_STUDENTS = "GET_STUDENTS";
        public const string GET_ADDRESSES = "GET_ADDRESSES";
        public const string GET_ASSETS = "GET_ASSETS";
        public const string GET_CONTACTS = "GET_CONTACTS";
        public const string GET_FAMILY_CONTACTS = "GET_FAMILY_CONTACTS";
        public const string GET_GROUPS = "GET_GROUPS";
        public const string GET_GROUP_STUDENTS = "GET_GROUP_STUDENTS";
        public const string GET_LABOR_STUDENTS = "GET_LABOR_STUDENTS";
        public const string GET_LOCATION = "GET_LOCATION";
        public const string GET_MAJOR = "GET_MAJOR";
        public const string GET_SCHOLARLY = "GET_SCHOLARLY";
        public const string GET_STUDENT_HISTORIAL = "GET_STUDENT_HISTORIAL";
        public const string GET_ASSET_QUERY = "SELECT DISTINCT ATTR1 FROM ASSET";
        public const string GET_ASSET_ID = "SELECT GET_ASSET('{{CODE}}')";
        #endregion


    }
}