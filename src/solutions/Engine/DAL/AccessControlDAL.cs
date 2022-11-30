using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using DataService.MySQL;
using Engine.Constants;
using Engine.BO;
using Engine.Interfaces;
using Engine.Services;
using Engine.BO.AccessControl;
using Engine.BL.Actuators;

namespace Engine.DAL
{
    public class AccessControlDAL : BaseDAL
    {
        public delegate void DALCallback(AccessControlDAL dal);
        private static ConnectionString? _ConnectionString => ConnectionString.InstanceByName(C.ACCESS_DB);
        public static AccessControlDAL Instance => new(); 
        public AccessControlDAL() : base(_ConnectionString) { }

        public List<Check> GetChecks(int? checkId, int? employeeId) {
            List<Check> model = new();

            TransactionBlock(this, () =>
            {
                using var cmd = CreateCommand(SQL.GET_CHECKS,  CommandType.StoredProcedure);
                IDataParameter pResult = CreateParameterOut("OUT_RESULT", MySqlDbType.String);

                cmd.Parameters.Add(CreateParameter("IN_CHECK", checkId, MySqlDbType.Int32));                
                cmd.Parameters.Add(CreateParameter("IN_EMPLOYEE", employeeId, MySqlDbType.Int32));
                cmd.Parameters.Add(pResult);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    model.Add(new()
                    {
                        Id = Validate.getDefaultIntIfDBNull(reader["CHECK_ID"]),                
                        CheckDt = Validate.getDefaultDateIfDBNull(reader["CHECK_DT"]),
                        CheckType = Validate.getDefaultStringIfDBNull(reader["TYPE"]),
                        Device = new BO.FlowControl.Device() { 
                            Id = Validate.getDefaultIntIfDBNull(reader["DEVICE_ID"]) 
                        },
                        Employee = new Employee()
                        {
                            Id = Validate.getDefaultIntIfDBNull(reader["EMPLOYEE_ID"])
                        }
                    });
                }
                reader.Close();

            }, (ex, msg) => SetExceptionResult("ControlAccessDAL.GetChecks", msg, ex));

            return model;
        }

        public List<Department> GetDepartments(int? deptoId) {
            List<Department> model = new List<Department>();

            TransactionBlock(this, () => {
                using var cmd = CreateCommand(SQL.GET_DEPARTMENTS,  CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_RESULT", MySqlDbType.String);
                cmd.Parameters.Add(CreateParameter("IN_DEPTO", deptoId, MySqlDbType.Int32));
                cmd.Parameters.Add(pResult);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    model.Add(new Department()
                    {
                        Id = Validate.getDefaultIntIfDBNull(reader["Department_ID"]),
                        Code = Validate.getDefaultStringIfDBNull(reader["CODE"]),
                        Name = Validate.getDefaultStringIfDBNull(reader["NAME"])
                    });
                }
                reader.Close();

            }, (ex, msg) => SetExceptionResult("ControlAccessDAL.GetDepartments", msg, ex));

            return model;
        }

        public List<Shift> GetShifts(int? shiftId) 
        {
            List<Shift> model = new();

            TransactionBlock(this, () => {
                using var cmd = CreateCommand(SQL.GET_SHIFTS,  CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_RESULT", MySqlDbType.String);
                cmd.Parameters.Add(CreateParameter("IN_SHIFT", shiftId, MySqlDbType.Int32));
                cmd.Parameters.Add(pResult);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    model.Add(new Shift()
                    {
                        Id = Validate.getDefaultIntIfDBNull(reader["SHIFT_ID"]),
                        Name = Validate.getDefaultStringIfDBNull(reader["NAME"]),
                        InTime = Validate.getDefaultTimeSpanIfDBNull(reader["CLOCK_IN"]),
                        OutTime = Validate.getDefaultTimeSpanIfDBNull(reader["CLOCK_OUT"]),
                        LunchTime = Validate.getDefaultTimeSpanIfDBNull(reader["LUNCH_TIME"]),
                        DayCount = Validate.getDefaultIntIfDBNull(reader["DAY_COUNT"])

                    });
                }
                reader.Close();

            }, (ex, msg) => SetExceptionResult("ControlAccessDAL.GetShifts", msg, ex));

            return model;
        }

        public List<Job> GetJobs(int? jobId)
        {
            List<Job> model = new();

            TransactionBlock(this, () => {
                using var cmd = CreateCommand(SQL.GET_JOBS,  CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_RESULT", MySqlDbType.String);
                cmd.Parameters.Add(CreateParameter("IN_JOB", jobId, MySqlDbType.Int32));
                cmd.Parameters.Add(pResult);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    model.Add(new Job()
                    {
                        Id = Validate.getDefaultIntIfDBNull(reader["JOB_ID"]),
                        Name = Validate.getDefaultStringIfDBNull(reader["NAME"]),
                        Description = Validate.getDefaultStringIfDBNull(reader["DESCRIPTION"])

                    });
                }
                reader.Close();

            }, (ex, msg) => SetExceptionResult("ControlAccessDAL.GetJobs", msg, ex));

            return model;
        }
        

        public List<Position> GetPositions(int? positionId, int? jobId, int? deptoId)
        {
            List<Position> model = new();

            TransactionBlock(this, () => {
                using var cmd = CreateCommand(SQL.GET_POSITIONS,  CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_RESULT", MySqlDbType.String);
                cmd.Parameters.Add(CreateParameter("IN_POSITION", positionId, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_JOB", jobId, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_DEPTO", deptoId, MySqlDbType.Int32));
                cmd.Parameters.Add(pResult);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    model.Add(new Position()
                    {
                        Id = Validate.getDefaultIntIfDBNull(reader["JOB_ID"]),
                        Name = Validate.getDefaultStringIfDBNull(reader["JOB"]),
                        Alias = Validate.getDefaultStringIfDBNull(reader["NAME"]),
                        PositionId = Validate.getDefaultIntIfDBNull(reader["POSITION_ID"]),
                        Description = Validate.getDefaultStringIfDBNull(reader["JOB_DETAIL"]),
                        Department = new Department()
                        {
                            Id = Validate.getDefaultIntIfDBNull(reader["Department_ID"]),
                            Code = Validate.getDefaultStringIfDBNull(reader["DEPTO_CODE"]),
                            Name = Validate.getDefaultStringIfDBNull(reader["Department"])
                        }
                    });
                }
                reader.Close();

            }, (ex, msg) => SetExceptionResult("ControlAccessDAL.GetPositions", msg, ex));

            return model;
        }

        public List<Employee> GetEmployees(int? employeeId) {
            List<Employee> model = new();

            TransactionBlock(this, () => {
                using var cmd = CreateCommand(SQL.GET_EMPLOYEE_DETAIL,  CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_RESULT", MySqlDbType.String);
                cmd.Parameters.Add(CreateParameter("IN_EMPLOYEE", employeeId, MySqlDbType.Int32));
                cmd.Parameters.Add(pResult);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {                                       
                    var position = new Position()
                    {
                        Id = Validate.getDefaultIntIfDBNull(reader["JOB_ID"]),
                        Alias = Validate.getDefaultStringIfDBNull(reader["POSITION_NAME"]),
                        Name = Validate.getDefaultStringIfDBNull(reader["JOB"]),
                        PositionId = Validate.getDefaultIntIfDBNull(reader["POSITION_ID"]),
                        Description = Validate.getDefaultStringIfDBNull(reader["JOB_DETAIL"]),
                        Department = new Department()
                        {
                            Id = Validate.getDefaultIntIfDBNull(reader["Department_ID"]),
                            Code = Validate.getDefaultStringIfDBNull(reader["DEPTO_CODE"]),
                            Name = Validate.getDefaultStringIfDBNull(reader["Department"])
                        }
                    };

                    var shift = new Shift()
                    {
                        Id = Validate.getDefaultIntIfDBNull(reader["SHIFT_ID"]),
                        Name = Validate.getDefaultStringIfDBNull(reader["SHIFT_CODE"]),
                        DayCount = Validate.getDefaultIntIfDBNull(reader["SHIFT_INTERVAL"]),
                        InTime = Validate.getDefaultTimeSpanIfDBNull(reader["IN_SHIFT"]),
                        OutTime = Validate.getDefaultTimeSpanIfDBNull(reader["OUT_SHIFT"]),
                        LunchTime = Validate.getDefaultTimeSpanIfDBNull(reader["LUNCH"])
                    };

                    model.Add(new()
                    {
                        Id = Validate.getDefaultIntIfDBNull(reader["EMPLOYEE_ID"]),
                        Name = Validate.getDefaultStringIfDBNull(reader["FIRST_NAME"]),
                        LastName = Validate.getDefaultStringIfDBNull(reader["LAST_NAME"]),
                        Image = new ImageData(Validate.getDefaultBytesIfDBNull(reader["IMAGE"])),
                        Status = Validate.getDefaultStringIfDBNull(reader["EMPLOYEE_STATUS"]),
                        Job = position.IsValidPosition() ? position : null,
                        Shift = shift.IsValid() ? shift : null,                        
                        AccessLevels = new List<AccessLevel>()
                    });
                }
                reader.Close();

            }, (ex, msg) => SetExceptionResult("ControlAccessDAL.GetEmployees", msg, ex));
            
            return model;
        }

        public List<Check> GetCheckDetails(DateTime from, DateTime to, int? employeeId = null)
        {
            List<Check> model = new ();

            TransactionBlock(this, () => {
                using var cmd = CreateCommand(SQL.GET_CHECK_DETAILS, CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_RESULT", MySqlDbType.String);
                cmd.Parameters.Add(CreateParameter("IN_FROM_DT", from, MySqlDbType.DateTime));
                cmd.Parameters.Add(CreateParameter("IN_TO_DT", to, MySqlDbType.DateTime));
                cmd.Parameters.Add(CreateParameter("IN_EMPLOYEE_ID", employeeId, MySqlDbType.Int32));
                cmd.Parameters.Add(pResult);
                
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    model.Add(new()
                    {
                        Id = Validate.getDefaultIntIfDBNull(reader["CHECK_ID"]),
                        CheckDt = Validate.getDefaultDateIfDBNull(reader["CHECK_DT"]),
                        CheckType = Validate.getDefaultStringIfDBNull(reader["TYPE"]),
                        Device = new BO.FlowControl.Device()
                        {
                            Id = Validate.getDefaultIntIfDBNull(reader["DEVICE_ID"])
                        },
                        Employee = new Employee()
                        {
                            Id = Validate.getDefaultIntIfDBNull(reader["EMPLOYEE_ID"])
                        }
                    });
                }
                reader.Close();

            }, (ex, msg) => SetExceptionResult("ControlAccessDAL.GetEmployeeAccessLevels", msg, ex));

            return model;
        }

        public List<EmployeeAccessLevel> GetEmployeeAccessLevels(int? employeeId) {
            List<EmployeeAccessLevel> model = new List<EmployeeAccessLevel>();

            TransactionBlock(this, () => {
                using var cmd = CreateCommand(SQL.GET_EMPLOYEE_ACCESS_LEVEL,  CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_RESULT", MySqlDbType.String);
                cmd.Parameters.Add(CreateParameter("IN_EMPLOYEE", employeeId, MySqlDbType.Int32));
                cmd.Parameters.Add(pResult);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    model.Add(new()
                    {
                        Id = Validate.getDefaultIntIfDBNull(reader["ACCESS_LEVEL_ID"]),
                        Name = Validate.getDefaultStringIfDBNull(reader["NAME"]),
                        Status = Validate.getDefaultStringIfDBNull(reader["STATUS"]),
                        EmployeeId = Validate.getDefaultIntIfDBNull(reader["EMPLOYEE_ID"])
                    });
                }
                reader.Close();

            }, (ex, msg) => SetExceptionResult("ControlAccessDAL.GetEmployeeAccessLevels", msg, ex));
            
            return model;
        }

        public List<AccessLevel> GetAccessLevels() {
            List<AccessLevel> model = new List<AccessLevel>();

            TransactionBlock(this, () => {
                using var cmd = CreateCommand(SQL.GET_ACCESS_LEVEL,  CommandType.Text);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    model.Add(new()
                    {
                        Id = Validate.getDefaultIntIfDBNull(reader["ACCESS_LEVEL_ID"]),
                        Name = Validate.getDefaultStringIfDBNull(reader["NAME"]),
                        Status = Validate.getDefaultStringIfDBNull(reader["STATUS"])
                    });
                }
                reader.Close();

            }, (ex, msg) => SetExceptionResult("ControlAccessDAL.GetAccessLevel", msg, ex));

            return model;
        }       

        public ResultInsert SetDepartment(Department Department, string txnUser) {
            ResultInsert result = new();
            string sSp = SQL.SET_DEPARTMENT;

            TransactionBlock(this, () => {                
                using var cmd = CreateCommand(sSp,  CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_RESULT", MySqlDbType.String);
                
                cmd.Parameters.Add(CreateParameter("IN_DEPTO", Department.Id, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_NAME", Department.Name, MySqlDbType.String));
                cmd.Parameters.Add(CreateParameter("IN_CODE", Department.Code, MySqlDbType.String));
                cmd.Parameters.Add(CreateParameter("IN_USER", txnUser, MySqlDbType.String));
                cmd.Parameters.Add(pResult);

                NonQueryBlock(cmd, () => GetResult(pResult, sSp, result));

            }, 
                (ex, msg) => SetExceptionResult("ControlAccessDAL.SetDepartment", msg, ex, result),
                () => SetResultInsert(result, Department)
            );

            return result;
        }

        public ResultInsert SetJob(Job job, string txnUser) {
            ResultInsert result = new();
            string sSp = SQL.SET_JOB;

            TransactionBlock(this, () => {
                using var cmd = CreateCommand(sSp,  CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_RESULT", MySqlDbType.String);

                cmd.Parameters.Add(CreateParameter("IN_JOB", job.Id, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_NAME", job.Name, MySqlDbType.String));
                cmd.Parameters.Add(CreateParameter("IN_DESCRIPTION", job.Description, MySqlDbType.String));
                cmd.Parameters.Add(CreateParameter("IN_USER", txnUser, MySqlDbType.String));
                cmd.Parameters.Add(pResult);

                NonQueryBlock(cmd, () => GetResult(pResult, sSp, result));

            }, 
                (ex, msg) => SetExceptionResult("ControlAccessDAL.SetJob", msg, ex, result),
                () => SetResultInsert(result, job)
            );

            return result;
        }

        public ResultInsert SetAccessLevel(AccessLevel level, string txnUser){
            ResultInsert result = new();
            string sSp = SQL.SET_ACCESS_LEVEL;

            TransactionBlock(this, () => {
                using var cmd = CreateCommand(sSp,  CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_RESULT", MySqlDbType.String);

                cmd.Parameters.Add(CreateParameter("IN_ACCESS_ID", level.Id, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_NAME", level.Name, MySqlDbType.String));                
                cmd.Parameters.Add(CreateParameter("IN_USER", txnUser, MySqlDbType.String));
                cmd.Parameters.Add(pResult);

                NonQueryBlock(cmd, () => GetResult(pResult, sSp, result));

            }, 
                (ex, msg) => SetExceptionResult("ControlAccessDAL.SetAccessLevel", msg, ex, result),
                () => SetResultInsert(result, level)
            );

            return result;
        }

        public ResultInsert SetShift(Shift shift, string txnUser) {
            ResultInsert result = new();
            string sSp = SQL.SET_SHIFT;

            TransactionBlock(this, () => {
                using var cmd = CreateCommand(sSp,  CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_RESULT", MySqlDbType.String);
                
                cmd.Parameters.Add(CreateParameter("IN_SHIFT", shift.Id, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_NAME", shift.Name, MySqlDbType.String));
                cmd.Parameters.Add(CreateParameter("IN_CLOCK_IN", shift.InTime, MySqlDbType.Time));
                cmd.Parameters.Add(CreateParameter("IN_CLOCK_OUT", shift.OutTime, MySqlDbType.Time));
                cmd.Parameters.Add(CreateParameter("IN_LUNCH", shift.LunchTime, MySqlDbType.Time));
                cmd.Parameters.Add(CreateParameter("IN_DAY", shift.DayCount, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_USER", txnUser, MySqlDbType.String));
                cmd.Parameters.Add(pResult);

                NonQueryBlock(cmd, () => GetResult(pResult, sSp, result));

            }, 
                (ex, msg) => SetExceptionResult("ControlAccessDAL.SetShift", msg, ex, result),
                () => SetResultInsert(result, shift)
            );

            return result;
        }

        public ResultInsert SetPosition(Position position, string txnUser) {
            ResultInsert result = new();
            string sSp = SQL.SET_POSITION;

            TransactionBlock(this, () => {
                using var cmd = CreateCommand(sSp,  CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_RESULT", MySqlDbType.String);
                
                cmd.Parameters.Add(CreateParameter("IN_POSITION", position.PositionId, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_NAME", position.Alias, MySqlDbType.String));
                cmd.Parameters.Add(CreateParameter("IN_DEPTO", 
                    position.Department?.Id,
                    MySqlDbType.Int32
                ));
                cmd.Parameters.Add(CreateParameter("IN_JOB", position.Id, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_USER", txnUser, MySqlDbType.String));
                cmd.Parameters.Add(pResult);

                NonQueryBlock(cmd, () => GetResult(pResult, sSp, result));

            }, 
                (ex, msg) => SetExceptionResult("ControlAccessDAL.SetPosition", msg, ex, result),
                () =>
                {
                    var positionId = position.PositionId;
                    SetResultInsert(result, new BaseBO() { Id = positionId != null ? (int)positionId : 0  });
                }
            );

            return result;
        }

        public ResultInsert SetEmployee(Employee employee, string txnUser) {
            ResultInsert result = new();
            string sSp = SQL.SET_EMPLOYEE;

            TransactionBlock(this, () => {
                using var cmd = CreateCommand(sSp,  CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_RESULT", MySqlDbType.String);
                cmd.Parameters.Add(CreateParameter("IN_EMPLOYEE", employee.Id, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_NAME", employee.Name, MySqlDbType.String));
                cmd.Parameters.Add(CreateParameter("IN_LAST_NAME", employee.LastName, MySqlDbType.String));
                cmd.Parameters.Add(CreateParameter("IN_POSITION", employee.Job?.PositionId, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_SHIFT", employee.Shift?.Id, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_IMG", employee.Image?.Hex, MySqlDbType.LongText));
                cmd.Parameters.Add(CreateParameter("IN_USER", txnUser, MySqlDbType.String));
                cmd.Parameters.Add(pResult);

                NonQueryBlock(cmd, () => GetResult(pResult, sSp, result));

            }, 
                (ex, msg) => SetExceptionResult("ControlAccessDAL.SetEmployee", msg, ex, result), 
                () => SetResultInsert(result, employee)
            );
            
            return result;
        }

        public Result SetDownEmployee(int employeeId, string txnUser) {
            Result result = new();
            string sSp = SQL.SET_DOWN_EMPLOYEE;

            TransactionBlock(this, () => {                
                using var cmd = CreateCommand(sSp,  CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_RESULT", MySqlDbType.String);
                cmd.Parameters.Add(CreateParameter("IN_EMPLOYEE", employeeId, MySqlDbType.Int32));                
                cmd.Parameters.Add(CreateParameter("IN_USER", txnUser, MySqlDbType.String));
                cmd.Parameters.Add(pResult);

                NonQueryBlock(cmd, () => GetResult(pResult, sSp, result));

            }, (ex, msg) => SetExceptionResult("ControlAccessDAL.SetDownEmployee", msg, ex, result));

            return result;
        }

        public Result SetEmployeeAccessLevel(int employeeNumber, int accessLevel, string status, string user) {
            Result result = new();
            string sSp = SQL.SET_EMPLOYEE_ACCESS;

            TransactionBlock(this, () => {
                using var cmd = CreateCommand(sSp,  CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_RESULT", MySqlDbType.String);

                cmd.Parameters.Add(CreateParameter("IN_EMPLOYEE", employeeNumber, MySqlDbType.Int32));                
                cmd.Parameters.Add(CreateParameter("IN_ACCESS", accessLevel, MySqlDbType.Int32));                
                cmd.Parameters.Add(CreateParameter("IN_STATUS", status, MySqlDbType.String));
                cmd.Parameters.Add(CreateParameter("IN_USER", user, MySqlDbType.String));
                cmd.Parameters.Add(pResult);

                NonQueryBlock(cmd, () => GetResult(pResult, sSp, result));

            }, (ex, msg) => SetExceptionResult("ControlAccessDAL.SetAccessLevel", msg, ex, result));

            return result;                      
        }

        public ResultInsert SetEmployeeHint(EmployeeHint hint, string user)
        {
            ResultInsert result = new(); 
            string sSp = SQL.SET_EMPLOYEE_HINT;

            TransactionBlock(this, () => {
                using var cmd = CreateCommand(sSp, CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_RESULT", MySqlDbType.String);

                cmd.Parameters.Add(CreateParameter("IN_HINT_ID", hint.Id, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_EMPLOYEE_ID", hint.Employee.Id, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_IMG", hint.ImageData.Hex, MySqlDbType.LongText));
                cmd.Parameters.Add(CreateParameter("IN_EXTENSION", "JPEG", MySqlDbType.String));
                cmd.Parameters.Add(CreateParameter("IN_USER", user, MySqlDbType.String));
                cmd.Parameters.Add(pResult);

                NonQueryBlock(cmd, () => GetResult(pResult, sSp, result));

            }, 
                (ex, msg) => SetExceptionResult("ControlAccessDAL.SetEmployeeHint", msg, ex, result),
                () => SetResultInsert(result, hint)                
            );

            return result;
        }

        public ResultInsert SetCheck(Check check, string txnUser)
        {
            ResultInsert result = new();
            string sSp = SQL.SET_CONTROL_CHECK;

            TransactionBlock(this, () => {
                using var cmd = CreateCommand(sSp, CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_RESULT", MySqlDbType.String);                
                cmd.Parameters.Add(CreateParameter("IN_DEVICE", check?.Device?.Id, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_EMPLOYEE", check?.Employee?.Id, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_USER", txnUser, MySqlDbType.String));
                cmd.Parameters.Add(pResult);

                NonQueryBlock(cmd, () => GetResult(pResult, sSp, result));

            },
                (ex, msg) => SetExceptionResult("ControlAccessDAL.SetCheck", msg, ex, result),
                () => SetResultInsert(result, check)
            );

            return result;
        }

        public ResultInsert SetCheckAlt(CheckAlt CheckAlt, string txnUser)
        {
            ResultInsert result = new();
            string sSp = SQL.SET_CHECK_ALT;

            TransactionBlock(this, () =>
            {
                using var cmd = CreateCommand(sSp, CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_RESULT", MySqlDbType.String);

                cmd.Parameters.Add(CreateParameter("IN_DEVICE", CheckAlt?.Device?.Id, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_EMPLOYEE", CheckAlt?.Employee?.Id, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_VL_CHECK_DT", CheckAlt?.CheckDt, MySqlDbType.DateTime));
                cmd.Parameters.Add(CreateParameter("IN_VL_TYPE", CheckAlt?.CheckType, MySqlDbType.String));

                cmd.Parameters.Add(CreateParameter("IN_USER", txnUser, MySqlDbType.String));
                cmd.Parameters.Add(pResult);

                NonQueryBlock(cmd, () => GetResult(pResult, sSp, result));
            },
                (ex, msg) => SetExceptionResult("AccessControlDAL.SetCheckAlt", msg, ex, result),
                () => SetResultInsert(result, CheckAlt)
            );
            return result;
        }

        public ResultInsert SetCheckEmlpoyee(Check check, string txnUser)
        {
            ResultInsert result = new ResultInsert();
            string sSp = SQL.SET_CHECK_EMPLOYEE;

            TransactionBlock(this, () =>
            {
                using var cmd = CreateCommand(sSp, CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_RESULT", MySqlDbType.String);

                cmd.Parameters.Add(CreateParameter("IN_EMPLOYEE_ID", check?.Employee?.Id , MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_CHECK_DT",check?.CheckDt, MySqlDbType.DateTime));
                cmd.Parameters.Add(CreateParameter("IN_TYPE", check?.CheckType, MySqlDbType.String));
                cmd.Parameters.Add(CreateParameter("IN_DEVICE_ID", check?.Device?.Id, MySqlDbType.Int32));

                cmd.Parameters.Add(CreateParameter("IN_USER", txnUser, MySqlDbType.String));
                cmd.Parameters.Add(pResult);

                NonQueryBlock(cmd, () => GetResult(pResult, sSp, result));
            },
                (ex, msg) => SetExceptionResult("AccessControlDAL.SetCheckEmployee", msg, ex, result),
                () => SetResultInsert(result, check)
            );
            return result;
        }

    }
}